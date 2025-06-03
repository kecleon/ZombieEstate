using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ZombieEstate2
{
	// Token: 0x02000043 RID: 67
	public partial class ErrorForm : Form
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x0000B993 File Offset: 0x00009B93
		public ErrorForm()
		{
			this.InitializeComponent();
			this.mLabel.Text = "An error occurred! Please help us fix these bugs! Copy and paste the below text to the Zombie Estate 2 Steam discussions page, or email us at support@zombieestate.com to help us out!";
			Label label = this.mLabel;
			label.Text += " Try to provide a description of what you were doing when it crashed. Thanks for your support!";
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B9CC File Offset: 0x00009BCC
		public void Init(string s, string path)
		{
			Label label = this.mLabel;
			label.Text = label.Text + "\n\nThis log has been also written to disk in: \n" + path;
			this.textBox1.Text = s;
		}
	}
}
