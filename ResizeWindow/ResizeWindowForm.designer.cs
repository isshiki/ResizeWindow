namespace ResizeWindow
{
    partial class MainWindow
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Forms Desiner's code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnResize = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxSelectedWindow = new System.Windows.Forms.ListBox();
            this.timerReload = new System.Windows.Forms.Timer(this.components);
            this.GroupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.txtHeight);
            this.GroupBox2.Controls.Add(this.Label2);
            this.GroupBox2.Controls.Add(this.txtWidth);
            this.GroupBox2.Controls.Add(this.Label1);
            this.GroupBox2.Location = new System.Drawing.Point(13, 340);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(370, 49);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "&Size";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(186, 19);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(70, 19);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.Text = "443";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(137, 22);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(40, 12);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "&Height:";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(53, 18);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(70, 19);
            this.txtWidth.TabIndex = 1;
            this.txtWidth.Text = "590";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(11, 22);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 12);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "&Width:";
            // 
            // btnResize
            // 
            this.btnResize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResize.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnResize.Location = new System.Drawing.Point(13, 395);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(508, 55);
            this.btnResize.TabIndex = 3;
            this.btnResize.Text = "&Resize";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listBoxSelectedWindow);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 322);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select a target &window to resize";
            // 
            // listBoxSelectedWindow
            // 
            this.listBoxSelectedWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSelectedWindow.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBoxSelectedWindow.FormattingEnabled = true;
            this.listBoxSelectedWindow.ItemHeight = 12;
            this.listBoxSelectedWindow.Location = new System.Drawing.Point(11, 18);
            this.listBoxSelectedWindow.Name = "listBoxSelectedWindow";
            this.listBoxSelectedWindow.ScrollAlwaysVisible = true;
            this.listBoxSelectedWindow.Size = new System.Drawing.Size(492, 292);
            this.listBoxSelectedWindow.Sorted = true;
            this.listBoxSelectedWindow.TabIndex = 0;
            this.listBoxSelectedWindow.SelectedIndexChanged += new System.EventHandler(this.listBoxSelectedWindow_SelectedIndexChanged);
            // 
            // timerReload
            // 
            this.timerReload.Enabled = true;
            this.timerReload.Interval = 1000;
            this.timerReload.Tick += new System.EventHandler(this.timerReload_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 461);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.btnResize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.Text = "Application\'s Window Resizer";
            this.TopMost = true;
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.TextBox txtHeight;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtWidth;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnResize;
        internal System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxSelectedWindow;
        private System.Windows.Forms.Timer timerReload;
    }
}

