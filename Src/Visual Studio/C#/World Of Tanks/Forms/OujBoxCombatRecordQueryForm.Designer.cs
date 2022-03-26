
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
			this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.ResultListView = new System.Windows.Forms.ListView();
			this.LabelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ContentColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.TankResultListView = new System.Windows.Forms.ListView();
			this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CombatNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VictoryRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VictoryNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.EvenNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.FailNumberColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageDurationColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageSurvivalTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageXPColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageCombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageDamageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageAssistColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageArmorResistanceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AverageHitRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AveragePenetrationRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AveragePenetrationRateIncludeNoHitColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ExportButton = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
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
			this.NameTextBox.Size = new System.Drawing.Size(609, 23);
			this.NameTextBox.TabIndex = 1;
			this.NameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.NameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NameTextBox_KeyUp);
			// 
			// QueryButton
			// 
			this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.QueryButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.QueryButton.Location = new System.Drawing.Point(1094, 5);
			this.QueryButton.Name = "QueryButton";
			this.QueryButton.Size = new System.Drawing.Size(75, 25);
			this.QueryButton.TabIndex = 2;
			this.QueryButton.Text = "查询";
			this.QueryButton.UseVisualStyleBackColor = true;
			this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
			// 
			// StartDateTimePicker
			// 
			this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StartDateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.StartDateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.StartDateTimePicker.Location = new System.Drawing.Point(659, 5);
			this.StartDateTimePicker.Name = "StartDateTimePicker";
			this.StartDateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.StartDateTimePicker.TabIndex = 3;
			this.StartDateTimePicker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StartDateTimePicker_KeyUp);
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
			this.ResultListView.Size = new System.Drawing.Size(260, 490);
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
            this.VictoryNumberColumnHeader,
            this.EvenNumberColumnHeader,
            this.FailNumberColumnHeader,
            this.AverageDurationColumnHeader,
            this.AverageSurvivalTimeColumnHeader,
            this.AverageXPColumnHeader,
            this.AverageCombatColumnHeader,
            this.AverageDamageColumnHeader,
            this.AverageAssistColumnHeader,
            this.AverageArmorResistanceColumnHeader,
            this.AverageHitRateColumnHeader,
            this.AveragePenetrationRateColumnHeader,
            this.AveragePenetrationRateIncludeNoHitColumnHeader});
			this.TankResultListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.TankResultListView.FullRowSelect = true;
			this.TankResultListView.HideSelection = false;
			this.TankResultListView.Location = new System.Drawing.Point(270, 35);
			this.TankResultListView.Name = "TankResultListView";
			this.TankResultListView.Size = new System.Drawing.Size(898, 490);
			this.TankResultListView.TabIndex = 25;
			this.TankResultListView.UseCompatibleStateImageBehavior = false;
			this.TankResultListView.View = System.Windows.Forms.View.Details;
			this.TankResultListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.TankResultListView_ColumnClick);
			// 
			// NameColumnHeader
			// 
			this.NameColumnHeader.Text = "坦克";
			this.NameColumnHeader.Width = 40;
			// 
			// CombatNumberColumnHeader
			// 
			this.CombatNumberColumnHeader.Text = "场次";
			this.CombatNumberColumnHeader.Width = 45;
			// 
			// VictoryRateColumnHeader
			// 
			this.VictoryRateColumnHeader.Text = "胜率";
			this.VictoryRateColumnHeader.Width = 44;
			// 
			// VictoryNumberColumnHeader
			// 
			this.VictoryNumberColumnHeader.Text = "胜";
			this.VictoryNumberColumnHeader.Width = 34;
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
			// AverageXPColumnHeader
			// 
			this.AverageXPColumnHeader.Text = "平均经验";
			this.AverageXPColumnHeader.Width = 74;
			// 
			// AverageCombatColumnHeader
			// 
			this.AverageCombatColumnHeader.Text = "平均效率";
			this.AverageCombatColumnHeader.Width = 70;
			// 
			// AverageDamageColumnHeader
			// 
			this.AverageDamageColumnHeader.Text = "平均伤害";
			this.AverageDamageColumnHeader.Width = 74;
			// 
			// AverageAssistColumnHeader
			// 
			this.AverageAssistColumnHeader.Text = "平均协助";
			this.AverageAssistColumnHeader.Width = 76;
			// 
			// AverageArmorResistanceColumnHeader
			// 
			this.AverageArmorResistanceColumnHeader.Text = "平均抵挡伤害";
			this.AverageArmorResistanceColumnHeader.Width = 105;
			// 
			// AverageHitRateColumnHeader
			// 
			this.AverageHitRateColumnHeader.Text = "平均命中率";
			// 
			// AveragePenetrationRateColumnHeader
			// 
			this.AveragePenetrationRateColumnHeader.Text = "平均击穿率";
			// 
			// AveragePenetrationRateIncludeNoHitColumnHeader
			// 
			this.AveragePenetrationRateIncludeNoHitColumnHeader.Text = "平均击穿率(含未命中)";
			// 
			// ExportButton
			// 
			this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ExportButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ExportButton.Location = new System.Drawing.Point(1094, 530);
			this.ExportButton.Name = "ExportButton";
			this.ExportButton.Size = new System.Drawing.Size(75, 25);
			this.ExportButton.TabIndex = 27;
			this.ExportButton.Text = "导出";
			this.ExportButton.UseVisualStyleBackColor = true;
			this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "CSV 文件|*.csv|所有文件|*.*";
			// 
			// EndDateTimePicker
			// 
			this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.EndDateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.EndDateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.EndDateTimePicker.Location = new System.Drawing.Point(889, 5);
			this.EndDateTimePicker.Name = "EndDateTimePicker";
			this.EndDateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.EndDateTimePicker.TabIndex = 33;
			this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
			this.EndDateTimePicker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EndDateTimePicker_KeyUp);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(864, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 34;
			this.label2.Text = "至";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OujBoxCombatRecordQueryForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1173, 561);
			this.Controls.Add(this.EndDateTimePicker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ExportButton);
			this.Controls.Add(this.ResultListView);
			this.Controls.Add(this.TankResultListView);
			this.Controls.Add(this.StartDateTimePicker);
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
		private System.Windows.Forms.DateTimePicker StartDateTimePicker;
		private System.Windows.Forms.ListView ResultListView;
		private System.Windows.Forms.ColumnHeader LabelColumnHeader;
		private System.Windows.Forms.ColumnHeader ContentColumnHeader;
		private System.Windows.Forms.ListView TankResultListView;
		private System.Windows.Forms.ColumnHeader NameColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatNumberColumnHeader;
		private System.Windows.Forms.ColumnHeader VictoryRateColumnHeader;
		private System.Windows.Forms.ColumnHeader VictoryNumberColumnHeader;
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
		private System.Windows.Forms.DateTimePicker EndDateTimePicker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader AverageArmorResistanceColumnHeader;
		private System.Windows.Forms.ColumnHeader AverageHitRateColumnHeader;
		private System.Windows.Forms.ColumnHeader AveragePenetrationRateColumnHeader;
		private System.Windows.Forms.ColumnHeader AveragePenetrationRateIncludeNoHitColumnHeader;
	}
}