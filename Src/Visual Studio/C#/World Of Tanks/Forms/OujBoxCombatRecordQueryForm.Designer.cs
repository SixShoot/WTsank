
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
			this.ClanButton = new System.Windows.Forms.Button();
			this.SummaryLabel = new System.Windows.Forms.Label();
			this.Panel = new System.Windows.Forms.Panel();
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
			this.NameTextBox.Size = new System.Drawing.Size(385, 23);
			this.NameTextBox.TabIndex = 1;
			this.NameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// QueryButton
			// 
			this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.QueryButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.QueryButton.Location = new System.Drawing.Point(640, 5);
			this.QueryButton.Name = "QueryButton";
			this.QueryButton.Size = new System.Drawing.Size(75, 25);
			this.QueryButton.TabIndex = 2;
			this.QueryButton.Text = "查询";
			this.QueryButton.UseVisualStyleBackColor = true;
			this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
			// 
			// DateTimePicker
			// 
			this.DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DateTimePicker.CalendarFont = new System.Drawing.Font("宋体", 9F);
			this.DateTimePicker.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.DateTimePicker.Location = new System.Drawing.Point(435, 5);
			this.DateTimePicker.Name = "DateTimePicker";
			this.DateTimePicker.Size = new System.Drawing.Size(200, 23);
			this.DateTimePicker.TabIndex = 3;
			// 
			// ClanButton
			// 
			this.ClanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ClanButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ClanButton.Location = new System.Drawing.Point(720, 5);
			this.ClanButton.Name = "ClanButton";
			this.ClanButton.Size = new System.Drawing.Size(75, 25);
			this.ClanButton.TabIndex = 5;
			this.ClanButton.Text = "军团";
			this.ClanButton.UseVisualStyleBackColor = true;
			this.ClanButton.Click += new System.EventHandler(this.ClanButton_Click);
			// 
			// SummaryLabel
			// 
			this.SummaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SummaryLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.SummaryLabel.Location = new System.Drawing.Point(5, 420);
			this.SummaryLabel.Name = "SummaryLabel";
			this.SummaryLabel.Size = new System.Drawing.Size(790, 25);
			this.SummaryLabel.TabIndex = 7;
			this.SummaryLabel.Text = "输入昵称，选择日期，查询玩家当天战绩或军团每个成员的数据";
			this.SummaryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Panel
			// 
			this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Panel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Panel.Location = new System.Drawing.Point(0, 30);
			this.Panel.Name = "Panel";
			this.Panel.Size = new System.Drawing.Size(800, 390);
			this.Panel.TabIndex = 8;
			// 
			// OujBoxCombatRecordQueryForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.Panel);
			this.Controls.Add(this.SummaryLabel);
			this.Controls.Add(this.ClanButton);
			this.Controls.Add(this.DateTimePicker);
			this.Controls.Add(this.QueryButton);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.label1);
			this.Name = "OujBoxCombatRecordQueryForm";
			this.Text = "OujBoxCombatRecordQueryForm";
			this.Load += new System.EventHandler(this.OujBoxCombatRecordQueryForm_Load);
			this.Resize += new System.EventHandler(this.OujBoxCombatRecordQueryForm_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NameTextBox;
		private System.Windows.Forms.Button QueryButton;
		private System.Windows.Forms.DateTimePicker DateTimePicker;
		private System.Windows.Forms.Button ClanButton;
		private System.Windows.Forms.Label SummaryLabel;
		private System.Windows.Forms.Panel Panel;
	}
}