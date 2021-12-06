
namespace WorldOfTanks {
	partial class ClanQueryResultForm {
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
			this.ResultListView = new System.Windows.Forms.ListView();
			this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// ResultListView
			// 
			this.ResultListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ResultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.CombatColumnHeader});
			this.ResultListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ResultListView.FullRowSelect = true;
			this.ResultListView.HideSelection = false;
			this.ResultListView.Location = new System.Drawing.Point(5, 5);
			this.ResultListView.Name = "ResultListView";
			this.ResultListView.Size = new System.Drawing.Size(790, 440);
			this.ResultListView.TabIndex = 7;
			this.ResultListView.UseCompatibleStateImageBehavior = false;
			this.ResultListView.View = System.Windows.Forms.View.Details;
			// 
			// NameColumnHeader
			// 
			this.NameColumnHeader.Text = "名称";
			this.NameColumnHeader.Width = 40;
			// 
			// CombatColumnHeader
			// 
			this.CombatColumnHeader.Text = "千场效率";
			this.CombatColumnHeader.Width = 73;
			// 
			// ClanQueryResultForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.ResultListView);
			this.Name = "ClanQueryResultForm";
			this.Text = "ClanQueryResultForm";
			this.Load += new System.EventHandler(this.ClanQueryResultForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView ResultListView;
		private System.Windows.Forms.ColumnHeader NameColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatColumnHeader;
	}
}