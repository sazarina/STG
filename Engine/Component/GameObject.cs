using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace STG.Engine.Component {
    public class GameObject {
        /// <summary>
        /// InstantiateされているGameObjectの数
        /// </summary>
        public static int Count => GameObjectManager.Instance.Count;

        /// <summary>
        /// すべてのInstantiateされるオブジェクトの既定の親
        /// </summary>
        public static Transform? Root { get; internal set; }
        /// <summary>
        /// GameObjectの一意な識別子
        /// </summary>
        public Guid Guid { get; private set; }
        /// <summary>
        /// GameObjectの名前
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// GameObjectのタグ
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// GameObjectの位置、回転、スケールを管理するTransformコンポーネント
        /// </summary>
        public Transform transform { get; set; }
        /// <summary>
        /// GameObjectがアクティブかどうかを示すフラグ
        /// </summary>
        public bool active { get; private set; } = true;
        /// <summary>
        /// GameObjectのアクティブ状態を設定します
        /// </summary>
        /// <param name="value"></param>
        public void SetActive(bool value) {
            active = value;
        }

        #region Mouse
        /// <summary>
        /// マウスカーソルがGameObjectのSpriteRendererの範囲内にあるかどうかを示すフラグ
        /// </summary>
        public bool IsMouseCursorPointed {
            get {
                if (IsRegisteredComponent<SpriteRenderer>()) {
                    SpriteRenderer? sr = GetComponent<SpriteRenderer>();
                    if (sr == null) {
                        Debug.LogException("GameObject", new Exception($"GameObject {name} に SpriteRenderer コンポーネントが登録されているのに、GetComponent<SpriteRenderer>() が null を返しました。"));
                        return false;
                    }
                    return sr.texture != null && sr.Rect.Contains(KeyInput.CurrentMouseState.Position);
                } else {
                    return false;
                }
            }
        }

        public bool IsMouseCursorClicked => IsMouseCursorPointed && KeyInput.MouseJustPressed(KeyInput.Mouses.LeftMouse);
        #endregion
        /// <summary>
        /// GameObjectにアタッチされているBehaviorのリスト
        /// </summary>
        internal Dictionary<Type, Behavior> Scripts { get; set; } = new Dictionary<Type, Behavior>();
        /// <summary>
        /// GameObjectにアタッチされているComponentのリスト
        /// BehaviorとComponentを区別して管理するため、BehaviorはScriptsに、ComponentはComponentsに格納される
        /// </summary>
        Dictionary<Type, Component> Components { get; set; } = new Dictionary<Type, Component>();

        #region Functions
        public void Update() {

        }

        #region Instantiate
        GameObject(Guid Guid, string name, string tag, int x, int y, Texture2D? texture) {
            this.Guid = Guid;
            this.name = name;
            this.tag = tag;

            Vector2 position = new Vector2(x, y);
            transform = new Transform(position, this, Vector2.Zero);
            Components.Add(typeof(Transform), transform);

            //this.texture = texture;

            OnDestroy += () => {
                foreach (var component in GetComponents()) {
                    component.OnDestroy?.Invoke();
                }
            };
        }

        public static GameObject Instantiate(string name, Transform? parent = null, Texture2D? texture = null, string tag = "") {
            GameObject gameObject = InstantiateInternal(0, 0, name, parent, texture, tag);
            return gameObject;
        }

        public static GameObject Instantiate(int x, int y, string name, Transform?   parent = null, Texture2D? texture = null, string tag = "") {
            GameObject gameObject = InstantiateInternal(x, y, name, parent, texture, tag);
            return gameObject;
        }

        public static T Instantiate<T>(int x, int y, string name, Transform? parent = null, Texture2D? texture = null, string tag = "") where T : Behavior, new() {
            GameObject gameObject = InstantiateInternal(x, y, name, parent, texture, tag);
            return gameObject.AddComponent<T>();
        }

        public static T Instantiate<T>(string name = "", Transform? parent = null, string tag = "") where T : Behavior, new() {
            if (name == "") {
                name = typeof(T).Name;
            }
            GameObject gameObject = InstantiateInternal(0, 0, name, parent, null, tag);
            return gameObject.AddComponent<T>();
        }

        static GameObject InstantiateInternal(int x, int y, string name, Transform? parent, Texture2D? texture = null, string tag = "") {
            GameObject gameObject = new GameObject(Guid.NewGuid(), name, tag, x, y, texture);

            GameObjectManager.AddGameObjectToQue(gameObject);

            //何もParentが指定されていなかったらRootを親にする、もしくは指定されていたら、指定しているものを親にする
            if (parent == null) {
                if (Root != null && gameObject.name != Root.name) {
                    gameObject.transform.SetParent(Root);
                }
            } else {
                gameObject.transform.SetParent(parent);
            }

            return gameObject;
        }
        #endregion

        #region Component 
        /// <summary>
        /// GameObjectにコンポーネントを追加します。
        /// </summary>
        /// <typeparam name="T">追加するコンポーネントの型</typeparam>
        /// <returns>追加されたコンポーネントのインスタンス</returns>
        public T AddComponent<T>() where T : Component, new() {
            Type type = typeof(T);

            if (!IsRegisteredComponent<T>()) {
                if (typeof(Behavior).IsAssignableFrom(type)) {
                    var script = ScriptController.Instance.AttachScript(type, this);
                    return (T)(object)script;
                } else if (typeof(Component).IsAssignableFrom(type)) {
                    Component component = new T {
                        gameObject = this
                    };

                    component.Initialize();

                    if (type == typeof(SpriteRenderer)) {
                        //これではSortingLayer:Defaultで登録されてしまうので、
                        //sr.SortingLayer = Layer("Character");
                        //代入されたときにsetterで登録をする。
                        //RenderManager.Instance().Register(component as SpriteRenderer);
                    }

                    Components.Add(type, component);
                    return (T)component;
                } else {
                    throw new NotImplementedException($"{type.Name}型のコンポーネントは実装されていません");
                }

            } else {
                Debug.LogError("GameObject", $"{type.Name}は既にコンポーネントが登録されています");
                //戻り値がnullになることはないので、null許容型を外すために!をつける
                return GetComponent<T>()!;
            }
        }

        public T? GetComponent<T>() {
            Type type = typeof(T);
            if (typeof(Behavior).IsAssignableFrom(type)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)Scripts[type];
                } else {
                    Debug.Log($"{type.Name}型のスクリプトはアタッチされていません");
                    return default;
                }
            } else if (typeof(Component).IsAssignableFrom(type)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)Components[type];
                } else {
                    Debug.Log($"{type.Name}型のコンポーネントはアタッチされていません");
                    return default;
                }
            } else {
                Debug.Log($"{type.Name}型の親を持つコンポーネントは見つかりません");
                return default;
            }
        }

        public Component[] GetComponents() {
            var components = Components.Values.ToList();
            components.AddRange(Scripts.Values);
            return components.ToArray();
        }


        public bool IsRegisteredComponent<T>() {
            Type type = typeof(T);
            // Behavior (スクリプト) と Component を区別して登録有無を確認する
            if (typeof(Behavior).IsAssignableFrom(type)) {
                return Scripts.ContainsKey(type);
            } else if (typeof(Component).IsAssignableFrom(type)) {
                return Components.ContainsKey(type);
            } else {
                Debug.Log($"コンポーネント {type.Name} は登録されていません");
                return false;
            }
        }

        public T? GetComponentInParent<T>() where T : Component, new() {
            if (transform.Parent == null) {
                Debug.LogException("GameObject", new Exception($"GameObject {name} の親が存在しません。"));
                return default;   
            }
            return transform.Parent?.gameObject.GetComponent<T>();
        }

        public T[]? GetComponentInChildren<T>() {
            List<T> components = new List<T>();

            if (transform.Children == default) {
                return null;
            }

            foreach (var child in transform.Children.Values) {
                if (child.IsRegisteredComponent<T>()) {
                    T? t = child.GetComponent<T>();
                    if (t != null) {
                        components.Add(t);
                    }
                }
            }

            return components.ToArray();
        }

        #endregion

        public static GameObject? Find(string name) =>
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
