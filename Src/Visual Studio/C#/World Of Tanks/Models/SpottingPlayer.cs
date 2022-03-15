using System;

namespace WorldOfTanks {

	class SpottingPlayer {

		public double ViewRange { get; set; }
		public double StaticConcealment { get; set; }
		public double MoveConcealment { get; set; }
		public bool IsMoving { get; set; }
		public CommanderVisionSystemType CommanderVisionSystemType { get; set; }
		public double CommanderVisionSystemValueByFoliage {

			get => _CommanderVisionSystemValueByFoliage;

			set {
				_CommanderVisionSystemValueByFoliage = value;
				if (CanInput ()) {
					CustomCommanderVisionSystemValueByFoliage = value;
				}
			}

		}
		public double CommanderVisionSystemValueByMove {

			get => _CommanderVisionSystemValueByMove;

			set {
				_CommanderVisionSystemValueByMove = value;
				if (CanInput ()) {
					CustomCommanderVisionSystemValueByMove = value;
				}
			}

		}
		public double CustomCommanderVisionSystemValueByFoliage { get; set; }
		public double CustomCommanderVisionSystemValueByMove { get; set; }
		public int SmallFoliageNumber { get; set; }
		public int LargeFoliageNumber { get; set; }

		double _CommanderVisionSystemValueByFoliage;
		double _CommanderVisionSystemValueByMove;

		public bool CanInput () {
			switch (CommanderVisionSystemType) {
				case CommanderVisionSystemType.SpecialSlot:
				case CommanderVisionSystemType.NonSpecialSlot:
				case CommanderVisionSystemType.None:
					return false;
				case CommanderVisionSystemType.Custom:
					return true;
				default:
					throw new NotImplementedException (CommanderVisionSystemType.ToString ());
			}
		}

	}

}