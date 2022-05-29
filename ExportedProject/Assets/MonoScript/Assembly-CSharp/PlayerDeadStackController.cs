using System;
using UnityEngine;

public class PlayerDeadStackController : MonoBehaviour
{
	public static PlayerDeadStackController sharedController;

	public PlayerDeadController[] playerDeads;

	public GameObject playerDeadObject;

	public GameObject playerDeadObjectLow;

	private int currentIndexHole;

	public PlayerDeadStackController()
	{
	}

	public PlayerDeadController GetCurrentParticle(bool _isUseMine)
	{
		bool flag = true;
		do
		{
			this.currentIndexHole++;
			if (this.currentIndexHole < (int)this.playerDeads.Length)
			{
				continue;
			}
			if (!flag)
			{
				return null;
			}
			this.currentIndexHole = 0;
			flag = false;
		}
		while (this.playerDeads[this.currentIndexHole].isUseMine && !_isUseMine);
		return this.playerDeads[this.currentIndexHole];
	}

	private void OnDestroy()
	{
		PlayerDeadStackController.sharedController = null;
	}

	private void Start()
	{
		GameObject gameObject;
		PlayerDeadStackController.sharedController = this;
		base.transform.position = Vector3.zero;
		this.playerDeads = new PlayerDeadController[10];
		for (int i = 0; i < (int)this.playerDeads.Length; i++)
		{
			gameObject = (!Device.isPixelGunLow ? UnityEngine.Object.Instantiate<GameObject>(this.playerDeadObject) : UnityEngine.Object.Instantiate<GameObject>(this.playerDeadObjectLow));
			gameObject.transform.parent = base.transform;
			this.playerDeads[i] = gameObject.GetComponent<PlayerDeadController>();
		}
		UnityEngine.Object.Destroy(this.playerDeadObjectLow);
		UnityEngine.Object.Destroy(this.playerDeadObject);
	}
}