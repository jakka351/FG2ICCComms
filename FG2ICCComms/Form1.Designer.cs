namespace FG2ICCComms
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.comboBox1 = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.comboBox2 = new global::System.Windows.Forms.ComboBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.button2 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button4 = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.comboBox1.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new global::System.Drawing.Point(45, 5);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new global::System.Drawing.Size(103, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedIndexChanged += new global::System.EventHandler(this.comboBox1_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(31, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "COM";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(154, 8);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(37, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "BAUD";
			this.comboBox2.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[]
			{
				"4800",
				"9600",
				"19200",
				"38400",
				"57600",
				"115200",
				"230400",
				"250000",
				"1000000",
				"2000000"
			});
			this.comboBox2.Location = new global::System.Drawing.Point(197, 5);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new global::System.Drawing.Size(87, 21);
			this.comboBox2.TabIndex = 3;
			this.comboBox2.SelectedIndexChanged += new global::System.EventHandler(this.comboBox2_SelectedIndexChanged);
			this.button1.Enabled = false;
			this.button1.Location = new global::System.Drawing.Point(290, 5);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(69, 21);
			this.button1.TabIndex = 4;
			this.button1.Text = "Connect";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.textBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.textBox1.Location = new global::System.Drawing.Point(1, 57);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(419, 132);
			this.textBox1.TabIndex = 6;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new global::System.Drawing.Point(365, 8);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new global::System.Drawing.Size(49, 17);
			this.checkBox1.TabIndex = 8;
			this.checkBox1.Text = "DTR";
			this.toolTip1.SetToolTip(this.checkBox1, "Enables the DTR signal in COM port accesses, this resets many Arduino boards, but it provides a more reliable connection method.");
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.Visible = false;
			this.checkBox1.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			this.timer2.Interval = 5;
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			this.button2.Enabled = false;
			this.button2.Location = new global::System.Drawing.Point(79, 32);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(69, 21);
			this.button2.TabIndex = 9;
			this.button2.Text = "Recore";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button3.Enabled = false;
			this.button3.Location = new global::System.Drawing.Point(233, 32);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(69, 21);
			this.button3.TabIndex = 11;
			this.button3.Text = "Reset";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button4.Enabled = false;
			this.button4.Location = new global::System.Drawing.Point(157, 32);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(69, 21);
			this.button4.TabIndex = 10;
			this.button4.Text = "ZAP E610";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(421, 189);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.checkBox1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.comboBox2);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.comboBox1);
			base.Name = "Form1";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "FG2 ICC Comms";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			base.Resize += new global::System.EventHandler(this.Form1_Resize);
			base.LocationChanged += new global::System.EventHandler(this.Form1_LocationChanged);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000002 RID: 2
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000003 RID: 3
		private global::System.Windows.Forms.ComboBox comboBox1;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.ComboBox comboBox2;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.Timer timer2;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.Button button4;
	}
}
