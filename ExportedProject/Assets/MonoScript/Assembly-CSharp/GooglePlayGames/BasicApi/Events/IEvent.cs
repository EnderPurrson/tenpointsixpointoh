using System;

namespace GooglePlayGames.BasicApi.Events
{
	public interface IEvent
	{
		ulong CurrentCount
		{
			get;
		}

		string Description
		{
			get;
		}

		string Id
		{
			get;
		}

		string ImageUrl
		{
			get;
		}

		string Name
		{
			get;
		}

		EventVisibility Visibility
		{
			get;
		}
	}
}