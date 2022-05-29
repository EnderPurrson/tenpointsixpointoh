using System;
using System.Linq;
using UnityEngine;

public class EnemyPortalStackController : MonoBehaviour
{
	public static EnemyPortalStackController sharedController;

	[ReadOnly]
	[SerializeField]
	private EnemyPortal[] _portals;

	private int currentIndex;

	public EnemyPortalStackController()
	{
	}

	private void Awake()
	{
		EnemyPortalStackController.sharedController = this;
	}

	public EnemyPortal GetPortal()
	{
		if (this._portals == null || !this._portals.Any<EnemyPortal>())
		{
			this.SetPortals();
		}
		this.currentIndex++;
		if (this.currentIndex >= (int)this._portals.Length)
		{
			this.currentIndex = 0;
		}
		return this._portals[this.currentIndex];
	}

	private void SetPortals()
	{
		this._portals = base.GetComponentsInChildren<EnemyPortal>(true);
		EnemyPortal[] enemyPortalArray = this._portals;
		for (int i = 0; i < (int)enemyPortalArray.Length; i++)
		{
			EnemyPortal enemyPortal = enemyPortalArray[i];
			if (enemyPortal.gameObject.activeInHierarchy)
			{
				enemyPortal.gameObject.SetActive(false);
			}
		}
	}

	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}