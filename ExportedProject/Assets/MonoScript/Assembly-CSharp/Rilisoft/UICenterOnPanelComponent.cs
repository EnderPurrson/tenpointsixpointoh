using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	public class UICenterOnPanelComponent : MonoBehaviour
	{
		[ReadOnly]
		[SerializeField]
		protected UIPanel _panel;

		[SerializeField]
		public Rilisoft.Direction Direction = Rilisoft.Direction.Horizontal;

		[SerializeField]
		public float Slack = 0.1f;

		[SerializeField]
		public UnityEvent OnCentered;

		[SerializeField]
		public UnityEvent OnCenteredLoss;

		[ReadOnly]
		[SerializeField]
		protected bool _centered;

		protected Vector2 Offset;

		public Vector3 Center
		{
			get
			{
				Vector3[] vector3Array = this._panel.worldCorners;
				return (vector3Array[2] + vector3Array[0]) * 0.5f;
			}
		}

		public Rilisoft.CenterDirection CenterDirection
		{
			get
			{
				return (this.Center.x - base.transform.position.x <= 0f ? Rilisoft.CenterDirection.OnRight : Rilisoft.CenterDirection.OnLeft);
			}
		}

		public UICenterOnPanelComponent()
		{
		}

		private void Awake()
		{
			this._panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			this.OnCentered = this.OnCentered ?? new UnityEvent();
			this.OnCenteredLoss = this.OnCenteredLoss ?? new UnityEvent();
		}

		private void OnEnable()
		{
			this._centered = false;
		}

		protected virtual void Update()
		{
			if (this._panel == null)
			{
				this._panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			}
			if (this._panel == null)
			{
				return;
			}
			float center = this.Center.x;
			Vector3 vector3 = base.transform.position;
			float single = Mathf.Abs(center - vector3.x);
			float center1 = this.Center.y;
			Vector3 vector31 = base.transform.position;
			this.Offset = new Vector2(single, Mathf.Abs(center1 - vector31.y));
			if ((this.Direction != Rilisoft.Direction.Horizontal ? this.Offset.y : this.Offset.x) <= this.Slack)
			{
				if (!this._centered)
				{
					this._centered = true;
					if (this.OnCentered != null)
					{
						this.OnCentered.Invoke();
					}
				}
			}
			else if (this._centered)
			{
				this._centered = false;
				if (this.OnCenteredLoss != null)
				{
					this.OnCenteredLoss.Invoke();
				}
			}
		}
	}
}