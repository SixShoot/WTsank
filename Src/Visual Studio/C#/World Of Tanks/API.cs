using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WorldOfTanks {

	class API {

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

		public static void Initialize () {
			for (int i = 0; i < CombatColors.Length; i++) {
				if (i > 0) {
					CombatColors[i].Min = CombatColors[i - 1].Max;
				}
				CombatColors[i].Difference = CombatColors[i].Max - CombatColors[i].Min;
				CombatColors[i].Middle = CombatColors[i].Min + CombatColors[i].Difference / 2;
			}
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
				return Lerp (Color.LightCoral, Color.LightGreen, Clamp (API.Divide (combat - min, basic - min), 0, 1));
			}
			return Lerp (Color.LightGreen, Color.Gold, Clamp (API.Divide (combat - basic, max - basic), 0, 1));
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