
namespace WorldOfTanks {
	partial class BoxCombatAnalysisForm {
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
			this.components = new System.ComponentModel.Container();
			this.CombatListView = new System.Windows.Forms.ListView();
			this.ResultColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ModeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CombatListCombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.QueryButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.TeamAListView = new System.Windows.Forms.ListView();
			this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ClanColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BoxCombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BoxWinRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BoxHitRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BoxCombatLevelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BoxDamageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.TankColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DamageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AssistColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ArmorResistanceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.HitRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PenetrationRateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PenetrationRateIncludeNoHitColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ShootCountColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.HitCountColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PenetrationCountColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.TeamBListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.TeamAInformationLabel = new System.Windows.Forms.Label();
			this.TeamBInformationLabel = new System.Windows.Forms.Label();
			this.StateLabel = new System.Windows.Forms.Label();
			this.NameComboBox = new System.Windows.Forms.ComboBox();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// CombatListView
			// 
			this.CombatListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.CombatListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ResultColumnHeader,
            this.ModeColumnHeader,
            this.DateColumnHeader,
            this.CombatListCombatColumnHeader});
			this.CombatListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.CombatListView.FullRowSelect = true;
			this.CombatListView.HideSelection = false;
			this.CombatListView.Location = new System.Drawing.Point(5, 35);
			this.CombatListView.Name = "CombatListView";
			this.CombatListView.OwnerDraw = true;
			this.CombatListView.Size = new System.Drawing.Size(225, 470);
			this.CombatListView.TabIndex = 27;
			this.CombatListView.UseCompatibleStateImageBehavior = false;
			this.CombatListView.View = System.Windows.Forms.View.Details;
			this.CombatListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.ListView_DrawColumnHeader);
			this.CombatListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.ListView_DrawSubItem);
			this.CombatListView.SelectedIndexChanged += new System.EventHandler(this.CombatListView_SelectedIndexChanged);
			this.CombatListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListView_MouseClick);
			// 
			// ResultColumnHeader
			// 
			this.ResultColumnHeader.Text = "结果";
			this.ResultColumnHeader.Width = 42;
			// 
			// ModeColumnHeader
			// 
			this.ModeColumnHeader.Text = "模式";
			this.ModeColumnHeader.Width = 46;
			// 
			// DateColumnHeader
			// 
			this.DateColumnHeader.Text = "时间";
			this.DateColumnHeader.Width = 42;
			// 
			// CombatListCombatColumnHeader
			// 
			this.CombatListCombatColumnHeader.Text = "效率";
			// 
			// EndDateTimePicker
			// 
			this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.EndDateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.EndDateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.EndDateTimePicker.Location = new System.Drawing.Point(921, 5);
			this.EndDateTimePicker.Name = "EndDateTimePicker";
			this.EndDateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.EndDateTimePicker.TabIndex = 39;
			this.EndDateTimePicker.CloseUp += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
			this.EndDateTimePicker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyUp);
			this.EndDateTimePicker.Leave += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(896, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 40;
			this.label2.Text = "至";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StartDateTimePicker
			// 
			this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StartDateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.StartDateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.StartDateTimePicker.Location = new System.Drawing.Point(691, 5);
			this.StartDateTimePicker.Name = "StartDateTimePicker";
			this.StartDateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.StartDateTimePicker.TabIndex = 38;
			this.StartDateTimePicker.CloseUp += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
			this.StartDateTimePicker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyUp);
			this.StartDateTimePicker.Leave += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
			// 
			// QueryButton
			// 
			this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.QueryButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.QueryButton.Location = new System.Drawing.Point(1126, 5);
			this.QueryButton.Name = "QueryButton";
			this.QueryButton.Size = new System.Drawing.Size(75, 25);
			this.QueryButton.TabIndex = 37;
			this.QueryButton.Text = "查询";
			this.QueryButton.UseVisualStyleBackColor = true;
			this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(6, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 35;
			this.label1.Text = "昵称：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TeamAListView
			// 
			this.TeamAListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TeamAListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.ClanColumnHeader,
            this.BoxCombatColumnHeader,
            this.BoxWinRateColumnHeader,
            this.BoxHitRateColumnHeader,
            this.BoxCombatLevelColumnHeader,
            this.BoxDamageColumnHeader,
            this.TankColumnHeader,
            this.CombatColumnHeader,
            this.DamageColumnHeader,
            this.AssistColumnHeader,
            this.ArmorResistanceColumnHeader,
            this.HitRateColumnHeader,
            this.PenetrationRateColumnHeader,
            this.PenetrationRateIncludeNoHitColumnHeader,
            this.ShootCountColumnHeader,
            this.HitCountColumnHeader,
            this.PenetrationCountColumnHeader});
			this.TeamAListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.TeamAListView.FullRowSelect = true;
			this.TeamAListView.HideSelection = false;
			this.TeamAListView.Location = new System.Drawing.Point(235, 35);
			this.TeamAListView.Name = "TeamAListView";
			this.TeamAListView.OwnerDraw = true;
			this.TeamAListView.Size = new System.Drawing.Size(965, 215);
			this.TeamAListView.TabIndex = 41;
			this.TeamAListView.UseCompatibleStateImageBehavior = false;
			this.TeamAListView.View = System.Windows.Forms.View.Details;
			this.TeamAListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
			this.TeamAListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.ListView_DrawColumnHeader);
			this.TeamAListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.ListView_DrawSubItem);
			this.TeamAListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListView_MouseClick);
			// 
			// NameColumnHeader
			// 
			this.NameColumnHeader.Text = "昵称";
			this.NameColumnHeader.Width = 40;
			// 
			// ClanColumnHeader
			// 
			this.ClanColumnHeader.Text = "军团";
			this.ClanColumnHeader.Width = 45;
			// 
			// BoxCombatColumnHeader
			// 
			this.BoxCombatColumnHeader.Text = "千场效率";
			this.BoxCombatColumnHeader.Width = 77;
			// 
			// BoxWinRateColumnHeader
			// 
			this.BoxWinRateColumnHeader.Text = "千场胜率";
			this.BoxWinRateColumnHeader.Width = 75;
			// 
			// BoxHitRateColumnHeader
			// 
			this.BoxHitRateColumnHeader.Text = "千场命中率";
			this.BoxHitRateColumnHeader.Width = 88;
			// 
			// BoxCombatLevelColumnHeader
			// 
			this.BoxCombatLevelColumnHeader.Text = "千场出战等级";
			this.BoxCombatLevelColumnHeader.Width = 98;
			// 
			// BoxDamageColumnHeader
			// 
			this.BoxDamageColumnHeader.Text = "千场均伤";
			this.BoxDamageColumnHeader.Width = 76;
			// 
			// TankColumnHeader
			// 
			this.TankColumnHeader.Text = "坦克";
			// 
			// CombatColumnHeader
			// 
			this.CombatColumnHeader.Text = "本局效率";
			this.CombatColumnHeader.Width = 70;
			// 
			// DamageColumnHeader
			// 
			this.DamageColumnHeader.Text = "伤害";
			this.DamageColumnHeader.Width = 43;
			// 
			// AssistColumnHeader
			// 
			this.AssistColumnHeader.Text = "协助";
			this.AssistColumnHeader.Width = 46;
			// 
			// ArmorResistanceColumnHeader
			// 
			this.ArmorResistanceColumnHeader.Text = "抵挡伤害";
			this.ArmorResistanceColumnHeader.Width = 71;
			// 
			// HitRateColumnHeader
			// 
			this.HitRateColumnHeader.Text = "命中率";
			// 
			// PenetrationRateColumnHeader
			// 
			this.PenetrationRateColumnHeader.Text = "击穿率";
			// 
			// PenetrationRateIncludeNoHitColumnHeader
			// 
			this.PenetrationRateIncludeNoHitColumnHeader.Text = "击穿率(含未命中)";
			this.PenetrationRateIncludeNoHitColumnHeader.Width = 128;
			// 
			// ShootCountColumnHeader
			// 
			this.ShootCountColumnHeader.Text = "射击次数";
			this.ShootCountColumnHeader.Width = 69;
			// 
			// HitCountColumnHeader
			// 
			this.HitCountColumnHeader.Text = "命中次数";
			this.HitCountColumnHeader.Width = 71;
			// 
			// PenetrationCountColumnHeader
			// 
			this.PenetrationCountColumnHeader.Text = "击穿次数";
			this.PenetrationCountColumnHeader.Width = 69;
			// 
			// TeamBListView
			// 
			this.TeamBListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TeamBListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
			this.TeamBListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.TeamBListView.FullRowSelect = true;
			this.TeamBListView.HideSelection = false;
			this.TeamBListView.Location = new System.Drawing.Point(235, 275);
			this.TeamBListView.Name = "TeamBListView";
			this.TeamBListView.OwnerDraw = true;
			this.TeamBListView.Size = new System.Drawing.Size(965, 230);
			this.TeamBListView.TabIndex = 42;
			this.TeamBListView.UseCompatibleStateImageBehavior = false;
			this.TeamBListView.View = System.Windows.Forms.View.Details;
			this.TeamBListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
			this.TeamBListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.ListView_DrawColumnHeader);
			this.TeamBListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.ListView_DrawSubItem);
			this.TeamBListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListView_MouseClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "昵称";
			this.columnHeader1.Width = 40;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "坦克";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "单局效率";
			this.columnHeader3.Width = 70;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "伤害";
			this.columnHeader4.Width = 43;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "协助";
			this.columnHeader5.Width = 46;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "抵挡伤害";
			this.columnHeader6.Width = 71;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "命中率";
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "击穿率";
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "击穿率(含未命中)";
			this.columnHeader9.Width = 128;
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "射击次数";
			this.columnHeader10.Width = 70;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "命中次数";
			this.columnHeader11.Width = 68;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "击穿次数";
			this.columnHeader12.Width = 84;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
			this.toolStripMenuItem1.Text = "复制";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
			// 
			// TeamAInformationLabel
			// 
			this.TeamAInformationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TeamAInformationLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.TeamAInformationLabel.Location = new System.Drawing.Point(235, 250);
			this.TeamAInformationLabel.Name = "TeamAInformationLabel";
			this.TeamAInformationLabel.Size = new System.Drawing.Size(965, 25);
			this.TeamAInformationLabel.TabIndex = 43;
			this.TeamAInformationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TeamBInformationLabel
			// 
			this.TeamBInformationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TeamBInformationLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.TeamBInformationLabel.Location = new System.Drawing.Point(235, 510);
			this.TeamBInformationLabel.Name = "TeamBInformationLabel";
			this.TeamBInformationLabel.Size = new System.Drawing.Size(965, 25);
			this.TeamBInformationLabel.TabIndex = 44;
			this.TeamBInformationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StateLabel
			// 
			this.StateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.StateLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.StateLabel.Location = new System.Drawing.Point(5, 510);
			this.StateLabel.Name = "StateLabel";
			this.StateLabel.Size = new System.Drawing.Size(225, 25);
			this.StateLabel.TabIndex = 45;
			this.StateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// NameComboBox
			// 
			this.NameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameComboBox.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.NameComboBox.FormattingEnabled = true;
			this.NameComboBox.Location = new System.Drawing.Point(50, 5);
			this.NameComboBox.Name = "NameComboBox";
			this.NameComboBox.Size = new System.Drawing.Size(635, 23);
			this.NameComboBox.TabIndex = 46;
			this.NameComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyUp);
			// 
			// BoxCombatAnalysisForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1205, 541);
			this.Controls.Add(this.NameComboBox);
			this.Controls.Add(this.StateLabel);
			this.Controls.Add(this.TeamBInformationLabel);
			this.Controls.Add(this.TeamAInformationLabel);
			this.Controls.Add(this.TeamBListView);
			this.Controls.Add(this.TeamAListView);
			this.Controls.Add(this.EndDateTimePicker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.StartDateTimePicker);
			this.Controls.Add(this.QueryButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.CombatListView);
			this.Name = "BoxCombatAnalysisForm";
			this.Text = "CombatAnalysisForm";
			this.Resize += new System.EventHandler(this.BoxCombatAnalysisForm_Resize);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView CombatListView;
		private System.Windows.Forms.ColumnHeader ResultColumnHeader;
		private System.Windows.Forms.ColumnHeader ModeColumnHeader;
		private System.Windows.Forms.DateTimePicker EndDateTimePicker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker StartDateTimePicker;
		private System.Windows.Forms.Button QueryButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView TeamAListView;
		private System.Windows.Forms.ColumnHeader NameColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatColumnHeader;
		private System.Windows.Forms.ColumnHeader DamageColumnHeader;
		private System.Windows.Forms.ColumnHeader AssistColumnHeader;
		private System.Windows.Forms.ColumnHeader ArmorResistanceColumnHeader;
		private System.Windows.Forms.ColumnHeader HitRateColumnHeader;
		private System.Windows.Forms.ColumnHeader PenetrationRateColumnHeader;
		private System.Windows.Forms.ColumnHeader PenetrationRateIncludeNoHitColumnHeader;
		private System.Windows.Forms.ColumnHeader DateColumnHeader;
		private System.Windows.Forms.ColumnHeader TankColumnHeader;
		private System.Windows.Forms.ListView TeamBListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader ShootCountColumnHeader;
		private System.Windows.Forms.ColumnHeader HitCountColumnHeader;
		private System.Windows.Forms.ColumnHeader PenetrationCountColumnHeader;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader BoxCombatColumnHeader;
		private System.Windows.Forms.ColumnHeader BoxWinRateColumnHeader;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.Label TeamAInformationLabel;
		private System.Windows.Forms.Label TeamBInformationLabel;
		private System.Windows.Forms.ColumnHeader BoxDamageColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatListCombatColumnHeader;
		private System.Windows.Forms.ColumnHeader BoxHitRateColumnHeader;
		private System.Windows.Forms.ColumnHeader BoxCombatLevelColumnHeader;
		private System.Windows.Forms.ColumnHeader ClanColumnHeader;
		private System.Windows.Forms.Label StateLabel;
		private System.Windows.Forms.ComboBox NameComboBox;
	}
}