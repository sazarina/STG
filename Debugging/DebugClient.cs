using STG.Engine.Component;
using Xenon.Core;

namespace STG.Engine.Debugging {
    public class DebugClient:GameObjectManager {
        MainWindow window = MainWindow.self;
        TreeView treeView => window.treeView1;

        HierarchyManager hierarchyManager = new HierarchyManager();

        public DebugClient(ScriptController scriptController) : base(scriptController) {
            Debug.isDebug = true;

            //Debugging.Log(DebugClient.Instance().GetType());
        }

        public override void Initialize() {
            base.Initialize();

            var obj = GameObject.Instantiate(0,0, "HierarchyManager");
            hierarchyManager = obj.AddComponent<HierarchyManager>();
            hierarchyManager.Start(this);

            Debug.Log($"GameObjectManagerIstance: {Instance()}"); ;
        }

        public override void Update() {
            base.Update();

            GameObjects.Values.ForEach(obj => {
                if (obj.IsMouseCursorClicked) {
                    Debug.Log(obj.name);

                    window.SelectItem(obj);
                }
            });

            hierarchyManager.Update();
        }

        /// <summary>
        /// 式木の操作
        /// </summary>
        public void ShowHierarchy() {
            // ツリービューをクリア
            treeView.Nodes.Clear();

            Transform root = GetRoot().transform;
            
            foreach (var transform in root.Children.Values.//GameObjects.Values.
                Select(x => x.transform)) {

                var trees = treeView.Nodes.Find(transform.parentName, true);

                // 最初だけ実行されるはず
                if (trees.Length == 0) {
                    treeView.Nodes.Add(transform.Name, transform.Name);
                }
                // 親のノードが見つかった
                if (trees.Length > 0) {
                    var parentNode = trees[0];
                    parentNode.Nodes.Add(transform.Name, transform.Name);
                }
            }
        }

    }
}
