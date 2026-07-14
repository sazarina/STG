using STG.Engine.Component;
using System.Collections;

namespace STG.Engine.Debugging {
    class HierarchyManager : Behavior {
        TreeView treeView => MainWindow.Instance.treeView1;

        public override void Start() {
            StartCoroutine(DebugLoop(5f));
        }

        IEnumerator DebugLoop(float delay) {
            while (true) {
                ShowHierarchy();
                yield return WaitForSeconds(delay);
            }
        }

        /// <summary>
        /// 階層構造の表示を更新するメソッド。GameObject.Root から始まり、すべての子オブジェクトをツリービューに追加します。
        /// </summary>
        public void ShowHierarchy() {
            // ツリービューをクリア
            treeView.Nodes.Clear();

            if (GameObject.Root == null) {
                Debug.LogException("HierarchyManager", new Exception("GameObject.Rootはnullです。"));
                return;
            }

            foreach (var transform in GameObject.Root.Children.Values.
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

        public override void Update() {

        }
    }
}
