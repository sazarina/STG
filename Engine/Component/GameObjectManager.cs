using System;
using System.Collections.Generic;
using System.Linq;
using STG.Engine.Graphics;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    public class GameObjectManager {
        #region シングルトン
        static GameObjectManager self = null;

        public GameObjectManager() {
            Debug.Log("Initialize/ctor()");
        }


        public static GameObjectManager Instance {
            get {
                if (self == null) {
                    Debug.Log($"GameObjectManager を {typeof(GameObjectManager).Name} として初期化します。");
                    self = new GameObjectManager();
                }

                return self;
            }
        }

        //public static T Instance<T>() where T : GameObjectManager {
        //    if (self == null) {
        //        Debug.Log($"GameObjectManager を {typeof(T).Name} として初期化します。");
        //        self = (T)Activator.CreateInstance(typeof(T));
        //    } else {
        //        throw new InvalidOperationException($"GameObjectManager はすでに {self.GetType().Name} として初期化されています。");
        //    }

        //    return (T)self;
        //}

        #endregion

        protected ScriptController scriptController;

        /// <summary>
        /// すべてのInstantiateされるオブジェクトの既定の親
        /// </summary>
        internal static GameObject Root;
        public static GameObject GetRoot() { return Root; }

        internal static string RootName {
            get {
                if (Root != null) {
                    return Root.name;
                } else {
                    return "Root";
                }
            }
        }

        /// <summary>
        /// InstantiateされたすべてのGameObjectを管理するリスト
        /// </summary>
        protected readonly Dictionary<Guid, GameObject> GameObjects = new Dictionary<Guid, GameObject>();

        readonly Queue<GameObject> addQueue = new Queue<GameObject>();
        readonly Queue<GameObject> removeQueue = new Queue<GameObject>();

        protected List<LayerGroup> LayerList = new List<LayerGroup>();
        /// <summary>
        /// 監視レイヤーリスト
        /// </summary>
        //public Dictionary<GameObject, LayerGroup> Layers = new Dictionary<GameObject, LayerGroup>();
        /// <summary>
        /// GameObjectの監視レイヤーを追加する
        /// </summary>
        /// <param name="gameObject">追加するオブジェクト</param>
        //public static void AddLayerGroup(GameObject gameObject) {
        //    Instance().Layers.Add(gameObject, gameObject.layerGroup);
        //}
        /// <summary>
        /// GameObjectの監視レイヤーを更新する
        /// </summary>
        /// <param name="gameObject">更新するオブジェクト</param>
        /// <param name="layerGroup">更新するオブジェクトのレイヤー</param>
        //public static void UpdateLayerGroup(GameObject gameObject, LayerGroup layerGroup) {
        //    if (Instance().Layers.ContainsKey(gameObject)) {
        //        Instance().Layers[gameObject] = layerGroup;
        //    } else {
        //        Debug.Log($"{layerGroup.Name}レイヤーは登録されていません");
        //    }
        //}

        public virtual void Initialize(ScriptController scriptController) {
            if (scriptController == null) {
                throw new InvalidOperationException("ScriptController が null です");
            }
            
            this.scriptController = scriptController;

            Root = GameObject.Instantiate(0, 0, "Root");
            Debug.Log("GameObjectManager.Initialize()");
        }

        public virtual void Update() {
            foreach (var gameObject in GameObjects.Values) {
                if (gameObject.active) {
                    gameObject.Update();
                    foreach (var component in gameObject.GetComponents().Values) {
                        component.Update();
                    }
                }
            }
        }

        public virtual void LateUpdate() {
            //Debug.Log("que:"+addQueue.Count);
            while (addQueue.Count > 0) {
                var gameObject = addQueue.Dequeue();
                self.GameObjects.Add(gameObject.Guid, gameObject);
            }

            //Debug.Log("que:"+addQueue.Count);
        }

        /// <summary>
        /// <para>レイヤー機能</para>
        /// まずlayerOrderで昇順にソートしてから、GroupごとにorderInLayerで昇順にして描画
        /// 
        /// 廃止
        /// 
        /// </summary>
        //public virtual void Draw() {
        //    var groupList = Layers.Values.GroupBy(x => x.layeres.layerOrder).OrderBy(x => x.Key);
        //    groupList.ForEach(group => {
        //        var sorted = group.OrderBy(x => x.orderInLayer);
        //        sorted.ForEach(layergroup => {
        //            var gameObject = layergroup.gameObject;
        //            if (gameObject.active) {
        //                gameObject.Draw();
        //                gameObject.GetComponents().Values.ForEach(component => component.Draw());
        //            }
        //        });
        //    });
        //}

        public static void AddGameObjectToQue(GameObject gameObject) {
            // すぐに追加すると、Update中にコレクションが変更される可能性があるため、キューに追加して後で処理する
            self.addQueue.Enqueue(gameObject);
        }


        public GameObject[] FindWithTags(string tag) {
            return GameObjects.Values.Where(x => x.tag == tag).ToArray();
        }

        public GameObject FindWithName(string name) {
            if (!GameObjects.Values.Any(x => x.name == name)) {
                return null;
            }

            return GameObjects.Values.Where(x => x.name == name).First();
        }

        public GameObject FindWithGuid(Guid guid) {
            if (!GameObjects.ContainsKey(guid)) {
                throw new NullReferenceException("GameObjectが見つかりません");
            } else {
                return GameObjects[guid];
            }
        }

        public void UpdateGameObjectList(GameObject gameObject) {
            if (!GameObjects.ContainsKey(gameObject.Guid)) {
                Debug.Log($"{gameObject.name}は登録されていません");
            } else {
                GameObjects[gameObject.Guid] = gameObject;
            }
        }

        public void Destroy(GameObject gameObject) {
            GameObjects.Remove(gameObject.Guid);
        }

    }
}