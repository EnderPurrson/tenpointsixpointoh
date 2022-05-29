using System;
using System.Text;
using UnityEngine;

public class X3DateConverter : MonoBehaviour
{
	public UIInput dateStartInput;

	public UIInput timeStartInput;

	public UIInput durationInput;

	public UILabel statusLabel;

	public X3DateConverter()
	{
	}

	public void CalculateAndCopyClick()
	{
		float single;
		string str = string.Format("{0}T{1}", this.dateStartInput.@value, this.timeStartInput.@value);
		DateTime dateTime = new DateTime();
		if (!DateTime.TryParse(str, out dateTime))
		{
			this.statusLabel.text = "Incorrect date or time format!";
			return;
		}
		if (!float.TryParse(this.durationInput.@value, out single))
		{
			this.statusLabel.text = "Incorrect duration format!";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("{");
		stringBuilder.AppendFormat("\t\"start\": {0}\n", this.ConvertToUnixTimestamp(dateTime));
		stringBuilder.AppendFormat("\t\"duration\": {0}\n", single * 360f);
		stringBuilder.AppendLine("}");
		EditorListBuilder.CopyTextInClipboard(stringBuilder.ToString());
		this.statusLabel.text = "Converted complete!";
	}

	private double ConvertToUnixTimestamp(DateTime date)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		DateTime dateTime1 = DateTime.SpecifyKind(date, DateTimeKind.Utc);
		return Math.Floor((dateTime1 - dateTime).TotalSeconds);
	}
}