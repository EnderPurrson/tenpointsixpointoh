using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
	private Action OnHided = new Action(() => {
	});

	public EnemyPortal()
	{
	}

	private void ChangeVisibleState(bool state, Action onComplete = null)
	{
		base.gameObject.SetActive(state);
		if (onComplete != null)
		{
			onComplete();
		}
	}

	public void OnAnimationOff()
	{
		this.ChangeVisibleState(false, this.OnHided);
	}

	public void Show(Vector3 position)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(position, Vector3.down, out raycastHit))
		{
			Debug.DrawLine(position, raycastHit.point, Color.blue);
			base.transform.position = raycastHit.point;
		}
		this.ChangeVisibleState(true, null);
	}

	public event Action OnHided
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OnHided += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OnHided -= value;
		}
	}
}