namespace dsdad
{
    partial class Form3
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.sfx = new System.Windows.Forms.CheckBox();
            this.remo = new System.Windows.Forms.CheckBox();
            this.map = new System.Windows.Forms.CheckBox();
            this.chr = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.sound = new System.Windows.Forms.CheckBox();
            this.shader = new System.Windows.Forms.CheckBox();
            this.parts = new System.Windows.Forms.CheckBox();
            this.other = new System.Windows.Forms.CheckBox();
            this.obj = new System.Windows.Forms.CheckBox();
            this.mtd = new System.Windows.Forms.CheckBox();
            this.menu = new System.Windows.Forms.CheckBox();
            this.font = new System.Windows.Forms.CheckBox();
            this.facegen = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.script = new System.Windows.Forms.CheckBox();
            this.dvdbndEvent = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.paramdef = new System.Windows.Forms.CheckBox();
            this.param = new System.Windows.Forms.CheckBox();
            this.msg = new System.Windows.Forms.CheckBox();
            this.allChB = new System.Windows.Forms.CheckBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.modifyBtn = new System.Windows.Forms.Button();
            this.debugChB = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dcxChB = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 80);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(219, 207);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sfx);
            this.tabPage1.Controls.Add(this.remo);
            this.tabPage1.Controls.Add(this.map);
            this.tabPage1.Controls.Add(this.chr);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(211, 178);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "dvdbnd0";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // sfx
            // 
            this.sfx.AutoSize = true;
            this.sfx.Location = new System.Drawing.Point(15, 96);
            this.sfx.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.sfx.Name = "sfx";
            this.sfx.Size = new System.Drawing.Size(47, 21);
            this.sfx.TabIndex = 4;
            this.sfx.Text = "sfx";
            this.sfx.UseVisualStyleBackColor = true;
            this.sfx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // remo
            // 
            this.remo.AutoSize = true;
            this.remo.Location = new System.Drawing.Point(15, 69);
            this.remo.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.remo.Name = "remo";
            this.remo.Size = new System.Drawing.Size(62, 21);
            this.remo.TabIndex = 3;
            this.remo.Text = "remo";
            this.remo.UseVisualStyleBackColor = true;
            this.remo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // map
            // 
            this.map.AutoSize = true;
            this.map.Location = new System.Drawing.Point(15, 42);
            this.map.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(57, 21);
            this.map.TabIndex = 2;
            this.map.Text = "map";
            this.map.UseVisualStyleBackColor = true;
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // chr
            // 
            this.chr.AutoSize = true;
            this.chr.Location = new System.Drawing.Point(15, 15);
            this.chr.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.chr.Name = "chr";
            this.chr.Size = new System.Drawing.Size(50, 21);
            this.chr.TabIndex = 1;
            this.chr.Text = "chr";
            this.chr.UseVisualStyleBackColor = true;
            this.chr.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.sound);
            this.tabPage2.Controls.Add(this.shader);
            this.tabPage2.Controls.Add(this.parts);
            this.tabPage2.Controls.Add(this.other);
            this.tabPage2.Controls.Add(this.obj);
            this.tabPage2.Controls.Add(this.mtd);
            this.tabPage2.Controls.Add(this.menu);
            this.tabPage2.Controls.Add(this.font);
            this.tabPage2.Controls.Add(this.facegen);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(211, 178);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "dvdbnd1";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // sound
            // 
            this.sound.AutoSize = true;
            this.sound.Location = new System.Drawing.Point(143, 96);
            this.sound.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.sound.Name = "sound";
            this.sound.Size = new System.Drawing.Size(69, 21);
            this.sound.TabIndex = 9;
            this.sound.Text = "sound";
            this.sound.UseVisualStyleBackColor = true;
            this.sound.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // shader
            // 
            this.shader.AutoSize = true;
            this.shader.Location = new System.Drawing.Point(143, 69);
            this.shader.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.shader.Name = "shader";
            this.shader.Size = new System.Drawing.Size(74, 21);
            this.shader.TabIndex = 8;
            this.shader.Text = "shader";
            this.shader.UseVisualStyleBackColor = true;
            this.shader.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // parts
            // 
            this.parts.AutoSize = true;
            this.parts.Location = new System.Drawing.Point(143, 42);
            this.parts.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.parts.Name = "parts";
            this.parts.Size = new System.Drawing.Size(62, 21);
            this.parts.TabIndex = 7;
            this.parts.Text = "parts";
            this.parts.UseVisualStyleBackColor = true;
            this.parts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // other
            // 
            this.other.AutoSize = true;
            this.other.Location = new System.Drawing.Point(143, 15);
            this.other.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.other.Name = "other";
            this.other.Size = new System.Drawing.Size(63, 21);
            this.other.TabIndex = 6;
            this.other.Text = "other";
            this.other.UseVisualStyleBackColor = true;
            this.other.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // obj
            // 
            this.obj.AutoSize = true;
            this.obj.Location = new System.Drawing.Point(15, 123);
            this.obj.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.obj.Name = "obj";
            this.obj.Size = new System.Drawing.Size(49, 21);
            this.obj.TabIndex = 5;
            this.obj.Text = "obj";
            this.obj.UseVisualStyleBackColor = true;
            this.obj.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // mtd
            // 
            this.mtd.AutoSize = true;
            this.mtd.Location = new System.Drawing.Point(15, 96);
            this.mtd.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.mtd.Name = "mtd";
            this.mtd.Size = new System.Drawing.Size(53, 21);
            this.mtd.TabIndex = 4;
            this.mtd.Text = "mtd";
            this.mtd.UseVisualStyleBackColor = true;
            this.mtd.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // menu
            // 
            this.menu.AutoSize = true;
            this.menu.Location = new System.Drawing.Point(15, 69);
            this.menu.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(65, 21);
            this.menu.TabIndex = 3;
            this.menu.Text = "menu";
            this.menu.UseVisualStyleBackColor = true;
            this.menu.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // font
            // 
            this.font.AutoSize = true;
            this.font.Location = new System.Drawing.Point(15, 42);
            this.font.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.font.Name = "font";
            this.font.Size = new System.Drawing.Size(54, 21);
            this.font.TabIndex = 2;
            this.font.Text = "font";
            this.font.UseVisualStyleBackColor = true;
            this.font.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // facegen
            // 
            this.facegen.AutoSize = true;
            this.facegen.Location = new System.Drawing.Point(15, 15);
            this.facegen.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.facegen.Name = "facegen";
            this.facegen.Size = new System.Drawing.Size(81, 21);
            this.facegen.TabIndex = 1;
            this.facegen.Text = "facegen";
            this.facegen.UseVisualStyleBackColor = true;
            this.facegen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.script);
            this.tabPage3.Controls.Add(this.dvdbndEvent);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(211, 178);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "dvdbnd2";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // script
            // 
            this.script.AutoSize = true;
            this.script.Location = new System.Drawing.Point(15, 42);
            this.script.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.script.Name = "script";
            this.script.Size = new System.Drawing.Size(64, 21);
            this.script.TabIndex = 1;
            this.script.Text = "script";
            this.script.UseVisualStyleBackColor = true;
            this.script.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // dvdbndEvent
            // 
            this.dvdbndEvent.AutoSize = true;
            this.dvdbndEvent.Location = new System.Drawing.Point(15, 15);
            this.dvdbndEvent.Margin = new System.Windows.Forms.Padding(15, 15, 3, 3);
            this.dvdbndEvent.Name = "dvdbndEvent";
            this.dvdbndEvent.Size = new System.Drawing.Size(65, 21);
            this.dvdbndEvent.TabIndex = 0;
            this.dvdbndEvent.Text = "event";
            this.dvdbndEvent.UseVisualStyleBackColor = true;
            this.dvdbndEvent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.paramdef);
            this.tabPage4.Controls.Add(this.param);
            this.tabPage4.Controls.Add(this.msg);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(211, 178);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "dvdbnd3";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // paramdef
            // 
            this.paramdef.AutoSize = true;
            this.paramdef.Location = new System.Drawing.Point(15, 69);
            this.paramdef.Margin = new System.Windows.Forms.Padding(15, 15, 3, 3);
            this.paramdef.Name = "paramdef";
            this.paramdef.Size = new System.Drawing.Size(90, 21);
            this.paramdef.TabIndex = 2;
            this.paramdef.Text = "paramdef";
            this.paramdef.UseVisualStyleBackColor = true;
            this.paramdef.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // param
            // 
            this.param.AutoSize = true;
            this.param.Location = new System.Drawing.Point(15, 42);
            this.param.Margin = new System.Windows.Forms.Padding(15, 15, 3, 3);
            this.param.Name = "param";
            this.param.Size = new System.Drawing.Size(70, 21);
            this.param.TabIndex = 1;
            this.param.Text = "param";
            this.param.UseVisualStyleBackColor = true;
            this.param.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // msg
            // 
            this.msg.AutoSize = true;
            this.msg.Location = new System.Drawing.Point(15, 15);
            this.msg.Margin = new System.Windows.Forms.Padding(15, 15, 3, 3);
            this.msg.Name = "msg";
            this.msg.Size = new System.Drawing.Size(56, 21);
            this.msg.TabIndex = 0;
            this.msg.Text = "msg";
            this.msg.UseVisualStyleBackColor = true;
            this.msg.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uncheckAll);
            // 
            // allChB
            // 
            this.allChB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.allChB.AutoSize = true;
            this.allChB.Location = new System.Drawing.Point(31, 293);
            this.allChB.Name = "allChB";
            this.allChB.Size = new System.Drawing.Size(45, 21);
            this.allChB.TabIndex = 1;
            this.allChB.Text = "All";
            this.allChB.UseVisualStyleBackColor = true;
            this.allChB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.allCheck);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(156, 293);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // modifyBtn
            // 
            this.modifyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modifyBtn.Location = new System.Drawing.Point(75, 293);
            this.modifyBtn.Name = "modifyBtn";
            this.modifyBtn.Size = new System.Drawing.Size(75, 23);
            this.modifyBtn.TabIndex = 3;
            this.modifyBtn.Text = "Modify";
            this.modifyBtn.UseVisualStyleBackColor = true;
            this.modifyBtn.Click += new System.EventHandler(this.modifyBtn_Click);
            // 
            // debugChB
            // 
            this.debugChB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.debugChB.AutoSize = true;
            this.debugChB.Enabled = false;
            this.debugChB.Location = new System.Drawing.Point(109, 53);
            this.debugChB.Name = "debugChB";
            this.debugChB.Size = new System.Drawing.Size(122, 21);
            this.debugChB.TabIndex = 4;
            this.debugChB.Text = "Debug version";
            this.debugChB.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 35);
            this.panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 35);
            this.label1.TabIndex = 5;
            this.label1.Text = "Checked directories will be loaded from the unpacked libraries.";
            // 
            // dcxChB
            // 
            this.dcxChB.AutoSize = true;
            this.dcxChB.Location = new System.Drawing.Point(16, 53);
            this.dcxChB.Name = "dcxChB";
            this.dcxChB.Size = new System.Drawing.Size(113, 21);
            this.dcxChB.TabIndex = 7;
            this.dcxChB.Text = "DCX enabled";
            this.dcxChB.UseVisualStyleBackColor = true;
            this.dcxChB.CheckedChanged += new System.EventHandler(this.dcxChB_CheckedChanged);
            // 
            // Form3
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(243, 330);
            this.Controls.Add(this.dcxChB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.debugChB);
            this.Controls.Add(this.modifyBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.allChB);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modify EXE";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox sfx;
        private System.Windows.Forms.CheckBox remo;
        private System.Windows.Forms.CheckBox map;
        private System.Windows.Forms.CheckBox chr;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox sound;
        private System.Windows.Forms.CheckBox shader;
        private System.Windows.Forms.CheckBox parts;
        private System.Windows.Forms.CheckBox other;
        private System.Windows.Forms.CheckBox obj;
        private System.Windows.Forms.CheckBox mtd;
        private System.Windows.Forms.CheckBox menu;
        private System.Windows.Forms.CheckBox font;
        private System.Windows.Forms.CheckBox facegen;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox script;
        private System.Windows.Forms.CheckBox dvdbndEvent;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox paramdef;
        private System.Windows.Forms.CheckBox param;
        private System.Windows.Forms.CheckBox msg;
        private System.Windows.Forms.CheckBox allChB;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button modifyBtn;
        private System.Windows.Forms.CheckBox debugChB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox dcxChB;
    }
}