using STG.Engine.Component;
using System.Windows.Forms;

namespace STG.Engine.Debugging {
    class TransformTreeNode : TreeNode {
        public Transform transform;
        //List<Transform> children = new List<Transform>();

        public TransformTreeNode(Transform transform) {
            this.transform = transform;
            Text = transform.Name;
            if (transform.Parent != null) {
                Text += "," + transform.Parent.Name;
            }
        }
    }

}
