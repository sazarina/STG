using System;
using System.Collections.Generic;
using System.Linq;
using Xenon.Core;
using STG.Engine.Graphics;
using _Debug = STG.Engine.Debug.Debug;

namespace STG.Engine.Component {
    public class GameObjectManager {
        #region シングルトン
        static GameObjectManager self = null;

        protected GameObjectManager(EntityManager entityManager) {
            this.entityManager = entityManager;
            scriptController = entityManager.scriptController;
            _Debug.Log("Initialize/ctor()");
        }


        public static GameObjectManager Instance(EntityManager entityManager = null) {
            if (self == null) {
                if (entityManager == null) {
                    throw new InvalidOperationException("最初の呼び出し時は EntityManager  を渡す必要があります");
                }

                if (entityManager.scriptController == null) {
                    throw new InvalidOperationException("EntityManager の ScriptController が null です");
                }

                self = new GameObjectManager(entityManager);
            }

            return self;
        }
        public static GameObjectManager Instance<T>(EntityManager entityManager = null) where T : GameObjectManager {
            if (self == null) {
                if (entityManager == null) {
                    throw new InvalidOperationException("最初の呼び出し時は EntityManager  を渡す必要があります");
                }

                if (entityManager.scriptController == null) {
                    throw new InvalidOperationException("EntityManager の ScriptController が null です");
                }

                _Debug.Log($"GameObjectManager を {typeof(T).Name} として初期化します。");
                self = (T)Activator.CreateInstance(typeof(T), entityManager);
            } else {
                if (entityManager != null) {
                    throw new InvalidOperationException($"GameObjectManager はすでに {self.GetType().Name} として初期化されています。");
                }
            }
            return self;
        }

        #endregion

        protected EntityManager entityManager;
        protected ScriptController scriptController;

        /// <summary>
        /// すべてのInstantiateされるオブジェクトの既定の親
        /// </summary>
        internal static GameObject Root;
        public static GameObject GetRoot() { return Root; }

        internal static string RootlName => Root.name;
        protected Dictionary<Guid, GameObject> GameObjects = new Dictionary<Guid, GameObject>();


        protected List<Layer> LayerList = new List<Layer>();
        /// <summary>
        /// 監視レイヤーリスト
        /// </summary>
        public Dictionary<GameObject, LayerGroup> Layers = new Dictionary<GameObject, LayerGroup>();
        /// <summary>
        /// GameObjectの監視レイヤーを追加する
        /// </summary>
        /// <param name="gameObject">追加するオブジェクト</param>
        public static void AddLayerGroup(GameObject gameObject) {
            Instance().Layers.Add(gameObject, gameObject.layerGroup);
        }
        /// <summary>
        /// GameObjectの監視レイヤーを更新する
        /// </summary>
        /// <param name="gameObject">更新するオブジェクト</param>
        /// <param name="layerGroup">更新するオブジェクトのレイヤー</param>
        public static void UpdateLayerGroup(GameObject gameObject, LayerGroup layerGroup) {
            if (Instance().Layers.ContainsKey(gameObject)) {
                Instance().Layers[gameObject] = layerGroup;
            } else {
                Debug.Debug.Log($"{layerGroup.Name}レイヤーは登録されていません");
            }
        }



        public virtual void Initialize() {
            Root = GameObject.Instantiate(0, 0, "OriginLocalPosition");
        }


        public virtual void Update() {
            var Objects = GameObjects.Values;
            Objects.ForEach(gameObject => {
                if (gameObject.active) {
                    gameObject.Update();
                    gameObject.GetComponents().Values.ForEach(component => {
                        component.Update();
                    });
                }
            });
        }
        /// <summary>
        /// <para>レイヤー機能</para>
        /// まずlayerOrderで昇順にソートしてから、GroupごとにorderInLayerで昇順にして描画
        /// </summary>
        public virtual void Draw() {
            var groupList = Layers.Values.GroupBy(x => x.layer.layerOrder).OrderBy(x => x.Key);
            groupList.ForEach(group => {
                var sorted = group.OrderBy(x => x.orderInLayer);
                sorted.ForEach(layergroup => {
                    var gameObject = layergroup.gameObject;
                    if (gameObject.active) {
                        gameObject.Draw();
                        gameObject.GetComponents().Values.ForEach(component => component.Draw());
                    }
                });
            });
        }

        public static void AddGameObjectToList(GameObject gameObject) {
            Instance().GameObjects.Add(gameObject.Guid, gameObject);
        }

        public GameObject[] FindWithTags(string tag) {
            return GameObjects.Values.Where(x => x.tag == tag).ToArray();
        }

        public GameObject FindWithName(string name) {
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
                _Debug.Log($"{gameObject.name}は登録されていません");
            } else {
                GameObjects[gameObject.Guid] = gameObject;
            }
        }

        public void Destroy(GameObject gameObject) {
            GameObjects.Remove(gameObject.Guid);
        }

    }
}