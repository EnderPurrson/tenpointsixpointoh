using System;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AdventuresMenuController : MonoBehaviour
	{
		[SerializeField]
		private UIButton sandboxButton;

		[SerializeField]
		private float period = 334f;

		private float _distance;

		public AdventuresMenuController()
		{
		}

		private void Awake()
		{
		}

		private bool IsSandboxEnabled()
		{
			return FriendsController.SandboxEnabled;
		}

		private void OnEnable()
		{
			this.Refresh();
		}

		private void Refresh()
		{
			this.sandboxButton.gameObject.SetActive(this.IsSandboxEnabled());
			Transform vector3 = this.sandboxButton.transform.parent;
			float single = (!this.IsSandboxEnabled() ? 0.5f * this.period : 0f);
			float single1 = vector3.localPosition.y;
			Vector3 vector31 = vector3.localPosition;
			vector3.localPosition = new Vector3(single, single1, vector31.z);
		}
	}
}