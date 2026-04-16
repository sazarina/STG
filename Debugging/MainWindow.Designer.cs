namespace STG.Engine.Debugging
{
    partial class MainWindow
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            panel1 = new Panel();
            treeView1 = new TreeView();
            textBox1 = new TextBox();
            listBox1 = new ListBox();
            game1 = new GameControl();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(treeView1);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(15, 20);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(564, 893);
            panel1.TabIndex = 1;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(4, 218);
            treeView1.Margin = new Padding(4, 5, 4, 5);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(555, 384);
            treeView1.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(4, 172);
            textBox1.Margin = new Padding(4, 5, 4, 5);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(124, 31);
            textBox1.TabIndex = 0;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(19, 25);
            listBox1.Margin = new Padding(4, 5, 4, 5);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(555, 154);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // game1
            // 
            game1.Location = new Point(582, 15);
            game1.Margin = new Padding(0);
            game1.MouseHoverUpdatesOnly = false;
            game1.Name = "game1";
            game1.Size = new Size(1200, 800);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            game1.TabIndex = 0;
            game1.Text = "game1";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1330, 933);
            Controls.Add(listBox1);
            Controls.Add(panel1);
            Controls.Add(game1);
            Margin = new Padding(5, 7, 5, 7);
            Name = "MainWindow";
            Text = "Client";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }



        #endregion

        private GameControl game1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        internal System.Windows.Forms.TreeView treeView1;
    }
}

