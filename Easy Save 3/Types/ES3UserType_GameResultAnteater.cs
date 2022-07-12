using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("LetterScorePairs", "WrongLetters", "AllLetters", "score")]
	public class ES3UserType_GameResultAnteater : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameResultAnteater() : base(typeof(GameResultAnteater)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (GameResultAnteater)obj;
			
			writer.WriteProperty("LetterScorePairs", instance.LetterScorePairs);
			writer.WriteProperty("WrongLetters", instance.WrongLetters);
			writer.WriteProperty("AllLetters", instance.AllLetters);
			writer.WriteProperty("score", instance.score, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new GameResultAnteater();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "LetterScorePairs":
						instance.LetterScorePairs = reader.Read<System.Collections.Generic.Dictionary<System.Char, System.Single>>();
						break;
					case "WrongLetters":
						instance.WrongLetters = reader.Read<System.Collections.Generic.List<System.Char>>();
						break;
					case "AllLetters":
						instance.AllLetters = reader.Read<System.Collections.Generic.List<System.Char>>();
						break;
					case "score":
						instance.score = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_GameResultAnteaterArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameResultAnteaterArray() : base(typeof(GameResultAnteater[]), ES3UserType_GameResultAnteater.Instance)
		{
			Instance = this;
		}
	}
}