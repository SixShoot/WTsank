﻿
namespace WorldOfTanks {
	partial class Form1 {
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose (bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent () {
			this.FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.OujBoxCombatRecordQueryRadioButton = new System.Windows.Forms.RadioButton();
			this.SpottingDistanceCalculatorRadioButton = new System.Windows.Forms.RadioButton();
			this.AimTimeCalculatorRadioButton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.Panel = new System.Windows.Forms.Panel();
			this.FlowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// FlowLayoutPanel
			// 
			this.FlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.FlowLayoutPanel.Controls.Add(this.OujBoxCombatRecordQueryRadioButton);
			this.FlowLayoutPanel.Controls.Add(this.SpottingDistanceCalculatorRadioButton);
			this.FlowLayoutPanel.Controls.Add(this.AimTimeCalculatorRadioButton);
			this.FlowLayoutPanel.Controls.Add(this.label1);
			this.FlowLayoutPanel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.FlowLayoutPanel.Name = "FlowLayoutPanel";
			this.FlowLayoutPanel.Size = new System.Drawing.Size(150, 604);
			this.FlowLayoutPanel.TabIndex = 1;
			// 
			// OujBoxCombatRecordQueryRadioButton
			// 
			this.OujBoxCombatRecordQueryRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.OujBoxCombatRecordQueryRadioButton.Location = new System.Drawing.Point(3, 3);
			this.OujBoxCombatRecordQueryRadioButton.Name = "OujBoxCombatRecordQueryRadioButton";
			this.OujBoxCombatRecordQueryRadioButton.Size = new System.Drawing.Size(147, 40);
			this.OujBoxCombatRecordQueryRadioButton.TabIndex = 4;
			this.OujBoxCombatRecordQueryRadioButton.TabStop = true;
			this.OujBoxCombatRecordQueryRadioButton.Text = "偶游盒子战绩查询";
			this.OujBoxCombatRecordQueryRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.OujBoxCombatRecordQueryRadioButton.UseVisualStyleBackColor = true;
			this.OujBoxCombatRecordQueryRadioButton.CheckedChanged += new System.EventHandler(this.OujBoxCombatRecordQueryRadioButton_CheckedChanged);
			// 
			// SpottingDistanceCalculatorRadioButton
			// 
			this.SpottingDistanceCalculatorRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.SpottingDistanceCalculatorRadioButton.Location = new System.Drawing.Point(3, 49);
			this.SpottingDistanceCalculatorRadioButton.Name = "SpottingDistanceCalculatorRadioButton";
			this.SpottingDistanceCalculatorRadioButton.Size = new System.Drawing.Size(147, 40);
			this.SpottingDistanceCalculatorRadioButton.TabIndex = 5;
			this.SpottingDistanceCalculatorRadioButton.TabStop = true;
			this.SpottingDistanceCalculatorRadioButton.Text = "点亮距离计算器";
			this.SpottingDistanceCalculatorRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.SpottingDistanceCalculatorRadioButton.UseVisualStyleBackColor = true;
			this.SpottingDistanceCalculatorRadioButton.CheckedChanged += new System.EventHandler(this.SpottingDistanceCalculatorRadioButton_CheckedChanged);
			// 
			// AimTimeCalculatorRadioButton
			// 
			this.AimTimeCalculatorRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.AimTimeCalculatorRadioButton.Location = new System.Drawing.Point(3, 95);
			this.AimTimeCalculatorRadioButton.Name = "AimTimeCalculatorRadioButton";
			this.AimTimeCalculatorRadioButton.Size = new System.Drawing.Size(147, 40);
			this.AimTimeCalculatorRadioButton.TabIndex = 6;
			this.AimTimeCalculatorRadioButton.TabStop = true;
			this.AimTimeCalculatorRadioButton.Text = "瞄准时间计算器";
			this.AimTimeCalculatorRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.AimTimeCalculatorRadioButton.UseVisualStyleBackColor = true;
			this.AimTimeCalculatorRadioButton.CheckedChanged += new System.EventHandler(this.AimTimeCalculatorRadioButton_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(3, 138);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 40);
			this.label1.TabIndex = 2;
			this.label1.Text = "作者：Eruru\r\nQQ：1633756198";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Panel
			// 
			this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Panel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Panel.Location = new System.Drawing.Point(150, 0);
			this.Panel.Margin = new System.Windows.Forms.Padding(0);
			this.Panel.Name = "Panel";
			this.Panel.Size = new System.Drawing.Size(899, 605);
			this.Panel.TabIndex = 2;
			this.Panel.Resize += new System.EventHandler(this.Panel_Resize);
			// 
			// Form1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1049, 604);
			this.Controls.Add(this.Panel);
			this.Controls.Add(this.FlowLayoutPanel);
			this.IsMdiContainer = true;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "坦克世界";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.FlowLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel;
		private System.Windows.Forms.Panel Panel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton OujBoxCombatRecordQueryRadioButton;
		private System.Windows.Forms.RadioButton SpottingDistanceCalculatorRadioButton;
		private System.Windows.Forms.RadioButton AimTimeCalculatorRadioButton;
	}
}
