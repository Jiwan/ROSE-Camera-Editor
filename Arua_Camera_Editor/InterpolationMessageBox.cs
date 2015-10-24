using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Arua_Camera_Editor
{
	public class InterpolationMessageBox : Form
	{
		private IContainer components = null;

		private Label label1;

		private NumericUpDown numericUpDownMinFrame;

		private NumericUpDown numericUpDownMaxFrame;

		private Label label2;

		private Label label3;

		private Button buttonApply;

		private Button buttonCancel;

		private NumericUpDown numericUpDownStep;

		public InterpolationMessageBox(int min, int max)
		{
			this.InitializeComponent();
			this.numericUpDownMinFrame.Minimum = min;
			this.numericUpDownMinFrame.Maximum = max;
			this.numericUpDownMaxFrame.Minimum = min;
			this.numericUpDownMaxFrame.Maximum = max;
		}

		public int GetSpeed()
		{
			return (int)this.numericUpDownStep.Value;
		}

		public int GetMinFrameIndex()
		{
			return (int)this.numericUpDownMinFrame.Value;
		}

		public int GetMaxFrameIndex()
		{
			return (int)this.numericUpDownMaxFrame.Value;
		}

		private void buttonApply_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
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
			this.label1 = new Label();
			this.numericUpDownMinFrame = new NumericUpDown();
			this.numericUpDownMaxFrame = new NumericUpDown();
			this.label2 = new Label();
			this.label3 = new Label();
			this.buttonApply = new Button();
			this.buttonCancel = new Button();
			this.numericUpDownStep = new NumericUpDown();
			((ISupportInitialize)this.numericUpDownMinFrame).BeginInit();
			((ISupportInitialize)this.numericUpDownMaxFrame).BeginInit();
			((ISupportInitialize)this.numericUpDownStep).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(151, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Apply interpolation from frame :";
			this.numericUpDownMinFrame.Location = new Point(12, 39);
			this.numericUpDownMinFrame.Name = "numericUpDownMinFrame";
			this.numericUpDownMinFrame.Size = new Size(120, 20);
			this.numericUpDownMinFrame.TabIndex = 2;
			this.numericUpDownMaxFrame.Location = new Point(167, 39);
			this.numericUpDownMaxFrame.Name = "numericUpDownMaxFrame";
			this.numericUpDownMaxFrame.Size = new Size(120, 20);
			this.numericUpDownMaxFrame.TabIndex = 3;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(138, 41);
			this.label2.Name = "label2";
			this.label2.Size = new Size(16, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "to";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(9, 88);
			this.label3.Name = "label3";
			this.label3.Size = new Size(38, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Step : ";
			this.buttonApply.Location = new Point(12, 132);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new Size(95, 26);
			this.buttonApply.TabIndex = 7;
			this.buttonApply.Text = "Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new EventHandler(this.buttonApply_Click);
			this.buttonCancel.Location = new Point(192, 132);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(95, 26);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.numericUpDownStep.Location = new Point(43, 86);
			this.numericUpDownStep.Name = "numericUpDownStep";
			this.numericUpDownStep.Size = new Size(120, 20);
			this.numericUpDownStep.TabIndex = 9;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(299, 170);
			base.ControlBox = false;
			base.Controls.Add(this.numericUpDownStep);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonApply);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.numericUpDownMaxFrame);
			base.Controls.Add(this.numericUpDownMinFrame);
			base.Controls.Add(this.label1);
			base.Name = "InterpolationMessageBox";
			this.Text = "Interpolation";
			((ISupportInitialize)this.numericUpDownMinFrame).EndInit();
			((ISupportInitialize)this.numericUpDownMaxFrame).EndInit();
			((ISupportInitialize)this.numericUpDownStep).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
