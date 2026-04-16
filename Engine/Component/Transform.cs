using Microsoft.Xna.Framework;
using STG.Engine.Debugging;
using System;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public class Transform : Component {
        public Dictionary<Guid, GameObject> Children { get; private set; } = new Dictionary<Guid, GameObject>();
        public Transform Parent { get; private set; } = null;
        /// <summary>
        /// 階層保持用
        /// </summary>
        internal int hierarchy = 0;

        public string parentName {
            get {
                if (Parent != null) {
                    return Parent.Name;
                } else {
                    return null;
                }
            }
        }

        public static Transform Empty => new Transform { Name = "Empty" };
        public static Transform Origin => new Transform { Name = "Origin" };

        public int ChildCount {
            get {
                return Children.Count;
            }
        }

        Vector2 _position = new Vector2();
        Vector2 _localPosition = new Vector2();

        public Vector2 position {
            get {
                if (Parent != null) {
                    if (Parent.Name == GameObjectManager.RootName) {
                        return _position;
                    } else {
                        return _position = Parent.position + localPosition;
                    }
                } else {
                    //Root
                    return _position;
                }
            }
            set {
                if (Parent != null) {
                    if (Parent.Name == GameObjectManager.RootName) {

                    } else {

                    }
                }
                _localPosition = value;
                _position = value;
            }
        }

        public Vector2 localPosition {
            get {
                if (Parent.Name == GameObjectManager.RootName) {
                    //Debug.Log(_Debug.SetDebugInfo(),"NoParent");
                    return _position;
                } else {
                    return _localPosition;
                }
            }
            set {
                //
                if (Name == GameObjectManager.RootName) {
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
            if (Parent.Name != GameObjectManager.RootName) {
                return _position - Parent._position;
            } else {
                return _position;
            }
        }

        public Vector2 center { get; private set; } = new Vector2(0, 0);

        #region Constructors
        public Transform() {

        }

        public Transform(Vector2 position, GameObject gameObject = null, Vector2 center = default) {
            if (center == default) {
                center = new Vector2(0, 0);
            }

            _position = position;

            this.center = center;
            this.gameObject = gameObject;
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
            GameObjectManager.Instance().UpdateGameObjectList(gameObject);
            child.ClearParent();
            GameObjectManager.Instance().UpdateGameObjectList(child.gameObject);
        }

        public void RemoveChildrenAll() {
            if (ChildCount == 0) {
                throw new Exception("このオブジェクトには子はいません");
            }
            var children = Children;
            

            GameObjectManager.Instance().UpdateGameObjectList(gameObject);
            foreach (var child in children.Values) {
                child.transform.ClearParent();
                GameObjectManager.Instance().UpdateGameObjectList(child);
            }
            ClearChildren();
        }

        public GameObject RemoveParent() {
            if (Parent == null) {
                Debug.Log("このオブジェクトには親がいません");
                return null;
            }
            GameObject parent = Parent.gameObject;
            RemoveParent_Internal(parent);
            return parent;
        }

        public GameObject RemoveParent_Internal(GameObject parent) {
            //position = position + Parent.position;

            ClearParent();
            GameObjectManager.Instance().UpdateGameObjectList(gameObject);

            parent.transform.Children.Remove(gameObject.Guid);
            if (parent.transform.Children.Count == 0) {
                parent.transform.ClearChildren();
            }
            GameObjectManager.Instance().UpdateGameObjectList(parent);
            return parent;
        }
        
        void ClearParent() {
            SetParent(GameObjectManager.Root);
        }

        void ClearChildren() {
            Children.Clear();
        }

        public void SetParent(GameObject parent) {
            SetParent(parent.transform);
        }

        public void SetChild(Transform child) {
            Children.Add(child.gameObject.Guid,child.gameObject);

            GameObjectManager.Instance().UpdateGameObjectList(gameObject);
            child.Parent = this;
            child.localPosition = -GetLocalPosition();
            GameObjectManager.Instance().UpdateGameObjectList(child.gameObject);
        }

        public void SetParent(Transform parent) {
            Parent = parent;
            if (!AddChild()) {
                
                return;
            }
            GameObjectManager.Instance().UpdateGameObjectList(Parent.gameObject);

            localPosition = GetLocalPosition();

            //position = Parent.position + localPosition;

            gameObject.transform = SetHierarchy(this,Parent.hierarchy+1);
            GameObjectManager.Instance().UpdateGameObjectList(gameObject); 
            

        }

        Transform SetHierarchy(Transform transform,int hierarchy) {
            transform.hierarchy = hierarchy;
            return transform;
        }

        /// <summary>
        /// childがParentを追加する、
        /// </summary>
        /// <returns></returns>
        bool AddChild() {
            if (!Parent.Children.ContainsKey(gameObject.Guid)) {

                gameObject.transform = SetHierarchy(this, Parent.hierarchy + 1);

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
