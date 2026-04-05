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
            listBox1.SelectedIndex = 0;
        }

        public GameObject GetSelectedGameObject() {
            var name = listBox1.SelectedItem as string;

            GameObject gameObject = DebugClient.Instance().FindWithName(name);
            Debug.Log($"GameObjectManagerIstance: {DebugClient.Instance()}"); ;
            textBox1.Text = $"{gameObject.transform.position}";
            return gameObject;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            GameObject gameObject = GetSelectedGameObject();



        }
    }
}




