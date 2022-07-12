using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("unitID", "unitLetterProgressList", "CapibaraCompleted", "AnteaterCompleted", "BlendingCompleted")]
	public class ES3UserType_CurrentUnitProgress : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CurrentUnitProgress() : base(typeof(CurrentUnitProgress)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (CurrentUnitProgress)obj;
			
			writer.WriteProperty("unitID", instance.unitID);
			writer.WriteProperty("unitLetterProgressList", instance.unitLetterProgressList);
			writer.WriteProperty("CapibaraCompleted", instance.CapibaraCompleted, ES3Type_bool.Instance);
			writer.WriteProperty("AnteaterCompleted", instance.AnteaterCompleted, ES3Type_bool.Instance);
			writer.WriteProperty("BlendingCompleted", instance.BlendingCompleted, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (CurrentUnitProgress)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "unitID":
						instance.unitID = reader.Read<System.Collections.Generic.List<System.Char>>();
						break;
					case "unitLetterProgressList":
						instance.unitLetterProgressList = reader.Read<System.Collections.Generic.List<UnitLetterProgress>>();
						break;
					case "CapibaraCompleted":
						instance.CapibaraCompleted = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "AnteaterCompleted":
						instance.AnteaterCompleted = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "BlendingCompleted":
						instance.BlendingCompleted = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new CurrentUnitProgress();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_CurrentUnitProgressArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CurrentUnitProgressArray() : base(typeof(CurrentUnitProgress[]), ES3UserType_CurrentUnitProgress.Instance)
		{
			Instance = this;
		}
	}
}