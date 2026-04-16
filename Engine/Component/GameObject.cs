using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public partial class GameObject {
        internal ScriptController scriptController;

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
            foreach (var script in AttachedScripts.Values) {
                script.Update();
            }
        }

        public void Draw() {
            //foreach (var script in AttachedScripts.Values) {
            //    script.Draw();
            //}
        }

        #region Instantiate
        public GameObject(Guid Guid, string name, string tag, Texture2D texture) {
            this.Guid = Guid;
            this.name = name;
            this.tag = tag;
            //this.texture = texture;
        }

        public static GameObject Instantiate(int x, int y, string name, Texture2D texture = null, string tag = "") {
            GameObject gameObject = InstantiateInternal(x, y, name, texture, tag);
            return gameObject;
        }

        public static GameObject Instantiate<T>(int x, int y, string name, Texture2D texture = null, string tag = "") where T : Behavior, new() {
            GameObject gameObject = InstantiateInternal(x, y, name, texture, tag);
            gameObject.AddComponent<T>();
            return gameObject;
        }

        public static GameObject Instantiate<T>(string name = "", string tag = "") where T : Behavior, new() {
            if (name == "") {
                name = typeof(T).Name;
            }
            GameObject gameObject = InstantiateInternal(0, 0, name, null, tag);
            gameObject.AddComponent<T>();
            return gameObject;
        }

        static GameObject InstantiateInternal(int x, int y, string name, Texture2D texture = null, string tag = "", Transform parent = default) {
            GameObject gameObject = new GameObject(Guid.NewGuid(), name, tag, texture);

            Vector2 position = new Vector2(x, y);
            Transform transform = new Transform(position, gameObject, Vector2.Zero);
            gameObject.ComponentList.Add(typeof(Transform), transform);

            gameObject.transform = transform;

            //gameObject.layerGroup.SetGameObject(gameObject);
            //GameObjectManager.AddLayerGroup(gameObject);

            //先にGameObjectを監視リストに追加しないと、
            //transform.SetParentで親を指定をすることができない
            //gameObject.CreateTransformFunc = () => {の前でもちゃんと実行されるか検証したい
            GameObjectManager.AddGameObjectToList(gameObject);

            //何もParentが指定されていなかったらRootを親にする、もしくは指定されていたら、指定しているものを親にする
            if (parent == default) {
                if (gameObject.name != GameObjectManager.RootName) {
                    gameObject.transform.SetParent(GameObjectManager.Root);
                }
            } else {
                gameObject.transform.SetParent(parent);
            }

            return gameObject;
        }
        #endregion

        Func<Transform> CreateTransformFunc;

        #endregion



    }
}
