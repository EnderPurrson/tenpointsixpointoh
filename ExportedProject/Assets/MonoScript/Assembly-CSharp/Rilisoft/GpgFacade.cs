using GooglePlayGames;
using System;

namespace Rilisoft
{
	internal struct GpgFacade
	{
		private readonly static GpgFacade s_instance;

		public static GpgFacade Instance
		{
			get
			{
				return GpgFacade.s_instance;
			}
		}

		static GpgFacade()
		{
			GpgFacade.s_instance = new GpgFacade();
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			PlayGamesPlatform.Instance.Authenticate(callback, silent);
		}

		public void IncrementAchievement(string achievementId, int steps, Action<bool> callback)
		{
			if (achievementId == null)
			{
				throw new ArgumentNullException("achievementId");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			PlayGamesPlatform.Instance.IncrementAchievement(achievementId, steps, callback);
		}

		public bool IsAuthenticated()
		{
			return PlayGamesPlatform.Instance.IsAuthenticated();
		}

		public void SignOut()
		{
			PlayGamesPlatform.Instance.SignOut();
		}
	}
}