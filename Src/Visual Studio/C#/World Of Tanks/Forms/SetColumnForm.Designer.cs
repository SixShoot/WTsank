
namespace WorldOfTanks {
	partial class SetColumnForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent () {
			this.ListView = new System.Windows.Forms.ListView();
			this.LabelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ContentColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ConfirmButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ListView
			// 
			this.ListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.ListView.CheckBoxes = true;
			this.ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LabelColumnHeader,
            this.ContentColumnHeader});
			this.ListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ListView.FullRowSelect = true;
			this.ListView.HideSelection = false;
			this.ListView.Location = new System.Drawing.Point(5, 5);
			this.ListView.Name = "ListView";
			this.ListView.Size = new System.Drawing.Size(312, 383);
			this.ListView.TabIndex = 27;
			this.ListView.UseCompatibleStateImageBehavior = false;
			this.ListView.View = System.Windows.Forms.View.Details;
			// 
			// LabelColumnHeader
			// 
			this.LabelColumnHeader.Text = "可见";
			this.LabelColumnHeader.Width = 160;
			// 
			// ContentColumnHeader
			// 
			this.ContentColumnHeader.Text = "列";
			this.ContentColumnHeader.Width = 123;
			// 
			// ConfirmButton
			// 
			this.ConfirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ConfirmButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ConfirmButton.Location = new System.Drawing.Point(241, 392);
			this.ConfirmButton.Name = "ConfirmButton";
			this.ConfirmButton.Size = new System.Drawing.Size(75, 25);
			this.ConfirmButton.TabIndex = 28;
			this.ConfirmButton.Text = "确定";
			this.ConfirmButton.UseVisualStyleBackColor = true;
			this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
			// 
			// SetColumnForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(322, 422);
			this.Controls.Add(this.ConfirmButton);
			this.Controls.Add(this.ListView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "SetColumnForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "设置可见列";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView ListView;
		private System.Windows.Forms.ColumnHeader LabelColumnHeader;
		private System.Windows.Forms.ColumnHeader ContentColumnHeader;
		private System.Windows.Forms.Button ConfirmButton;
	}
}