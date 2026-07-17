using ChevyRay.Coroutines;
using Microsoft.Xna.Framework;
using STG.Engine.Debugging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public class ScriptController {
        #region シングルトン
        static ScriptController instance = null;

        ScriptController() {
            Debug.Log("ScriptController.ctor()");
        }

        internal static ScriptController Instance {
            get {
                if (instance == null) {
                    instance = new ScriptController();
                }
                return instance;
            }
        }

        #endregion

        public CoroutineRunner coroutineRunner = new CoroutineRunner();

        public Dictionary<IEnumerator, CoroutineHandle> Coroutines { get; private set; } =
            new Dictionary<IEnumerator, CoroutineHandle>();

        public void AddCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle)
            => Coroutines.Add(routine, coroutineHandle);

        public void UpdateCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle)
            => Coroutines[routine] = coroutineHandle;

        public CoroutineHandle GetCoroutine(IEnumerator routine) => Coroutines[routine];


        List<Behavior> ScriptList = new List<Behavior>();
        Queue<Behavior> AddScriptQueue = new Queue<Behavior>();

        /// <summary>
        /// 指定された型のスクリプトをアタッチします。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Component AttachScript(Type type, GameObject gameObject) {
            // ジェネリックにしない理由:
            // コンパイラは呼び出し側で `T` を宣言された制約（ここでは `Component`）としてしか扱えないため、
            // `Behavior` 固有のメンバーを直接呼ぶとコンパイルエラーになるので、
            // そのため `Activator.CreateInstance` で生成して `Behavior` にキャストしている
            var script = (Behavior?)Activator.CreateInstance(type);
            if (script == null) {
                throw new InvalidOperationException($"Failed to create instance of type {type.FullName}");
            }

            script.Initialize(gameObject);

            //ScriptControllerのLateUpdateで遅延してStartを呼び出すようにする
            Register(script);
            gameObject.AttachedScripts.Add(type, script);
            return script;
        }

        internal static void Register(Behavior Script) { 
            Instance.AddScriptQueue.Enqueue(Script);
        }

        public void Initialize() {
            Debug.Log("ScriptController.Initialize()");
        }

        public void Update(GameTime gameTime) {
            coroutineRunner.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (Behavior script in ScriptList) {
                if (script.isActive) {
                    script.Update();
                }
            }
        }

        public void LateUpdate() {
            while (AddScriptQueue.Count > 0) {
                var script = AddScriptQueue.Dequeue();
                script.isActive = true;
                script.Start();

                ScriptList.Add(script);
            }
        }
    }
}
