using System;

namespace WorldOfTanks {

	class SpottingPlayer {

		public float ViewRange { get; set; }
		public float StaticConcealment { get; set; }
		public float MoveConcealment { get; set; }
		public bool IsMoving { get; set; }
		public CommanderVisionSystemType CommanderVisionSystemType { get; set; }
		public float CommanderVisionSystemValueByFoliage {

			get => _CommanderVisionSystemValueByFoliage;

			set {
				_CommanderVisionSystemValueByFoliage = value;
				if (CanInput ()) {
					CustomCommanderVisionSystemValueByFoliage = value;
				}
			}

		}
		public float CommanderVisionSystemValueByMove {

			get => _CommanderVisionSystemValueByMove;

			set {
				_CommanderVisionSystemValueByMove = value;
				if (CanInput ()) {
					CustomCommanderVisionSystemValueByMove = value;
				}
			}

		}
		public float CustomCommanderVisionSystemValueByFoliage { get; set; }
		public float CustomCommanderVisionSystemValueByMove { get; set; }
		public int SmallFoliageNumber { get; set; }
		public int LargeFoliageNumber { get; set; }

		float _CommanderVisionSystemValueByFoliage;
		float _CommanderVisionSystemValueByMove;

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