using STG.Engine;
using STG.Engine.Component;
using Microsoft.Xna.Framework.Input;
using STG.Engine.Debug;
using STG.Engine.Graphics;
using System.Collections;
using System.Linq;
using Xenon.Core;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace STG.Engine.Debug {
    class HierarchyManager : ScriptBase {
        DebugClient clientManager;

        public override void Draw() {

        }

        public void Start(DebugClient clientManager) {
            this.clientManager = clientManager;
        }

        public override void Update() {
            if (KeyInput.JustPressed(Keys.Enter)) {
                StartCoroutine(0f, Show());
                Debug.Log("");
            }

        }

        IEnumerator Show() {
            yield return WaitForSecond(0.5f);
            clientManager.ShowHierarchy();

        }
    }
}
