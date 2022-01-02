
namespace WorldOfTanks {
	partial class OujBoxCombatRecordQueryForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.NameTextBox = new System.Windows.Forms.TextBox();
			this.QueryButton = new System.Windows.Forms.Button();
			this.DateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.ToTodayButton = new System.Windows.Forms.Button();
			this.ResultListView = new System.Windows.Forms.ListView();
			this.LabelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ContentColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
			this.ExportButton = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "昵称：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// NameTextBox
			// 
			this.NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameTextBox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.NameTextBox.Location = new System.Drawing.Point(45, 5);
			this.NameTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(569, 23);
			this.NameTextBox.TabIndex = 1;
			this.NameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.NameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NameTextBox_KeyUp);
			// 
			// QueryButton
			// 
			this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.QueryButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.QueryButton.Location = new System.Drawing.Point(825, 5);
			this.QueryButton.Name = "QueryButton";
			this.QueryButton.Size = new System.Drawing.Size(75, 25);
			this.QueryButton.TabIndex = 2;
			this.QueryButton.Text = "当天";
			this.QueryButton.UseVisualStyleBackColor = true;
			this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
			// 
			// DateTimePicker
			// 
			this.DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.DateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.DateTimePicker.Location = new System.Drawing.Point(620, 5);
			this.DateTimePicker.Name = "DateTimePicker";
			this.DateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.DateTimePicker.TabIndex = 3;
			// 
			// ToTodayButton
			// 
			this.ToTodayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ToTodayButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ToTodayButton.Location = new System.Drawing.Point(905, 5);
			this.ToTodayButton.Name = "ToTodayButton";
			this.ToTodayButton.Size = new System.Drawing.Size(75, 25);
			this.ToTodayButton.TabIndex = 9;
			this.ToTodayButton.Text = "至今";
			this.ToTodayButton.UseVisualStyleBackColor = true;
			this.ToTodayButton.Click += new System.EventHandler(this.ToTodayButton_Click);
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
			this.ResultListView.Location = new System.Drawing.Point(5, 35);
			this.ResultListView.Name = "ResultListView";
			this.ResultListView.Size = new System.Drawing.Size(240, 490);
			this.ResultListView.TabIndex = 26;
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
			this.TankResultListView.Location = new System.Drawing.Point(250, 35);
			this.TankResultListView.Name = "TankResultListView";
			this.TankResultListView.Size = new System.Drawing.Size(729, 490);
			this.TankResultListView.TabIndex = 25;
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
			// ExportButton
			// 
			this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ExportButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ExportButton.Location = new System.Drawing.Point(905, 530);
			this.ExportButton.Name = "ExportButton";
			this.ExportButton.Size = new System.Drawing.Size(75, 25);
			this.ExportButton.TabIndex = 27;
			this.ExportButton.Text = "导出";
			this.ExportButton.UseVisualStyleBackColor = true;
			this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
			// 
			// OujBoxCombatRecordQueryForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(984, 561);
			this.Controls.Add(this.ExportButton);
			this.Controls.Add(this.ResultListView);
			this.Controls.Add(this.TankResultListView);
			this.Controls.Add(this.ToTodayButton);
			this.Controls.Add(this.DateTimePicker);
			this.Controls.Add(this.QueryButton);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.label1);
			this.Name = "OujBoxCombatRecordQueryForm";
			this.Text = "OujBoxCombatRecordQueryForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NameTextBox;
		private System.Windows.Forms.Button QueryButton;
		private System.Windows.Forms.DateTimePicker DateTimePicker;
		private System.Windows.Forms.Button ToTodayButton;
		private System.Windows.Forms.ListView ResultListView;
		private System.Windows.Forms.ColumnHeader LabelColumnHeader;
		private System.Windows.Forms.ColumnHeader ContentColumnHeader;
		private System.Windows.Forms.ListView TankResultListView;
		private System.Windows.Forms.ColumnHeader NameColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatNumberColumnHeader;
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
		private System.Windows.Forms.Button ExportButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}