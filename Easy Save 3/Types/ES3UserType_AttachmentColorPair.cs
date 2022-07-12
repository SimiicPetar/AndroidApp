using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("attachmentName", "attachmentColor")]
	public class ES3UserType_AttachmentColorPair : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_AttachmentColorPair() : base(typeof(AttachmentColorPair)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (AttachmentColorPair)obj;
			
			writer.WriteProperty("attachmentName", instance.attachmentName, ES3Type_string.Instance);
			writer.WriteProperty("attachmentColor", instance.attachmentColor, ES3Type_Color.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new AttachmentColorPair();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "attachmentName":
						instance.attachmentName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "attachmentColor":
						instance.attachmentColor = reader.Read<UnityEngine.Color>(ES3Type_Color.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_AttachmentColorPairArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_AttachmentColorPairArray() : base(typeof(AttachmentColorPair[]), ES3UserType_AttachmentColorPair.Instance)
		{
			Instance = this;
		}
	}
}