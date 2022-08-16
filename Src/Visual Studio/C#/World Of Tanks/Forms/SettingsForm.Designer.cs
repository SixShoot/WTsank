
namespace WorldOfTanks {
	partial class SettingsForm {
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
			this.NetworkRequestIntervalTextBox = new System.Windows.Forms.TextBox();
			this.QueryCacheSizeLabel = new System.Windows.Forms.Label();
			this.ClearQueryCacheButton = new System.Windows.Forms.Button();
			this.MaxQueryHistoryCountTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "网络请求间隔：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// NetworkRequestIntervalTextBox
			// 
			this.NetworkRequestIntervalTextBox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.NetworkRequestIntervalTextBox.Location = new System.Drawing.Point(105, 5);
			this.NetworkRequestIntervalTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.NetworkRequestIntervalTextBox.Name = "NetworkRequestIntervalTextBox";
			this.NetworkRequestIntervalTextBox.Size = new System.Drawing.Size(75, 23);
			this.NetworkRequestIntervalTextBox.TabIndex = 2;
			this.NetworkRequestIntervalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.NetworkRequestIntervalTextBox.TextChanged += new System.EventHandler(this.NetworkRequestIntervalTextBox_TextChanged);
			// 
			// QueryCacheSizeLabel
			// 
			this.QueryCacheSizeLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.QueryCacheSizeLabel.Location = new System.Drawing.Point(5, 35);
			this.QueryCacheSizeLabel.Name = "QueryCacheSizeLabel";
			this.QueryCacheSizeLabel.Size = new System.Drawing.Size(195, 23);
			this.QueryCacheSizeLabel.TabIndex = 3;
			this.QueryCacheSizeLabel.Text = "查询缓存大小：0 KB";
			this.QueryCacheSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ClearQueryCacheButton
			// 
			this.ClearQueryCacheButton.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ClearQueryCacheButton.Location = new System.Drawing.Point(5, 65);
			this.ClearQueryCacheButton.Name = "ClearQueryCacheButton";
			this.ClearQueryCacheButton.Size = new System.Drawing.Size(120, 25);
			this.ClearQueryCacheButton.TabIndex = 28;
			this.ClearQueryCacheButton.Text = "清理查询缓存";
			this.ClearQueryCacheButton.UseVisualStyleBackColor = true;
			this.ClearQueryCacheButton.Click += new System.EventHandler(this.ClearQueryCacheButton_Click);
			// 
			// MaxQueryHistoryCountTextBox
			// 
			this.MaxQueryHistoryCountTextBox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MaxQueryHistoryCountTextBox.Location = new System.Drawing.Point(295, 5);
			this.MaxQueryHistoryCountTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.MaxQueryHistoryCountTextBox.Name = "MaxQueryHistoryCountTextBox";
			this.MaxQueryHistoryCountTextBox.Size = new System.Drawing.Size(75, 23);
			this.MaxQueryHistoryCountTextBox.TabIndex = 30;
			this.MaxQueryHistoryCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.MaxQueryHistoryCountTextBox.TextChanged += new System.EventHandler(this.MaxQueryHistoryCountTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(183, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(127, 23);
			this.label2.TabIndex = 29;
			this.label2.Text = "最大查询历史数：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SettingsForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.MaxQueryHistoryCountTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ClearQueryCacheButton);
			this.Controls.Add(this.QueryCacheSizeLabel);
			this.Controls.Add(this.NetworkRequestIntervalTextBox);
			this.Controls.Add(this.label1);
			this.Name = "SettingsForm";
			this.Text = "SettingsForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NetworkRequestIntervalTextBox;
		private System.Windows.Forms.Label QueryCacheSizeLabel;
		private System.Windows.Forms.Button ClearQueryCacheButton;
		private System.Windows.Forms.TextBox MaxQueryHistoryCountTextBox;
		private System.Windows.Forms.Label label2;
	}
}