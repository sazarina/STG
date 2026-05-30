using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;
using System;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public class GameObject {
        internal ScriptController scriptController;


        /// <summary>
        /// InstantiateされているGameObjectの数
        /// </summary>
        public static int Count => GameObjectManager.Instance.Count;

        /// <summary>
        /// すべてのInstantiateされるオブジェクトの既定の親
        /// </summary>
        public static GameObject Root { get; internal set; }

        public Guid Guid { get; private set; }

        public string name { get; set; }
        public string tag { get; set; }

        public Transform transform { get; set; }

        public bool active { get; private set; } = true;
        public void SetActive(bool value) {
            active = value;
        }

        #region Mouse
        public bool IsMouseCursorPointed {
            get {
                SpriteRenderer sr;
                if (IsRegisteredComponent<SpriteRenderer>()) {
                    sr = GetComponent<SpriteRenderer>();
                    return sr.texture != null && sr.Rect.Contains(KeyInput.CurrentMouseState.Position);
                } else {
                    return false;
                }
            }
        }

        public bool IsMouseCursorClicked => IsMouseCursorPointed && KeyInput.MouseJustPressed(KeyInput.Mouses.LeftMouse);
        #endregion

        Dictionary<Type, Behavior> AttachedScripts = new Dictionary<Type, Behavior>();
        Dictionary<Type, Component> ComponentList = new Dictionary<Type, Component>();

        #region Functions
        public void Update() {

        }

        #region Instantiate
        GameObject(Guid Guid, string name, string tag, Texture2D texture) {
            this.Guid = Guid;
            this.name = name;
            this.tag = tag;
            //this.texture = texture;

            OnDestroy += () => {
                foreach (var component in GetComponents().Values) {
                    component.OnDestroy?.Invoke();
                }
            };
        }

        public static GameObject Instantiate(int x, int y, string name, Transform parent = null, Texture2D texture = null, string tag = "") {
            GameObject gameObject = InstantiateInternal(x, y, name, parent, texture, tag);
            return gameObject;
        }

        public static GameObject Instantiate<T>(int x, int y, string name, Transform parent = null, Texture2D texture = null, string tag = "") where T : Behavior, new() {
            GameObject gameObject = InstantiateInternal(x, y, name, parent, texture, tag);
            gameObject.AddComponent<T>();
            return gameObject;
        }

        public static GameObject Instantiate<T>(string name = "", Transform parent = null, string tag = "") where T : Behavior, new() {
            if (name == "") {
                name = typeof(T).Name;
            }
            GameObject gameObject = InstantiateInternal(0, 0, name, parent, null, tag);
            gameObject.AddComponent<T>();
            return gameObject;
        }

        static GameObject InstantiateInternal(int x, int y, string name, Transform parent, Texture2D texture = null, string tag = "") {
            GameObject gameObject = new GameObject(Guid.NewGuid(), name, tag, texture);

            Vector2 position = new Vector2(x, y);
            Transform transform = new Transform(position, gameObject, Vector2.Zero);
            gameObject.ComponentList.Add(typeof(Transform), transform);

            gameObject.transform = transform;

            GameObjectManager.AddGameObjectToQue(gameObject);

            //何もParentが指定されていなかったらRootを親にする、もしくは指定されていたら、指定しているものを親にする
            if (parent == null) {
                if (Root != null && gameObject.name != Root.name) {
                    gameObject.transform.SetParent(Root.transform);
                }
            } else {
                gameObject.transform.SetParent(parent);
            }

            return gameObject;
        }
        #endregion

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
                    component.Initialize();
                    component.gameObject = this;

                    if (type == typeof(SpriteRenderer)) {
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
            script.Initialize(ScriptController.Instance, this);

            //ScriptControllerのLateUpdateで遅延してStartを呼び出すようにする
            ScriptController.Register(script);
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
            GameObjectManager.RemoveGameObjectToQue(this);
        }

        internal Action OnDestroy;
    }
        #endregion
}
