﻿using System;
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
			TotalCombat += combatRecord.Combat;
			TotalDamage += combatRecord.Damage;
			TotalAssist += combatRecord.Assist;
			TotalShootCount += combatRecord.ShootCount;
			TotalHitCount += combatRecord.HitCount;
			TotalPenetrationCount += combatRecord.PenetrationCount;
			TotalArmorResistence += combatRecord.ArmorResistence;
			TotalSurvivalTime += combatRecord.SurvivalTime;
			TotalXP += combatRecord.XP;
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
			VictoryRate = (float)VictoryNumber / CombatNumber;
			AverageDuration = TotalDuration / CombatNumber;
			AverageCombat = TotalCombat / CombatNumber;
			AverageDamage = TotalDamage / CombatNumber;
			AverageAssist = TotalAssist / CombatNumber;
			AverageHitRate = TotalHitCount / TotalShootCount;
			AveragePenetrationRate = TotalPenetrationCount / TotalHitCount;
			AveragePenetrationRateIncludeNoHit = TotalPenetrationCount / TotalShootCount;
			AverageArmorResistence = TotalArmorResistence / CombatNumber;
			AverageSurvivalTime = TotalSurvivalTime / CombatNumber;
			AverageXP = TotalXP / CombatNumber;
		}

	}

}