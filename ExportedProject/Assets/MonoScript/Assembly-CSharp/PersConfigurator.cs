using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class PersConfigurator : MonoBehaviour
{
	public static PersConfigurator currentConfigurator;

	public Transform armorPoint;

	public Transform boots;

	public Transform cape;

	public Transform hat;

	public Transform maskPoint;

	public GameObject body;

	public GameObject gun;

	private GameObject weapon;

	private NickLabelController _label;

	private GameObject shadow;

	private AnimationClip profile;

	public PersConfigurator()
	{
	}

	public void _AddCapeAndHat()
	{
		List<Transform> transforms = new List<Transform>();
		for (int i = 0; i < this.cape.childCount; i++)
		{
			transforms.Add(this.cape.GetChild(i));
		}
		foreach (Transform transform in transforms)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		string str = Storager.getString(Defs.CapeEquppedSN, false);
		if (!str.Equals(Defs.CapeNoneEqupped))
		{
			GameObject gameObject = Resources.Load(ResPath.Combine(Defs.CapesDir, str)) as GameObject;
			if (gameObject == null)
			{
				UnityEngine.Debug.LogWarning("capePrefab == null");
			}
			else
			{
				GameObject vector3 = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				vector3.transform.parent = this.cape;
				vector3.transform.localPosition = new Vector3(0f, -0.8f, 0f);
				vector3.transform.localRotation = Quaternion.identity;
				vector3.GetComponent<Animation>().Play("Profile");
			}
		}
		transforms = new List<Transform>();
		for (int j = 0; j < this.maskPoint.childCount; j++)
		{
			transforms.Add(this.maskPoint.GetChild(j));
		}
		foreach (Transform transform1 in transforms)
		{
			UnityEngine.Object.Destroy(transform1.gameObject);
		}
		string str1 = Storager.getString("MaskEquippedSN", false);
		if (!str1.Equals("MaskNoneEquipped"))
		{
			GameObject gameObject1 = Resources.Load(ResPath.Combine("Masks", str1)) as GameObject;
			if (gameObject1 == null)
			{
				UnityEngine.Debug.LogWarning("maskPrefab == null");
			}
			else
			{
				GameObject vector31 = UnityEngine.Object.Instantiate(gameObject1, Vector3.zero, Quaternion.identity) as GameObject;
				vector31.transform.parent = this.maskPoint;
				vector31.transform.localPosition = new Vector3(0f, 0f, 0f);
				vector31.transform.localRotation = Quaternion.identity;
			}
		}
		transforms = new List<Transform>();
		for (int k = 0; k < this.hat.childCount; k++)
		{
			transforms.Add(this.hat.GetChild(k));
		}
		foreach (Transform transforms1 in transforms)
		{
			transforms1.parent = null;
			UnityEngine.Object.Destroy(transforms1.gameObject);
		}
		string str2 = Storager.getString(Defs.HatEquppedSN, false);
		string str3 = Storager.getString(Defs.VisualHatArmor, false);
		if (!string.IsNullOrEmpty(str3) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(str2) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(str2) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(str3))
		{
			str2 = str3;
		}
		if (!str2.Equals(Defs.HatNoneEqupped))
		{
			GameObject gameObject2 = Resources.Load(ResPath.Combine(Defs.HatsDir, str2)) as GameObject;
			if (gameObject2 == null)
			{
				UnityEngine.Debug.LogWarning("hatPrefab == null");
			}
			else
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject3.transform.parent = this.hat;
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.localRotation = Quaternion.identity;
			}
		}
		transforms = new List<Transform>();
		for (int l = 0; l < this.boots.childCount; l++)
		{
			this.boots.GetChild(l).gameObject.SetActive(false);
		}
		string str4 = Storager.getString(Defs.BootsEquppedSN, false);
		if (!str4.Equals(Defs.BootsNoneEqupped))
		{
			IEnumerator enumerator = this.boots.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform current = (Transform)enumerator.Current;
					if (!current.gameObject.name.Equals(str4))
					{
						current.gameObject.SetActive(false);
					}
					else
					{
						current.gameObject.SetActive(true);
					}
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
		}
		transforms = new List<Transform>();
		for (int m = 0; m < this.armorPoint.childCount; m++)
		{
			transforms.Add(this.armorPoint.GetChild(m));
		}
		foreach (Transform transform2 in transforms)
		{
			ArmorRefs component = transform2.GetChild(0).GetComponent<ArmorRefs>();
			if (component == null)
			{
				continue;
			}
			if (component.leftBone != null)
			{
				component.leftBone.parent = transform2.GetChild(0);
			}
			if (component.rightBone != null)
			{
				component.rightBone.parent = transform2.GetChild(0);
			}
			transform2.parent = null;
			UnityEngine.Object.Destroy(transform2.gameObject);
		}
		string str5 = Storager.getString(Defs.ArmorNewEquppedSN, false);
		string str6 = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(str6) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(str5) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(str5) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(str6))
		{
			str5 = str6;
		}
		if (!str5.Equals(Defs.ArmorNewNoneEqupped))
		{
			GameObject gameObject4 = Resources.Load(string.Concat("Armor/", str5)) as GameObject;
			if (gameObject4 == null)
			{
				return;
			}
			GameObject vector32 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
			ArmorRefs leftArmorHand = vector32.transform.GetChild(0).GetComponent<ArmorRefs>();
			if (leftArmorHand != null)
			{
				if (this.weapon != null)
				{
					WeaponSounds weaponSound = this.weapon.GetComponent<WeaponSounds>();
					if (weaponSound != null && leftArmorHand.leftBone != null && weaponSound.LeftArmorHand != null)
					{
						leftArmorHand.leftBone.parent = weaponSound.LeftArmorHand;
						leftArmorHand.leftBone.localPosition = Vector3.zero;
						leftArmorHand.leftBone.localRotation = Quaternion.identity;
						leftArmorHand.leftBone.localScale = new Vector3(1f, 1f, 1f);
					}
					if (weaponSound != null && leftArmorHand.rightBone != null && weaponSound.RightArmorHand != null)
					{
						leftArmorHand.rightBone.parent = weaponSound.RightArmorHand;
						leftArmorHand.rightBone.localPosition = Vector3.zero;
						leftArmorHand.rightBone.localRotation = Quaternion.identity;
						leftArmorHand.rightBone.localScale = new Vector3(1f, 1f, 1f);
					}
				}
				vector32.transform.parent = this.armorPoint.transform;
				vector32.transform.localPosition = Vector3.zero;
				vector32.transform.localRotation = Quaternion.identity;
				vector32.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		ShopNGUIController.SetPersHatVisible(this.hat);
		ShopNGUIController.SetPersArmorVisible(this.armorPoint);
	}

	private void Awake()
	{
		PersConfigurator.currentConfigurator = this;
	}

	private void HandleShowArmorChanged()
	{
		this._AddCapeAndHat();
	}

	private void OnDestroy()
	{
		if (ShopNGUIController.sharedShop != null)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
			ShopNGUIController.sharedShop.wearEquipAction = null;
			ShopNGUIController.sharedShop.wearUnequipAction = null;
		}
		if (this.profile != null)
		{
			Resources.UnloadAsset(this.profile);
		}
		ShopNGUIController.ShowArmorChanged -= new Action(this.HandleShowArmorChanged);
		PersConfigurator.currentConfigurator = null;
	}

	private void SetCurrentSkin()
	{
		Texture texture = SkinsController.currentSkinForPers;
		if (texture != null)
		{
			texture.filterMode = FilterMode.Point;
			List<GameObject> gameObjects = new List<GameObject>(new GameObject[] { this.gun, this.cape.gameObject, this.hat.gameObject, this.boots.gameObject, this.armorPoint.gameObject, this.maskPoint.gameObject });
			if (this.weapon != null)
			{
				WeaponSounds component = this.weapon.GetComponent<WeaponSounds>();
				if (component.LeftArmorHand != null)
				{
					gameObjects.Add(component.LeftArmorHand.gameObject);
				}
				if (component.RightArmorHand != null)
				{
					gameObjects.Add(component.RightArmorHand.gameObject);
				}
				if (component.grenatePoint != null)
				{
					gameObjects.Add(component.grenatePoint.gameObject);
				}
				List<GameObject> listWeaponAnimEffects = component.GetListWeaponAnimEffects();
				if (listWeaponAnimEffects != null)
				{
					gameObjects.AddRange(listWeaponAnimEffects);
				}
			}
			Player_move_c.SetTextureRecursivelyFrom(base.gameObject, texture, gameObjects.ToArray());
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		PersConfigurator.u003cStartu003ec__IteratorD1 variable = null;
		return variable;
	}

	private void Update()
	{
		RaycastHit raycastHit;
		if (Camera.main != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			Touch[] touchArray = Input.touches;
			for (int i = 0; i < (int)touchArray.Length; i++)
			{
				if (touchArray[i].phase == TouchPhase.Began && Physics.Raycast(ray, out raycastHit, 1000f, -5) && raycastHit.collider.gameObject.name.Equals("MainMenu_Pers"))
				{
					PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 1);
					ConnectSceneNGUIController.GoToProfile();
					return;
				}
			}
		}
	}
}