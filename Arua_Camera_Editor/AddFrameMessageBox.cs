using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Arua_Camera_Editor
{
	public class AddFrameMessageBox : Form
	{
		private IContainer components = null;

		private Button buttonOK;

		private Button buttonCancel;

		private RadioButton radioButtonAfter;

		private RadioButton radioButtonBefore;

		public int GetFramePosition()
		{
			return this.radioButtonAfter.Checked ? 1 : 0;
		}

		public AddFrameMessageBox()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.radioButtonAfter = new RadioButton();
			this.radioButtonBefore = new RadioButton();
			base.SuspendLayout();
			this.buttonOK.Location = new Point(27, 67);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "Ok";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.button1_Click);
			this.buttonCancel.Location = new Point(108, 67);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.button2_Click);
			this.radioButtonAfter.AutoSize = true;
			this.radioButtonAfter.Checked = true;
			this.radioButtonAfter.Location = new Point(12, 23);
			this.radioButtonAfter.Name = "radioButtonAfter";
			this.radioButtonAfter.Size = new Size(90, 17);
			this.radioButtonAfter.TabIndex = 2;
			this.radioButtonAfter.TabStop = true;
			this.radioButtonAfter.Text = "After selected";
			this.radioButtonAfter.UseVisualStyleBackColor = true;
			this.radioButtonBefore.AutoSize = true;
			this.radioButtonBefore.Location = new Point(108, 23);
			this.radioButtonBefore.Name = "radioButtonBefore";
			this.radioButtonBefore.Size = new Size(101, 17);
			this.radioButtonBefore.TabIndex = 3;
			this.radioButtonBefore.TabStop = true;
			this.radioButtonBefore.Text = "Before Selected";
			this.radioButtonBefore.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(212, 102);
			base.ControlBox = false;
			base.Controls.Add(this.radioButtonBefore);
			base.Controls.Add(this.radioButtonAfter);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Name = "AddFrameMessageBox";
			this.Text = "Add Frame :";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
