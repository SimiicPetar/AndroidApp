using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("spineAvatarInfo", "AvatarName", "typeOfChosenLetterFont")]
	public class ES3UserType_AvatarSpineInfo : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_AvatarSpineInfo() : base(typeof(AvatarSpineInfo)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (AvatarSpineInfo)obj;
			
			writer.WriteProperty("spineAvatarInfo", instance.spineAvatarInfo);
			writer.WriteProperty("AvatarName", instance.AvatarName, ES3Type_string.Instance);
			writer.WriteProperty("typeOfChosenLetterFont", instance.typeOfChosenLetterFont, ES3Type_enum.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new AvatarSpineInfo();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "spineAvatarInfo":
						instance.spineAvatarInfo = reader.Read<System.Collections.Generic.Dictionary<System.String, AttachmentColorPair>>();
						break;
					case "AvatarName":
						instance.AvatarName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "typeOfChosenLetterFont":
						instance.typeOfChosenLetterFont = reader.Read<TypeOfChosenLetterFont>(ES3Type_enum.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_AvatarSpineInfoArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_AvatarSpineInfoArray() : base(typeof(AvatarSpineInfo[]), ES3UserType_AvatarSpineInfo.Instance)
		{
			Instance = this;
		}
	}
}