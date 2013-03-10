namespace ConsoleLauncher
{
    partial class ServicePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicePanel));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKill = new System.Windows.Forms.Button();
            this.btnSendCtrlC = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnKill);
            this.panel1.Controls.Add(this.btnSendCtrlC);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 208);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(749, 34);
            this.panel1.TabIndex = 0;
            // 
            // btnKill
            // 
            this.btnKill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKill.ImageIndex = 1;
            this.btnKill.Location = new System.Drawing.Point(651, 6);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(94, 24);
            this.btnKill.TabIndex = 2;
            this.btnKill.Text = "Kill duplicates";
            this.btnKill.UseVisualStyleBackColor = true;
            // 
            // btnSendCtrlC
            // 
            this.btnSendCtrlC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendCtrlC.ImageIndex = 1;
            this.btnSendCtrlC.Location = new System.Drawing.Point(589, 6);
            this.btnSendCtrlC.Name = "btnSendCtrlC";
            this.btnSendCtrlC.Size = new System.Drawing.Size(57, 24);
            this.btnSendCtrlC.TabIndex = 1;
            this.btnSendCtrlC.Text = "Ctrl-C";
            this.btnSendCtrlC.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.ImageIndex = 0;
            this.btnStart.ImageList = this.imageList1;
            this.btnStart.Location = new System.Drawing.Point(3, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(36, 24);
            this.btnStart.TabIndex = 0;
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "play.png");
            this.imageList1.Images.SetKeyName(1, "stop.png");
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(749, 208);
            this.textBox1.TabIndex = 2;
            this.textBox1.WordWrap = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(45, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // ServicePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Name = "ServicePanel";
            this.Size = new System.Drawing.Size(749, 242);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button btnSendCtrlC;
        public System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.RichTextBox textBox1;
        public System.Windows.Forms.Button btnKill;
        public System.Windows.Forms.Button btnClear;
    }
}
