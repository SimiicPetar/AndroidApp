using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("letter", "soundLetterCompleted", "tracingLetterCompleted", "manateeGameCompleted")]
	public class ES3UserType_UnitLetterProgress : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_UnitLetterProgress() : base(typeof(UnitLetterProgress)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (UnitLetterProgress)obj;
			
			writer.WriteProperty("letter", instance.letter, ES3Type_char.Instance);
			writer.WriteProperty("soundLetterCompleted", instance.soundLetterCompleted, ES3Type_bool.Instance);
			writer.WriteProperty("tracingLetterCompleted", instance.tracingLetterCompleted, ES3Type_bool.Instance);
			writer.WriteProperty("manateeGameCompleted", instance.manateeGameCompleted, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (UnitLetterProgress)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "letter":
						instance.letter = reader.Read<System.Char>(ES3Type_char.Instance);
						break;
					case "soundLetterCompleted":
						instance.soundLetterCompleted = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "tracingLetterCompleted":
						instance.tracingLetterCompleted = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "manateeGameCompleted":
						instance.manateeGameCompleted = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new UnitLetterProgress();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_UnitLetterProgressArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UnitLetterProgressArray() : base(typeof(UnitLetterProgress[]), ES3UserType_UnitLetterProgress.Instance)
		{
			Instance = this;
		}
	}
}