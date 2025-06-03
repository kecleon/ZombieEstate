namespace ZombieEstate2
{
	// Token: 0x02000043 RID: 67
	public partial class ErrorForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x0000B9F6 File Offset: 0x00009BF6
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000BA18 File Offset: 0x00009C18
		private void InitializeComponent()
		{
			this.mLabel = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.mLabel.Location = new global::System.Drawing.Point(113, 110);
			this.mLabel.Name = "mLabel";
			this.mLabel.Size = new global::System.Drawing.Size(470, 105);
			this.mLabel.TabIndex = 0;
			this.mLabel.Text = "x";
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label1.Location = new global::System.Drawing.Point(256, 22);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(185, 25);
			this.label1.TabIndex = 1;
			this.label1.Text = "An error occurred!";
			this.textBox1.Location = new global::System.Drawing.Point(116, 242);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new global::System.Drawing.Size(467, 213);
			this.textBox1.TabIndex = 2;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(697, 467);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.mLabel);
			base.Name = "ErrorForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ZE2 Error";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000108 RID: 264
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000109 RID: 265
		private global::System.Windows.Forms.Label mLabel;

		// Token: 0x0400010A RID: 266
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400010B RID: 267
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
