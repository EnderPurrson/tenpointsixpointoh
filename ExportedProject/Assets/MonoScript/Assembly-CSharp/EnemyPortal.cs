using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache1;

	public event Action OnHided;

	public EnemyPortal()
	{
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = _003COnHided_003Em__263;
		}
		this.OnHided = _003C_003Ef__am_0024cache1;
		base._002Ector();
	}

	public void OnAnimationOff()
	{
		ChangeVisibleState(false, this.OnHided);
	}

	public void Show(Vector3 position)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(position, Vector3.down, out hitInfo))
		{
			Debug.DrawLine(position, hitInfo.point, Color.blue);
			base.transform.position = hitInfo.point;
		}
		ChangeVisibleState(true);
	}

	private void ChangeVisibleState(bool state, Action onComplete = null)
	{
		base.gameObject.SetActive(state);
		if (onComplete != null)
		{
			onComplete();
		}
	}

	[CompilerGenerated]
	private static void _003COnHided_003Em__263()
	{
	}
}
