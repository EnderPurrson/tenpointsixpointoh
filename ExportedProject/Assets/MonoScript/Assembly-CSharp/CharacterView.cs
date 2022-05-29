using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
	public Transform character;

	public Transform mech;

	public SkinnedMeshRenderer mechBodyRenderer;

	public SkinnedMeshRenderer mechHandRenderer;

	public SkinnedMeshRenderer mechGunRenderer;

	public Material[] mechGunMaterials;

	public Material[] mechBodyMaterials;

	public Transform turret;

	public Transform hatPoint;

	public Transform maskPoint;

	public Transform capePoint;

	public Transform bootsPoint;

	public Transform armorPoint;

	public Transform body;

	private AnimationCoroutineRunner _animationRunner;

	private AnimationClip _profile;

	private GameObject _weapon;

	private AnimationCoroutineRunner AnimationRunner
	{
		get
		{
			if (this._animationRunner == null)
			{
				this._animationRunner = base.GetComponent<AnimationCoroutineRunner>();
			}
			return this._animationRunner;
		}
	}

	public CharacterView()
	{
	}

	public static Texture2D GetClanLogo(string logoBase64)
	{
		if (string.IsNullOrEmpty(logoBase64))
		{
			return null;
		}
		byte[] numArray = Convert.FromBase64String(logoBase64);
		Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
		texture2D.LoadImage(numArray);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	private void PlayWeaponAnimation()
	{
		if (this._profile == null)
		{
			Debug.LogWarning("_profile == null");
		}
		else
		{
			Animation component = this._weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>();
			if (Time.timeScale == 0f)
			{
				this.AnimationRunner.StopAllCoroutines();
				if (component.GetClip("Profile") != null)
				{
					Debug.LogWarning("Animation clip is null.");
				}
				else
				{
					component.AddClip(this._profile, "Profile");
				}
				this.AnimationRunner.StartPlay(component, "Profile", false, null);
			}
			else
			{
				if (component.GetClip("Profile") != null)
				{
					Debug.LogWarning("Animation clip is null.");
				}
				else
				{
					component.AddClip(this._profile, "Profile");
				}
				component.Play("Profile");
			}
		}
	}

	public void RemoveArmor()
	{
		if (this.armorPoint.childCount > 0)
		{
			Transform child = this.armorPoint.GetChild(0);
			ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					component.leftBone.parent = child.GetChild(0);
				}
				if (component.rightBone != null)
				{
					component.rightBone.parent = child.GetChild(0);
				}
				child.parent = null;
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	public void RemoveBoots()
	{
		IEnumerator enumerator = this.bootsPoint.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				((Transform)enumerator.Current).gameObject.SetActive(false);
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

	public void RemoveCape()
	{
		for (int i = 0; i < this.capePoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.capePoint.transform.GetChild(i).gameObject);
		}
	}

	public void RemoveHat()
	{
		List<Transform> transforms = new List<Transform>();
		for (int i = 0; i < this.hatPoint.childCount; i++)
		{
			transforms.Add(this.hatPoint.GetChild(i));
		}
		foreach (Transform transform in transforms)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	public void RemoveMask()
	{
		List<Transform> transforms = new List<Transform>();
		for (int i = 0; i < this.maskPoint.childCount; i++)
		{
			transforms.Add(this.maskPoint.GetChild(i));
		}
		foreach (Transform transform in transforms)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	public void SetSkinTexture(Texture skin)
	{
		GameObject component;
		WeaponSounds weaponSound;
		if (skin == null)
		{
			return;
		}
		if (this.body.transform.childCount <= 0)
		{
			component = null;
		}
		else
		{
			component = this.body.transform.GetChild(0).GetComponent<WeaponSounds>().bonusPrefab;
		}
		GameObject gameObject = component;
		List<GameObject> gameObjects = new List<GameObject>()
		{
			this.capePoint.gameObject,
			this.hatPoint.gameObject,
			this.bootsPoint.gameObject,
			this.armorPoint.gameObject,
			this.maskPoint.gameObject,
			gameObject
		};
		List<GameObject> gameObjects1 = gameObjects;
		if (this.body.transform.childCount <= 0)
		{
			weaponSound = null;
		}
		else
		{
			weaponSound = this.body.transform.GetChild(0).GetComponent<WeaponSounds>();
		}
		WeaponSounds weaponSound1 = weaponSound;
		if (weaponSound1 != null)
		{
			List<GameObject> listWeaponAnimEffects = weaponSound1.GetListWeaponAnimEffects();
			if (listWeaponAnimEffects != null)
			{
				gameObjects1.AddRange(listWeaponAnimEffects);
			}
		}
		Player_move_c.SetTextureRecursivelyFrom(this.character.gameObject, skin, gameObjects1.ToArray());
	}

	public void SetupWeaponGrenade(GameObject weaponGrenade)
	{
		GameObject gameObject = Resources.Load<GameObject>("Rocket");
		Rocket component = (UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Rocket>();
		component.enabled = false;
		component.dontExecStart = true;
		component.GetComponent<Rigidbody>().useGravity = false;
		component.GetComponent<Rigidbody>().isKinematic = true;
		component.rockets[10].SetActive(true);
		Player_move_c.SetLayerRecursively(component.gameObject, base.gameObject.layer);
		component.transform.parent = weaponGrenade.GetComponent<WeaponSounds>().grenatePoint;
		component.transform.localPosition = Vector3.zero;
		component.transform.localRotation = Quaternion.identity;
		component.transform.localScale = Vector3.one;
	}

	public void SetWeaponAndSkin(string tg, Texture skinForPers, bool replaceRemovedWeapons)
	{
		this.AnimationRunner.StopAllCoroutines();
		if (WeaponManager.sharedManager == null)
		{
			return;
		}
		if (this.armorPoint.childCount > 0)
		{
			ArmorRefs child = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (child != null)
			{
				if (child.leftBone != null)
				{
					child.leftBone.parent = this.armorPoint.GetChild(0).GetChild(0);
				}
				if (child.rightBone != null)
				{
					child.rightBone.parent = this.armorPoint.GetChild(0).GetChild(0);
				}
			}
		}
		List<Transform> transforms = new List<Transform>();
		IEnumerator enumerator = this.body.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				transforms.Add((Transform)enumerator.Current);
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
		foreach (Transform transform in transforms)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (tg == null)
		{
			return;
		}
		if (this._profile != null)
		{
			Resources.UnloadAsset(this._profile);
			this._profile = null;
		}
		GameObject gameObject = null;
		if (tg != "WeaponGrenade")
		{
			try
			{
				string prefabName = ItemDb.GetByTag(tg).PrefabName;
				gameObject = WeaponManager.sharedManager.weaponsInGame.OfType<GameObject>().FirstOrDefault<GameObject>((GameObject wp) => wp.name == prefabName);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				if (Application.isEditor)
				{
					Debug.LogError(string.Concat("Exception in var weaponName = ItemDb.GetByTag(tg).PrefabName: ", exception));
				}
			}
			if (replaceRemovedWeapons && gameObject != null)
			{
				WeaponSounds weaponSound = gameObject.GetComponent<WeaponSounds>();
				if (weaponSound != null && (WeaponManager.Removed150615_PrefabNames.Contains(gameObject.name) || weaponSound.tier > 100))
				{
					gameObject = WeaponManager.sharedManager.weaponsInGame.OfType<GameObject>().FirstOrDefault<GameObject>((GameObject wp) => wp.name.Equals(weaponSound.alternativeName));
				}
			}
		}
		else
		{
			gameObject = Resources.Load<GameObject>("WeaponGrenade");
		}
		if (gameObject == null)
		{
			Debug.Log("pref == null");
			return;
		}
		this._profile = Resources.Load<AnimationClip>(string.Concat("ProfileAnimClips/", gameObject.name, "_Profile"));
		GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		Player_move_c.PerformActionRecurs(gameObject1, (Transform t) => {
			MeshRenderer component = t.GetComponent<MeshRenderer>();
			SkinnedMeshRenderer skinnedMeshRenderer = t.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				component.useLightProbes = false;
			}
			if (skinnedMeshRenderer != null)
			{
				skinnedMeshRenderer.useLightProbes = false;
			}
		});
		Player_move_c.SetLayerRecursively(gameObject1, base.gameObject.layer);
		gameObject1.transform.parent = this.body;
		this._weapon = gameObject1;
		this._weapon.transform.localScale = new Vector3(1f, 1f, 1f);
		this._weapon.transform.position = this.body.transform.position;
		this._weapon.transform.localPosition = Vector3.zero;
		this._weapon.transform.localRotation = Quaternion.identity;
		WeaponSounds weaponSound1 = this._weapon.GetComponent<WeaponSounds>();
		if (this.armorPoint.childCount > 0 && weaponSound1 != null)
		{
			ArmorRefs leftArmorHand = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (leftArmorHand != null)
			{
				if (leftArmorHand.leftBone != null && weaponSound1.LeftArmorHand != null)
				{
					leftArmorHand.leftBone.parent = weaponSound1.LeftArmorHand;
					leftArmorHand.leftBone.localPosition = Vector3.zero;
					leftArmorHand.leftBone.localRotation = Quaternion.identity;
					leftArmorHand.leftBone.localScale = new Vector3(1f, 1f, 1f);
				}
				if (leftArmorHand.rightBone != null && weaponSound1.RightArmorHand != null)
				{
					leftArmorHand.rightBone.parent = weaponSound1.RightArmorHand;
					leftArmorHand.rightBone.localPosition = Vector3.zero;
					leftArmorHand.rightBone.localRotation = Quaternion.identity;
					leftArmorHand.rightBone.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		this.PlayWeaponAnimation();
		this.SetSkinTexture(skinForPers);
		if (tg == "WeaponGrenade")
		{
			this.SetupWeaponGrenade(gameObject1);
		}
	}

	public void ShowCharacterType(CharacterView.CharacterType characterType)
	{
		this.character.gameObject.SetActive(false);
		if (this.mech != null)
		{
			this.mech.gameObject.SetActive(false);
		}
		if (this.turret != null)
		{
			this.turret.gameObject.SetActive(false);
		}
		switch (characterType)
		{
			case CharacterView.CharacterType.Player:
			{
				this.character.gameObject.SetActive(true);
				break;
			}
			case CharacterView.CharacterType.Mech:
			{
				this.mech.gameObject.SetActive(true);
				break;
			}
			case CharacterView.CharacterType.Turret:
			{
				this.turret.gameObject.SetActive(true);
				break;
			}
		}
	}

	public void UpdateArmor(string armor)
	{
		this.RemoveArmor();
		string str = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(str) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(str))
		{
			armor = str;
		}
		GameObject gameObject = Resources.Load(string.Concat("Armor/", armor)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("armorPrefab == null");
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		ArmorRefs component = vector3.transform.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null && this._weapon != null)
		{
			WeaponSounds weaponSound = this._weapon.GetComponent<WeaponSounds>();
			if (component.leftBone != null && weaponSound.LeftArmorHand != null)
			{
				component.leftBone.parent = weaponSound.LeftArmorHand;
				component.leftBone.localPosition = Vector3.zero;
				component.leftBone.localRotation = Quaternion.identity;
				component.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component.rightBone != null && weaponSound.RightArmorHand != null)
			{
				component.rightBone.parent = weaponSound.RightArmorHand;
				component.rightBone.localPosition = Vector3.zero;
				component.rightBone.localRotation = Quaternion.identity;
				component.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
			vector3.transform.parent = this.armorPoint.transform;
			vector3.transform.localPosition = Vector3.zero;
			vector3.transform.localRotation = Quaternion.identity;
			vector3.transform.localScale = new Vector3(1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(vector3, base.gameObject.layer);
		}
	}

	public void UpdateBoots(string bs)
	{
		IEnumerator enumerator = this.bootsPoint.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				current.gameObject.SetActive(current.gameObject.name.Equals(bs));
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

	public void UpdateCape(string cape, Texture capeTex = null)
	{
		this.RemoveCape();
		GameObject gameObject = Resources.Load(string.Concat("Capes/", cape)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("capePrefab == null");
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		vector3.transform.parent = this.capePoint.transform;
		vector3.transform.localPosition = new Vector3(0f, -0.8f, 0f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(vector3, base.gameObject.layer);
		if (capeTex != null)
		{
			Player_move_c.SetTextureRecursivelyFrom(vector3, capeTex, new GameObject[0]);
		}
	}

	public void UpdateHat(string hat)
	{
		this.RemoveHat();
		string str = Storager.getString(Defs.VisualHatArmor, false);
		if (!string.IsNullOrEmpty(str) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hat) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hat) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(str))
		{
			hat = str;
		}
		GameObject gameObject = Resources.Load(string.Concat("Hats/", hat)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("hatPrefab == null");
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		Transform transforms = vector3.transform;
		vector3.transform.parent = this.hatPoint.transform;
		vector3.transform.localPosition = Vector3.zero;
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(vector3, base.gameObject.layer);
	}

	public void UpdateMask(string mask)
	{
		this.RemoveMask();
		GameObject gameObject = Resources.Load(string.Concat("Masks/", mask)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("maskPrefab == null");
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		vector3.transform.parent = this.maskPoint.transform;
		vector3.transform.localPosition = new Vector3(0f, 0f, 0f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(vector3, base.gameObject.layer);
	}

	public void UpdateMech(int mechUpgrade)
	{
		this.mechBodyRenderer.material = this.mechBodyMaterials[mechUpgrade];
		this.mechHandRenderer.material = this.mechBodyMaterials[mechUpgrade];
		this.mechGunRenderer.material = this.mechGunMaterials[mechUpgrade];
		this.mechBodyRenderer.material.SetColor("_ColorRili", Color.white);
		this.mechHandRenderer.material.SetColor("_ColorRili", Color.white);
	}

	public void UpdateTurret(int turretUpgrade)
	{
		TurretController component = this.turret.GetComponent<TurretController>();
		if (component.turretRunMaterials != null && (int)component.turretRunMaterials.Length > turretUpgrade && component.turretRunMaterials[turretUpgrade] != null)
		{
			component.turretRenderer.material = component.turretRunMaterials[turretUpgrade];
			component.turretRenderer.material.SetColor("_ColorRili", Color.white);
		}
	}

	public enum CharacterType
	{
		Player,
		Mech,
		Turret
	}
}