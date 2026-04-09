using ChevyRay.Coroutines;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    public class ScriptController {
        #region シングルトン
        static ScriptController self = null;

        ScriptController() {
            Debug.Log("ScriptController.ctor()");
        }

        public static ScriptController Instance() {
            if (self == null) {
                self = new ScriptController();
            }
            return self;
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
        public void AddScript<T>(T t) where T : Behavior, new() {
            t.Initialize(this,null);
            t.Start();
            ScriptList.Add(t);
        }

        public void Initialize() {
            Debug.Log("ScriptContoller.Initalize()");
        }
        //どうしようかなアタッチするってことはScriptController_Updateメソッドでは実行しないように変更しようかな
        public void Update(GameTime gameTime) {
            coroutineRunner.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            //ScriptList.ForEach(script => script.Update());
        }

        public void Draw() {
            //ScriptList.ForEach(script => script.Draw());
        }
    }
}
