using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STG.Engine.Debugging {
    public partial class DebugWindowForm : Form {
        public List<ListViewItem> OriginItemCorection = new List<ListViewItem>();

        public enum DebugState {
            None,
            Filer,
        }

        public DebugState debugState = DebugState.None;

        public DebugWindowForm() {
            InitializeComponent();
            Init_ListView();
            Init_ListViewCopy();
            Debug.Init(this);
        }

        private void Init_ListView() {
            ColumnHeader[] columns = {
                new ColumnHeader() { Text = "Type", Width = 70, },
                new ColumnHeader() { Text = "Tag", Width = 50, },
                new ColumnHeader() { Text = "Message", Width = 200, },
                new ColumnHeader() { Text = "RelativePath", Width = 400, },
                new ColumnHeader() { Text = "MemberName", Width = 130, },
                new ColumnHeader() { Text = "Line", Width = 50, }
            };
            listView1.Columns.AddRange(columns);

        }

        private void Init_ListViewCopy() {
            listView1.MultiSelect = true;
            listView1.KeyDown += (sender, e) => {
                if (e.Control && e.KeyCode == Keys.C) {
                    if (listView1.SelectedItems.Count > 0) {
                        StringBuilder sb = new StringBuilder();
                        foreach (ListViewItem item in listView1.SelectedItems) {
                                sb.AppendLine(string.Join("\t", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(si => si.Text)));
                        }
                        Clipboard.SetText(sb.ToString());
                    }
                }
            };
        }

        private void FilterButton_Click(object sender, EventArgs e) {
            List<ListViewItem> foundListView = new List<ListViewItem>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Debug.Log($"{stopwatch.Elapsed}");

            if (textBox1.Text == "") {
                return;
            }
            int foundCount = 0;
            for (int i = 0; i < listView1.Items.Count; i++) {
                string str = listView1.Items[i].Text;
                for (int j = 0; j < listView1.Items[i].SubItems.Count; j++) {
                    str += $" {listView1.Items[i].SubItems[j].Text}";
                }

                Debug.Log($"{str}");

                if (str.Contains(textBox1.Text)) {
                    foundListView.Add(listView1.Items[i]);
                    foundCount++;
                }
            }

            if (foundCount == 0) {
                MessageBox.Show("Filtering Option:" + textBox1.Text + " was not found");
                return;
            }

            listView1.Items.Clear();
            listView1.Items.AddRange(foundListView.ToArray());

            Debug.Log($"{stopwatch.Elapsed}");
        }

        private void UndoButton_Click(object sender, EventArgs e) {
            listView1.Items.Clear();
            listView1.Items.AddRange(OriginItemCorection.ToArray());
        }

        private void ClearButton_Click(object sender, EventArgs e) {

        }
    }
}