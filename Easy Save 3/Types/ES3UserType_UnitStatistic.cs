using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("unitID", "unitProgress", "capybaraGameResult", "anteaterGameResult", "manateeGameResult", "manateeGameResultDictionary")]
	public class ES3UserType_UnitStatistic : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_UnitStatistic() : base(typeof(UnitStatistic)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (UnitStatistic)obj;
			
			writer.WritePrivateField("unitID", instance);
			writer.WriteProperty("unitProgress", instance.unitProgress, ES3UserType_CurrentUnitProgress.Instance);
			writer.WriteProperty("capybaraGameResult", instance.capybaraGameResult, ES3UserType_GameResult.Instance);
			writer.WriteProperty("anteaterGameResult", instance.anteaterGameResult, ES3UserType_GameResultAnteater.Instance);
			writer.WriteProperty("manateeGameResult", instance.manateeGameResult, ES3UserType_GameResultManatee.Instance);
			writer.WriteProperty("manateeGameResultDictionary", instance.manateeGameResultDictionary);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (UnitStatistic)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "unitID":
					reader.SetPrivateField("unitID", reader.Read<System.String>(), instance);
					break;
					case "unitProgress":
						instance.unitProgress = reader.Read<CurrentUnitProgress>(ES3UserType_CurrentUnitProgress.Instance);
						break;
					case "capybaraGameResult":
						instance.capybaraGameResult = reader.Read<GameResult>(ES3UserType_GameResult.Instance);
						break;
					case "anteaterGameResult":
						instance.anteaterGameResult = reader.Read<GameResultAnteater>(ES3UserType_GameResultAnteater.Instance);
						break;
					case "manateeGameResult":
						instance.manateeGameResult = reader.Read<GameResultManatee>(ES3UserType_GameResultManatee.Instance);
						break;
					case "manateeGameResultDictionary":
						instance.manateeGameResultDictionary = reader.Read<System.Collections.Generic.Dictionary<System.Char, GameResultManatee>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new UnitStatistic();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_UnitStatisticArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UnitStatisticArray() : base(typeof(UnitStatistic[]), ES3UserType_UnitStatistic.Instance)
		{
			Instance = this;
		}
	}
}