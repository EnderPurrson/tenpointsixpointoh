using System;
using UnityEngine;

namespace Prime31
{
	public class TwitterEventListener : MonoBehaviour
	{
		public TwitterEventListener()
		{
		}

		private void loginFailed(string error)
		{
			Debug.Log(string.Concat("Twitter login failed: ", error));
		}

		private void loginSucceeded(string username)
		{
			Debug.Log(string.Concat("Successfully logged in to Twitter: ", username));
		}

		private void OnDisable()
		{
			TwitterManager.twitterInitializedEvent -= new Action(this.twitterInitializedEvent);
			TwitterManager.loginSucceededEvent -= new Action<string>(this.loginSucceeded);
			TwitterManager.loginFailedEvent -= new Action<string>(this.loginFailed);
			TwitterManager.requestDidFinishEvent -= new Action<object>(this.requestDidFinishEvent);
			TwitterManager.requestDidFailEvent -= new Action<string>(this.requestDidFailEvent);
			TwitterManager.tweetSheetCompletedEvent -= new Action<bool>(this.tweetSheetCompletedEvent);
		}

		private void OnEnable()
		{
			TwitterManager.twitterInitializedEvent += new Action(this.twitterInitializedEvent);
			TwitterManager.loginSucceededEvent += new Action<string>(this.loginSucceeded);
			TwitterManager.loginFailedEvent += new Action<string>(this.loginFailed);
			TwitterManager.requestDidFinishEvent += new Action<object>(this.requestDidFinishEvent);
			TwitterManager.requestDidFailEvent += new Action<string>(this.requestDidFailEvent);
			TwitterManager.tweetSheetCompletedEvent += new Action<bool>(this.tweetSheetCompletedEvent);
		}

		private void requestDidFailEvent(string error)
		{
			Debug.Log(string.Concat("requestDidFailEvent: ", error));
		}

		private void requestDidFinishEvent(object result)
		{
			if (result != null)
			{
				Debug.Log("requestDidFinishEvent");
				Utils.logObject(result);
			}
		}

		private void tweetSheetCompletedEvent(bool didSucceed)
		{
			Debug.Log(string.Concat("tweetSheetCompletedEvent didSucceed: ", didSucceed));
		}

		private void twitterInitializedEvent()
		{
			Debug.Log("twitterInitializedEvent");
		}
	}
}