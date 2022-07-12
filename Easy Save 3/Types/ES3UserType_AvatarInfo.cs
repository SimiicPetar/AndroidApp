using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("skinColor", "hairColor", "hairstyleIndex", "outfitIndex", "AvatarName", "typeOfChosenLetterFont")]
	public class ES3UserType_AvatarInfo : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_AvatarInfo() : base(typeof(AvatarInfo)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (AvatarInfo)obj;
			
			writer.WriteProperty("skinColor", instance.skinColor, ES3Type_Color.Instance);
			writer.WriteProperty("hairColor", instance.hairColor, ES3Type_Color.Instance);
			writer.WriteProperty("hairstyleIndex", instance.hairstyleIndex, ES3Type_int.Instance);
			writer.WriteProperty("outfitIndex", instance.outfitIndex, ES3Type_int.Instance);
			writer.WriteProperty("AvatarName", instance.AvatarName, ES3Type_string.Instance);
			writer.WriteProperty("typeOfChosenLetterFont", instance.typeOfChosenLetterFont);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new AvatarInfo();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "skinColor":
						instance.skinColor = reader.Read<UnityEngine.Color>(ES3Type_Color.Instance);
						break;
					case "hairColor":
						instance.hairColor = reader.Read<UnityEngine.Color>(ES3Type_Color.Instance);
						break;
					case "hairstyleIndex":
						instance.hairstyleIndex = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "outfitIndex":
						instance.outfitIndex = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "AvatarName":
						instance.AvatarName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "typeOfChosenLetterFont":
						instance.typeOfChosenLetterFont = reader.Read<TypeOfChosenLetterFont>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_AvatarInfoArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_AvatarInfoArray() : base(typeof(AvatarInfo[]), ES3UserType_AvatarInfo.Instance)
		{
			Instance = this;
		}
	}
}