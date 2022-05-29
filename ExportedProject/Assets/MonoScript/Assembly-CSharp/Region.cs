using System;

public class Region
{
	public CloudRegionCode Code;

	public string HostAndPort;

	public int Ping;

	public Region()
	{
	}

	public static CloudRegionCode Parse(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionCode cloudRegionCode = CloudRegionCode.none;
		if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
		{
			cloudRegionCode = (CloudRegionCode)((int)Enum.Parse(typeof(CloudRegionCode), codeAsString));
		}
		return cloudRegionCode;
	}

	internal static CloudRegionFlag ParseFlag(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionFlag cloudRegionFlag = (CloudRegionFlag)0;
		if (Enum.IsDefined(typeof(CloudRegionFlag), codeAsString))
		{
			cloudRegionFlag = (CloudRegionFlag)((int)Enum.Parse(typeof(CloudRegionFlag), codeAsString));
		}
		return cloudRegionFlag;
	}

	public override string ToString()
	{
		return string.Format("'{0}' \t{1}ms \t{2}", this.Code, this.Ping, this.HostAndPort);
	}
}