using ChevyRay.Coroutines;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    /// <summary>
    /// スクリプトの基底クラス
    /// </summary>
    public abstract class ScriptBase {
        internal GameObject gameObject { get; private set; }

        public Transform transform => gameObject.transform;

        /// <summary>
        /// スクリプトを管理するクラス
        /// </summary>
        ScriptController scriptController;
        /// <summary>
        /// ChevyRay.Coroutinesのコルーチンを実行するクラス
        /// </summary>
        CoroutineRunner coroutineRunner;

        public void Initialize(ScriptController scriptController,GameObject gameObject) {
            this.scriptController = scriptController;
            this.gameObject = gameObject;
            coroutineRunner = scriptController.coroutineRunner;
        }

        public virtual void Start() { }

        public virtual void Update() { }

        public virtual void Draw() { }

        protected Texture2D LoadTexture(string path,string name) => GraphicsUltis.CreateTexture(path,name);
    
        protected void AddCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle) =>
            scriptController.AddCoroutine(routine, coroutineHandle);

        protected void UpdateCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle) =>
            scriptController.UpdateCoroutine(routine, coroutineHandle);

        protected CoroutineHandle GetCoroutine(IEnumerator routine) =>
            scriptController.GetCoroutine(routine);

        #region ChevyRay.Coroutinesのラッパー関数
        /// <summary>
        /// コルーチン実行
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        protected CoroutineHandle StartCoroutine(float delay, IEnumerator coroutine) {
            var handle = coroutineRunner.Run(delay, coroutine);
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
            coroutineRunner.Stop(coroutine);
        }
        protected void StopAll() {
            coroutineRunner.StopAll();
        }

        protected bool IsRunning(CoroutineHandle coroutineHandle) =>
            coroutineRunner.IsRunning(coroutineHandle);

        protected bool IsRunning(IEnumerator routine) =>
             coroutineRunner.IsRunning(routine);

        protected int CoroutineCount => coroutineRunner.Count;
        #endregion

        public static IEnumerator Empty() {
            yield return null;
        }

        protected IEnumerator WaitForSecond(float second) {
            yield return coroutineRunner.Run(second, Empty()).Wait();
        }
    }
}
