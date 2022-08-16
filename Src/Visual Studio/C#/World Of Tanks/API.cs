﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WorldOfTanks {

	class Api {

		public const string DateTimeFormatText = "yyyy年MM月dd日 HH时mm分ss秒";
		public const string DateTimeExcludeSecondFormatText = "yyyy年MM月dd日 HH时mm分";
		public const string DateFormatText = "yyyy年MM月dd日";

		static readonly CombatColor[] CombatColors = {
			new CombatColor (600, "FFFFFF", "BB0022"),//0-600
			new CombatColor (800, "FFFFFF", "FF4444"),//600-800
			new CombatColor (1000, "FFFFFF", "000000", "22BB22"),//800-1000
			new CombatColor (1200, "000000", "55FF55"),//1000-1200
			new CombatColor (1400, "000000", "FFFFFF", "66AAFF"),//1200-1400
			new CombatColor (1600, "FFFFFF", "CC44FF"),//1400-1600
			new CombatColor (1800, "FFFFFF", "000000", "DDCC00"),//1600-1800
			new CombatColor (2000, "000000", "FFAA33")//1800-2000
		};
		static readonly Random Random = new Random ();
		static readonly string[] FileUnits = { "B", "KB", "MB", "GB", "TB" };

		public static void Initialize () {
			for (int i = 0; i < CombatColors.Length; i++) {
				if (i > 0) {
					CombatColors[i].Min = CombatColors[i - 1].Max;
				}
				CombatColors[i].Difference = CombatColors[i].Max - CombatColors[i].Min;
				CombatColors[i].Middle = CombatColors[i].Min + CombatColors[i].Difference / 2;
			}
		}

		public static string FormatFileSize (long byteLength) {
			float size = byteLength;
			int index = 0;
			while (size >= 1024) {
				index++;
				size /= 1024;
			}
			return string.Format ("{0:F2} {1}", size, FileUnits[index]);
		}

		public static void Invoke (Control control, Action action, params object[] args) {
			if (control.IsDisposed || control.Disposing) {
				return;
			}
			if (control.InvokeRequired) {
				control.Invoke (action, args);
				return;
			}
			action.DynamicInvoke (args);
		}

		public static void SetCompareColor (Control a, Control b, int compareResult) {
			if (compareResult == 0) {
				a.ForeColor = Color.Black;
				a.BackColor = Color.White;
				b.ForeColor = Color.Black;
				b.BackColor = Color.White;
				return;
			}
			if (compareResult > 0) {
				a.ForeColor = Color.Black;
				a.BackColor = Color.LightGreen;
				b.ForeColor = Color.White;
				b.BackColor = Color.LightCoral;
				return;
			}
			a.ForeColor = Color.White;
			a.BackColor = Color.LightCoral;
			b.ForeColor = Color.Black;
			b.BackColor = Color.LightGreen;
		}

		public static Color GetCombatColor (float combat, float basic, float min, float max) {
			if (combat <= basic) {
				return Lerp (Color.LightCoral, Color.LightGreen, Clamp (Api.Divide (combat - min, basic - min), 0, 1));
			}
			return Lerp (Color.LightGreen, Color.Gold, Clamp (Api.Divide (combat - basic, max - basic), 0, 1));
		}
		public static Color GetCombatColor (float combat, float basic) {
			return GetCombatColor (combat, basic, basic - 500, basic + 500);
		}
		public static Color GetCombatColor (float combat, out Color contrastColor) {
			for (int i = 0; i < CombatColors.Length; i++) {
				if (combat >= CombatColors[i].Min && combat < CombatColors[i].Max) {
					//return CombatColors[i].Color;
					int startIndex = i;
					int endIndex = i;
					if (combat < CombatColors[i].Middle) {
						startIndex--;
						contrastColor = CombatColors[i].DoubleColor.ForeColor;
					} else {
						endIndex++;
						contrastColor = CombatColors[i].RightForeColor;
					}
					DoubleColor startColor = CombatColors[startIndex < 0 ? 0 : startIndex].DoubleColor;
					DoubleColor endColor = CombatColors[endIndex >= CombatColors.Length ? CombatColors.Length - 1 : endIndex].DoubleColor;
					float startCombat = startIndex < 0 ? CombatColors[0].Min : CombatColors[startIndex].Middle;
					float endCombat = endIndex >= CombatColors.Length ? CombatColors[CombatColors.Length - 1].Max : CombatColors[endIndex].Middle;
					float difference = endCombat - startCombat;
					return Lerp (startColor.BackColor, endColor.BackColor, Clamp (Divide (combat - startCombat, difference), 0, 1));
				}
			}
			contrastColor = CombatColors[CombatColors.Length - 1].DoubleColor.ForeColor;
			return CombatColors[CombatColors.Length - 1].DoubleColor.BackColor;
		}
		public static DoubleColor GetCombatColor (float combat) {
			Color backColor = GetCombatColor (combat, out Color foreColor);
			return new DoubleColor (foreColor, backColor);
		}

		public static Color HexToColor (string hex) {
			return Color.FromArgb (
				int.Parse (hex.Substring (0, 2), NumberStyles.AllowHexSpecifier),
				int.Parse (hex.Substring (2, 2), NumberStyles.AllowHexSpecifier),
				int.Parse (hex.Substring (4, 2), NumberStyles.AllowHexSpecifier)
			);
		}

		public static bool IsLightColor (Color color) {
			return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255 > 0.5;
		}

		public static Color ReverseColor (Color color) {
			return Color.FromArgb (color.R ^ 255, color.G ^ 255, color.B ^ 255);
		}

		public static Color ContrastColor (Color color) {
			return IsLightColor (color) ? Color.Black : Color.White;
		}

		public static float Clamp (float value, float min, float max) {
			return value < min ? min : (value > max ? max : value);
		}

		public static float GetMedian (IList<float> list) {
			if (list.Count == 0) {
				return 0;
			}
			int half = list.Count / 2;
			if (list.Count % 2 == 0) {
				return (list[half - 1] + list[half]) / 2;
			} else {
				return list[half];
			}
		}

		public static void AutoResizeListViewColumns (ListView listView, bool autoWidth = false) {
			int width = listView.Width;
			int totalWidth = 0;
			listView.BeginUpdate ();
			listView.Width = 0;
			int[] widths = new int[listView.Columns.Count];
			listView.AutoResizeColumns (ColumnHeaderAutoResizeStyle.HeaderSize);
			for (int i = 0; i < listView.Columns.Count; i++) {
				widths[i] = listView.Columns[i].Width;
			}
			listView.AutoResizeColumns (ColumnHeaderAutoResizeStyle.ColumnContent);
			for (int i = 0; i < listView.Columns.Count; i++) {
				if (listView.Columns[i].Width < widths[i]) {
					listView.Columns[i].Width = widths[i];
				}
				totalWidth += listView.Columns[i].Width + 2;
			}
			totalWidth += SystemInformation.VerticalScrollBarWidth;
			listView.Width = autoWidth ? totalWidth : width;
			listView.EndUpdate ();
		}

		public static void ExportCSV (ListView leftListView, ListView listView, string filePath) {
			FileInfo fileInfo = new FileInfo (filePath);
			Directory.CreateDirectory (fileInfo.DirectoryName);
			var encoding = new UTF8Encoding (true);
			using (StreamWriter streamWriter = new StreamWriter (File.Create (fileInfo.FullName), encoding)) {
				int columnNumber = leftListView.Columns.Count + listView.Columns.Count + 1;
				int rowNumber = Math.Max (leftListView.Items.Count, listView.Items.Count);
				for (int row = -1; row < rowNumber; row++) {
					if (row > -1) {
						streamWriter.WriteLine ();
					}
					for (int column = 0; column < columnNumber; column++) {
						if (column > 0) {
							streamWriter.Write (',');
						}
						if (row == -1) {
							if (column < leftListView.Columns.Count) {
								streamWriter.Write (leftListView.Columns[column].Text);
								continue;
							}
							if (column == leftListView.Columns.Count) {
								continue;
							}
							streamWriter.Write (listView.Columns[column - leftListView.Columns.Count - 1].Text);
							continue;
						}
						if (column < leftListView.Columns.Count) {
							if (row < leftListView.Items.Count) {
								streamWriter.Write (leftListView.Items[row].SubItems[column].Text);
							}
							continue;
						}
						if (column == leftListView.Columns.Count) {
							continue;
						}
						if (row < listView.Items.Count) {
							streamWriter.Write (listView.Items[row].SubItems[column - leftListView.Columns.Count - 1].Text);
						}
					}
				}
			}
			MessageBox.Show ("完成");
		}

		public static bool CheckDateTime (DateTime startDateTime, DateTime endDateTime) {
			if (endDateTime.Date > DateTime.Now.Date) {
				MessageBox.Show ("结束日期不能大于今天");
				return false;
			}
			if (startDateTime.Date > endDateTime.Date) {
				MessageBox.Show ("开始日期不能大于结束日期");
				return false;
			}
			return true;
		}

		public static Color Lerp (Color startColor, Color endColor, float amount) {
			return Color.FromArgb (
				(int)Lerp (startColor.R, endColor.R, amount),
				(int)Lerp (startColor.G, endColor.G, amount),
				(int)Lerp (startColor.B, endColor.B, amount)
			);
		}
		public static float Lerp (float a, float b, float amount) {
			return a + (b - a) * amount;
		}

		public static string CombatResultToString (CombatResult combatResult) {
			switch (combatResult) {
				case CombatResult.Victory:
					return "胜利";
				case CombatResult.Fail:
					return "失败";
				case CombatResult.Even:
					return "平局";
				default:
					throw new NotImplementedException (combatResult.ToString ());
			}
		}

		public static float Divide (float a, float b) {
			return b == 0 ? 0 : a / b;
		}
		public static double Divide (double a, double b) {
			return b == 0 ? 0 : a / b;
		}

		public static TankType ParseTankType (string type) {
			switch (type) {
				default:
					throw new NotImplementedException (type);
				case "lightTank":
				case "轻坦":
				case "轻型坦克":
					return TankType.LightTank;
				case "mediumTank":
				case "中坦":
				case "中型坦克":
					return TankType.MediumTank;
				case "heavyTank":
				case "重坦":
				case "重型坦克":
					return TankType.HeavyTank;
				case "AT-SPG":
				case "反坦":
				case "自行反坦克炮":
					return TankType.TankDestroyer;
				case "SPG":
				case "火炮":
				case "自行火炮":
					return TankType.SPG;
			}
		}

		public static string TankTypeLocalization (TankType type, bool shortName = false) {
			switch (type) {
				default:
					throw new NotImplementedException (type.ToString ());
				case TankType.LightTank:
					return shortName ? "轻坦" : "轻型坦克";
				case TankType.MediumTank:
					return shortName ? "中坦" : "中型坦克";
				case TankType.HeavyTank:
					return shortName ? "重坦" : "重型坦克";
				case TankType.TankDestroyer:
					return shortName ? "反坦" : "自行反坦克炮";
				case TankType.SPG:
					return shortName ? "火炮" : "自行火炮";
			}
		}

		public static float TankLevelCombatLimit (int level, float combat) {
			switch (level) {
				case 1:
				case 2:
				case 3:
				case 4:
					return Math.Min (combat, 1000);
				case 5:
					return Math.Min (combat, 1350);
				case 6:
					return Math.Min (combat, 1500);
				case 7:
					return Math.Min (combat, 1650);
				case 8:
					return Math.Min (combat, 2000);
				case 9:
					return Math.Min (combat, 2300);
			}
			return combat;
		}

		#region 生成高斯分布数
		// mean：均值，variance：方差
		// min和max用于去掉不需要的偏差值
		public static double NextGaussian (double mean, double variance, double min, double max) {
			double x;
			do {
				x = NextGaussian (mean, variance);
			} while (x < min || x > max);
			return x;
		}

		public static double NextGaussian (double mean, double standard_deviation) {
			return mean + NextGaussian () * standard_deviation;
		}

		public static double NextGaussian () {
			double v1, v2, s;
			do {
				v1 = 2.0f * Random.NextDouble () - 1.0f;
				v2 = 2.0f * Random.NextDouble () - 1.0f;
				s = v1 * v1 + v2 * v2;
			} while (s >= 1.0f || s == 0f);
			s = Math.Sqrt ((-2.0f * Math.Log (s)) / s);
			return v1 * s;
		}
		#endregion

		public static bool PointInsideCircle (double x, double y, double radius) {
			return Math.Sqrt (Math.Pow (x, 2) + Math.Pow (y, 2)) <= radius;
		}

		class CombatColor {

			public float Max { get; set; }
			public float Middle { get; set; }
			public float Min { get; set; }
			public float Difference { get; set; }
			public Color RightForeColor;
			public DoubleColor DoubleColor { get; set; }

			public CombatColor (float max, string leftForeColor, string rightForeColor, string backColor) {
				Max = max;
				RightForeColor = HexToColor (rightForeColor);
				DoubleColor = new DoubleColor (HexToColor (leftForeColor), HexToColor (backColor));
			}
			public CombatColor (float max, string foreColor, string backColor) : this (max, foreColor, foreColor, backColor) {

			}

		}

	}

}