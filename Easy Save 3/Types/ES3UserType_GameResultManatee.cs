using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("WrongPictures", "score")]
	public class ES3UserType_GameResultManatee : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameResultManatee() : base(typeof(GameResultManatee)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (GameResultManatee)obj;
			
			writer.WriteProperty("WrongPictures", instance.WrongPictures);
			writer.WriteProperty("score", instance.score, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new GameResultManatee();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "WrongPictures":
						instance.WrongPictures = reader.Read<System.Collections.Generic.List<System.String>>();
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


	public class ES3UserType_GameResultManateeArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameResultManateeArray() : base(typeof(GameResultManatee[]), ES3UserType_GameResultManatee.Instance)
		{
			Instance = this;
		}
	}
}