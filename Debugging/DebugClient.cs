using STG.Engine.Component;

namespace STG.Engine.Debugging {
    public class DebugClient : GameObjectManager {
        MainWindow window = MainWindow.Instance;

        public override void Initialize(ScriptController scriptController) {
            base.Initialize(scriptController);

            Debug.isDebug = true;

            GameObject.Instantiate<HierarchyManager>(0, 0, "HierarchyManager");

            Debug.Log($"DebugClient.Initialize()"); ;
            OnInitialized += () => {
            };
        }

        public override void Update() {
            base.Update();

            foreach (var obj in GameObjects.Values) {
                if (obj.IsMouseCursorClicked) {
                    Debug.Log(obj.name);

                    window.SelectItem(obj);
                }
            }
        }
        public override void LateUpdate() {
            base.LateUpdate();
        }
    }
}
