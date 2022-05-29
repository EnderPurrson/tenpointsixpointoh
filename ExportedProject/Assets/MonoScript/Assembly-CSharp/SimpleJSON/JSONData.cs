using System;
using System.IO;

namespace SimpleJSON
{
	public class JSONData : JSONNode
	{
		private string m_Data;

		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jSONDatum = new JSONData(string.Empty)
			{
				AsInt = this.AsInt
			};
			if (jSONDatum.m_Data == this.m_Data)
			{
				aWriter.Write((byte)4);
				aWriter.Write(this.AsInt);
				return;
			}
			jSONDatum.AsFloat = this.AsFloat;
			if (jSONDatum.m_Data == this.m_Data)
			{
				aWriter.Write((byte)7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jSONDatum.AsDouble = this.AsDouble;
			if (jSONDatum.m_Data == this.m_Data)
			{
				aWriter.Write((byte)5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jSONDatum.AsBool = this.AsBool;
			if (jSONDatum.m_Data == this.m_Data)
			{
				aWriter.Write((byte)6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write((byte)3);
			aWriter.Write(this.m_Data);
		}

		public override string ToString()
		{
			return string.Concat("\"", JSONNode.Escape(this.m_Data), "\"");
		}

		public override string ToString(string aPrefix)
		{
			return string.Concat("\"", JSONNode.Escape(this.m_Data), "\"");
		}
	}
}