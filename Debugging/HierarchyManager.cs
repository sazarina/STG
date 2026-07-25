using STG.Engine.Component;
using System.Collections;

namespace STG.Engine.Debugging {
    class HierarchyManager : Behavior {
        TreeView treeView => MainWindow.Instance.treeView1;

        public override void Start() {
            InitializeHierarchy();
            StartCoroutine(DebugLoop(5f));
        }

        IEnumerator DebugLoop(float delay) {
            while (true) {
                //InitializeHierarchy();
                yield return WaitForSeconds(delay);
            }
        }

        /// <summary>
        /// 階層構造の表示を更新するメソッド。GameObject.Root から始まり、すべての子オブジェクトをツリービューに追加します。
        /// </summary>
        public void InitializeHierarchy() {
            if (GameObject.Root == null) {
                Debug.LogException("HierarchyManager", new Exception("GameObject.Rootはnullです。"));
                return;
            }

            foreach (var transform in GameObject.Root.Children) { 
                var tree = treeView.Nodes.Add(transform.Guid.ToString(), transform.name);
                if (transform.Children.Count > 0) {
                    foreach (var child in transform.Children) {
                        tree.Nodes.Add(child.Guid.ToString(), child.name);
                    }
                }
            }
        }

        public override void Update() {

        }
    }
}
