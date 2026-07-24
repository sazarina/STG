using System;
using System.Collections.Generic;
using STG.Engine.Graphics;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    public class GameObjectManager {
        #region シングルトン
        static GameObjectManager? instance = null;

        bool isInitialized = false;

        public GameObjectManager() {
            Debug.Log("Initialize/ctor()");
        }

        internal static T CreateInstance<T>() where T:GameObjectManager { 
            var ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            if(ctor == null) {
                throw new InvalidOperationException($"型 {typeof(T).Name} にパラメータなしのコンストラクタが存在しません。");
            }
            
            T instance = (T)ctor.Invoke(null);
            GameObjectManager.instance = instance;
            return instance;
        }

        internal static GameObjectManager Instance {
            get {
                if (instance == null) {
                    Debug.Log($"GameObjectManager を {typeof(GameObjectManager).Name} として初期化します。");
                    instance = new GameObjectManager();
                }

                return instance;
            }
        }

        #endregion

        protected ScriptController? scriptController;

        /// <summary>
        /// InstantiateされたすべてのGameObjectを管理するリスト
        /// </summary>
        protected readonly Dictionary<Guid, GameObject> GameObjects = new Dictionary<Guid, GameObject>();

        public int Count => GameObjects.Count;

        readonly Queue<GameObject> addQueue = new Queue<GameObject>();
        readonly Queue<GameObject> removeQueue = new Queue<GameObject>();

        protected List<LayerGroup> LayerList = new List<LayerGroup>();

        public virtual void Initialize(ScriptController scriptController) {
            if (scriptController == null) {
                throw new InvalidOperationException("ScriptController が null です");
            }

            this.scriptController = scriptController;

            GameObject.Root = GameObject.Instantiate(0, 0, "Root").transform;
            Debug.Log("GameObjectManager.Initialize()");
        }

        /// <summary>
        /// 初回のLateUpdateが呼ばれたときに、OnInitializedイベントを発火させる
        /// </summary>
        public event Action OnInitialized = delegate { };

        public virtual void Awake() { }

        public virtual void Update() {
            foreach (var gameObject in GameObjects.Values) {
                if (gameObject.active) {
                    gameObject.Update();
                    foreach (var component in gameObject.GetComponents()) {
                        if (component is not Behavior) {
                            component.Update();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// LateUpdateは、Updateの後に呼び出されるメソッドで、
        /// Update中にGameObjectのコレクションが変更されるのを防ぐために、
        /// 追加されたGameObjectをキューからGameObjectsリストに移動するために使用されます。
        /// </summary>
        public virtual void LateUpdate() {
            //Debug.Log("que:"+addQueue.Count);
            while (addQueue.Count > 0) {
                var gameObject = addQueue.Dequeue();
                GameObjects.Add(gameObject.Guid, gameObject);
            }

            while (removeQueue.Count > 0) {
                var gameObject = removeQueue.Dequeue();
                gameObject.OnDestroy?.Invoke();
                GameObjects.Remove(gameObject.Guid);
            }

            if (!isInitialized) {
                isInitialized = true;
                OnInitialized?.Invoke();
            }
        }

        /// <summary>
        /// すぐに追加すると、Update中にコレクションが変更される可能性があるため、キューに追加して後で処理する
        /// </summary>
        internal static void AddGameObjectToQue(GameObject gameObject) {
            if (instance == null) {
                throw new InvalidOperationException("GameObjectManager が初期化されていません。Initialize() を先に呼び出してください。");
            }
            instance.addQueue.Enqueue(gameObject);
        }

        internal static void RemoveGameObjectToQue(GameObject gameObject) {
            if (instance == null) {
                throw new InvalidOperationException("GameObjectManager が初期化されていません。Initialize() を先に呼び出してください。");
            }
            instance.removeQueue.Enqueue(gameObject);
        }


        internal IEnumerable<GameObject> FindWithTags(string tag) {
            foreach (var gameObject in GameObjects.Values) {
                if (gameObject.tag == tag) {
                    yield return gameObject;
                }
            }
        }

        internal GameObject? Find(string name) {
            foreach (var gameObject in GameObjects.Values) {
                if (gameObject.name == name) {
                    return gameObject;
                }
            }

            return null;
        }

        internal IEnumerable<GameObject> FindObjects(string name) {
            foreach (var gameObject in GameObjects.Values) {
                if (gameObject.name == name) {
                    yield return gameObject;
                }
            }
        }

        internal GameObject? FindWithGuid(Guid guid) {
            if (!GameObjects.TryGetValue(guid, out GameObject? value)) {
               Debug.LogException("FindWithGuid", new KeyNotFoundException("GameObjectが見つかりません"));
                return null;
            } else {
                return value;
            }
            
        }
    }
}