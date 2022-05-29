using Facebook.Unity.Mobile;
using System;
using UnityEngine;

namespace Facebook.Unity.Mobile.Android
{
	internal class AndroidFacebookGameObject : MobileFacebookGameObject
	{
		public AndroidFacebookGameObject()
		{
		}

		protected override void OnAwake()
		{
			AndroidJNIHelper.debug = Debug.isDebugBuild;
		}
	}
}