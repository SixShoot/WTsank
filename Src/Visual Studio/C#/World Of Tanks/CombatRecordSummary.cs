using System;
using System.Collections.Generic;

namespace WorldOfTanks {

	class CombatRecordSummary {

		public int CombatNumber { get; set; }
		public int VictoryNumber { get; set; }
		public int FailNumber { get; set; }
		public int EvenNumber { get; set; }
		public float VictoryRate { get; set; }
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
		public Dictionary<string, CombatRecordSummary> Tanks { get; set; } = new Dictionary<string, CombatRecordSummary> ();

		public void Append (CombatRecord combatRecord) {
			CombatNumber++;
			AddCombatResult (combatRecord.Result);
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

		public void AddCombatResult (CombatResult combatResult) {
			switch (combatResult) {
				case CombatResult.Victory:
					VictoryNumber++;
					break;
				case CombatResult.Fail:
					FailNumber++;
					break;
				case CombatResult.Even:
					EvenNumber++;
					break;
				default:
					throw new NotImplementedException (combatResult.ToString ());
			}
		}

		public void Summary () {
			VictoryRate = API.Divide ((float)VictoryNumber, CombatNumber);
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