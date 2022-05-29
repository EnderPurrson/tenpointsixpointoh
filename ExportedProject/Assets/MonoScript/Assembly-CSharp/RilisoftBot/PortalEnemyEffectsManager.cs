using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	[RequireComponent(typeof(BaseBot))]
	public class PortalEnemyEffectsManager : MonoBehaviour, IEnemyEffectsManager
	{
		private const string SpawnShaderParamName = "_Burn";

		private const float SpawnPlayTime = 1f;

		private const float SpawnBurnAmountStart = 0.25f;

		private const float SpawnBurnAmountEnd = 1.25f;

		private BaseBot _bot;

		private Material _portalMaterialPref;

		public PortalEnemyEffectsManager()
		{
		}

		[DebuggerHidden]
		private IEnumerator AnimateMaterial(Renderer rend)
		{
			PortalEnemyEffectsManager.u003cAnimateMaterialu003ec__Iterator11C variable = null;
			return variable;
		}

		private void Awake()
		{
			this._bot = base.GetComponent<BaseBot>();
			this._portalMaterialPref = Resources.Load<Material>("Enemy_Portal");
			if (this._portalMaterialPref == null)
			{
				UnityEngine.Debug.LogError("material not found");
			}
		}

		public void ShowSpawnEffect()
		{
			this.ShowSpawnMaterials();
			this.ShowSpawnPortal();
		}

		private void ShowSpawnMaterials()
		{
			base.StartCoroutine(this.ShowSpawnMaterialsCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator ShowSpawnMaterialsCoroutine()
		{
			PortalEnemyEffectsManager.u003cShowSpawnMaterialsCoroutineu003ec__Iterator11B variable = null;
			return variable;
		}

		private void ShowSpawnPortal()
		{
			EnemyPortal portal = EnemyPortalStackController.sharedController.GetPortal();
			if (portal == null)
			{
				return;
			}
			portal.Show(base.gameObject.transform.position);
		}
	}
}