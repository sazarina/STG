using ChevyRay.Coroutines;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    /// <summary>
    /// スクリプトの基底クラス
    /// </summary>
    public abstract class Behavior : Component {
        /// <summary>
        /// スクリプトを管理するクラス
        /// </summary>
        ScriptController ScriptController => ScriptController.Instance;
        /// <summary>
        /// ChevyRay.Coroutinesのコルーチンを実行するクラス
        /// </summary>
        protected CoroutineRunner CoroutineRunner {
            get {
                return ScriptController.coroutineRunner;
            }
        }

        public void Initialize(GameObject gameObject) {
            this.gameObject = gameObject;
        }

        public virtual void Start() { }

        protected Texture2D LoadTexture(string path, string name) => GraphicsUltis.CreateTexture(path, name);

        protected Texture2D LoadTexture(string assetName) {
            return AssetManager.Load(assetName);
        }

        public static LayerGroup Layer(string name) {
            if(RenderManager.SortingLayers.ContainsKey(name)) {
                return RenderManager.SortingLayers[name];
            } else {
                Debug.LogError("LayerGroup", $"LayerGroup '{name}' is not registered. Using default LayerGroup.");

                RenderManager.SortingLayers.Add(name, LayerGroup.Default);
                return LayerGroup.Default;
            }
        }

        public T AddComponent<T>() where T : Component, new() => gameObject.AddComponent<T>();
        public T? GetComponent<T>() where T : Component=> gameObject.GetComponent<T>();
        public bool IsRegisteredComponent<T>() where T : Component => gameObject.IsRegisteredComponent<T>();
        public Component[] GetComponents<T>() where T : Component => gameObject.GetComponents();
        public T? GetComponentInParent<T>() where T : Component => gameObject.GetComponentInParent<T>();

        #region ChevyRay.Coroutinesのラッパー関数
        protected void AddCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle) =>
            ScriptController.AddCoroutine(routine, coroutineHandle);

        protected void UpdateCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle) =>
            ScriptController.UpdateCoroutine(routine, coroutineHandle);

        protected CoroutineHandle GetCoroutine(IEnumerator routine) =>
            ScriptController.GetCoroutine(routine);
        /// <summary>
        /// コルーチン実行
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        protected CoroutineHandle StartCoroutine(float delay, IEnumerator coroutine) {
            var handle = CoroutineRunner.Run(delay, coroutine);
            AddCoroutine(coroutine, handle);
            Debug.Log(coroutine);
            return handle;
        }

        /// <summary>
        /// コルーチン実行
        /// </summary>
        /// <param name="coroutine"></param>
        protected CoroutineHandle StartCoroutine(IEnumerator coroutine) =>
            StartCoroutine(0f, coroutine);

        protected void StopCoroutine(IEnumerator coroutine) {
            CoroutineRunner.Stop(coroutine);
        }
        protected void StopAll() {
            CoroutineRunner.StopAll();
        }

        protected bool IsRunning(CoroutineHandle coroutineHandle) =>
            CoroutineRunner.IsRunning(coroutineHandle);

        protected bool IsRunning(IEnumerator routine) =>
             CoroutineRunner.IsRunning(routine);

        protected int CoroutineCount => CoroutineRunner.Count;
        #endregion

        public static IEnumerator Empty() {
            yield return null;
        }

        protected IEnumerator WaitForSeconds(float second) {
            yield return CoroutineRunner.Run(second, Empty()).Wait();
        }
    }
}
