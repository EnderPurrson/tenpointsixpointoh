using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeAchievement : BaseReferenceHolder
	{
		private const ulong MinusOne = 18446744073709551615L;

		internal NativeAchievement(IntPtr selfPointer) : base(selfPointer)
		{
		}

		internal GooglePlayGames.BasicApi.Achievement AsAchievement()
		{
			GooglePlayGames.BasicApi.Achievement achievement = new GooglePlayGames.BasicApi.Achievement()
			{
				Id = this.Id(),
				Name = this.Name(),
				Description = this.Description()
			};
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong num = this.LastModifiedTime();
			if (num == (long)-1)
			{
				num = (ulong)0;
			}
			achievement.LastModifiedTime = dateTime.AddMilliseconds((double)((float)num));
			achievement.Points = this.getXP();
			achievement.RevealedImageUrl = this.getRevealedImageUrl();
			achievement.UnlockedImageUrl = this.getUnlockedImageUrl();
			if (this.Type() == Types.AchievementType.INCREMENTAL)
			{
				achievement.IsIncremental = true;
				achievement.CurrentSteps = (int)this.CurrentSteps();
				achievement.TotalSteps = (int)this.TotalSteps();
			}
			achievement.IsRevealed = (this.State() == Types.AchievementState.REVEALED ? true : this.State() == Types.AchievementState.UNLOCKED);
			achievement.IsUnlocked = this.State() == Types.AchievementState.UNLOCKED;
			return achievement;
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Dispose(selfPointer);
		}

		internal uint CurrentSteps()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_CurrentSteps(base.SelfPtr());
		}

		internal string Description()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Description(base.SelfPtr(), out_string, out_size));
		}

		internal string getRevealedImageUrl()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_RevealedIconUrl(base.SelfPtr(), out_string, out_size));
		}

		internal string getUnlockedImageUrl()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_UnlockedIconUrl(base.SelfPtr(), out_string, out_size));
		}

		internal ulong getXP()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_XP(base.SelfPtr());
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Id(base.SelfPtr(), out_string, out_size));
		}

		internal ulong LastModifiedTime()
		{
			if (!GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Valid(base.SelfPtr()))
			{
				return (ulong)0;
			}
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_LastModifiedTime(base.SelfPtr());
		}

		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Name(base.SelfPtr(), out_string, out_size));
		}

		internal Types.AchievementState State()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_State(base.SelfPtr());
		}

		internal uint TotalSteps()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_TotalSteps(base.SelfPtr());
		}

		internal Types.AchievementType Type()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Type(base.SelfPtr());
		}
	}
}