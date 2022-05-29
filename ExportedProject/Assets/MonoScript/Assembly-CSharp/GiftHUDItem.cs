using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GiftHUDItem : MonoBehaviour
{
	public bool isInfo;

	public UISprite sprIcon;

	public UITexture textureIcon;

	public UILabel nameGift;

	public GameObject parentForSkin;

	public BoxCollider colliderForDrag;

	public UILabel lbInfoGift;

	private Transform skinModelTransform;

	[ReadOnly]
	[SerializeField]
	private string nameAndCountGift = string.Empty;

	private Vector3 offsetSkin = new Vector3(0f, -44.12f, 0f);

	private Vector3 scaleSkin = new Vector3(45f, 45f, 45f);

	private bool endAnim;

	[SerializeField]
	private SlotInfo _data;

	public GiftHUDItem()
	{
	}

	[DebuggerHidden]
	private IEnumerator ActiveSkinAfterWait()
	{
		GiftHUDItem.u003cActiveSkinAfterWaitu003ec__Iterator14B variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Crt_Anim_InCenter(GameObject obj, Vector3 offset, float width)
	{
		GiftHUDItem.u003cCrt_Anim_InCenteru003ec__Iterator14C variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Crt_TimerAnim()
	{
		GiftHUDItem.u003cCrt_TimerAnimu003ec__Iterator14D variable = null;
		return variable;
	}

	private void FastCenter(UIScrollView scroll, Vector3 needPos)
	{
		float single = RealTime.deltaTime;
		Vector3 vector3 = scroll.transform.localPosition;
		Vector3 vector31 = needPos;
		scroll.transform.localPosition = vector31;
		Vector3 vector32 = vector31 - vector3;
		Vector2 vector2 = scroll.panel.clipOffset;
		vector2.x -= vector32.x;
		vector2.y -= vector32.y;
		scroll.panel.clipOffset = vector2;
	}

	public void InCenter(bool anim = false, int countBut = 1)
	{
		UIScrollView componentInParent = base.GetComponentInParent<UIScrollView>();
		if (componentInParent == null)
		{
			return;
		}
		Transform transforms = base.transform;
		Vector3[] vector3Array = componentInParent.panel.worldCorners;
		Vector3 vector3 = (vector3Array[2] + vector3Array[0]) * 0.5f;
		if (transforms != null && componentInParent != null && componentInParent.panel != null)
		{
			Transform transforms1 = componentInParent.panel.cachedTransform;
			GameObject gameObject = transforms.gameObject;
			Vector3 vector31 = transforms1.InverseTransformPoint(transforms.position);
			Vector3 vector32 = vector31 - transforms1.InverseTransformPoint(vector3);
			if (!componentInParent.canMoveHorizontally)
			{
				vector32.x = 0f;
			}
			if (!componentInParent.canMoveVertically)
			{
				vector32.y = 0f;
			}
			vector32.z = 0f;
			if (!anim)
			{
				Vector3 vector33 = Vector3.zero;
				if (componentInParent.transform.localPosition.Equals(transforms1.localPosition - vector32))
				{
					vector33 = new Vector3(1f, 0f, 0f);
				}
				SpringPanel.Begin(componentInParent.gameObject, (transforms1.localPosition - vector32) + vector33, 10f);
			}
			else
			{
				Vector3 vector34 = transforms1.localPosition - vector32;
				base.StartCoroutine(this.Crt_Anim_InCenter(componentInParent.panel.cachedGameObject, vector34, (float)(countBut * 130)));
			}
		}
	}

	private void OnEnable()
	{
		if (this.colliderForDrag == null)
		{
			this.colliderForDrag = base.GetComponent<BoxCollider>();
		}
		if (!this.isInfo)
		{
			base.StartCoroutine(this.ActiveSkinAfterWait());
		}
	}

	private void SetImage()
	{
		int num;
		Texture itemIcon = null;
		string str = null;
		switch (this._data.category.Type)
		{
			case GiftCategoryType.Coins:
			{
				itemIcon = Resources.Load<Texture>("OfferIcons/Marathon/bonus_coins");
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Gems:
			{
				itemIcon = Resources.Load<Texture>("OfferIcons/Marathon/bonus_gems");
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Grenades:
			{
				string empty = string.Empty;
				string id = this._data.gift.Id;
				if (id != null)
				{
					if (GiftHUDItem.u003cu003ef__switchu0024mapB == null)
					{
						Dictionary<string, int> strs = new Dictionary<string, int>(2)
						{
							{ "GrenadeID", 0 },
							{ "LikeID", 1 }
						};
						GiftHUDItem.u003cu003ef__switchu0024mapB = strs;
					}
					if (GiftHUDItem.u003cu003ef__switchu0024mapB.TryGetValue(id, out num))
					{
						if (num == 0)
						{
							empty = "Marathon/bonus_grenade";
						}
						else if (num == 1)
						{
							empty = "LikeID";
						}
					}
				}
				itemIcon = Resources.Load<Texture>(string.Concat("OfferIcons/", empty));
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Gear:
			{
				string empty1 = string.Empty;
				if (this._data.gift.Id.Equals("MusicBox"))
				{
					empty1 = "Dater_bonus_turret";
				}
				if (this._data.gift.Id.Equals("Wings"))
				{
					empty1 = "Dater_bonus_jetpack";
				}
				if (this._data.gift.Id.Equals("Bear"))
				{
					empty1 = "Dater_bonus_mech";
				}
				if (this._data.gift.Id.Equals("BigHeadPotion"))
				{
					empty1 = "Dater_bonus_potion";
				}
				itemIcon = Resources.Load<Texture>(string.Concat("OfferIcons/Marathon/", empty1));
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Skins:
			{
				return;
			}
			case GiftCategoryType.ArmorAndHat:
			case GiftCategoryType.Wear:
			case GiftCategoryType.Masks:
			case GiftCategoryType.Capes:
			case GiftCategoryType.Boots:
			case GiftCategoryType.Hats_random:
			case GiftCategoryType.Gun1:
			case GiftCategoryType.Gun2:
			case GiftCategoryType.Gun3:
			case GiftCategoryType.Guns_gray:
			{
				ShopNGUIController.CategoryNames categoryName = (!this._data.gift.TypeShopCat.HasValue ? ShopNGUIController.CategoryNames.ArmorCategory : this._data.gift.TypeShopCat.Value);
				int? nullable = null;
				itemIcon = ItemDb.GetItemIcon(this._data.gift.Id, categoryName, nullable);
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Event_content:
			{
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Editor:
			{
				if (this._data.gift.Id == "editor_Cape")
				{
					itemIcon = Resources.Load<Texture>("OfferIcons/editor_win_cape");
				}
				else if (this._data.gift.Id != "editor_Skin")
				{
					UnityEngine.Debug.LogError(string.Format("[GIFT] unknown gift id: '{0}'", this._data.gift.Id));
				}
				else
				{
					itemIcon = Resources.Load<Texture>("OfferIcons/editor_win_skin2");
				}
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Stickers:
			{
				TypePackSticker? nullable1 = null;
				TypePackSticker? @enum = this._data.gift.Id.ToEnum<TypePackSticker>(nullable1);
				TypePackSticker value = @enum.Value;
				if (value == TypePackSticker.classic)
				{
					itemIcon = Resources.Load<Texture>("OfferIcons/free_smile");
				}
				else if (value == TypePackSticker.christmas)
				{
					itemIcon = Resources.Load<Texture>("OfferIcons/free_christmas_smile");
				}
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			case GiftCategoryType.Freespins:
			{
				itemIcon = Resources.Load<Texture>(string.Format("OfferIcons/free_spin_{0}", this._data.gift.Count.Value));
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
			default:
			{
				if (itemIcon == null)
				{
					this.sprIcon.spriteName = str;
					this.sprIcon.gameObject.SetActive(true);
					this.textureIcon.gameObject.SetActive(false);
				}
				else
				{
					this.textureIcon.mainTexture = itemIcon;
					this.textureIcon.gameObject.SetActive(true);
					this.sprIcon.gameObject.SetActive(false);
				}
				return;
			}
		}
	}

	public void SetInfoButton(SlotInfo curInfo)
	{
		this._data = curInfo;
		if (this._data == null)
		{
			UnityEngine.Debug.LogError("SetInfoButton");
			return;
		}
		if (this.sprIcon)
		{
			this.sprIcon.gameObject.SetActive(false);
		}
		if (this.textureIcon)
		{
			this.textureIcon.gameObject.SetActive(false);
		}
		if (this.skinModelTransform != null)
		{
			UnityEngine.Object.Destroy(this.skinModelTransform.gameObject);
			this.skinModelTransform = null;
		}
		string str = (this._data.CountGift <= 1 ? string.Empty : string.Concat(this._data.CountGift, " "));
		switch (this._data.category.Type)
		{
			case GiftCategoryType.Coins:
			{
				this.nameAndCountGift = string.Concat(str, LocalizationStore.Get("Key_0275"));
				break;
			}
			case GiftCategoryType.Gems:
			{
				this.nameAndCountGift = string.Concat(str, LocalizationStore.Get("Key_0951"));
				break;
			}
			case GiftCategoryType.Grenades:
			case GiftCategoryType.Gear:
			case GiftCategoryType.ArmorAndHat:
			case GiftCategoryType.Wear:
			case GiftCategoryType.Masks:
			case GiftCategoryType.Capes:
			case GiftCategoryType.Boots:
			case GiftCategoryType.Hats_random:
			case GiftCategoryType.Gun1:
			case GiftCategoryType.Gun2:
			case GiftCategoryType.Gun3:
			case GiftCategoryType.Guns_gray:
			{
				string str1 = str;
				string id = this._data.gift.Id;
				ShopNGUIController.CategoryNames? typeShopCat = this._data.gift.TypeShopCat;
				this.nameAndCountGift = string.Concat(str1, RespawnWindowItemToBuy.GetItemName(id, (!typeShopCat.HasValue ? ShopNGUIController.CategoryNames.ArmorCategory : typeShopCat.Value), 0));
				break;
			}
			case GiftCategoryType.Skins:
			{
				this.nameAndCountGift = SkinsController.skinsNamesForPers[this._data.gift.Id];
				break;
			}
			case GiftCategoryType.Event_content:
			{
				break;
			}
			case GiftCategoryType.Editor:
			{
				if (this._data.gift.Id == "editor_Cape")
				{
					this.nameAndCountGift = LocalizationStore.Get("Key_0746");
				}
				else if (this._data.gift.Id != "editor_Skin")
				{
					UnityEngine.Debug.LogError(string.Format("[GIFT] unknown gift id: '{0}'", this._data.gift.Id));
				}
				else
				{
					this.nameAndCountGift = LocalizationStore.Get("Key_0086");
				}
				break;
			}
			case GiftCategoryType.Stickers:
			{
				if (this._data.gift.Id == "classic")
				{
					this.nameAndCountGift = LocalizationStore.Get("Key_1756");
				}
				else if (this._data.gift.Id == "christmas")
				{
					this.nameAndCountGift = LocalizationStore.Get("Key_1758");
				}
				break;
			}
			case GiftCategoryType.Freespins:
			{
				this.nameAndCountGift = string.Format(LocalizationStore.Get("Key_2196"), this._data.CountGift);
				break;
			}
			default:
			{
				this.nameAndCountGift = string.Concat(str, LocalizationStore.Get(this._data.gift.KeyTranslateInfo));
				break;
			}
		}
		if (this.nameGift)
		{
			this.nameGift.text = this.nameAndCountGift;
		}
		if (this.lbInfoGift != null)
		{
			if (!string.IsNullOrEmpty(this._data.gift.KeyTranslateInfo))
			{
				this.lbInfoGift.text = ScriptLocalization.Get(this._data.gift.KeyTranslateInfo);
				this.lbInfoGift.gameObject.SetActive(true);
			}
			else if (string.IsNullOrEmpty(this._data.category.KeyTranslateInfoCommon))
			{
				this.lbInfoGift.gameObject.SetActive(false);
			}
			else
			{
				this.lbInfoGift.text = ScriptLocalization.Get(this._data.category.KeyTranslateInfoCommon);
				this.lbInfoGift.gameObject.SetActive(true);
			}
		}
		switch (this._data.category.Type)
		{
			case GiftCategoryType.Skins:
			{
				if (this.parentForSkin)
				{
					if (!this.isInfo)
					{
						this.parentForSkin.layer = LayerMask.NameToLayer("FriendsWindowGUI");
					}
					this.skinModelTransform = SkinsController.SkinModel(this._data.gift.Id, 1, this.parentForSkin.transform, this.offsetSkin, this.scaleSkin);
				}
				break;
			}
			case GiftCategoryType.ArmorAndHat:
			case GiftCategoryType.Wear:
			{
				this.SetImage();
				break;
			}
			case GiftCategoryType.Event_content:
			{
				break;
			}
			default:
			{
				goto case GiftCategoryType.Wear;
			}
		}
	}
}