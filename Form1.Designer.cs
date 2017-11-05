namespace mbxScan2
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			this.cmdDisarm = new System.Windows.Forms.Button();
			this.cmdAttach = new System.Windows.Forms.Button();
			this.txtDebug = new System.Windows.Forms.TextBox();
			this.tmrCheckLogin = new System.Windows.Forms.Timer(this.components);
			this.cmdStop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdDisarm
			// 
			this.cmdDisarm.Location = new System.Drawing.Point(93, 12);
			this.cmdDisarm.Name = "cmdDisarm";
			this.cmdDisarm.Size = new System.Drawing.Size(75, 23);
			this.cmdDisarm.TabIndex = 0;
			this.cmdDisarm.Text = "Disarm";
			this.cmdDisarm.UseVisualStyleBackColor = true;
			this.cmdDisarm.Click += new System.EventHandler(this.cmdDisarm_Click);
			// 
			// cmdAttach
			// 
			this.cmdAttach.Location = new System.Drawing.Point(12, 12);
			this.cmdAttach.Name = "cmdAttach";
			this.cmdAttach.Size = new System.Drawing.Size(75, 23);
			this.cmdAttach.TabIndex = 1;
			this.cmdAttach.Text = "Attach";
			this.cmdAttach.UseVisualStyleBackColor = true;
			this.cmdAttach.Click += new System.EventHandler(this.cmdAttach_Click);
			// 
			// txtDebug
			// 
			this.txtDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.txtDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDebug.Location = new System.Drawing.Point(12, 117);
			this.txtDebug.Multiline = true;
			this.txtDebug.Name = "txtDebug";
			this.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDebug.Size = new System.Drawing.Size(268, 144);
			this.txtDebug.TabIndex = 2;
			this.txtDebug.TextChanged += new System.EventHandler(this.txtDebug_TextChanged);
			// 
			// tmrCheckLogin
			// 
			this.tmrCheckLogin.Tick += new System.EventHandler(this.tmrCheckLogin_Tick);
			// 
			// cmdStop
			// 
			this.cmdStop.Location = new System.Drawing.Point(175, 12);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(75, 23);
			this.cmdStop.TabIndex = 3;
			this.cmdStop.Text = "Stop";
			this.cmdStop.UseVisualStyleBackColor = true;
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.cmdStop);
			this.Controls.Add(this.txtDebug);
			this.Controls.Add(this.cmdAttach);
			this.Controls.Add(this.cmdDisarm);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdDisarm;
		private System.Windows.Forms.Button cmdAttach;
		private System.Windows.Forms.TextBox txtDebug;
		private System.Windows.Forms.Timer tmrCheckLogin;
		private System.Windows.Forms.Button cmdStop;
	}
}

