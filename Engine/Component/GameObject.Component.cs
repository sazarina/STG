using System;
using System.Collections.Generic;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    public partial class GameObject {
        #region Component 

        public T AddComponent<T>() where T : Component, new() {
            Type type = typeof(T);

            if (!IsRegisteredComponent<T>()) {
                if (typeof(Behavior).IsAssignableFrom(type)) {
                    var script = AttachScript(type);
                    ComponentList.Add(type, script);
                    return (T)script;
                } else if (typeof(Component).IsAssignableFrom(type)) {
                    Component component = new T();
                    component.gameObject = this;

                    if (type == typeof(SpriteRenderer)){
                        //これではSortingLayer:Defaultで登録されてしまうので、
                        //sr.SortingLayer = Layer("Character");
                        //代入されたときにsetterで登録をする。
                        //RenderManager.Instance().Register(component as SpriteRenderer);
                    }

                    ComponentList.Add(type, component);
                    return (T)component;
                } else {
                    throw new NotImplementedException($"{type.Name}型のは実装されていません");
                }

            } else {
                throw new ArgumentException($"{type.Name}は既にコンポーネントが登録されています");
            }
        }

        public T GetComponent<T>() {
            Type type = typeof(T);
            if (typeof(Behavior).IsAssignableFrom(type)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)AttachedScripts[type];
                } else {
                    Debug.Log($"{type.Name}型のスクリプトはアタッチされていません");
                    return default;
                }

            } else if (typeof(Component).IsAssignableFrom(type)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)ComponentList[type];

                } else {
                    Debug.Log($"{type.Name}型のコンポーネントはアタッチされていません");
                    return default;
                }

            } else {
                Debug.Log($"{type.Name}型の親を持つコンポーネントは見つかりません");
                return default;
            }
        }

        public Dictionary<Type, Component> GetComponents()
            => ComponentList;

        public bool IsRegisteredComponent<T>() {
            Type type = typeof(T);
            if (typeof(Component).IsAssignableFrom(type)) {
                return ComponentList.ContainsKey(type);
            } else {
                Debug.Log($"{type.Name}型の親を持つコンポーネントは見つかりません");
                return false;
            }
        }

        public T GetComponentInParent<T>() where T : Component, new() =>
            transform.Parent.gameObject.GetComponent<T>();

        public T[] GetComponentInChildren<T>() {
            List<T> components = new List<T>();

            if (transform.Children == null) {
                return null;
            }

            foreach (var child in transform.Children.Values) {
                if (child.IsRegisteredComponent<T>()) {
                    T t = child.GetComponent<T>();
                    if (t != null) {
                        components.Add(t);
                    }
                }
            }

            return components.ToArray();
        }

        #endregion


        /// <summary>
        /// 指定された型のスクリプトをアタッチします。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Component AttachScript(Type type) {
            // ジェネリックにしない理由:
            // コンパイラは呼び出し側で `T` を宣言された制約（ここでは `Component`）としてしか扱えないため、
            // `Behavior` 固有のメンバーを直接呼ぶとコンパイルエラーになる。
            // そのため `Activator.CreateInstance` で生成して `Behavior` にキャストしている。。
            var script = (Behavior)Activator.CreateInstance(type);
            Debug.Log($"スクリプト:{type.Name}を{name}にアタッチします");
            script.Initialize(ScriptController.Instance, this);
            script.Start();
            AttachedScripts.Add(type, script);
            return script;
        }

        public static GameObject Find(string name) =>
            GameObjectManager.Instance.Find(name);

        public static IEnumerable<GameObject> FindObjects(string name) =>
            GameObjectManager.Instance.FindObjects(name);

        public static IEnumerable<GameObject> FindWithTags(string tag) => 
            GameObjectManager.Instance.FindWithTags(tag);

        public void Destroy() {
            GameObjectManager.Instance.Destroy(this);
        }
    }
}