using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class GameServicesBuilder : BaseReferenceHolder
	{
		private GameServicesBuilder(IntPtr selfPointer) : base(selfPointer)
		{
			InternalHooks.InternalHooks_ConfigureForUnityPlugin(base.SelfPtr());
		}

		internal void AddOauthScope(string scope)
		{
			Builder.GameServices_Builder_AddOauthScope(base.SelfPtr(), scope);
		}

		internal GooglePlayGames.Native.PInvoke.GameServices Build(PlatformConfiguration configRef)
		{
			IntPtr intPtr = Builder.GameServices_Builder_Create(base.SelfPtr(), HandleRef.ToIntPtr(configRef.AsHandle()));
			if (intPtr.Equals(IntPtr.Zero))
			{
				throw new InvalidOperationException("There was an error creating a GameServices object. Check for log errors from GamesNativeSDK");
			}
			return new GooglePlayGames.Native.PInvoke.GameServices(intPtr);
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Builder.GameServices_Builder_Dispose(selfPointer);
		}

		internal static GameServicesBuilder Create()
		{
			return new GameServicesBuilder(Builder.GameServices_Builder_Construct());
		}

		internal void EnableSnapshots()
		{
			Builder.GameServices_Builder_EnableSnapshots(base.SelfPtr());
		}

		[MonoPInvokeCallback(typeof(Builder.OnAuthActionFinishedCallback))]
		private static void InternalAuthFinishedCallback(Types.AuthOperation op, CommonErrorStatus.AuthStatus status, IntPtr data)
		{
			GameServicesBuilder.AuthFinishedCallback permanentCallback = Callbacks.IntPtrToPermanentCallback<GameServicesBuilder.AuthFinishedCallback>(data);
			if (permanentCallback == null)
			{
				return;
			}
			try
			{
				permanentCallback(op, status);
			}
			catch (Exception exception)
			{
				Logger.e(string.Concat("Error encountered executing InternalAuthFinishedCallback. Smothering to avoid passing exception into Native: ", exception));
			}
		}

		[MonoPInvokeCallback(typeof(Builder.OnAuthActionStartedCallback))]
		private static void InternalAuthStartedCallback(Types.AuthOperation op, IntPtr data)
		{
			GameServicesBuilder.AuthStartedCallback permanentCallback = Callbacks.IntPtrToPermanentCallback<GameServicesBuilder.AuthStartedCallback>(data);
			try
			{
				if (permanentCallback != null)
				{
					permanentCallback(op);
				}
			}
			catch (Exception exception)
			{
				Logger.e(string.Concat("Error encountered executing InternalAuthStartedCallback. Smothering to avoid passing exception into Native: ", exception));
			}
		}

		[MonoPInvokeCallback(typeof(Builder.OnMultiplayerInvitationEventCallback))]
		private static void InternalOnMultiplayerInvitationEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
		{
			Action<Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation> permanentCallback = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation>>(userData);
			using (GooglePlayGames.Native.PInvoke.MultiplayerInvitation multiplayerInvitation = GooglePlayGames.Native.PInvoke.MultiplayerInvitation.FromPointer(match))
			{
				try
				{
					if (permanentCallback != null)
					{
						permanentCallback(eventType, matchId, multiplayerInvitation);
					}
				}
				catch (Exception exception)
				{
					Logger.e(string.Concat("Error encountered executing InternalOnMultiplayerInvitationEventCallback. Smothering to avoid passing exception into Native: ", exception));
				}
			}
		}

		[MonoPInvokeCallback(typeof(Builder.OnTurnBasedMatchEventCallback))]
		private static void InternalOnTurnBasedMatchEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
		{
			Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> permanentCallback = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch>>(userData);
			using (NativeTurnBasedMatch nativeTurnBasedMatch = NativeTurnBasedMatch.FromPointer(match))
			{
				try
				{
					if (permanentCallback != null)
					{
						permanentCallback(eventType, matchId, nativeTurnBasedMatch);
					}
				}
				catch (Exception exception)
				{
					Logger.e(string.Concat("Error encountered executing InternalOnTurnBasedMatchEventCallback. Smothering to avoid passing exception into Native: ", exception));
				}
			}
		}

		internal void RequireGooglePlus()
		{
			Builder.GameServices_Builder_RequireGooglePlus(base.SelfPtr());
		}

		internal void SetOnAuthFinishedCallback(GameServicesBuilder.AuthFinishedCallback callback)
		{
			Builder.GameServices_Builder_SetOnAuthActionFinished(base.SelfPtr(), new Builder.OnAuthActionFinishedCallback(GameServicesBuilder.InternalAuthFinishedCallback), Callbacks.ToIntPtr(callback));
		}

		internal void SetOnAuthStartedCallback(GameServicesBuilder.AuthStartedCallback callback)
		{
			Builder.GameServices_Builder_SetOnAuthActionStarted(base.SelfPtr(), new Builder.OnAuthActionStartedCallback(GameServicesBuilder.InternalAuthStartedCallback), Callbacks.ToIntPtr(callback));
		}

		internal void SetOnMultiplayerInvitationEventCallback(Action<Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
		{
			IntPtr intPtr = Callbacks.ToIntPtr(callback);
			Builder.GameServices_Builder_SetOnMultiplayerInvitationEvent(base.SelfPtr(), new Builder.OnMultiplayerInvitationEventCallback(GameServicesBuilder.InternalOnMultiplayerInvitationEventCallback), intPtr);
		}

		internal void SetOnTurnBasedMatchEventCallback(Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> callback)
		{
			IntPtr intPtr = Callbacks.ToIntPtr(callback);
			Builder.GameServices_Builder_SetOnTurnBasedMatchEvent(base.SelfPtr(), new Builder.OnTurnBasedMatchEventCallback(GameServicesBuilder.InternalOnTurnBasedMatchEventCallback), intPtr);
		}

		internal delegate void AuthFinishedCallback(Types.AuthOperation operation, CommonErrorStatus.AuthStatus status);

		internal delegate void AuthStartedCallback(Types.AuthOperation operation);
	}
}