using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("LetterScorePairs", "WrongWords", "GameLetters", "score")]
	public class ES3UserType_GameResult : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameResult() : base(typeof(GameResult)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (GameResult)obj;
			
			writer.WriteProperty("LetterScorePairs", instance.LetterScorePairs);
			writer.WriteProperty("WrongWords", instance.WrongWords);
			writer.WriteProperty("GameLetters", instance.GameLetters);
			writer.WriteProperty("score", instance.score, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new GameResult();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "LetterScorePairs":
						instance.LetterScorePairs = reader.Read<System.Collections.Generic.Dictionary<System.Char, System.Single>>();
						break;
					case "WrongWords":
						instance.WrongWords = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "GameLetters":
						instance.GameLetters = reader.Read<System.Collections.Generic.List<System.Char>>();
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


	public class ES3UserType_GameResultArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameResultArray() : base(typeof(GameResult[]), ES3UserType_GameResult.Instance)
		{
			Instance = this;
		}
	}
}