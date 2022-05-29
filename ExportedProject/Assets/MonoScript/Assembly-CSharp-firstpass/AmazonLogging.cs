using System;
using UnityEngine;

public class AmazonLogging
{
	private const string errorMessage = "{0} error: {1}";

	private const string warningMessage = "{0} warning: {1}";

	private const string logMessage = "{0}: {1}";

	public AmazonLogging()
	{
	}

	public static void Log(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
	{
		if (reportLevel != AmazonLogging.AmazonLoggingLevel.Verbose)
		{
			return;
		}
		Debug.Log(string.Format("{0}: {1}", service, message));
	}

	public static void LogError(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
	{
		if (reportLevel == AmazonLogging.AmazonLoggingLevel.Silent)
		{
			return;
		}
		string str = string.Format("{0} error: {1}", service, message);
		switch (reportLevel)
		{
			case AmazonLogging.AmazonLoggingLevel.Critical:
			case AmazonLogging.AmazonLoggingLevel.Errors:
			case AmazonLogging.AmazonLoggingLevel.Warnings:
			case AmazonLogging.AmazonLoggingLevel.Verbose:
			{
				Debug.LogError(str);
				break;
			}
			case AmazonLogging.AmazonLoggingLevel.ErrorsAsExceptions:
			{
				throw new Exception(str);
			}
		}
	}

	public static void LogWarning(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
	{
		switch (reportLevel)
		{
			case AmazonLogging.AmazonLoggingLevel.Warnings:
			case AmazonLogging.AmazonLoggingLevel.Verbose:
			{
				Debug.LogWarning(string.Format("{0} warning: {1}", service, message));
				break;
			}
		}
	}

	public static AmazonLogging.SDKLoggingLevel pluginToSDKLoggingLevel(AmazonLogging.AmazonLoggingLevel pluginLoggingLevel)
	{
		switch (pluginLoggingLevel)
		{
			case AmazonLogging.AmazonLoggingLevel.Silent:
			{
				return AmazonLogging.SDKLoggingLevel.LogOff;
			}
			case AmazonLogging.AmazonLoggingLevel.Critical:
			{
				return AmazonLogging.SDKLoggingLevel.LogCritical;
			}
			case AmazonLogging.AmazonLoggingLevel.ErrorsAsExceptions:
			case AmazonLogging.AmazonLoggingLevel.Errors:
			{
				return AmazonLogging.SDKLoggingLevel.LogError;
			}
			case AmazonLogging.AmazonLoggingLevel.Warnings:
			case AmazonLogging.AmazonLoggingLevel.Verbose:
			{
				return AmazonLogging.SDKLoggingLevel.LogWarning;
			}
		}
		return AmazonLogging.SDKLoggingLevel.LogWarning;
	}

	public enum AmazonLoggingLevel
	{
		Silent,
		Critical,
		ErrorsAsExceptions,
		Errors,
		Warnings,
		Verbose
	}

	public enum SDKLoggingLevel
	{
		LogOff,
		LogCritical,
		LogError,
		LogWarning
	}
}