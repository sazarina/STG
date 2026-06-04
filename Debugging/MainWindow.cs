using STG.Engine.Component;

namespace STG.Engine.Debugging {
    public partial class MainWindow : Form {
        public static MainWindow self;

        DebugWindowForm debugWindow = new DebugWindowForm();

        public MainWindow() {
            self = this;

            InitializeComponent();
            debugWindow.Show();
            //Console.SetOut(new DebugTextWritter());
            Console.WriteLine("Game() Started");

            ColumnHeader[] Columns = new ColumnHeader[]{
                 new ColumnHeader() {Text = "GameObject",Width = 200 },
                  new ColumnHeader() {Text = "Position",Width = 200 },

            };

            //listView1.Columns.AddRange(Columns);
        }

        public void SelectItem(GameObject gameObject) {
            listBox1.Items.Clear();
            listBox1.Items.Add(gameObject.name);
        }

        void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender is ListBox listBox) {
                var name = listBox.SelectedItem as string;

                var gameObject = GameObject.Find(name);
                if (gameObject != null) {
                    textBox1.Text = $"position:{gameObject.transform.position}\n" +
                    $"localPosition:{gameObject.transform.localPosition}\n" +
                    $"parent:{gameObject.transform.parentName}\n";
                } else {
                    throw new Exception("GameObjectが見つかりませんでした");
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            if(e.Node == null) return;

            var gameObject = GameObject.Find(e.Node.Text);
            if (gameObject != null) {
                SelectItem(gameObject);
            }
        }
    }
}




