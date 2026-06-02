using Microsoft.Xna.Framework;
using STG.Engine.Debugging;
using System;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public class Transform : Component {
        public Dictionary<Guid, GameObject> Children { get; private set; } = new Dictionary<Guid, GameObject>();

        /// <summary>
        /// Parentがnullの場合はRootが親になる
        /// </summary>
        public Transform Parent { get; private set; } = null;
        /// <summary>
        /// 階層保持用
        /// </summary>
        internal int Hierarchy { get; private set; } = 0;

        public string parentName {
            get {
                if (Parent != null) {
                    return Parent.Name;
                } else {
                    return null;
                }
            }
        }

        public int ChildCount {
            get {
                return Children.Count;
            }
        }

        Vector2 _position = new Vector2();
        Vector2 _localPosition = new Vector2();

        public Vector2 position {
            get {
                if (Parent == GameObject.Root || Parent == null) {
                    return _position;
                } else {
                    return _position = Parent.position + localPosition;
                }
            }
            set {
                if (Parent == GameObject.Root || Parent == null) {

                } else {

                }
                _localPosition = value;
                _position = value;
            }
        }

        public Vector2 localPosition {
            get {
                if (Parent == GameObject.Root || Parent == null) {
                    //Debug.Log(_Debug.SetDebugInfo(),"NoParent");
                    return _position;
                } else {
                    return _localPosition;
                }
            }
            set {
                //
                if (Parent == GameObject.Root || Parent == null) {
                    //Debug.Log(_Debug.SetDebugInfo(), Parent.Name);
                    _localPosition = value;
                } else {
                    //Debug.Log(_Debug.SetDebugInfo(), Name);
                    
                    _localPosition = value;
                    _position = Parent._position + localPosition;
                }
            }
        }

        /// <summary>
        /// Local座標を求める関数
        /// 親がいない場合はそのオブジェクトのpositionを返す
        /// </summary>
        /// <returns></returns>
        public Vector2 GetLocalPosition() {
            if (Parent == GameObject.Root || Parent == null) {
                return _position;
            } else {
                return _position - Parent._position;
            }
        }

        public Vector2 center { get; private set; } = new Vector2(0, 0);

        #region Constructors
        internal Transform() {

        }

        internal Transform(Vector2 position, GameObject gameObject = null, Vector2 center = default) {
            if (center == default) {
                center = new Vector2(0, 0);
            }

            _position = position;

            this.center = center;
            this.gameObject = gameObject;

            OnDestroy += () => {
                //if (Parent != null) {
                //    RemoveParent_Internal(Parent.gameObject);
                //}
                //if (Children.Count > 0) {
                //    RemoveChildrenAll();
                //}
            };
        }

        #endregion

        #region 親子関係

        public void RemoveChild(Transform child) {
            if (Children.Count == 0) {
                throw new Exception("このオブジェクトには子はいません");
            }
            Children.Remove(child.gameObject.Guid);
            if (Children.Count == 0) {
                ClearChildren();
            }
            child.ClearParent();
        }

        public void RemoveChildrenAll() {
            if (ChildCount == 0) {
                throw new Exception("このオブジェクトには子はいません");
            }
            var children = Children;
            
            foreach (var child in children.Values) {
                child.transform.ClearParent();
            }
            ClearChildren();
        }

        public void RemoveParent() {
            if (Parent == null || Parent == GameObject.Root) {
                throw new InvalidOperationException("このオブジェクトには親がいません");
            }
            //position = position + Parent.position;

            Parent.Children.Remove(gameObject.Guid);
            if (Parent.Children.Count == 0) {
                Parent.ClearChildren();
            }
            
            SetParent(GameObject.Root);
        }

        void ClearParent() {
            SetParent(GameObject.Root);
        }

        void ClearChildren() {
            Children.Clear();
        }

        public void SetChild(Transform child) {
            Children.Add(child.gameObject.Guid, child.gameObject);

            child.Parent = this;
            child.localPosition = -GetLocalPosition();
        }

        public void SetParent(Transform parent) {
            Parent = parent;
            if (!AddChild()) {

                return;
            }
            localPosition = GetLocalPosition();

            //position = Parent.position + localPosition;

            //階層を更新
            Hierarchy = Parent.Hierarchy + 1;
        }

        /// <summary>
        /// childがParentを追加する、
        /// </summary>
        /// <returns></returns>
        bool AddChild() {
            if (!Parent.Children.ContainsKey(gameObject.Guid)) {

                //階層を更新
                Hierarchy = Parent.Hierarchy + 1;

                Parent.Children.Add(gameObject.Guid, gameObject);
                return true;
            } else {
                //上書きしないで置く
                //Debug.Log(_Debug.SetDebugInfo(), $"{Name}には既に親:{Parent.Name}が設定されています", "上書きします");
                //Parent.Children[gameObject.Guid] = gameObject;
                return false;
            }
        }

        #endregion
    }
}
