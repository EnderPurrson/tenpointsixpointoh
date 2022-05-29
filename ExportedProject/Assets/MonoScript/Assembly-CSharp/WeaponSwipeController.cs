using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class WeaponSwipeController : MonoBehaviour
{
	private UIWrapContent _wrapContent;

	private UIScrollView _scrollView;

	private Player_move_c move;

	private MyCenterOnChild _center;

	private bool _disabled;

	public WeaponSwipeController()
	{
	}

	[DebuggerHidden]
	private IEnumerator _DisableSwiping(float tm)
	{
		WeaponSwipeController.u003c_DisableSwipingu003ec__Iterator1DD variable = null;
		return variable;
	}

	private void HandleCenteringFinished()
	{
		int num;
		TrainingState trainingState;
		if (this._disabled)
		{
			return;
		}
		if (!int.TryParse(this._center.centeredObject.name.Replace("preview_", string.Empty), out num))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("HandleCenteringFinished: error parse");
			}
			return;
		}
		num--;
		if (!this.move)
		{
			if (Defs.isMulti)
			{
				this.move = WeaponManager.sharedManager.myPlayerMoveC;
			}
			else
			{
				this.move = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
			}
		}
		if (num != WeaponManager.sharedManager.CurrentWeaponIndex)
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				if ((!TrainingController.stepTrainingList.TryGetValue("SwipeWeapon", out trainingState) ? false : TrainingController.stepTraining == trainingState))
				{
					TrainingController.isNextStep = trainingState;
				}
			}
			WeaponManager.sharedManager.CurrentWeaponIndex = num % WeaponManager.sharedManager.playerWeapons.Count;
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
			if (this.move != null)
			{
				this.move.ChangeWeapon(WeaponManager.sharedManager.CurrentWeaponIndex, false);
			}
		}
	}

	private void HandleWeaponEquipped()
	{
		this.UpdateContent();
	}

	private void OnDestroy()
	{
		this._center.onFinished -= new SpringPanel.OnFinished(this.HandleCenteringFinished);
	}

	private void OnEnable()
	{
		base.StartCoroutine(this._DisableSwiping(0.5f));
	}

	private void Start()
	{
		this._wrapContent = base.GetComponentInChildren<UIWrapContent>();
		this._center = base.GetComponentInChildren<MyCenterOnChild>();
		this._scrollView = base.GetComponent<UIScrollView>();
		this._center.onFinished += new SpringPanel.OnFinished(this.HandleCenteringFinished);
		this.UpdateContent();
	}

	public void UpdateContent()
	{
		List<string> strs = new List<string>();
		IEnumerator enumerator = WeaponManager.sharedManager.playerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				strs.Add(string.Concat(current.weaponPrefab.name, "_InGamePreview"));
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		UITexture[] componentsInChildren = base.GetComponentsInChildren<UITexture>();
		List<Texture> textures = new List<Texture>();
		UITexture[] uITextureArray = componentsInChildren;
		for (int i = 0; i < (int)uITextureArray.Length; i++)
		{
			UITexture uITexture = uITextureArray[i];
			if (uITexture.mainTexture)
			{
				textures.Add(uITexture.mainTexture);
			}
		}
		List<string> strs1 = new List<string>();
		foreach (string str in strs)
		{
			bool flag = false;
			foreach (Texture texture in textures)
			{
				if (!texture.name.Equals(str))
				{
					continue;
				}
				flag = true;
				break;
			}
			if (flag)
			{
				continue;
			}
			strs1.Add(str);
		}
		foreach (string str1 in strs1)
		{
			Texture texture1 = Resources.Load(string.Concat(WeaponManager.WeaponPreviewsPath, "/", str1)) as Texture;
			texture1.name = str1;
			if (texture1 == null)
			{
				continue;
			}
			textures.Add(texture1);
		}
		Transform child = base.transform.GetChild(0);
		int num = child.childCount;
		if (num > strs.Count)
		{
			for (int j = strs.Count; j < num; j++)
			{
				Transform transforms = child.GetChild(j);
				transforms.parent = null;
				UnityEngine.Object.Destroy(transforms.gameObject);
			}
		}
		else if (num < strs.Count)
		{
			for (int k = num; k < strs.Count; k++)
			{
				if (k >= num)
				{
					GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("WeaponPreviewPrefab") as GameObject);
					vector3.transform.parent = child;
					vector3.name = string.Concat("preview_", k + 1);
					vector3.transform.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		for (int l = 0; l < strs.Count; l++)
		{
			Transform child1 = child.GetChild(l);
			if (child1)
			{
				foreach (Texture texture2 in textures)
				{
					if (!texture2.name.Equals(strs[l]))
					{
						continue;
					}
					child1.GetComponent<UITexture>().mainTexture = texture2;
					break;
				}
			}
		}
		this._wrapContent.SortAlphabetically();
		Transform transforms1 = this._center.transform.GetChild(0);
		IEnumerator enumerator1 = this._wrapContent.transform.GetEnumerator();
		try
		{
			while (enumerator1.MoveNext())
			{
				Transform current1 = (Transform)enumerator1.Current;
				if (!current1.gameObject.name.Equals(string.Concat("preview_", WeaponManager.sharedManager.CurrentWeaponIndex + 1)))
				{
					continue;
				}
				transforms1 = current1;
				break;
			}
		}
		finally
		{
			IDisposable disposable1 = enumerator1 as IDisposable;
			if (disposable1 == null)
			{
			}
			disposable1.Dispose();
		}
		this._center.CenterOn(transforms1);
	}
}