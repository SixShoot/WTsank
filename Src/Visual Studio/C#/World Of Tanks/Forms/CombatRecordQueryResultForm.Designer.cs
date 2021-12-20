
namespace WorldOfTanks {
	partial class CombatRecordQueryResultForm {
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
			this.TankResultListView = new System.Windows.Forms.ListView();
			this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CombatNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VictoryRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VictoryNumberHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.EvenNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.FailNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageDurationColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageSurvivalTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageCombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageXPColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageDamageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageAssistColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ResultListView = new System.Windows.Forms.ListView();
			this.LabelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ContentColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// TankResultListView
			// 
			this.TankResultListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TankResultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.CombatNumberColumnHeader,
            this.VictoryRateColumnHeader,
            this.VictoryNumberHeader,
            this.EvenNumberColumnHeader,
            this.FailNumberColumnHeader,
            this.AverageDurationColumnHeader,
            this.AverageSurvivalTimeColumnHeader,
            this.AverageCombatColumnHeader,
            this.AverageXPColumnHeader,
            this.AverageDamageColumnHeader,
            this.AverageAssistColumnHeader});
			this.TankResultListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.TankResultListView.FullRowSelect = true;
			this.TankResultListView.HideSelection = false;
			this.TankResultListView.Location = new System.Drawing.Point(250, 5);
			this.TankResultListView.Name = "TankResultListView";
			this.TankResultListView.Size = new System.Drawing.Size(974, 565);
			this.TankResultListView.TabIndex = 23;
			this.TankResultListView.UseCompatibleStateImageBehavior = false;
			this.TankResultListView.View = System.Windows.Forms.View.Details;
			// 
			// NameColumnHeader
			// 
			this.NameColumnHeader.Text = "名称";
			this.NameColumnHeader.Width = 135;
			// 
			// CombatNumberColumnHeader
			// 
			this.CombatNumberColumnHeader.Text = "场数";
			// 
			// VictoryRateColumnHeader
			// 
			this.VictoryRateColumnHeader.Text = "胜率";
			this.VictoryRateColumnHeader.Width = 44;
			// 
			// VictoryNumberHeader
			// 
			this.VictoryNumberHeader.Text = "胜";
			this.VictoryNumberHeader.Width = 34;
			// 
			// EvenNumberColumnHeader
			// 
			this.EvenNumberColumnHeader.Text = "平";
			this.EvenNumberColumnHeader.Width = 31;
			// 
			// FailNumberColumnHeader
			// 
			this.FailNumberColumnHeader.Text = "负";
			this.FailNumberColumnHeader.Width = 34;
			// 
			// AverageDurationColumnHeader
			// 
			this.AverageDurationColumnHeader.Text = "平均对局时间";
			this.AverageDurationColumnHeader.Width = 100;
			// 
			// AverageSurvivalTimeColumnHeader
			// 
			this.AverageSurvivalTimeColumnHeader.Text = "平均存活时间";
			this.AverageSurvivalTimeColumnHeader.Width = 101;
			// 
			// AverageCombatColumnHeader
			// 
			this.AverageCombatColumnHeader.Text = "平均效率";
			this.AverageCombatColumnHeader.Width = 70;
			// 
			// AverageXPColumnHeader
			// 
			this.AverageXPColumnHeader.DisplayIndex = 10;
			this.AverageXPColumnHeader.Text = "平均经验";
			this.AverageXPColumnHeader.Width = 74;
			// 
			// AverageDamageColumnHeader
			// 
			this.AverageDamageColumnHeader.DisplayIndex = 9;
			this.AverageDamageColumnHeader.Text = "平均伤害";
			this.AverageDamageColumnHeader.Width = 74;
			// 
			// AverageAssistColumnHeader
			// 
			this.AverageAssistColumnHeader.Text = "平均协助";
			this.AverageAssistColumnHeader.Width = 76;
			// 
			// ResultListView
			// 
			this.ResultListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.ResultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LabelColumnHeader,
            this.ContentColumnHeader});
			this.ResultListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ResultListView.FullRowSelect = true;
			this.ResultListView.HideSelection = false;
			this.ResultListView.Location = new System.Drawing.Point(5, 5);
			this.ResultListView.Name = "ResultListView";
			this.ResultListView.Size = new System.Drawing.Size(240, 565);
			this.ResultListView.TabIndex = 24;
			this.ResultListView.UseCompatibleStateImageBehavior = false;
			this.ResultListView.View = System.Windows.Forms.View.Details;
			// 
			// LabelColumnHeader
			// 
			this.LabelColumnHeader.Text = "名称";
			this.LabelColumnHeader.Width = 132;
			// 
			// ContentColumnHeader
			// 
			this.ContentColumnHeader.Text = "值";
			this.ContentColumnHeader.Width = 28;
			// 
			// CombatRecordQueryResultForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1229, 575);
			this.Controls.Add(this.ResultListView);
			this.Controls.Add(this.TankResultListView);
			this.Name = "CombatRecordQueryResultForm";
			this.Text = "BattleRecordQueryResultForm";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ListView TankResultListView;
		private System.Windows.Forms.ColumnHeader NameColumnHeader;
		private System.Windows.Forms.ColumnHeader VictoryRateColumnHeader;
		private System.Windows.Forms.ColumnHeader VictoryNumberHeader;
		private System.Windows.Forms.ColumnHeader EvenNumberColumnHeader;
		private System.Windows.Forms.ColumnHeader FailNumberColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageDurationColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageSurvivalTimeColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageCombatColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageXPColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageDamageColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageAssistColumnHeader;
		private System.Windows.Forms.ListView ResultListView;
		private System.Windows.Forms.ColumnHeader LabelColumnHeader;
		private System.Windows.Forms.ColumnHeader ContentColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatNumberColumnHeader;
	}
}