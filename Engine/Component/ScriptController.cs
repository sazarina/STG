using ChevyRay.Coroutines;
using Microsoft.Xna.Framework;
using STG.Engine.Debugging;
using System.Collections;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public class ScriptController {
        #region シングルトン
        static ScriptController self = null;

        ScriptController() {
            Debug.Log("ScriptController.ctor()");
        }

        public static ScriptController Instance {
            get {
                if (self == null) {
                    self = new ScriptController();
                }
                return self;
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

        public void AddScript<T>(T t) where T : Behavior, new() {
            t.Initialize(this,null);
            t.Start();
            ScriptList.Add(t);
        }

        internal static void Register(Behavior Script) { 
            Instance.AddScriptQueue.Enqueue(Script);
        }

        public void Initialize() {
            Debug.Log("ScriptController.Initialize()");
        }
        //どうしようかなアタッチするってことはScriptController_Updateメソッドでは実行しないように変更しようかな
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
                script.Start();
                ScriptList.Add(script);
            }
        }
    }
}
