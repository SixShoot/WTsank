
namespace WorldOfTanks {
	partial class OujBoxClanQueryForm {
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
			this.MemberResultListView = new System.Windows.Forms.ListView();
			this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CombatColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AttendanceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.AttendanceButton = new System.Windows.Forms.Button();
			this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.QueryButton = new System.Windows.Forms.Button();
			this.NameTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ResultListView = new System.Windows.Forms.ListView();
			this.LabelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ContentColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.StateLabel = new System.Windows.Forms.Label();
			this.ExportButton = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.ModeComboBox = new System.Windows.Forms.ComboBox();
			this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// MemberResultListView
			// 
			this.MemberResultListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MemberResultListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.CombatColumnHeader,
            this.AttendanceColumnHeader});
			this.MemberResultListView.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MemberResultListView.FullRowSelect = true;
			this.MemberResultListView.HideSelection = false;
			this.MemberResultListView.Location = new System.Drawing.Point(250, 35);
			this.MemberResultListView.Name = "MemberResultListView";
			this.MemberResultListView.OwnerDraw = true;
			this.MemberResultListView.Size = new System.Drawing.Size(729, 490);
			this.MemberResultListView.TabIndex = 8;
			this.MemberResultListView.UseCompatibleStateImageBehavior = false;
			this.MemberResultListView.View = System.Windows.Forms.View.Details;
			this.MemberResultListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.MemberResultListView_ColumnClick);
			this.MemberResultListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.MemberResultListView_DrawColumnHeader);
			this.MemberResultListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.MemberResultListView_DrawSubItem);
			this.MemberResultListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MemberResultListView_MouseClick);
			// 
			// NameColumnHeader
			// 
			this.NameColumnHeader.Text = "昵称";
			this.NameColumnHeader.Width = 40;
			// 
			// CombatColumnHeader
			// 
			this.CombatColumnHeader.Text = "千场效率";
			this.CombatColumnHeader.Width = 73;
			// 
			// AttendanceColumnHeader
			// 
			this.AttendanceColumnHeader.Text = "出勤天数";
			this.AttendanceColumnHeader.Width = 87;
			// 
			// AttendanceButton
			// 
			this.AttendanceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AttendanceButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.AttendanceButton.Location = new System.Drawing.Point(905, 5);
			this.AttendanceButton.Name = "AttendanceButton";
			this.AttendanceButton.Size = new System.Drawing.Size(75, 25);
			this.AttendanceButton.TabIndex = 14;
			this.AttendanceButton.Text = "考勤";
			this.AttendanceButton.UseVisualStyleBackColor = true;
			this.AttendanceButton.Click += new System.EventHandler(this.AttendanceButton_Click);
			// 
			// StartDateTimePicker
			// 
			this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StartDateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.StartDateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.StartDateTimePicker.Location = new System.Drawing.Point(285, 5);
			this.StartDateTimePicker.Name = "StartDateTimePicker";
			this.StartDateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.StartDateTimePicker.TabIndex = 13;
			this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
			this.StartDateTimePicker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StartDateTimePicker_KeyUp);
			// 
			// QueryButton
			// 
			this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.QueryButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.QueryButton.Location = new System.Drawing.Point(720, 5);
			this.QueryButton.Name = "QueryButton";
			this.QueryButton.Size = new System.Drawing.Size(75, 25);
			this.QueryButton.TabIndex = 12;
			this.QueryButton.Text = "查询";
			this.QueryButton.UseVisualStyleBackColor = true;
			this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
			// 
			// NameTextBox
			// 
			this.NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameTextBox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.NameTextBox.Location = new System.Drawing.Point(45, 5);
			this.NameTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(235, 23);
			this.NameTextBox.TabIndex = 11;
			this.NameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.NameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NameTextBox_KeyUp);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 10;
			this.label1.Text = "名称：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.ResultListView.OwnerDraw = true;
			this.ResultListView.Size = new System.Drawing.Size(240, 490);
			this.ResultListView.TabIndex = 27;
			this.ResultListView.UseCompatibleStateImageBehavior = false;
			this.ResultListView.View = System.Windows.Forms.View.Details;
			this.ResultListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.ResultListView_DrawColumnHeader);
			this.ResultListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.ResultListView_DrawSubItem);
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
			// StateLabel
			// 
			this.StateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.StateLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.StateLabel.Location = new System.Drawing.Point(5, 530);
			this.StateLabel.Name = "StateLabel";
			this.StateLabel.Size = new System.Drawing.Size(895, 25);
			this.StateLabel.TabIndex = 28;
			this.StateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ExportButton
			// 
			this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ExportButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ExportButton.Location = new System.Drawing.Point(905, 530);
			this.ExportButton.Name = "ExportButton";
			this.ExportButton.Size = new System.Drawing.Size(75, 25);
			this.ExportButton.TabIndex = 29;
			this.ExportButton.Text = "导出";
			this.ExportButton.UseVisualStyleBackColor = true;
			this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "CSV 文件|*.csv|所有文件|*.*";
			// 
			// ModeComboBox
			// 
			this.ModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ModeComboBox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ModeComboBox.FormattingEnabled = true;
			this.ModeComboBox.Items.AddRange(new object[] {
            "要塞",
            "领土战"});
			this.ModeComboBox.Location = new System.Drawing.Point(800, 5);
			this.ModeComboBox.Name = "ModeComboBox";
			this.ModeComboBox.Size = new System.Drawing.Size(100, 22);
			this.ModeComboBox.TabIndex = 30;
			// 
			// EndDateTimePicker
			// 
			this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.EndDateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.EndDateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.EndDateTimePicker.Location = new System.Drawing.Point(515, 5);
			this.EndDateTimePicker.Name = "EndDateTimePicker";
			this.EndDateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.EndDateTimePicker.TabIndex = 31;
			this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
			this.EndDateTimePicker.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EndDateTimePicker_KeyUp);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(490, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 32;
			this.label2.Text = "至";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
			// 
			// CopyToolStripMenuItem
			// 
			this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
			this.CopyToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.CopyToolStripMenuItem.Text = "复制";
			this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
			// 
			// OujBoxClanQueryForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(984, 561);
			this.Controls.Add(this.EndDateTimePicker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ModeComboBox);
			this.Controls.Add(this.ExportButton);
			this.Controls.Add(this.StateLabel);
			this.Controls.Add(this.ResultListView);
			this.Controls.Add(this.AttendanceButton);
			this.Controls.Add(this.StartDateTimePicker);
			this.Controls.Add(this.QueryButton);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.MemberResultListView);
			this.Name = "OujBoxClanQueryForm";
			this.Text = "OujBoxClanQueryForm";
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView MemberResultListView;
		private System.Windows.Forms.ColumnHeader NameColumnHeader;
		private System.Windows.Forms.ColumnHeader CombatColumnHeader;
		private System.Windows.Forms.Button AttendanceButton;
		private System.Windows.Forms.DateTimePicker StartDateTimePicker;
		private System.Windows.Forms.Button QueryButton;
		private System.Windows.Forms.TextBox NameTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView ResultListView;
		private System.Windows.Forms.ColumnHeader LabelColumnHeader;
		private System.Windows.Forms.ColumnHeader ContentColumnHeader;
		private System.Windows.Forms.ColumnHeader AttendanceColumnHeader;
		private System.Windows.Forms.Label StateLabel;
		private System.Windows.Forms.Button ExportButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ComboBox ModeComboBox;
		private System.Windows.Forms.DateTimePicker EndDateTimePicker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
	}
}