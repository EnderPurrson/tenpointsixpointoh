using System;
using UnityEngine;

namespace UnityStandardAssets.SceneUtils
{
	public class PlaceTargetWithMouse : MonoBehaviour
	{
		public float surfaceOffset = 1.5f;

		public GameObject setTargetOn;

		public PlaceTargetWithMouse()
		{
		}

		private void Update()
		{
			RaycastHit raycastHit;
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
			{
				return;
			}
			base.transform.position = raycastHit.point + (raycastHit.normal * this.surfaceOffset);
			if (this.setTargetOn != null)
			{
				this.setTargetOn.SendMessage("SetTarget", base.transform);
			}
		}
	}
}