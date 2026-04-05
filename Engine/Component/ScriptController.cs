using ChevyRay.Coroutines;
using STG.Engine.Debug;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public class ScriptController {
        public static ScriptController self;

        public CoroutineRunner coroutineRunner = new CoroutineRunner();

        public Dictionary<IEnumerator, CoroutineHandle> Coroutines { get; private set; } =
            new Dictionary<IEnumerator, CoroutineHandle>();

        public void AddCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle)
            => Coroutines.Add(routine, coroutineHandle);

        public void UpdateCoroutine(IEnumerator routine, CoroutineHandle coroutineHandle)
            => Coroutines[routine] = coroutineHandle;

        public CoroutineHandle GetCoroutine(IEnumerator routine) => Coroutines[routine];


        List<ScriptBase> ScriptList = new List<ScriptBase>();
        public void AddScript<T>(T t) where T : ScriptBase, new() {
            t.Initialize(this,null);
            t.Start();
            ScriptList.Add(t);
        }

        public void Initialize() {
            self = this;
            Debug.Debug.Log("----------");
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
