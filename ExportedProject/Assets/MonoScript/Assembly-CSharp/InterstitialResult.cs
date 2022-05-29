using System;

internal sealed class InterstitialResult
{
	private readonly string _closeReason;

	private readonly string _errorMessage;

	public string CloseReason
	{
		get
		{
			return this._closeReason;
		}
	}

	public string ErrorMessage
	{
		get
		{
			return this._errorMessage;
		}
	}

	private InterstitialResult(string closeReason, string errorMessage)
	{
		this._closeReason = closeReason ?? string.Empty;
		this._errorMessage = errorMessage ?? string.Empty;
	}

	public static InterstitialResult FromCloseReason(string closeReason)
	{
		return new InterstitialResult(closeReason, string.Empty);
	}

	public static InterstitialResult FromErrorMessage(string errorMessage)
	{
		return new InterstitialResult(string.Empty, errorMessage);
	}
}