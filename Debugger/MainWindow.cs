using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using STG.Engine;
using STG.Engine.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xenon.Core;
using STG.Engine.Debug;

namespace STG.Engine.Debug {
    // System.Drawing および XNA Framework は、共に Color 型を定義します。
    // 競合を回避するため、両方のショートカット名を定義します。
    using GdiColor = System.Drawing.Color;
    using XnaColor = Microsoft.Xna.Framework.Color;

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




