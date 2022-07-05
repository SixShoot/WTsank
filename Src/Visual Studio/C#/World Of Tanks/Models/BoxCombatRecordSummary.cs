using Eruru.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldOfTanks {

	class BoxCombatRecordSummary {

		public int TankLevelCount { get; set; }
		public int TankLevel { get; set; }
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
		public float TotalTankLevel { get; set; }
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
		public float PredictCombat { get; set; }
		public float AverageDamage { get; set; }
		public float AverageAssist { get; set; }
		public float AverageHitRate { get; set; }
		public float AveragePenetrationRate { get; set; }
		public float AveragePenetrationRateIncludeNoHit { get; set; }
		public float AverageArmorResistence { get; set; }
		public float AverageSurvivalTime { get; set; }
		public float AverageXP { get; set; }
		public float AverageTankLevel { get; set; }
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
			List<Tank> enemyTeamTanks = new List<Tank> ();
			foreach (JsonValue enemyTeamPlayer in combatRecord.EnemyTeamPlayers) {
				enemyTeamTanks.Add (WarGamingNetService.Instance.GetTankByName (enemyTeamPlayer["tank_title"]));
			}
			List<Tank> playerTeamTanks = new List<Tank> ();
			foreach (JsonValue playerTeamPlayer in combatRecord.PlayerTeamPlayers) {
				playerTeamTanks.Add (WarGamingNetService.Instance.GetTankByName (playerTeamPlayer["tank_title"]));
			}
			int spgNumber = Math.Max (enemyTeamTanks.Count (item => item?.Type == TankType.SPG), playerTeamTanks.Count (item => item?.Type == TankType.SPG));
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
			Tank playerTank = WarGamingNetService.Instance.GetTankByName (combatRecord.TeamPlayer.TankName);
			int playerLevel;
			if (playerTank == null) {
				Console.WriteLine ($"没有找到名为：{combatRecord.TeamPlayer.TankName} 的玩家坦克，无法推断分房等级");
				playerLevel = -1;
			} else {
				playerLevel = playerTank.Tier;
			}
			int minLevel = playerLevel, maxLevel = playerLevel;
			List<Tank> allTanks = new List<Tank> ();
			allTanks.AddRange (enemyTeamTanks);
			allTanks.AddRange (playerTeamTanks);
			foreach (Tank tank in allTanks) {
				if (tank == null) {
					continue;
				}
				if (minLevel == -1) {
					minLevel = tank.Tier;
					maxLevel = tank.Tier;
					continue;
				}
				if (tank.Tier < minLevel && (tank.Tier >= maxLevel - 2)) {
					minLevel = tank.Tier;
				}
				if (tank.Tier > maxLevel && (tank.Tier <= minLevel + 2)) {
					maxLevel = tank.Tier;
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

		public void Summary (string tankName = null) {
			WinRate = Api.Divide (WinNumber, CombatNumber);
			MeetSPGRate = Api.Divide (MeetSPGNumber, CombatNumber);
			MeetOneSPGRate = Api.Divide (MeetOneSPGNumber, CombatNumber);
			MeetTwoSPGRate = Api.Divide (MeetTwoSPGNumber, CombatNumber);
			MeetThreeSPGRate = Api.Divide (MeetThreeSPGNumber, CombatNumber);
			MinusTwoLevelRate = Api.Divide (MinusTwoLevelNumber, CombatNumber);
			MinusOneLevelRate = Api.Divide (MinusOneLevelNumber, CombatNumber);
			SameLevelRate = Api.Divide (SameLevelNumber, CombatNumber);
			MiddleLevelRate = Api.Divide (MiddleLevelNumber, CombatNumber);
			PlusOneLevelRate = Api.Divide (PlusOneLevelNumber, CombatNumber);
			PlusTwoLevelRate = Api.Divide (PlusTwoLevelNumber, CombatNumber);
			AverageSPGNumber = Api.Divide (TotalSPGNumber, CombatNumber);
			AverageDuration = Api.Divide (TotalDuration, CombatNumber);
			AverageCombat = Api.Divide (TotalCombat, CombatNumber);
			Combats.Sort ();
			MedianCombat = Api.GetMedian (Combats);
			PredictCombat = Api.TankLevelCombatLimit (TankLevel, (1 + Api.Divide (WinRate - 0.5F, 0.5F)) * AverageCombat);
			AverageDamage = Api.Divide (TotalDamage, CombatNumber);
			AverageAssist = Api.Divide (TotalAssist, CombatNumber);
			AverageHitRate = Api.Divide (TotalHitCount, TotalShootCount);
			AveragePenetrationRate = Api.Divide (TotalPenetrationCount, TotalHitCount);
			AveragePenetrationRateIncludeNoHit = Api.Divide (TotalPenetrationCount, TotalShootCount);
			AverageArmorResistence = Api.Divide (TotalArmorResistence, CombatNumber);
			AverageSurvivalTime = Api.Divide (TotalSurvivalTime, CombatNumber);
			AverageXP = Api.Divide (TotalXP, CombatNumber);
			AverageTankLevel = Api.Divide (TotalTankLevel, TankLevelCount);
			if (MinusTwoLevelNumber + MinusOneLevelNumber + SameLevelNumber + MiddleLevelNumber + PlusOneLevelNumber + PlusTwoLevelNumber != CombatNumber) {
				Console.WriteLine ($"{tankName} 所有分房等级次数和战斗次数不匹配");
			}
		}

	}

}