using System;

public class RaiseEventOptions
{
	public readonly static RaiseEventOptions Default;

	public EventCaching CachingOption;

	public byte InterestGroup;

	public int[] TargetActors;

	public ReceiverGroup Receivers;

	public byte SequenceChannel;

	public bool ForwardToWebhook;

	public bool Encrypt;

	static RaiseEventOptions()
	{
		RaiseEventOptions.Default = new RaiseEventOptions();
	}

	public RaiseEventOptions()
	{
	}
}