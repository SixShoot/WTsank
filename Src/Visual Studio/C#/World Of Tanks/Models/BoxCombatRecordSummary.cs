using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldOfTanks {

	class BoxCombatRecordSummary {

		public int CombatNumber { get; set; }
		public int WinNumber { get; set; }
		public int FailNumber { get; set; }
		public int EvenNumber { get; set; }
		public int MeetSPGNumber { get; set; }
		public int MeetOneSPGNumber { get; set; }
		public int MeetTwoSPGNumber { get; set; }
		public int MeetThreeSPGNumber { get; set; }
		public int MinusTwoLevelNumber { get; set; }
		public int MinusOneLevelNumber { get; set; }
		public int SameLevelNumber { get; set; }
		public int MiddleLevelNumber { get; set; }
		public int PlusOneLevelNumber { get; set; }
		public int PlusTwoLevelNumber { get; set; }
		public float WinRate { get; set; }
		public int TotalSPGNumber { get; set; }
		public float TotalDuration { get; set; }
		public float TotalCombat { get; set; }
		public List<float> Combats { get; set; } = new List<float> ();
		public float TotalDamage { get; set; }
		public float TotalAssist { get; set; }
		public float TotalShootCount { get; set; }
		public float TotalHitCount { get; set; }
		public float TotalPenetrationCount { get; set; }
		public float TotalArmorResistence { get; set; }
		public float TotalSurvivalTime { get; set; }
		public float TotalXP { get; set; }
		public float MeetSPGRate { get; set; }
		public float MeetOneSPGRate { get; set; }
		public float MeetTwoSPGRate { get; set; }
		public float MeetThreeSPGRate { get; set; }
		public float MinusTwoLevelRate { get; set; }
		public float MinusOneLevelRate { get; set; }
		public float SameLevelRate { get; set; }
		public float MiddleLevelRate { get; set; }
		public float PlusOneLevelRate { get; set; }
		public float PlusTwoLevelRate { get; set; }
		public float AverageSPGNumber { get; set; }
		public float AverageDuration { get; set; }
		public float AverageCombat { get; set; }
		public float MedianCombat { get; set; }
		public float AverageDamage { get; set; }
		public float AverageAssist { get; set; }
		public float AverageHitRate { get; set; }
		public float AveragePenetrationRate { get; set; }
		public float AveragePenetrationRateIncludeNoHit { get; set; }
		public float AverageArmorResistence { get; set; }
		public float AverageSurvivalTime { get; set; }
		public float AverageXP { get; set; }
		public Dictionary<string, BoxCombatRecordSummary> Tanks { get; set; } = new Dictionary<string, BoxCombatRecordSummary> ();

		public void AddCombatRecord (BoxCombatRecord combatRecord) {
			CombatNumber++;
			switch (combatRecord.Result) {
				case CombatResult.Victory:
					WinNumber++;
					break;
				case CombatResult.Fail:
					FailNumber++;
					break;
				case CombatResult.Even:
					EvenNumber++;
					break;
				default:
					throw new NotImplementedException (combatRecord.Result.ToString ());
			}
			List<Tank> enemyTanks = new List<Tank> ();
			for (int i = 0; i < combatRecord.EnemyTeamPlayers.Count; i++) {
				Tank tank = tank = WarGamingNetService.Instance.GetTankByName (combatRecord.EnemyTeamPlayers[i]["tank_title"]);
				enemyTanks.Add (tank);
			}
			int spgNumber = enemyTanks.Count (item => item?.Type == TankType.SPG);
			if (spgNumber > 0) {
				MeetSPGNumber++;
				switch (spgNumber) {
					case 1:
						MeetOneSPGNumber++;
						break;
					case 2:
						MeetTwoSPGNumber++;
						break;
					case 3:
						MeetThreeSPGNumber++;
						break;
				}
			}
			List<Tank> playerTanks = new List<Tank> ();
			int playerLevel = WarGamingNetService.Instance.GetTankByName (combatRecord.TeamPlayer.TankName)?.Tier ?? -1;
			int minLevel = playerLevel, maxLevel = playerLevel;
			for (int i = 0; i < combatRecord.PlayerTeamPlayers.Count; i++) {
				Tank tank = WarGamingNetService.Instance.GetTankByName (combatRecord.PlayerTeamPlayers[i]["tank_title"]);
				playerTanks.Add (tank);
				if (tank != null) {
					if (minLevel == -1) {
						minLevel = tank.Tier;
						maxLevel = tank.Tier;
						continue;
					}
					if (tank.Tier < minLevel) {
						minLevel = tank.Tier;
					}
					if (tank.Tier > maxLevel) {
						maxLevel = tank.Tier;
					}
				}
			}
			if (playerLevel > 0) {
				if (maxLevel == playerLevel && minLevel == playerLevel) {
					SameLevelNumber++;
				} else if (maxLevel == playerLevel) {
					if (minLevel == playerLevel - 2) {
						MinusTwoLevelNumber++;
					} else if (minLevel == playerLevel - 1) {
						MinusOneLevelNumber++;
					}
				} else if (minLevel == playerLevel) {
					if (maxLevel == playerLevel + 1) {
						PlusOneLevelNumber++;
					} else if (maxLevel == playerLevel + 2) {
						PlusTwoLevelNumber++;
					}
				} else if ((minLevel == playerLevel - 1) && (maxLevel == playerLevel + 1)) {
					MiddleLevelNumber++;
				}
			}
			TotalSPGNumber += spgNumber;
			TotalDuration += combatRecord.Duration;
			TotalCombat += combatRecord.TeamPlayer.Combat;
			Combats.Add (combatRecord.TeamPlayer.Combat);
			TotalDamage += combatRecord.TeamPlayer.Damage;
			TotalAssist += combatRecord.TeamPlayer.Assist;
			TotalShootCount += combatRecord.TeamPlayer.ShootCount;
			TotalHitCount += combatRecord.TeamPlayer.HitCount;
			TotalPenetrationCount += combatRecord.TeamPlayer.PenetrationCount;
			TotalArmorResistence += combatRecord.TeamPlayer.ArmorResistence;
			TotalSurvivalTime += combatRecord.TeamPlayer.SurvivalTime;
			TotalXP += combatRecord.TeamPlayer.XP;
		}

		public void Summary () {
			WinRate = API.Divide (WinNumber, CombatNumber);
			MeetSPGRate = API.Divide (MeetSPGNumber, CombatNumber);
			MeetOneSPGRate = API.Divide (MeetOneSPGNumber, CombatNumber);
			MeetTwoSPGRate = API.Divide (MeetTwoSPGNumber, CombatNumber);
			MeetThreeSPGRate = API.Divide (MeetThreeSPGNumber, CombatNumber);
			MinusTwoLevelRate = API.Divide (MinusTwoLevelNumber, CombatNumber);
			MinusOneLevelRate = API.Divide (MinusOneLevelNumber, CombatNumber);
			SameLevelRate = API.Divide (SameLevelNumber, CombatNumber);
			MiddleLevelRate = API.Divide (MiddleLevelNumber, CombatNumber);
			PlusOneLevelRate = API.Divide (PlusOneLevelNumber, CombatNumber);
			PlusTwoLevelRate = API.Divide (PlusTwoLevelNumber, CombatNumber);
			AverageSPGNumber = API.Divide (TotalSPGNumber, CombatNumber);
			AverageDuration = API.Divide (TotalDuration, CombatNumber);
			AverageCombat = API.Divide (TotalCombat, CombatNumber);
			Combats.Sort ();
			MedianCombat = API.GetMedian (Combats);
			AverageDamage = API.Divide (TotalDamage, CombatNumber);
			AverageAssist = API.Divide (TotalAssist, CombatNumber);
			AverageHitRate = API.Divide (TotalHitCount, TotalShootCount);
			AveragePenetrationRate = API.Divide (TotalPenetrationCount, TotalHitCount);
			AveragePenetrationRateIncludeNoHit = API.Divide (TotalPenetrationCount, TotalShootCount);
			AverageArmorResistence = API.Divide (TotalArmorResistence, CombatNumber);
			AverageSurvivalTime = API.Divide (TotalSurvivalTime, CombatNumber);
			AverageXP = API.Divide (TotalXP, CombatNumber);
		}

	}

}