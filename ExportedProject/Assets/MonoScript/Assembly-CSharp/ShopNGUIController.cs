using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Rilisoft;
using UnityEngine;

public class ShopNGUIController : MonoBehaviour
{
	public enum TrainingState
	{
		NotInSniperCategory = 0,
		OnSniperRifle = 1,
		InSniperCategoryNotOnSniperRifle = 2,
		NotInArmorCategory = 3,
		OnArmor = 4,
		InArmorCategoryNotOnArmor = 5,
		BackBlinking = 6
	}

	public enum CategoryNames
	{
		PrimaryCategory = 0,
		BackupCategory = 1,
		MeleeCategory = 2,
		SpecilCategory = 3,
		SniperCategory = 4,
		PremiumCategory = 5,
		HatsCategory = 6,
		ArmorCategory = 7,
		SkinsCategory = 8,
		CapesCategory = 9,
		BootsCategory = 10,
		GearCategory = 11,
		MaskCategory = 12
	}

	public delegate void Action7<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

	[CompilerGenerated]
	private sealed class _003CTryToBuy_003Ec__AnonStorey31C
	{
		internal EventHandler handleBackFromBank;

		internal GameObject mainPanel;

		internal Action onReturnFromBank;

		internal Action onExitCoinsShopAction;

		internal EventHandler act;

		internal ItemPrice price;

		internal Func<bool> successAdditionalCond;

		internal Action onSuccess;

		internal Action onFailure;

		internal Action onEnterCoinsShopAction;

		internal void _003C_003Em__4C5(object sender, EventArgs e)
		{
			BankController.Instance.BackRequested -= handleBackFromBank;
			BankController.Instance.InterfaceEnabled = false;
			coinsShop.thisScript.notEnoughCurrency = null;
			if (mainPanel != null)
			{
				mainPanel.SetActive(true);
			}
			if (onReturnFromBank != null)
			{
				onReturnFromBank();
			}
			if (onExitCoinsShopAction != null)
			{
				onExitCoinsShopAction();
			}
		}

		internal void _003C_003Em__4C6(object sender, EventArgs e)
		{
			BankController.Instance.BackRequested -= act;
			mainPanel.SetActive(true);
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int @int = Storager.getInt(price.Currency, false);
			int num = @int;
			num -= price.Price;
			bool flag = num >= 0;
			bool num2;
			if (successAdditionalCond != null)
			{
				if (successAdditionalCond())
				{
					goto IL_0088;
				}
				num2 = flag;
			}
			else
			{
				num2 = flag;
			}
			if (!num2)
			{
				if (onFailure != null)
				{
					onFailure();
				}
				coinsShop.thisScript.notEnoughCurrency = price.Currency;
				Debug.Log("Trying to display bank interface...");
				BankController.Instance.BackRequested += handleBackFromBank;
				BankController.Instance.InterfaceEnabled = true;
				mainPanel.SetActive(false);
				if (onEnterCoinsShopAction != null)
				{
					onEnterCoinsShopAction();
				}
				return;
			}
			goto IL_0088;
			IL_0088:
			Storager.setInt(price.Currency, num, false);
			SpendBoughtCurrency(price.Currency, price.Price);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
			if (FriendsController.useBuffSystem)
			{
				BuffSystem.instance.OnSomethingPurchased();
			}
			if (onSuccess != null)
			{
				onSuccess();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CFilterUpgrades_003Ec__AnonStorey31D
	{
		internal GameObject prefab;

		internal bool _003C_003Em__4C7(List<string> l)
		{
			return l.Contains(prefab.name);
		}
	}

	[CompilerGenerated]
	private sealed class _003CUpdateIcon_003Ec__AnonStorey31F
	{
		internal ToggleButton tb;

		internal List<GameObject> toDestroy;

		internal CategoryNames c;

		internal bool animateModel;

		internal ShopNGUIController _003C_003Ef__this;

		private static TweenDelegate.TweenCallback _003C_003Ef__am_0024cache5;

		internal void _003C_003Em__4CA(Transform ch)
		{
			if (!(ch.gameObject == tb.gameObject) && !(ch.gameObject == tb.onButton.gameObject) && !(ch.gameObject == tb.offButton.gameObject) && !ch.gameObject.name.Equals("Label") && !ch.gameObject.name.Equals("ShopIcon"))
			{
				if (ch.gameObject.name.Equals("Sprite"))
				{
					ch.gameObject.SetActive(false);
				}
				else
				{
					toDestroy.Add(ch.gameObject);
				}
			}
		}

		internal void _003C_003Em__4CB(GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float sc, int _unusedTier, int _unusedLeague)
		{
			manipulateObject.transform.parent = tb.transform;
			if (c == CategoryNames.SkinsCategory)
			{
				Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.currentSkinForPers, new GameObject[0]);
			}
			float num = 0.5f;
			Transform transform = manipulateObject.transform;
			transform.localPosition = tb.onButton.transform.localPosition + positionShop * num;
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
			transform.Rotate(rotationShop, Space.World);
			transform.localScale = new Vector3(sc * num, sc * num, sc * num);
			if (c == CategoryNames.CapesCategory && _003C_003Ef__this._currentCape.Equals("cape_Custom") && SkinsController.capeUserTexture != null)
			{
				Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.capeUserTexture, new GameObject[0]);
			}
			_003C_003Ef__this.SetIconModelsPositions(transform, c);
			if (animateModel)
			{
				Vector3 localScale = transform.localScale;
				transform.localScale *= 1.25f;
				TweenParms tweenParms = new TweenParms().Prop("localScale", localScale).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear);
				if (_003C_003Ef__am_0024cache5 == null)
				{
					_003C_003Ef__am_0024cache5 = _003C_003Em__51E;
				}
				HOTween.To(transform, 0.25f, tweenParms.OnComplete(_003C_003Ef__am_0024cache5));
			}
		}

		private static void _003C_003Em__51E()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CUpdateIcon_003Ec__AnonStorey320
	{
		internal Texture texture;

		private static TweenDelegate.TweenCallback _003C_003Ef__am_0024cache1;

		internal void _003C_003Em__4CE(Transform ch)
		{
			if (ch.gameObject.name.Equals("Sprite"))
			{
				ch.gameObject.SetActive(false);
			}
			else
			{
				if (!ch.gameObject.name.Equals("ShopIcon"))
				{
					return;
				}
				UITexture component = ch.GetComponent<UITexture>();
				if (component.mainTexture == null || !component.mainTexture.name.Equals(texture.name) || HOTween.IsTweening(ch))
				{
					HOTween.Kill(ch);
					ch.localScale = new Vector3(1.25f, 1.25f, 1.25f);
					TweenParms tweenParms = new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear);
					if (_003C_003Ef__am_0024cache1 == null)
					{
						_003C_003Ef__am_0024cache1 = _003C_003Em__51F;
					}
					HOTween.To(ch, 0.25f, tweenParms.OnComplete(_003C_003Ef__am_0024cache1));
				}
				component.mainTexture = texture;
			}
		}

		private static void _003C_003Em__51F()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CShowTryGunIfPossible_003Ec__AnonStorey321
	{
		internal string tg;

		internal bool _003C_003Em__4D0(UnityEngine.Object w)
		{
			return ItemDb.GetByPrefabName(w.name).Tag == tg;
		}

		internal bool _003C_003Em__4D1(string t)
		{
			return t == tg;
		}
	}

	[CompilerGenerated]
	private sealed class _003CShowTryGunIfPossible_003Ec__AnonStorey322
	{
		internal int maximumCoinBank;

		internal bool _003C_003Em__4D7(ItemRecord rec)
		{
			return PriceIfGunWillBeTryGun(rec.Tag) > maximumCoinBank;
		}
	}

	[CompilerGenerated]
	private sealed class _003CShowTryGunIfPossible_003Ec__AnonStorey323
	{
		internal int maximumGemBank;

		internal bool _003C_003Em__4D9(ItemRecord rec)
		{
			return PriceIfGunWillBeTryGun(rec.Tag) > maximumGemBank;
		}
	}

	[CompilerGenerated]
	private sealed class _003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey325
	{
		internal CategoryNames cat;

		internal bool _003C_003Em__4DC(WeaponSounds ws)
		{
			return ws.categoryNabor - 1 == (int)cat && ws.tier == ExpController.OurTierForAnyPlace();
		}

		internal bool _003C_003Em__4DF(WeaponSounds ws)
		{
			return WeaponManager.tryGunsTable[cat][ExpController.OurTierForAnyPlace()].Contains(ItemDb.GetByTag(ItemDb.GetByPrefabName(ws.name).Tag).PrefabName);
		}
	}

	[CompilerGenerated]
	private sealed class _003CHandleProfileButton_003Ec__AnonStorey326
	{
		internal GameObject mainMenu;

		internal GameObject inGameGui;

		internal GameObject networkTable;

		internal void _003C_003Em__4E4()
		{
			GuiActive = true;
			if ((bool)mainMenu)
			{
				mainMenu.SetActive(true);
			}
			if ((bool)inGameGui)
			{
				inGameGui.SetActive(true);
			}
			if ((bool)networkTable)
			{
				networkTable.SetActive(true);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CReloadCarousel_003Ec__AnonStorey328
	{
		internal string idToChoose;

		internal ShopNGUIController _003C_003Ef__this;

		internal bool _003C_003Em__4E8(GameObject go)
		{
			return go.nameNoClone() == idToChoose;
		}
	}

	[CompilerGenerated]
	private sealed class _003CReloadCarousel_003Ec__AnonStorey327
	{
		internal ItemRecord itemRecord;

		internal bool _003C_003Em__4E7(GameObject go)
		{
			return go.nameNoClone() == itemRecord.PrefabName;
		}
	}

	[CompilerGenerated]
	private sealed class _003CReloadCarousel_003Ec__AnonStorey329
	{
		private sealed class _003CReloadCarousel_003Ec__AnonStorey32A
		{
			internal CategoryNames category;

			internal _003CReloadCarousel_003Ec__AnonStorey329 _003C_003Ef__ref_0024809;

			internal void _003C_003Em__520(GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float scaleCoefShop, int tier, int league)
			{
				if (_003C_003Ef__ref_0024809.sce == null)
				{
					UnityEngine.Object.Destroy(manipulateObject);
					return;
				}
				if (category != CategoryNames.SkinsCategory)
				{
					_003C_003Ef__ref_0024809.sce.readableName = readableName ?? string.Empty;
				}
				manipulateObject.transform.parent = _003C_003Ef__ref_0024809.sce.transform;
				_003C_003Ef__ref_0024809.sce.baseScale = new Vector3(scaleCoefShop, scaleCoefShop, scaleCoefShop);
				_003C_003Ef__ref_0024809.sce.model = manipulateObject.transform;
				_003C_003Ef__ref_0024809.sce.ourPosition = positionShop;
				_003C_003Ef__ref_0024809.sce.SetPos((!_003C_003Ef__ref_0024809._003C_003Ef__this.EnableConfigurePos) ? 0f : 1f, 0f);
				_003C_003Ef__ref_0024809.sce.model.Rotate(rotationShop, Space.World);
				if (category == CategoryNames.SkinsCategory)
				{
					Player_move_c.SetTextureRecursivelyFrom(manipulateObject, (!_003C_003Ef__ref_0024809.itenID.Equals("CustomSkinID")) ? SkinsController.skinsForPers[_003C_003Ef__ref_0024809.itenID] : (Resources.Load("Skin_Start") as Texture), new GameObject[0]);
				}
				if (_003C_003Ef__ref_0024809.itenID.Equals("cape_Custom") && SkinsController.capeUserTexture != null)
				{
					Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.capeUserTexture, new GameObject[0]);
				}
				if (ExpController.Instance != null && ExpController.Instance.OurTier < tier && tier < 100 && ((IsWeaponCategory(category) && _003C_003Ef__ref_0024809.sce.itemID.Equals(WeaponManager.FirstUnboughtOrForOurTier(_003C_003Ef__ref_0024809.sce.itemID)) && !_003C_003Ef__ref_0024809.isBought) || (IsWearCategory(category) && _003C_003Ef__ref_0024809.sce.itemID.Equals(WeaponManager.FirstUnboughtTag(_003C_003Ef__ref_0024809.sce.itemID)) && _003C_003Ef__ref_0024809.sce.itemID != "cape_Custom" && _003C_003Ef__ref_0024809.sce.itemID != "boots_tabi")) && _003C_003Ef__ref_0024809.sce.locked != null)
				{
					_003C_003Ef__ref_0024809.sce.locked.SetActive(true);
				}
				if ((IsWeaponCategory(category) && !_003C_003Ef__ref_0024809.sce.itemID.Equals(WeaponManager.FirstUnboughtOrForOurTier(_003C_003Ef__ref_0024809.sce.itemID)) && tier < 100) || (IsWearCategory(category) && !_003C_003Ef__ref_0024809.sce.itemID.Equals(WeaponManager.FirstUnboughtTag(_003C_003Ef__ref_0024809.sce.itemID))))
				{
					if (_003C_003Ef__ref_0024809.sce.arrow != null)
					{
						_003C_003Ef__ref_0024809.sce.arrow.gameObject.SetActive(true);
					}
					if (category == CategoryNames.HatsCategory)
					{
						_003C_003Ef__ref_0024809.sce.arrnoInitialPos = new Vector3(85f, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.y, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.z);
					}
					if (category == CategoryNames.ArmorCategory)
					{
						_003C_003Ef__ref_0024809.sce.arrnoInitialPos = new Vector3(105f, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.y, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.z);
					}
					if (category == CategoryNames.CapesCategory)
					{
						_003C_003Ef__ref_0024809.sce.arrnoInitialPos = new Vector3(75f, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.y, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.z);
					}
					if (category == CategoryNames.BootsCategory)
					{
						_003C_003Ef__ref_0024809.sce.arrnoInitialPos = new Vector3(81f, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.y, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.z);
					}
					if (category == CategoryNames.MaskCategory)
					{
						_003C_003Ef__ref_0024809.sce.arrnoInitialPos = new Vector3(75f, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.y, _003C_003Ef__ref_0024809.sce.arrnoInitialPos.z);
					}
				}
			}
		}

		internal GameObject pref;

		internal ShopCarouselElement sce;

		internal string itenID;

		internal bool isBought;

		internal ShopNGUIController _003C_003Ef__this;

		internal void _003C_003Em__4E9(GameObject loadedOBject, CategoryNames category)
		{
			_003CReloadCarousel_003Ec__AnonStorey32A _003CReloadCarousel_003Ec__AnonStorey32A = new _003CReloadCarousel_003Ec__AnonStorey32A();
			_003CReloadCarousel_003Ec__AnonStorey32A._003C_003Ef__ref_0024809 = this;
			_003CReloadCarousel_003Ec__AnonStorey32A.category = category;
			AddModel(loadedOBject, _003CReloadCarousel_003Ec__AnonStorey32A._003C_003Em__520, _003CReloadCarousel_003Ec__AnonStorey32A.category, false, (!IsWeaponCategory(_003CReloadCarousel_003Ec__AnonStorey32A.category)) ? null : pref.GetComponent<WeaponSounds>());
		}

		internal bool _003C_003Em__4EB(GameObject go)
		{
			return go.nameNoClone() == itenID;
		}
	}

	[CompilerGenerated]
	private sealed class _003CReloadCarousel_003Ec__AnonStorey32B
	{
		internal ItemRecord itemRecord;

		internal bool _003C_003Em__4EA(GameObject go)
		{
			return go.name.Replace("(Clone)", string.Empty) == itemRecord.PrefabName;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAddModel_003Ec__AnonStorey32C
	{
		internal ItemRecord firstRec;

		internal bool _003C_003Em__4EC(WeaponSounds weapon)
		{
			return weapon.name == firstRec.PrefabName;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D
	{
		internal string id;

		internal bool _003C_003Em__4EE(List<string> l)
		{
			return l.Contains(id);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCategoryChosen_003Ec__AnonStorey32E
	{
		internal string fu;

		internal ShopNGUIController _003C_003Ef__this;

		internal int _003C_003Em__4F0(string item)
		{
			return Wear.LeagueForWear(item, _003C_003Ef__this.currentCategory);
		}

		internal bool _003C_003Em__4F1(ShopPositionParams go)
		{
			return go.name.Equals(fu);
		}
	}

	[CompilerGenerated]
	private sealed class _003CSetRenderersVisibleFromPoint_003Ec__AnonStorey32F
	{
		internal bool showArmor;

		internal void _003C_003Em__4F2(Transform t)
		{
			Renderer component = t.GetComponent<Renderer>();
			if (component != null)
			{
				component.material.shader = Shader.Find((!showArmor) ? "Mobile/Transparent-Shop" : "Mobile/Diffuse");
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CAwake_003Ec__AnonStorey330
	{
		private sealed class _003CAwake_003Ec__AnonStorey331
		{
			internal EventHandler handleBackFromBank;

			internal void _003C_003Em__521(object sender_, EventArgs e_)
			{
				if (BankController.Instance.InterfaceEnabledCoroutineLocked)
				{
					Debug.LogWarning("InterfaceEnabledCoroutineLocked");
					return;
				}
				BankController.Instance.BackRequested -= handleBackFromBank;
				BankController.Instance.InterfaceEnabled = false;
				GuiActive = true;
			}
		}

		private sealed class _003CAwake_003Ec__AnonStorey334
		{
			internal int priceAmount;

			internal string priceCurrency;

			internal _003CAwake_003Ec__AnonStorey330 _003C_003Ef__ref_0024816;

			internal void _003C_003Em__522()
			{
				if (Defs.isSoundFX)
				{
					_003C_003Ef__ref_0024816._003C_003Ef__this.enable.GetComponent<UIPlaySound>().Play();
				}
				if (ShopNGUIController.GunBought != null)
				{
					ShopNGUIController.GunBought();
				}
				FlurryPluginWrapper.LogPurchaseByModes(CategoryNames.SkinsCategory, Defs.SkinsMakerInProfileBought, 1, false);
				FlurryPluginWrapper.LogPurchasesPoints(false);
				FlurryPluginWrapper.LogPurchaseByPoints(CategoryNames.SkinsCategory, Defs.SkinsMakerInProfileBought, 1);
				_003C_003Ef__ref_0024816._003C_003Ef__this.LogShopPurchasesTotalAndPayingNonPaying(Defs.SkinsMakerInProfileBought);
				string text = FlurryEvents.shopCategoryToLogSalesNamesMapping[CategoryNames.SkinsCategory];
				AnalyticsStuff.LogSales(Defs.SkinsMakerInProfileBought, text);
				AnalyticsFacade.InAppPurchase(Defs.SkinsMakerInProfileBought, text, 1, priceAmount, priceCurrency);
				Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
				SynchronizeAndroidPurchases("Custom skin");
				FlurryPluginWrapper.LogEvent("Enable_Custom Skin");
				_003C_003Ef__ref_0024816._003C_003Ef__this.wholePrice.gameObject.SetActive(false);
				if (!_003C_003Ef__ref_0024816._003C_003Ef__this.inGame)
				{
					_003C_003Ef__ref_0024816._003C_003Ef__this.goToSM();
				}
			}

			internal void _003C_003Em__524()
			{
				_003C_003Ef__ref_0024816._003C_003Ef__this.PlayWeaponAnimation();
			}

			internal void _003C_003Em__526()
			{
				_003C_003Ef__ref_0024816._003C_003Ef__this.SetOtherCamerasEnabled(false);
			}
		}

		private sealed class _003CAwake_003Ec__AnonStorey335
		{
			internal string skinWhereDelteWasPressed;

			internal _003CAwake_003Ec__AnonStorey330 _003C_003Ef__ref_0024816;

			internal void _003C_003Em__527()
			{
				ButtonClickSound.Instance.PlayClick();
				string currentSkinNameForPers = SkinsController.currentSkinNameForPers;
				if (skinWhereDelteWasPressed != null)
				{
					SkinsController.DeleteUserSkin(skinWhereDelteWasPressed);
					if (skinWhereDelteWasPressed.Equals(currentSkinNameForPers))
					{
						_003C_003Ef__ref_0024816._003C_003Ef__this.SetSkinAsCurrent("0");
						_003C_003Ef__ref_0024816._003C_003Ef__this.UpdateIcon(CategoryNames.SkinsCategory);
					}
				}
				_003C_003Ef__ref_0024816._003C_003Ef__this.ReloadCarousel(SkinsController.currentSkinNameForPers ?? "0");
			}
		}

		internal Action toggleShowArmor;

		internal Action toggleShowHat;

		internal ShopNGUIController _003C_003Ef__this;

		private static Action _003C_003Ef__am_0024cache3;

		private static Action _003C_003Ef__am_0024cache4;

		internal void _003C_003Em__4F3(object sender, MultipleToggleEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			if (Defs.isSoundFX && _003C_003Ef__this.category.buttons != null && _003C_003Ef__this.category.buttons.Length > e.Num)
			{
				_003C_003Ef__this.category.buttons[e.Num].offButton.GetComponent<UIPlaySound>().Play();
			}
			try
			{
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					if (e.Num == 4 && _003C_003Ef__this.trainingState == TrainingState.NotInSniperCategory)
					{
						AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Category_Sniper);
						_003C_003Ef__this.setOnSniperRifle();
					}
					else if (e.Num != 4 && (_003C_003Ef__this.trainingState == TrainingState.OnSniperRifle || _003C_003Ef__this.trainingState == TrainingState.InSniperCategoryNotOnSniperRifle))
					{
						_003C_003Ef__this.setNotInSniperCategory();
					}
					else if (e.Num == 7 && _003C_003Ef__this.trainingState == TrainingState.NotInArmorCategory)
					{
						_003C_003Ef__this.setOnArmor();
						AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Category_Armor);
					}
					else if (e.Num != 7 && (_003C_003Ef__this.trainingState == TrainingState.OnArmor || _003C_003Ef__this.trainingState == TrainingState.InArmorCategoryNotOnArmor))
					{
						_003C_003Ef__this.setNotInArmorCategory();
					}
				}
				if (_003C_003Ef__this.InTrainingAfterNoviceArmorRemoved)
				{
					if (e.Num == 7 && _003C_003Ef__this.trainingStateRemoveNoviceArmor == TrainingState.NotInArmorCategory)
					{
						_003C_003Ef__this.setOnArmorRemovedNoviceArmor();
						AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Category_Armor);
					}
					else if (e.Num != 7 && (_003C_003Ef__this.trainingStateRemoveNoviceArmor == TrainingState.OnArmor || _003C_003Ef__this.trainingStateRemoveNoviceArmor == TrainingState.InArmorCategoryNotOnArmor))
					{
						_003C_003Ef__this.setNotInArmorCategoryRemovedNoviceArmor();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exceptio in training in category.Clicked: " + ex);
			}
			_003C_003Ef__this.CategoryChosen((CategoryNames)e.Num);
		}

		internal void _003C_003Em__4F4(object sender, EventArgs e)
		{
			_003C_003Ef__this.BuyOrUpgradeWeapon();
		}

		internal void _003C_003Em__4F5(object sender, EventArgs e)
		{
			_003C_003Ef__this.BuyOrUpgradeWeapon();
		}

		internal void _003C_003Em__4F6(object sender, EventArgs e)
		{
			_003C_003Ef__this.BuyOrUpgradeWeapon(true);
		}

		internal void _003C_003Em__4F7(object sender, EventArgs e)
		{
			_003C_003Ef__this.BuyOrUpgradeWeapon(true);
		}

		internal void _003C_003Em__4F8()
		{
			ShowArmor = !ShowArmor;
			SetPersArmorVisible(_003C_003Ef__this.armorPoint);
			PlayerPrefs.SetInt("ShowArmorKeySetting", ShowArmor ? 1 : 0);
		}

		internal void _003C_003Em__4F9()
		{
			ShowHat = !ShowHat;
			SetPersHatVisible(_003C_003Ef__this.hatPoint);
			PlayerPrefs.SetInt("ShowHatKeySetting", ShowHat ? 1 : 0);
		}

		internal void _003C_003Em__4FA(object sender, ToggleButtonEventArgs e)
		{
			toggleShowArmor();
			_003C_003Ef__this.showArmorButtonTempArmor.SetCheckedWithoutEvent(!ShowArmor);
		}

		internal void _003C_003Em__4FB(object sender, ToggleButtonEventArgs e)
		{
			toggleShowHat();
			_003C_003Ef__this.showHatButtonTempHat.SetCheckedWithoutEvent(!ShowHat);
		}

		internal void _003C_003Em__4FC(object sender, ToggleButtonEventArgs e)
		{
			toggleShowArmor();
			_003C_003Ef__this.showArmorButton.SetCheckedWithoutEvent(!ShowArmor);
		}

		internal void _003C_003Em__4FD(object sender, ToggleButtonEventArgs e)
		{
			toggleShowHat();
			_003C_003Ef__this.showHatButton.SetCheckedWithoutEvent(!ShowHat);
		}

		internal void _003C_003Em__4FE(object sender, EventArgs e)
		{
			if (!(Time.realtimeSinceStartup - _003C_003Ef__this.timeOfEnteringShopForProtectFromPressingCoinsButton < 0.5f) && BankController.Instance != null)
			{
				_003CAwake_003Ec__AnonStorey331 _003CAwake_003Ec__AnonStorey = new _003CAwake_003Ec__AnonStorey331();
				if (BankController.Instance.InterfaceEnabledCoroutineLocked)
				{
					Debug.LogWarning("InterfaceEnabledCoroutineLocked");
					return;
				}
				_003CAwake_003Ec__AnonStorey.handleBackFromBank = null;
				_003CAwake_003Ec__AnonStorey.handleBackFromBank = _003CAwake_003Ec__AnonStorey._003C_003Em__521;
				BankController.Instance.BackRequested += _003CAwake_003Ec__AnonStorey.handleBackFromBank;
				BankController.Instance.InterfaceEnabled = true;
				GuiActive = false;
			}
		}

		internal void _003C_003Em__4FF(object sender, EventArgs e)
		{
			_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.BackAfterDelay());
		}

		internal void _003C_003Em__501(object sender, EventArgs e)
		{
			if (Defs.isSoundFX)
			{
				_003C_003Ef__this.unequip.GetComponent<UIPlaySound>().Play();
			}
			if (_003C_003Ef__this.WearCategory)
			{
				CategoryNames currentCategory = _003C_003Ef__this.currentCategory;
				UnequipCurrentWearInCategory(currentCategory, _003C_003Ef__this.inGame);
			}
			_003C_003Ef__this.UpdateButtons();
		}

		internal void _003C_003Em__502(object sender, EventArgs e)
		{
			if (_003C_003Ef__this.viewedId != null && _003C_003Ef__this.viewedId.Equals("CustomSkinID"))
			{
				_003CAwake_003Ec__AnonStorey334 _003CAwake_003Ec__AnonStorey = new _003CAwake_003Ec__AnonStorey334();
				_003CAwake_003Ec__AnonStorey._003C_003Ef__ref_0024816 = this;
				ItemPrice itemPrice = currentPrice(_003C_003Ef__this.viewedId, _003C_003Ef__this.currentCategory);
				_003CAwake_003Ec__AnonStorey.priceAmount = itemPrice.Price;
				_003CAwake_003Ec__AnonStorey.priceCurrency = itemPrice.Currency;
				GameObject mainPanel = _003C_003Ef__this.mainPanel;
				Action onSuccess = _003CAwake_003Ec__AnonStorey._003C_003Em__522;
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003C_003Em__523;
				}
				Action onFailure = _003C_003Ef__am_0024cache3;
				Action onReturnFromBank = _003CAwake_003Ec__AnonStorey._003C_003Em__524;
				if (_003C_003Ef__am_0024cache4 == null)
				{
					_003C_003Ef__am_0024cache4 = _003C_003Em__525;
				}
				TryToBuy(mainPanel, itemPrice, onSuccess, onFailure, null, onReturnFromBank, _003C_003Ef__am_0024cache4, _003CAwake_003Ec__AnonStorey._003C_003Em__526);
				_003C_003Ef__this.UpdateButtons();
			}
			else if (_003C_003Ef__this.viewedId != null && _003C_003Ef__this.viewedId.Equals("cape_Custom"))
			{
				_003C_003Ef__this.BuyOrUpgradeWeapon();
			}
		}

		internal void _003C_003Em__503(object sender, EventArgs e)
		{
			ButtonClickSound.Instance.PlayClick();
			if (!_003C_003Ef__this.inGame)
			{
				_003C_003Ef__this.goToSM();
			}
		}

		internal void _003C_003Em__504(object sender, EventArgs e)
		{
			ButtonClickSound.Instance.PlayClick();
			if (!_003C_003Ef__this.inGame)
			{
				_003C_003Ef__this.goToSM();
			}
		}

		internal void _003C_003Em__505(object sender, EventArgs e)
		{
			_003CAwake_003Ec__AnonStorey335 _003CAwake_003Ec__AnonStorey = new _003CAwake_003Ec__AnonStorey335();
			_003CAwake_003Ec__AnonStorey._003C_003Ef__ref_0024816 = this;
			_003CAwake_003Ec__AnonStorey.skinWhereDelteWasPressed = _003C_003Ef__this.viewedId;
			InfoWindowController.ShowDialogBox(LocalizationStore.Get("Key_1693"), _003CAwake_003Ec__AnonStorey._003C_003Em__527);
		}

		private static void _003C_003Em__523()
		{
			FlurryPluginWrapper.LogEvent("Try_Enable_Custom Skin");
		}

		private static void _003C_003Em__525()
		{
			SetBankCamerasEnabled();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAwake_003Ec__AnonStorey332
	{
		private sealed class _003CAwake_003Ec__AnonStorey333
		{
			internal string prefabName;

			internal bool _003C_003Em__528(Weapon weapon)
			{
				return weapon.weaponPrefab.nameNoClone() == prefabName;
			}
		}

		internal UIButton ee;

		internal ShopNGUIController _003C_003Ef__this;

		internal void _003C_003Em__500(object sender, EventArgs e)
		{
			if (Defs.isSoundFX)
			{
				ee.GetComponent<UIPlaySound>().Play();
			}
			if (_003C_003Ef__this.WeaponCategory)
			{
				_003CAwake_003Ec__AnonStorey333 _003CAwake_003Ec__AnonStorey = new _003CAwake_003Ec__AnonStorey333();
				string text = WeaponManager.LastBoughtTag(_003C_003Ef__this.viewedId);
				if (text == null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(_003C_003Ef__this.viewedId))
				{
					text = _003C_003Ef__this.viewedId;
				}
				if (text == null)
				{
					return;
				}
				_003CAwake_003Ec__AnonStorey.prefabName = ItemDb.GetByTag(text).PrefabName;
				Weapon w = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault(_003CAwake_003Ec__AnonStorey._003C_003Em__528);
				WeaponManager.sharedManager.EquipWeapon(w, true, true);
				WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
				if (_003C_003Ef__this.equipAction != null)
				{
					_003C_003Ef__this.equipAction(text);
				}
				_003C_003Ef__this.chosenId = text;
				_003C_003Ef__this.UpdateIcon(_003C_003Ef__this.currentCategory);
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted && _003C_003Ef__this.trainingState == TrainingState.OnSniperRifle && text != null && text == WeaponTags.HunterRifleTag)
				{
					if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1)
					{
						_003C_003Ef__this.setBackBlinking();
					}
					else
					{
						_003C_003Ef__this.setNotInArmorCategory();
						AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Equip_Sniper);
					}
				}
				if (_003C_003Ef__this.InTrainingAfterNoviceArmorRemoved)
				{
					_003C_003Ef__this.HideAllTrainingInterfaceRemovedNoviceArmor();
					_003C_003Ef__this.InTrainingAfterNoviceArmorRemoved = false;
					_003C_003Ef__this.HandleActionsUUpdated();
				}
			}
			else if (_003C_003Ef__this.WearCategory)
			{
				string text2 = WeaponManager.LastBoughtTag(_003C_003Ef__this.viewedId);
				if (text2 != null)
				{
					_003C_003Ef__this.EquipWear(text2);
				}
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					if (_003C_003Ef__this.trainingState == TrainingState.OnArmor)
					{
						_003C_003Ef__this.setBackBlinking();
						AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Equip_Armor);
					}
				}
				else if (_003C_003Ef__this.InTrainingAfterNoviceArmorRemoved)
				{
					_003C_003Ef__this.HideAllTrainingInterfaceRemovedNoviceArmor();
					_003C_003Ef__this.InTrainingAfterNoviceArmorRemoved = false;
					_003C_003Ef__this.HandleActionsUUpdated();
				}
			}
			else if (_003C_003Ef__this.currentCategory == CategoryNames.SkinsCategory)
			{
				_003C_003Ef__this.SetSkinAsCurrent(_003C_003Ef__this.viewedId);
				_003C_003Ef__this.UpdateIcon(_003C_003Ef__this.currentCategory, true);
			}
			_003C_003Ef__this.UpdateButtons();
		}
	}

	[CompilerGenerated]
	private sealed class _003CgoToSM_003Ec__AnonStorey336
	{
		internal Action<string> backHandler;

		internal ShopNGUIController _003C_003Ef__this;

		internal void _003C_003Em__506(string n)
		{
			SkinEditorController.ExitFromSkinEditor -= backHandler;
			MenuBackgroundMusic.sharedMusic.StopCustomMusicFrom(SkinEditorController.sharedController.gameObject);
			_003C_003Ef__this.mainPanel.SetActive(true);
			if (_003C_003Ef__this.currentCategory == CategoryNames.CapesCategory || n != null)
			{
				if (_003C_003Ef__this.viewedId != null && _003C_003Ef__this.viewedId.Equals("CustomSkinID"))
				{
					_003C_003Ef__this.SetSkinAsCurrent(n);
				}
				if (_003C_003Ef__this.currentCategory == CategoryNames.SkinsCategory && _003C_003Ef__this.viewedId != null && _003C_003Ef__this.viewedId.Equals(SkinsController.currentSkinNameForPers))
				{
					_003C_003Ef__this.FireOnEquipSkin(n);
				}
				if (_003C_003Ef__this.viewedId != null && _003C_003Ef__this.viewedId.Equals("cape_Custom"))
				{
					_003C_003Ef__this.EquipWear("cape_Custom");
				}
				_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.ReloadAfterEditing(n));
			}
			else
			{
				_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.ReloadAfterEditing(n, n == null));
			}
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC == null)
			{
				if (_003C_003Ef__this.currentCategory != CategoryNames.CapesCategory && PlayerPrefs.GetInt(Defs.ShownRewardWindowForSkin, 0) == 0)
				{
					_003C_003Ef__this._shouldShowRewardWindowSkin = true;
				}
				if (_003C_003Ef__this.currentCategory == CategoryNames.CapesCategory && PlayerPrefs.GetInt(Defs.ShownRewardWindowForCape, 0) == 0)
				{
					_003C_003Ef__this._shouldShowRewardWindowCape = true;
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CBuyOrUpgradeWeapon_003Ec__AnonStorey337
	{
		internal bool upgradeNotBuy;

		internal string id;

		internal string tg;

		internal ItemPrice price;

		internal ShopNGUIController _003C_003Ef__this;

		internal void _003C_003Em__507()
		{
			if (Defs.isSoundFX)
			{
				((!upgradeNotBuy) ? _003C_003Ef__this.buy : _003C_003Ef__this.upgrade).GetComponent<UIPlaySound>().Play();
			}
			_003C_003Ef__this.ActualBuy(id, tg, price);
		}

		internal void _003C_003Em__508()
		{
			if (_003C_003Ef__this.currentCategory == CategoryNames.CapesCategory && _003C_003Ef__this.viewedId.Equals("cape_Custom"))
			{
				FlurryPluginWrapper.LogEvent("Try_Enable_Custom Cape");
			}
		}

		internal void _003C_003Em__509()
		{
			_003C_003Ef__this.PlayWeaponAnimation();
		}

		internal void _003C_003Em__50B()
		{
			_003C_003Ef__this.SetOtherCamerasEnabled(false);
			_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.ReloadAfterEditing(_003C_003Ef__this.viewedId));
		}
	}

	[CompilerGenerated]
	private sealed class _003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey338
	{
		internal Action<string> customEquipWearAction;

		internal bool equipWear;

		internal CategoryNames c;

		internal bool equipSkin;

		internal void _003C_003Em__50C(string item)
		{
			if (customEquipWearAction != null)
			{
				customEquipWearAction(item);
			}
			else if (equipWear)
			{
				SetAsEquippedAndSendToServer(item, c);
			}
		}

		internal void _003C_003Em__50D(string item)
		{
			if (equipSkin)
			{
				SaveSkinAndSendToServer(item);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CActualBuy_003Ec__AnonStorey339
	{
		internal CategoryNames c;

		internal string id;

		internal ShopNGUIController _003C_003Ef__this;

		internal void _003C_003Em__50F(string item)
		{
			_003C_003Ef__this.EquipWear(item);
		}

		internal void _003C_003Em__510(string item)
		{
			if (IsWeaponCategory(c) || IsWearCategory(c))
			{
				_003C_003Ef__this.FireBuyAction(item);
			}
			_003C_003Ef__this.purchaseSuccessful.SetActive(true);
			_003C_003Ef__this._timePurchaseSuccessfulShown = Time.realtimeSinceStartup;
		}

		internal void _003C_003Em__511(string item)
		{
			_003C_003Ef__this.SetSkinAsCurrent(item);
		}

		internal bool _003C_003Em__512(KeyValuePair<string, string> item)
		{
			return item.Value == id;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A
	{
		internal string comment;
	}

	[CompilerGenerated]
	private sealed class _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B
	{
		internal Action ResetWeaponManager;

		internal _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A _003C_003Ef__ref_0024826;

		internal void _003C_003Em__517(bool success)
		{
			Debug.LogFormat("[Rilisoft] ShopNguiController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", success, Time.realtimeSinceStartup);
			try
			{
				Debug.LogFormat("Google purchases syncronized ({0}): {1}", _003C_003Ef__ref_0024826.comment, success);
				if (success)
				{
					ResetWeaponManager();
				}
			}
			finally
			{
				Debug.LogFormat("[Rilisoft] ShopNguiController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", success, Time.realtimeSinceStartup);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003Csort_003Ec__AnonStorey31B
	{
		internal CategoryNames c;

		internal int _003C_003Em__518(ShopPositionParams go1, ShopPositionParams go2)
		{
			List<string> list = null;
			List<string> list2 = null;
			foreach (List<string> item in Wear.wear[c])
			{
				if (item.Contains(go1.name))
				{
					list = item;
				}
				if (item.Contains(go2.name))
				{
					list2 = item;
				}
			}
			if (list == null || list2 == null)
			{
				return 0;
			}
			if (list == list2)
			{
				return list.IndexOf(go1.name) - list.IndexOf(go2.name);
			}
			return Wear.wear[c].IndexOf(list) - Wear.wear[c].IndexOf(list2);
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillModelsList_003Ec__AnonStorey31E
	{
		internal CategoryNames cn;

		internal int _003C_003Em__519(GameObject lh, GameObject rh)
		{
			List<string> list = null;
			List<string> list2 = null;
			foreach (List<string> item in Wear.wear[cn])
			{
				if (item.Contains(lh.name))
				{
					list = item;
				}
				if (item.Contains(rh.name))
				{
					list2 = item;
				}
			}
			if (list == null || list2 == null)
			{
				return 0;
			}
			if (list == list2)
			{
				return list.IndexOf(lh.name) - list.IndexOf(rh.name);
			}
			return Wear.wear[cn].IndexOf(list) - Wear.wear[cn].IndexOf(list2);
		}
	}

	[CompilerGenerated]
	private sealed class _003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey324
	{
		internal CategoryNames cat;

		internal bool _003C_003Em__51B(WeaponSounds ws)
		{
			return ws.categoryNabor - 1 == (int)cat && ws.tier == ExpController.OurTierForAnyPlace();
		}
	}

	public const string BoughtCurrencsySettingBase = "BoughtCurrency";

	public const string TrainingShopStageStepKey = "ShopNGUIController.TrainingShopStageStepKey";

	public const string CustomSkinID = "CustomSkinID";

	private const string ShowArmorKey = "ShowArmorKeySetting";

	private const string ShowHatKey = "ShowHatKeySetting";

	[Header("Training After Removing Novice Armor")]
	public GameObject trainingRemoveNoviceArmorCollider;

	public List<GameObject> trainingTipsRemovedNoviceArmor = new List<GameObject>();

	[Header("Training")]
	public GameObject trainingColliders;

	public List<GameObject> trainingTips = new List<GameObject>();

	private bool _trainStateInitialized;

	private UISprite toBlink;

	private Dictionary<TrainingState, Action> _setTrainingStateMethods;

	private TrainingState _trainingState;

	private TrainingState _trainingStateRemovedNoviceArmor;

	public static ShopNGUIController sharedShop;

	public GameObject tryGunPanel;

	public UILabel tryGunMatchesCount;

	public GameObject tryGunDiscountPanel;

	public UILabel tryGunDiscountTime;

	public UILabel nonArmorWearDEscription;

	public UILabel armorWearDescription;

	public UILabel armorCountLabel;

	public UIButton facebookLoginLockedSkinButton;

	public List<UISprite> effectsSprites;

	public List<UILabel> effectsLabels;

	public GameObject hatLock;

	public GameObject armorLock;

	public BoxCollider shopCarouselCollider;

	public ToggleButton showArmorButtonTempArmor;

	public ToggleButton showHatButtonTempHat;

	public GameObject prolongateRentText;

	public GameObject purchaseSuccessfulRent;

	public UILabel salePercRent;

	public GameObject saleRent;

	public GameObject rentProperties;

	public GameObject notRented;

	public UILabel daysLeftLabel;

	public UILabel timeLeftLabel;

	public UILabel daysLeftValueLabel;

	public UILabel timeLeftValueLabel;

	public GameObject redBackForTime;

	public UIButton rent;

	public Transform rentScreenPoint;

	public ToggleButton showArmorButton;

	public ToggleButton showHatButton;

	public PropertyInfoScreenController infoScreen;

	public GameObject stub;

	public UIButton upgradeGear;

	public UISprite currencyImagePrice;

	public UISprite currencyImagePriceGear;

	public UISprite currencyImagePriceUpgradeGear;

	public UILabel price2Gear;

	public UILabel priceUpgradeGear;

	public GameObject wholePrice2Gear;

	public GameObject wholePriceUpgradeGear;

	public UIButton buyGear;

	public UITexture wholePriceBG;

	public UITexture wholePriceBG_Discount;

	public UITexture wholePriceBG2Gear;

	public UITexture wholePriceBG2Gear_Discount;

	public UITexture wholePriceUpgradeBG2Gear;

	public UITexture wholePriceUpgradeBG2Gear_Discount;

	public UILabel fireRate;

	public UILabel fireRateMElee;

	public UILabel mobility;

	public UILabel mobilityMelee;

	public UILabel capacity;

	public UILabel damage;

	public UILabel damageMelee;

	public GameObject needTier;

	public UILabel needTierLabel;

	public GameObject purchaseSuccessful;

	public List<Light> mylights;

	[Range(-200f, 200f)]
	public float firstOFfset = -50f;

	[Range(-200f, 200f)]
	public float secondOffset = 50f;

	public GameObject nonArmorWearProperties;

	public GameObject armorWearProperties;

	public GameObject skinProperties;

	public GameObject gearProperties;

	public GameObject border;

	public Action OnBecomeActive;

	private string offerID;

	public CategoryNames offerCategory;

	public float scaleCoef = 0.5f;

	public Transform maskPoint;

	public Transform hatPoint;

	public Transform capePoint;

	public Transform bootsPoint;

	public Transform armorPoint;

	public GameObject mainPanel;

	public UIButton backButton;

	public UIButton coinShopButton;

	public UIPanel scrollViewPanel;

	public UIGrid wrapContent;

	public MyCenterOnChild carouselCenter;

	public GameObject body;

	public GameObject weaponPointShop;

	public Transform MainMenu_Pers;

	public string viewedId;

	public string chosenId;

	public Action resumeAction;

	public Action wearResumeAction;

	public Action<CategoryNames, string> wearUnequipAction;

	public Action<CategoryNames, string, string> wearEquipAction;

	public Action<string> buyAction;

	public Action<string> equipAction;

	public Action<string> activatePotionAction;

	public Action<string> onEquipSkinAction;

	private GameObject weapon;

	private AnimationClip profile;

	public bool EnableConfigurePos;

	public MultipleToggleButton category;

	public UIButton[] equips;

	public UISprite[] equippeds;

	public UIButton create;

	public UIButton buy;

	public UIButton upgrade;

	public UIButton unequip;

	public UIButton edit;

	public UIButton enable;

	public UIButton delete;

	public GameObject weaponProperties;

	public GameObject meleeProperties;

	public GameObject upgradesAnchor;

	public GameObject upgrade_1;

	public GameObject upgrade_2;

	public GameObject upgrade_3;

	public GameObject SpecialParams;

	public GameObject[] upgradeSprites3;

	public GameObject[] upgradeSprites2;

	public GameObject[] upgradeSprites1;

	public GameObject wholePrice;

	public GameObject sale;

	public UILabel price;

	public UILabel caption;

	public UILabel salePerc;

	public CategoryNames currentCategory;

	public bool inGame = true;

	public List<Camera> ourCameras;

	public AnimationCoroutineRunner animationCoroutineRunner;

	public GameObject ActiveObject;

	private List<ShopPositionParams> hats = new List<ShopPositionParams>();

	private List<ShopPositionParams> capes = new List<ShopPositionParams>();

	private List<ShopPositionParams> boots = new List<ShopPositionParams>();

	private List<ShopPositionParams> masks = new List<ShopPositionParams>();

	private List<ShopPositionParams> armor = new List<ShopPositionParams>();

	private Action<List<ShopPositionParams>, CategoryNames> sort;

	private GameObject pixlMan;

	private int numberOfLoadingModels;

	public static readonly Dictionary<string, string> weaponCategoryLocKeys = new Dictionary<string, string>
	{
		{
			CategoryNames.PrimaryCategory.ToString(),
			"Key_0352"
		},
		{
			CategoryNames.BackupCategory.ToString(),
			"Key_0442"
		},
		{
			CategoryNames.MeleeCategory.ToString(),
			"Key_0441"
		},
		{
			CategoryNames.SpecilCategory.ToString(),
			"Key_0440"
		},
		{
			CategoryNames.SniperCategory.ToString(),
			"Key_1669"
		},
		{
			CategoryNames.PremiumCategory.ToString(),
			"Key_0093"
		}
	};

	private Transform highlightedCarouselObject;

	public int itemIndex;

	private GameObject _lastSelectedItem;

	private GameObject[] _onPersArmorRefs;

	private static bool _ShowArmor = true;

	private static bool _ShowHat = true;

	private float timeToUpdateTempGunTime;

	private bool _shouldShowRewardWindowSkin;

	private bool _shouldShowRewardWindowCape;

	private GameObject _refOnLowPolyArmor;

	private Material[] _refsOnLowPolyArmorMaterials;

	public static string[] gearOrder = PotionsController.potions;

	private string _currentCape;

	private string _currentHat;

	private string _currentBoots;

	private string _currentArmor;

	private string _currentMask;

	private float lastTime;

	public static float IdleTimeoutPers = 5f;

	private float idleTimerLastTime;

	private float _timePurchaseSuccessfulShown;

	private float _timePurchaseRentSuccessfulShown;

	private IDisposable _backSubscription;

	private bool _escapeRequested;

	private float timeOfEnteringShopForProtectFromPressingCoinsButton;

	private Color? _storedAmbientLight;

	private bool? _storedFogEnabled;

	private bool _isFromPromoActions;

	private string _promoActionsIdClicked;

	private string _assignedWeaponTag;

	private bool InTrainingAfterNoviceArmorRemoved;

	[CompilerGenerated]
	private static Action<List<ShopPositionParams>, CategoryNames> _003C_003Ef__am_0024cacheB4;

	[CompilerGenerated]
	private static Func<CategoryNames, Comparison<GameObject>> _003C_003Ef__am_0024cacheB5;

	[CompilerGenerated]
	private static Action<Transform> _003C_003Ef__am_0024cacheB6;

	[CompilerGenerated]
	private static Action<Transform> _003C_003Ef__am_0024cacheB7;

	[CompilerGenerated]
	private static Action<Transform> _003C_003Ef__am_0024cacheB8;

	[CompilerGenerated]
	private static Func<KeyValuePair<CategoryNames, List<List<string>>>, IEnumerable<string>> _003C_003Ef__am_0024cacheB9;

	[CompilerGenerated]
	private static Func<string, ItemRecord> _003C_003Ef__am_0024cacheBA;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cacheBB;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cacheBC;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cacheBD;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cacheBE;

	[CompilerGenerated]
	private static Func<CategoryNames, int> _003C_003Ef__am_0024cacheBF;

	[CompilerGenerated]
	private static Func<UnityEngine.Object, WeaponSounds> _003C_003Ef__am_0024cacheC0;

	[CompilerGenerated]
	private static Func<WeaponSounds, bool> _003C_003Ef__am_0024cacheC1;

	[CompilerGenerated]
	private static Func<WeaponSounds, bool> _003C_003Ef__am_0024cacheC2;

	[CompilerGenerated]
	private static Func<WeaponSounds, bool> _003C_003Ef__am_0024cacheC3;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cacheC4;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cacheC5;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cacheC6;

	[CompilerGenerated]
	private static Comparison<string> _003C_003Ef__am_0024cacheC7;

	[CompilerGenerated]
	private static Predicate<string> _003C_003Ef__am_0024cacheC8;

	[CompilerGenerated]
	private static Comparison<GameObject> _003C_003Ef__am_0024cacheC9;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cacheCA;

	[CompilerGenerated]
	private static Action<string> _003C_003Ef__am_0024cacheCB;

	[CompilerGenerated]
	private static Func<KeyValuePair<string, string>, string> _003C_003Ef__am_0024cacheCC;

	[CompilerGenerated]
	private static Predicate<GameObject> _003C_003Ef__am_0024cacheCD;

	[CompilerGenerated]
	private static Action<Transform> _003C_003Ef__am_0024cacheCE;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cacheCF;

	[CompilerGenerated]
	private static Func<UnityEngine.Object, WeaponSounds> _003C_003Ef__am_0024cacheD0;

	[CompilerGenerated]
	private static Func<WeaponSounds, bool> _003C_003Ef__am_0024cacheD1;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cacheD2;

	public static bool NoviceArmorAvailable
	{
		get
		{
			return Storager.getInt("Training.NoviceArmorUsedKey", false) == 1 && (!TrainingController.TrainingCompleted || Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1);
		}
	}

	private TrainingState trainingStateRemoveNoviceArmor
	{
		get
		{
			return _trainingStateRemovedNoviceArmor;
		}
		set
		{
			try
			{
				if (_trainingStateRemovedNoviceArmor != value)
				{
					_trainingStateRemovedNoviceArmor = value;
				}
				for (int i = 0; i < trainingTipsRemovedNoviceArmor.Count; i++)
				{
					GameObject obj = trainingTipsRemovedNoviceArmor[i];
					string text = trainingTipsRemovedNoviceArmor[i].name;
					int trainingStateRemovedNoviceArmor = (int)_trainingStateRemovedNoviceArmor;
					obj.SetActive(text == trainingStateRemovedNoviceArmor.ToString());
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in trainingStateRemoveNoviceArmor setter: " + ex);
			}
		}
	}

	private TrainingState trainingState
	{
		get
		{
			if (!_trainStateInitialized)
			{
				_trainingState = (TrainingState)Storager.getInt("ShopNGUIController.TrainingShopStageStepKey", false);
				_trainStateInitialized = true;
			}
			return _trainingState;
		}
		set
		{
			try
			{
				if (_trainingState != value)
				{
					_trainingState = value;
					if (_trainingState == TrainingState.NotInArmorCategory || _trainingState == TrainingState.BackBlinking)
					{
						Storager.setInt("ShopNGUIController.TrainingShopStageStepKey", (int)_trainingState, false);
					}
				}
				_trainStateInitialized = true;
				for (int i = 0; i < trainingTips.Count; i++)
				{
					trainingTips[i].SetActive(i == (int)_trainingState);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in trainingState setter: " + ex);
			}
		}
	}

	public bool WeaponCategory
	{
		get
		{
			return IsWeaponCategory(currentCategory);
		}
	}

	public static bool ShowArmor
	{
		get
		{
			return _ShowArmor;
		}
		private set
		{
			if (_ShowArmor != value)
			{
				_ShowArmor = value;
				Action showArmorChanged = ShopNGUIController.ShowArmorChanged;
				if (showArmorChanged != null)
				{
					showArmorChanged();
				}
			}
		}
	}

	public static bool ShowHat
	{
		get
		{
			return _ShowHat;
		}
		private set
		{
			if (_ShowHat != value)
			{
				_ShowHat = value;
				Action showArmorChanged = ShopNGUIController.ShowArmorChanged;
				if (showArmorChanged != null)
				{
					showArmorChanged();
				}
			}
		}
	}

	public bool WearCategory
	{
		get
		{
			return IsWearCategory(currentCategory);
		}
	}

	private string NextUpgradeIDForCurrentGear
	{
		get
		{
			if (viewedId == null)
			{
				return null;
			}
			return GearManager.UpgradeIDForGear(GearManager.HolderQuantityForID(viewedId), GearManager.CurrentNumberOfUphradesForGear(viewedId) + 1);
		}
	}

	private string IDForCurrentGear
	{
		get
		{
			if (viewedId == null)
			{
				return null;
			}
			return GearManager.HolderQuantityForID(viewedId);
		}
	}

	private string _CurrentEquippedSN
	{
		get
		{
			return SnForWearCategory(currentCategory);
		}
	}

	private string _CurrentNoneEquipped
	{
		get
		{
			return NoneEquippedForWearCategory(currentCategory);
		}
	}

	public string _CurrentEquippedWear
	{
		get
		{
			return WearForCat(currentCategory);
		}
	}

	public bool IsFromPromoActions
	{
		get
		{
			return _isFromPromoActions;
		}
	}

	public static bool GuiActive
	{
		get
		{
			return sharedShop != null && sharedShop.ActiveObject.activeInHierarchy;
		}
		set
		{
			if (value)
			{
				if (sharedShop._backSubscription != null)
				{
					sharedShop._backSubscription.Dispose();
				}
				sharedShop._backSubscription = BackSystem.Instance.Register(sharedShop.HandleEscape, "Shop");
			}
			else if (sharedShop._backSubscription != null)
			{
				sharedShop._backSubscription.Dispose();
				sharedShop._backSubscription = null;
				Storager.RefreshWeaponDigestIfDirty();
			}
			if (value)
			{
				Transform transform = sharedShop.category.buttons[11].transform;
				transform.localPosition = new Vector3(-10000f, transform.localPosition.y, transform.localPosition.z);
			}
			if (ZombieCreator.sharedCreator != null)
			{
				ZombieCreator.sharedCreator.SuppressDrawingWaveMessage();
			}
			if (Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				QualitySettings.antiAliasing = 0;
			}
			else
			{
				QualitySettings.antiAliasing = ((value && !Device.isWeakDevice) ? 4 : 0);
			}
			FreeAwardShowHandler.CheckShowChest(value);
			if (!(sharedShop != null))
			{
				return;
			}
			sharedShop.SetOtherCamerasEnabled(!value);
			if (value)
			{
				sharedShop.armorLock.SetActive(Defs.isHunger);
				sharedShop.stub.SetActive(true);
				try
				{
					sharedShop._storedAmbientLight = RenderSettings.ambientLight;
					sharedShop._storedFogEnabled = RenderSettings.fog;
					RenderSettings.ambientLight = Defs.AmbientLightColorForShop();
					RenderSettings.fog = false;
					sharedShop.timeOfEnteringShopForProtectFromPressingCoinsButton = Time.realtimeSinceStartup;
					sharedShop.LoadCurrentWearToVars();
					sharedShop.UpdateIcons();
					CategoryNames cn = CategoryNames.PrimaryCategory;
					string text;
					if (sharedShop.offerID != null)
					{
						text = sharedShop.offerID;
						cn = sharedShop.offerCategory;
						sharedShop.offerID = null;
					}
					else
					{
						CategoryNames categoryNames = CategoryNames.PrimaryCategory;
						if (WeaponManager.sharedManager != null && WeaponManager.sharedManager._currentFilterMap == 1)
						{
							cn = CategoryNames.MeleeCategory;
							categoryNames = CategoryNames.MeleeCategory;
						}
						else if (WeaponManager.sharedManager != null && WeaponManager.sharedManager._currentFilterMap == 2)
						{
							cn = CategoryNames.SniperCategory;
							categoryNames = CategoryNames.SniperCategory;
						}
						text = _CurrentWeaponSetIDs()[(int)categoryNames] ?? TempGunOrHighestDPSGun(categoryNames, out cn);
					}
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
					{
						AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Open_Shop);
						sharedShop.trainingColliders.SetActive(true);
						if (sharedShop.trainingState >= TrainingState.OnArmor)
						{
							cn = CategoryNames.ArmorCategory;
							text = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
						}
						else if (sharedShop.trainingState >= TrainingState.OnSniperRifle)
						{
							cn = CategoryNames.SniperCategory;
							text = WeaponTags.HunterRifleTag;
						}
					}
					else
					{
						sharedShop.HideAllTrainingInterface();
						if (!sharedShop.InTrainingAfterNoviceArmorRemoved)
						{
							sharedShop.HideAllTrainingInterfaceRemovedNoviceArmor();
						}
					}
					string text2 = WeaponManager.LastBoughtTag(text);
					if (text2 == null)
					{
						if (IsWearCategory(cn))
						{
							foreach (List<List<string>> value2 in Wear.wear.Values)
							{
								foreach (List<string> item in value2)
								{
									if (item.Contains(text))
									{
										text = item[0];
										break;
									}
								}
							}
						}
					}
					else
					{
						text = text2;
					}
					sharedShop.CategoryChosen(cn, text, true);
					SetIconChosen(cn);
					sharedShop.MakeACtiveAfterDelay(text, cn);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in ShopNGUIController.GuiActive: " + ex);
				}
				sharedShop.StartCoroutine(sharedShop.DisableStub());
			}
			else
			{
				Color? storedAmbientLight = sharedShop._storedAmbientLight;
				RenderSettings.ambientLight = ((!storedAmbientLight.HasValue) ? RenderSettings.ambientLight : storedAmbientLight.Value);
				bool? storedFogEnabled = sharedShop._storedFogEnabled;
				RenderSettings.fog = ((!storedFogEnabled.HasValue) ? RenderSettings.fog : storedFogEnabled.Value);
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
				{
					sharedShop.HideAllTrainingInterface();
					CategoryNames cn2;
					string tg = TempGunOrHighestDPSGun(CategoryNames.PrimaryCategory, out cn2);
					tg = WeaponManager.FirstUnboughtTag(tg);
					sharedShop.CategoryChosen(cn2, tg, true);
				}
				else if (sharedShop.InTrainingAfterNoviceArmorRemoved)
				{
					sharedShop.HideAllTrainingInterfaceRemovedNoviceArmor();
				}
				MyCenterOnChild myCenterOnChild = sharedShop.carouselCenter;
				myCenterOnChild.onFinished = (SpringPanel.OnFinished)Delegate.Remove(myCenterOnChild.onFinished, new SpringPanel.OnFinished(sharedShop.HandleCarouselCentering));
				PromoActionsManager.ActionsUUpdated -= sharedShop.HandleActionsUUpdated;
				sharedShop.SetWeapon(null);
				sharedShop.ActiveObject.SetActive(false);
				sharedShop.carouselCenter.enabled = false;
				WeaponManager.ClearCachedInnerPrefabs();
			}
		}
	}

	public static event Action GunOrArmorBought;

	public static event Action<string> TryGunBought;

	public static event Action ShowArmorChanged;

	public static event Action GunBought;

	public ShopNGUIController()
	{
		if (_003C_003Ef__am_0024cacheB4 == null)
		{
			_003C_003Ef__am_0024cacheB4 = _003Csort_003Em__4C4;
		}
		sort = _003C_003Ef__am_0024cacheB4;
		itemIndex = -1;
		_assignedWeaponTag = string.Empty;
		base._002Ector();
	}

	public static void GoToShop(CategoryNames cat, string id)
	{
		sharedShop.SetOfferID(id);
		sharedShop.offerCategory = cat;
		if (GuiActive)
		{
			sharedShop.CategoryChosen(cat, id);
			SetIconChosen(cat);
		}
		else if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
		{
			if (MainMenuController.sharedController != null)
			{
				MainMenuController.sharedController.HandleShopClicked(null, EventArgs.Empty);
			}
		}
		else
		{
			sharedShop.resumeAction = null;
			GuiActive = true;
		}
	}

	public static void AddBoughtCurrency(string currency, int count)
	{
		if (currency == null)
		{
			Debug.LogWarning("AddBoughtCurrency: currency == null");
			return;
		}
		if (Debug.isDebugBuild)
		{
			Debug.Log(string.Format("<color=#ff00ffff>AddBoughtCurrency {0} {1}</color>", currency, count));
		}
		Storager.setInt("BoughtCurrency" + currency, Storager.getInt("BoughtCurrency" + currency, false) + count, false);
	}

	public static void SpendBoughtCurrency(string currency, int count)
	{
		if (currency == null)
		{
			Debug.LogWarning("SpendBoughtCurrency: currency == null");
		}
		else if (Debug.isDebugBuild)
		{
			Debug.Log(string.Format("<color=#ff00ffff>SpendBoughtCurrency {0} {1}</color>", currency, count));
		}
	}

	public static void TryToBuy(GameObject mainPanel, ItemPrice price, Action onSuccess, Action onFailure = null, Func<bool> successAdditionalCond = null, Action onReturnFromBank = null, Action onEnterCoinsShopAction = null, Action onExitCoinsShopAction = null)
	{
		_003CTryToBuy_003Ec__AnonStorey31C _003CTryToBuy_003Ec__AnonStorey31C = new _003CTryToBuy_003Ec__AnonStorey31C();
		_003CTryToBuy_003Ec__AnonStorey31C.mainPanel = mainPanel;
		_003CTryToBuy_003Ec__AnonStorey31C.onReturnFromBank = onReturnFromBank;
		_003CTryToBuy_003Ec__AnonStorey31C.onExitCoinsShopAction = onExitCoinsShopAction;
		_003CTryToBuy_003Ec__AnonStorey31C.price = price;
		_003CTryToBuy_003Ec__AnonStorey31C.successAdditionalCond = successAdditionalCond;
		_003CTryToBuy_003Ec__AnonStorey31C.onSuccess = onSuccess;
		_003CTryToBuy_003Ec__AnonStorey31C.onFailure = onFailure;
		_003CTryToBuy_003Ec__AnonStorey31C.onEnterCoinsShopAction = onEnterCoinsShopAction;
		Debug.Log("Trying to buy from ShopNGUIController...");
		if (BankController.Instance == null)
		{
			Debug.LogWarning("BankController.Instance == null");
			return;
		}
		if (_003CTryToBuy_003Ec__AnonStorey31C.price == null)
		{
			Debug.LogWarning("price == null");
			return;
		}
		_003CTryToBuy_003Ec__AnonStorey31C.handleBackFromBank = null;
		_003CTryToBuy_003Ec__AnonStorey31C.handleBackFromBank = _003CTryToBuy_003Ec__AnonStorey31C._003C_003Em__4C5;
		_003CTryToBuy_003Ec__AnonStorey31C.act = null;
		_003CTryToBuy_003Ec__AnonStorey31C.act = _003CTryToBuy_003Ec__AnonStorey31C._003C_003Em__4C6;
		_003CTryToBuy_003Ec__AnonStorey31C.act(BankController.Instance, EventArgs.Empty);
	}

	private void FilterUpgrades(List<GameObject> modelsList, GameObject prefab, CategoryNames category, string visualDefName)
	{
		_003CFilterUpgrades_003Ec__AnonStorey31D _003CFilterUpgrades_003Ec__AnonStorey31D = new _003CFilterUpgrades_003Ec__AnonStorey31D();
		_003CFilterUpgrades_003Ec__AnonStorey31D.prefab = prefab;
		if (_003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name.Replace("(Clone)", string.Empty) == "Armor_Novice")
		{
			return;
		}
		if (_003CFilterUpgrades_003Ec__AnonStorey31D.prefab != null && TempItemsController.PriceCoefs.ContainsKey(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name) && TempItemsController.sharedController != null)
		{
			if (TempItemsController.sharedController.ContainsItem(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name))
			{
				modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
			}
			return;
		}
		List<List<string>> list = null;
		try
		{
			list = Wear.wear[category];
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat("Exception in FilterUpgrades ll = Wear.wear [category]: ", ex, " category = ", category.ToString()));
		}
		if (list == null)
		{
			Debug.LogError("FilterUpgrades: ll == null   category = " + category);
			return;
		}
		List<string> list2 = list.FirstOrDefault(_003CFilterUpgrades_003Ec__AnonStorey31D._003C_003Em__4C7).ToList();
		if (list2 == null)
		{
			return;
		}
		string text = ((!string.IsNullOrEmpty(visualDefName)) ? Storager.getString(visualDefName, false) : string.Empty);
		int num = list2.IndexOf(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name);
		if (Storager.getInt(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name, true) > 0)
		{
			if (num == list2.Count - 1)
			{
				modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
			}
			else if (num >= 0 && num < list2.Count - 1 && Storager.getInt(list2[num + 1], true) == 0)
			{
				modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
			}
			return;
		}
		if (num == 0 && Wear.LeagueForWear(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name, category) <= (int)RatingSystem.instance.currentLeague)
		{
			modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
		}
		if (num <= 0)
		{
			return;
		}
		if (Storager.getInt(list2[num - 1], true) > 0)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (list2.IndexOf(text) <= num)
				{
					modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
				}
			}
			else
			{
				modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
			}
		}
		else if (!string.IsNullOrEmpty(text) && _003CFilterUpgrades_003Ec__AnonStorey31D.prefab.name.Equals(text))
		{
			modelsList.Add(_003CFilterUpgrades_003Ec__AnonStorey31D.prefab);
		}
	}

	public static bool ShowLockedFacebookSkin()
	{
		if (!FacebookController.FacebookSupported)
		{
			return false;
		}
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && SkinsController.shopKeyFromNameSkin.ContainsKey("61"))
		{
			string value = SkinsController.shopKeyFromNameSkin["61"];
			if (Array.IndexOf(StoreKitEventListener.skinIDs, value) >= 0)
			{
				foreach (KeyValuePair<string, string> value2 in InAppData.inAppData.Values)
				{
					if (value2.Key != null && value2.Key.Equals(value) && Storager.getInt(value2.Value, true) == 0)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public List<GameObject> FillModelsList(CategoryNames c)
	{
		if (_003C_003Ef__am_0024cacheB5 == null)
		{
			_003C_003Ef__am_0024cacheB5 = _003CFillModelsList_003Em__4C8;
		}
		Func<CategoryNames, Comparison<GameObject>> func = _003C_003Ef__am_0024cacheB5;
		List<GameObject> list = new List<GameObject>();
		if (IsWeaponCategory(c))
		{
			list = WeaponManager.sharedManager.FilteredShopLists[(int)c];
		}
		else
		{
			switch (c)
			{
			case CategoryNames.HatsCategory:
				foreach (ShopPositionParams hat in hats)
				{
					FilterUpgrades(list, hat.gameObject, CategoryNames.HatsCategory, Defs.VisualHatArmor);
				}
				list.Sort(func(CategoryNames.HatsCategory));
				break;
			case CategoryNames.ArmorCategory:
				if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1 && !TrainingController.TrainingCompleted)
				{
					GameObject gameObject = Resources.Load<GameObject>("Armor_Info/Armor_Novice");
					if (gameObject != null)
					{
						list.Add(gameObject);
					}
					else
					{
						Debug.LogError("No novice armor when Storager.getInt(Defs.NoviceArmorUsedKey,false) == 1 && !TrainingController.TrainingCompleted");
					}
				}
				else
				{
					foreach (ShopPositionParams item in armor)
					{
						FilterUpgrades(list, item.gameObject, CategoryNames.ArmorCategory, Defs.VisualArmor);
					}
				}
				list.Sort(func(CategoryNames.ArmorCategory));
				break;
			case CategoryNames.SkinsCategory:
				foreach (string key in SkinsController.skinsForPers.Keys)
				{
					list.Add(pixlMan);
				}
				if (ShowLockedFacebookSkin())
				{
					list.Add(pixlMan);
				}
				break;
			case CategoryNames.CapesCategory:
				foreach (ShopPositionParams cape in capes)
				{
					FilterUpgrades(list, cape.gameObject, CategoryNames.CapesCategory, string.Empty);
				}
				list.Sort(func(CategoryNames.CapesCategory));
				break;
			case CategoryNames.BootsCategory:
				foreach (ShopPositionParams boot in boots)
				{
					FilterUpgrades(list, boot.gameObject, CategoryNames.BootsCategory, string.Empty);
				}
				list.Sort(func(CategoryNames.BootsCategory));
				break;
			case CategoryNames.MaskCategory:
				foreach (ShopPositionParams mask in masks)
				{
					FilterUpgrades(list, mask.gameObject, CategoryNames.MaskCategory, string.Empty);
				}
				list.Sort(func(CategoryNames.MaskCategory));
				break;
			}
		}
		return list;
	}

	private static string ItemIDForPrefab(string name, CategoryNames c)
	{
		switch (c)
		{
		case CategoryNames.ArmorCategory:
		{
			string string2 = Storager.getString(Defs.VisualArmor, false);
			if (!string.IsNullOrEmpty(string2) && Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(name) >= 0 && Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(name) < Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(string2))
			{
				return string2;
			}
			break;
		}
		case CategoryNames.HatsCategory:
		{
			string @string = Storager.getString(Defs.VisualHatArmor, false);
			if (!string.IsNullOrEmpty(@string) && Wear.wear[CategoryNames.HatsCategory][0].IndexOf(name) >= 0 && Wear.wear[CategoryNames.HatsCategory][0].IndexOf(name) < Wear.wear[CategoryNames.HatsCategory][0].IndexOf(@string))
			{
				return @string;
			}
			break;
		}
		}
		return name;
	}

	private static string ItemIDForPrefabReverse(string name, CategoryNames c)
	{
		switch (c)
		{
		case CategoryNames.ArmorCategory:
		{
			string string2 = Storager.getString(Defs.VisualArmor, false);
			if (string.IsNullOrEmpty(string2) || !string2.Equals(name) || Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(name) < 0)
			{
				break;
			}
			for (int j = 1; j < Wear.wear[CategoryNames.ArmorCategory][0].Count; j++)
			{
				if (Storager.getInt(Wear.wear[CategoryNames.ArmorCategory][0][j], true) == 0)
				{
					return Wear.wear[CategoryNames.ArmorCategory][0][j];
				}
			}
			break;
		}
		case CategoryNames.HatsCategory:
		{
			string @string = Storager.getString(Defs.VisualHatArmor, false);
			if (string.IsNullOrEmpty(@string) || !@string.Equals(name) || Wear.wear[CategoryNames.HatsCategory][0].IndexOf(name) < 0)
			{
				break;
			}
			for (int i = 1; i < Wear.wear[CategoryNames.HatsCategory][0].Count; i++)
			{
				if (Storager.getInt(Wear.wear[CategoryNames.HatsCategory][0][i], true) == 0)
				{
					return Wear.wear[CategoryNames.HatsCategory][0][i];
				}
			}
			break;
		}
		}
		return name;
	}

	private static string GetWeaponStatText(int currentValue, int nextValue)
	{
		if (nextValue - currentValue == 0)
		{
			return currentValue.ToString();
		}
		if (nextValue - currentValue > 0)
		{
			return string.Format("{0}[00ff00]+{1}[-]", currentValue, nextValue - currentValue);
		}
		return string.Format("{0}[FACC2E]{1}[-]", currentValue, nextValue - currentValue);
	}

	public void SetCamera()
	{
		Transform mainMenu_Pers = MainMenu_Pers;
		HOTween.Kill(mainMenu_Pers);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		Vector3 vector3 = new Vector3(1f, 1f, 1f);
		switch (currentCategory)
		{
		case CategoryNames.CapesCategory:
			vector = new Vector3(0f, 0f, 0f);
			vector2 = new Vector3(0f, -130.76f, 0f);
			break;
		case CategoryNames.HatsCategory:
			vector = new Vector3(1.06f, -0.54f, 2.19f);
			vector2 = new Vector3(0f, -9.5f, 0f);
			break;
		case CategoryNames.MaskCategory:
			vector = new Vector3(1.06f, -0.54f, 2.19f);
			vector2 = new Vector3(0f, -9.5f, 0f);
			break;
		default:
			vector = new Vector3(0f, 0f, 0f);
			vector2 = new Vector3(0f, 0f, 0f);
			break;
		}
		float p_duration = 0.5f;
		idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(mainMenu_Pers, p_duration, new TweenParms().Prop("localPosition", vector).Prop("localRotation", new PlugQuaternion(vector2)).UpdateType(UpdateType.TimeScaleIndependentUpdate)
			.Ease(EaseType.Linear)
			.OnComplete(_003CSetCamera_003Em__4C9));
	}

	private void LogPurchaseAfterPaymentAnalyticsEvent(string itemName)
	{
		if (!FlurryEvents.PaymentTime.HasValue)
		{
			return;
		}
		float? paymentTime = FlurryEvents.PaymentTime;
		float? num = ((!paymentTime.HasValue) ? null : new float?(Time.realtimeSinceStartup - paymentTime.Value));
		Dictionary<string, string> dictionary;
		if (num.HasValue && num.Value < 30f)
		{
			dictionary = new Dictionary<string, string>();
			dictionary.Add("0-30", itemName);
			Dictionary<string, string> parameters = dictionary;
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", parameters);
		}
		else
		{
			float? paymentTime2 = FlurryEvents.PaymentTime;
			float? num2 = ((!paymentTime2.HasValue) ? null : new float?(Time.realtimeSinceStartup - paymentTime2.Value));
			if (num2.HasValue && num2.Value < 60f)
			{
				dictionary = new Dictionary<string, string>();
				dictionary.Add("30-60", itemName);
				Dictionary<string, string> parameters2 = dictionary;
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", parameters2);
			}
			else
			{
				float? paymentTime3 = FlurryEvents.PaymentTime;
				float? num3 = ((!paymentTime3.HasValue) ? null : new float?(Time.realtimeSinceStartup - paymentTime3.Value));
				if (num3.HasValue && num3.Value < 90f)
				{
					dictionary = new Dictionary<string, string>();
					dictionary.Add("60-90", itemName);
					Dictionary<string, string> parameters3 = dictionary;
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", parameters3);
				}
				else
				{
					dictionary = new Dictionary<string, string>();
					dictionary.Add("90+", itemName);
					Dictionary<string, string> parameters4 = dictionary;
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", parameters4);
				}
			}
		}
		float? paymentTime4 = FlurryEvents.PaymentTime;
		float? num4 = ((!paymentTime4.HasValue) ? null : new float?(Time.realtimeSinceStartup - paymentTime4.Value));
		if (num4.HasValue && num4.Value < 30f)
		{
			dictionary = new Dictionary<string, string>();
			dictionary.Add("0-30", itemName);
			Dictionary<string, string> parameters5 = dictionary;
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", parameters5);
		}
		float? paymentTime5 = FlurryEvents.PaymentTime;
		float? num5 = ((!paymentTime5.HasValue) ? null : new float?(Time.realtimeSinceStartup - paymentTime5.Value));
		if (num5.HasValue && num5.Value < 60f)
		{
			dictionary = new Dictionary<string, string>();
			dictionary.Add("0-60", itemName);
			Dictionary<string, string> parameters6 = dictionary;
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", parameters6);
		}
		float? paymentTime6 = FlurryEvents.PaymentTime;
		float? num6 = ((!paymentTime6.HasValue) ? null : new float?(Time.realtimeSinceStartup - paymentTime6.Value));
		if (num6.HasValue && num6.Value < 90f)
		{
			dictionary = new Dictionary<string, string>();
			dictionary.Add("0-90", itemName);
			Dictionary<string, string> parameters7 = dictionary;
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", parameters7);
		}
		dictionary = new Dictionary<string, string>();
		dictionary.Add("All", itemName);
		Dictionary<string, string> parameters8 = dictionary;
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", parameters8);
	}

	public void PlayWeaponAnimation()
	{
		if (profile != null && weapon != null)
		{
			Animation component = weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>();
			if (Time.timeScale != 0f)
			{
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(profile, "Profile");
				}
				component.Play("Profile");
			}
			else
			{
				animationCoroutineRunner.StopAllCoroutines();
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(profile, "Profile");
				}
				if (animationCoroutineRunner.gameObject.activeInHierarchy)
				{
					animationCoroutineRunner.StartPlay(component, "Profile", false, null);
				}
				else
				{
					Debug.LogWarning("Coroutine couldn't be started because the the game object 'AnimationCoroutineRunner' is inactive!");
				}
			}
		}
		MainMenu_Pers.GetComponent<Animation>().Stop();
		MainMenu_Pers.GetComponent<Animation>().Play("Idle");
	}

	public static Texture TextureForCat(int cc)
	{
		string text = (IsWeaponCategory((CategoryNames)cc) ? _CurrentWeaponSetIDs()[cc] : ((!IsWearCategory((CategoryNames)cc)) ? "potion" : sharedShop.WearForCat((CategoryNames)cc)));
		if (text == null)
		{
			return null;
		}
		text = ItemIDForPrefab(text, (CategoryNames)cc);
		int num = 1;
		if (IsWeaponCategory((CategoryNames)cc))
		{
			ItemRecord byTag = ItemDb.GetByTag(text);
			if ((byTag == null || !byTag.UseImagesFromFirstUpgrade) && (byTag == null || !byTag.TemporaryGun))
			{
				bool maxUpgrade;
				num = _CurrentNumberOfUpgrades(text, out maxUpgrade, (CategoryNames)cc);
			}
		}
		string text2 = ((!IsWeaponCategory((CategoryNames)cc)) ? text : (_TagForId(text) ?? string.Empty)) + "_icon" + num;
		string text3 = text2 + "_big";
		Texture texture = Resources.Load<Texture>("OfferIcons/" + text3);
		if (texture == null)
		{
			texture = Resources.Load<Texture>("ShopIcons/" + text2);
		}
		return texture;
	}

	public void SetIconModelsPositions(Transform t, CategoryNames c)
	{
		switch (c)
		{
		case CategoryNames.ArmorCategory:
		{
			t.transform.localPosition = new Vector3(0f, 0f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			float num5 = 1f;
			t.transform.localScale = new Vector3(num5, num5, num5);
			break;
		}
		case CategoryNames.BootsCategory:
		{
			t.transform.localPosition = new Vector3(-0.4f, -0.1f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(13f, 149f, 0f));
			float num4 = 75f;
			t.transform.localScale = new Vector3(num4, num4, num4);
			break;
		}
		case CategoryNames.CapesCategory:
		{
			t.transform.localPosition = new Vector3(-0.720093f, -0.00859833f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 30f, -15f));
			float num3 = 50f;
			t.transform.localScale = new Vector3(num3, num3, num3);
			break;
		}
		case CategoryNames.SkinsCategory:
			SkinsController.SetTransformParamtersForSkinModel(t);
			break;
		case CategoryNames.GearCategory:
		{
			t.transform.localPosition = new Vector3(4.648193f, 2.444565f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 30f, -30f));
			float num2 = 319.3023f;
			t.transform.localScale = new Vector3(num2, num2, num2);
			break;
		}
		case CategoryNames.HatsCategory:
		{
			t.transform.localPosition = new Vector3(-0.62f, -0.04f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(-75f, -165f, -90f));
			float num = 82.5f;
			t.transform.localScale = new Vector3(num, num, num);
			break;
		}
		}
	}

	private void DisableGunflashes(GameObject root)
	{
		if (root == null)
		{
			return;
		}
		if (root.name.Equals("GunFlash"))
		{
			root.SetActive(false);
		}
		foreach (Transform item in root.transform)
		{
			if (!(null == item))
			{
				DisableGunflashes(item.gameObject);
			}
		}
	}

	public void UpdateIcons()
	{
		for (int i = 0; i < category.buttons.Length; i++)
		{
			UpdateIcon((CategoryNames)i);
		}
	}

	public void UpdateIcon(CategoryNames c, bool animateModel = false)
	{
		_003CUpdateIcon_003Ec__AnonStorey31F _003CUpdateIcon_003Ec__AnonStorey31F = new _003CUpdateIcon_003Ec__AnonStorey31F();
		_003CUpdateIcon_003Ec__AnonStorey31F.c = c;
		_003CUpdateIcon_003Ec__AnonStorey31F.animateModel = animateModel;
		_003CUpdateIcon_003Ec__AnonStorey31F._003C_003Ef__this = this;
		_003CUpdateIcon_003Ec__AnonStorey31F.tb = category.buttons[(int)_003CUpdateIcon_003Ec__AnonStorey31F.c];
		_003CUpdateIcon_003Ec__AnonStorey31F.toDestroy = new List<GameObject>();
		Player_move_c.PerformActionRecurs(_003CUpdateIcon_003Ec__AnonStorey31F.tb.gameObject, _003CUpdateIcon_003Ec__AnonStorey31F._003C_003Em__4CA);
		foreach (GameObject item in _003CUpdateIcon_003Ec__AnonStorey31F.toDestroy)
		{
			UnityEngine.Object.Destroy(item);
		}
		if (_003CUpdateIcon_003Ec__AnonStorey31F.c == CategoryNames.SkinsCategory || (_003CUpdateIcon_003Ec__AnonStorey31F.c == CategoryNames.CapesCategory && _currentCape.Equals("cape_Custom")))
		{
			List<GameObject> list = FillModelsList(_003CUpdateIcon_003Ec__AnonStorey31F.c);
			GameObject gameObject = ((_003CUpdateIcon_003Ec__AnonStorey31F.c != CategoryNames.SkinsCategory) ? ItemDb.GetWearFromResources("cape_Custom", CategoryNames.CapesCategory) : list[0]);
			if (gameObject != null)
			{
				AddModel(gameObject, _003CUpdateIcon_003Ec__AnonStorey31F._003C_003Em__4CB, _003CUpdateIcon_003Ec__AnonStorey31F.c);
			}
			else
			{
				GameObject obj = _003CUpdateIcon_003Ec__AnonStorey31F.tb.gameObject;
				if (_003C_003Ef__am_0024cacheB6 == null)
				{
					_003C_003Ef__am_0024cacheB6 = _003CUpdateIcon_003Em__4CC;
				}
				Player_move_c.PerformActionRecurs(obj, _003C_003Ef__am_0024cacheB6);
			}
			GameObject obj2 = _003CUpdateIcon_003Ec__AnonStorey31F.tb.gameObject;
			if (_003C_003Ef__am_0024cacheB7 == null)
			{
				_003C_003Ef__am_0024cacheB7 = _003CUpdateIcon_003Em__4CD;
			}
			Player_move_c.PerformActionRecurs(obj2, _003C_003Ef__am_0024cacheB7);
			return;
		}
		_003CUpdateIcon_003Ec__AnonStorey320 _003CUpdateIcon_003Ec__AnonStorey = new _003CUpdateIcon_003Ec__AnonStorey320();
		_003CUpdateIcon_003Ec__AnonStorey.texture = TextureForCat((int)_003CUpdateIcon_003Ec__AnonStorey31F.c);
		if (_003CUpdateIcon_003Ec__AnonStorey.texture != null)
		{
			Player_move_c.PerformActionRecurs(_003CUpdateIcon_003Ec__AnonStorey31F.tb.gameObject, _003CUpdateIcon_003Ec__AnonStorey._003C_003Em__4CE);
			return;
		}
		GameObject obj3 = _003CUpdateIcon_003Ec__AnonStorey31F.tb.gameObject;
		if (_003C_003Ef__am_0024cacheB8 == null)
		{
			_003C_003Ef__am_0024cacheB8 = _003CUpdateIcon_003Em__4CF;
		}
		Player_move_c.PerformActionRecurs(obj3, _003C_003Ef__am_0024cacheB8);
	}

	private static string _TagForId(string id)
	{
		foreach (List<string> upgrade in WeaponUpgrades.upgrades)
		{
			if (upgrade.Contains(id))
			{
				return upgrade[0];
			}
		}
		return id;
	}

	public void EquipWear(string tg)
	{
		EquipWearInCategory(tg, currentCategory, inGame);
	}

	public static void EquipWearInCategoryIfNotEquiped(string tg, CategoryNames cat, bool inGameLocal)
	{
		if (!Storager.hasKey(SnForWearCategory(cat)))
		{
			Storager.setString(SnForWearCategory(cat), NoneEquippedForWearCategory(cat), false);
		}
		if (!Storager.getString(SnForWearCategory(cat), false).Equals(tg))
		{
			EquipWearInCategory(tg, cat, inGameLocal);
		}
	}

	private static void EquipWearInCategory(string tg, CategoryNames cat, bool inGameLocal)
	{
		bool flag = !Defs.isMulti;
		string @string = Storager.getString(SnForWearCategory(cat), false);
		Player_move_c player_move_c = null;
		if (inGameLocal)
		{
			if (flag)
			{
				if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && !SceneLoader.ActiveSceneName.Equals("ChooseLevel"))
				{
					player_move_c = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		SetAsEquippedAndSendToServer(tg, cat);
		sharedShop.SetWearForCategory(cat, tg);
		if (sharedShop.wearEquipAction != null)
		{
			sharedShop.wearEquipAction(cat, @string ?? NoneEquippedForWearCategory(cat), sharedShop.WearForCat(cat));
		}
		if (cat == CategoryNames.BootsCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.bootsMethods.ContainsKey(@string))
			{
				Wear.bootsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.bootsMethods.ContainsKey(tg))
			{
				Wear.bootsMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (cat == CategoryNames.CapesCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.capesMethods.ContainsKey(@string))
			{
				Wear.capesMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.capesMethods.ContainsKey(tg))
			{
				Wear.capesMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (cat == CategoryNames.HatsCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.hatsMethods.ContainsKey(@string))
			{
				Wear.hatsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.hatsMethods.ContainsKey(tg))
			{
				Wear.hatsMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (cat == CategoryNames.ArmorCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.armorMethods.ContainsKey(@string))
			{
				Wear.armorMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.armorMethods.ContainsKey(tg))
			{
				Wear.armorMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (GuiActive)
		{
			sharedShop.UpdateButtons();
			sharedShop.UpdateIcon(cat, true);
		}
	}

	public static void UnequipCurrentWearInCategory(CategoryNames cat, bool inGameLocal)
	{
		bool flag = !Defs.isMulti;
		string @string = Storager.getString(SnForWearCategory(cat), false);
		Player_move_c player_move_c = null;
		if (inGameLocal)
		{
			if (flag)
			{
				if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && !SceneLoader.ActiveSceneName.Equals("ChooseLevel"))
				{
					player_move_c = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		Storager.setString(SnForWearCategory(cat), NoneEquippedForWearCategory(cat), false);
		FriendsController.sharedController.SendAccessories();
		sharedShop.SetWearForCategory(cat, NoneEquippedForWearCategory(cat));
		if (sharedShop.wearEquipAction != null)
		{
			sharedShop.wearEquipAction(cat, @string ?? NoneEquippedForWearCategory(cat), NoneEquippedForWearCategory(cat));
		}
		if (cat == CategoryNames.BootsCategory && inGameLocal && player_move_c != null && !@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.bootsMethods.ContainsKey(@string))
		{
			Wear.bootsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (cat == CategoryNames.CapesCategory && inGameLocal && player_move_c != null && !@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.capesMethods.ContainsKey(@string))
		{
			Wear.capesMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (cat == CategoryNames.HatsCategory && inGameLocal && player_move_c != null && !@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.hatsMethods.ContainsKey(@string))
		{
			Wear.hatsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (cat == CategoryNames.ArmorCategory && inGameLocal && player_move_c != null && !@string.Equals(NoneEquippedForWearCategory(cat)) && Wear.armorMethods.ContainsKey(@string))
		{
			Wear.armorMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (sharedShop.wearUnequipAction != null)
		{
			sharedShop.wearUnequipAction(cat, @string ?? NoneEquippedForWearCategory(cat));
		}
		if (GuiActive)
		{
			sharedShop.UpdateIcon(cat);
		}
	}

	public static void ShowTryGunIfPossible(bool placeForGiveNewTryGun, Transform point, string layer, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
		if (!Defs.isHunger && !placeForGiveNewTryGun && WeaponManager.sharedManager != null && WeaponManager.sharedManager.ExpiredTryGuns.Count > 0 && TrainingController.TrainingCompleted)
		{
			_003CShowTryGunIfPossible_003Ec__AnonStorey321 _003CShowTryGunIfPossible_003Ec__AnonStorey = new _003CShowTryGunIfPossible_003Ec__AnonStorey321();
			{
				foreach (string expiredTryGun in WeaponManager.sharedManager.ExpiredTryGuns)
				{
					_003CShowTryGunIfPossible_003Ec__AnonStorey.tg = expiredTryGun;
					try
					{
						if (WeaponManager.sharedManager.weaponsInGame.FirstOrDefault(_003CShowTryGunIfPossible_003Ec__AnonStorey._003C_003Em__4D0) != null)
						{
							WeaponManager.sharedManager.ExpiredTryGuns.RemoveAll(_003CShowTryGunIfPossible_003Ec__AnonStorey._003C_003Em__4D1);
							if (WeaponManager.LastBoughtTag(_003CShowTryGunIfPossible_003Ec__AnonStorey.tg) == null)
							{
								ShowAddTryGun(_003CShowTryGunIfPossible_003Ec__AnonStorey.tg, point, layer, onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction, true);
								break;
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception in foreach (var tg in WeaponManager.sharedManager.ExpiredTryGuns): " + ex);
					}
				}
				return;
			}
		}
		if (Defs.isHunger || Defs.isDaterRegim || WeaponManager.sharedManager._currentFilterMap != 0 || !((!FriendsController.useBuffSystem) ? KillRateCheck.instance.giveWeapon : BuffSystem.instance.giveTryGun) || !TrainingController.TrainingCompleted)
		{
			return;
		}
		try
		{
			_003CShowTryGunIfPossible_003Ec__AnonStorey322 _003CShowTryGunIfPossible_003Ec__AnonStorey2 = new _003CShowTryGunIfPossible_003Ec__AnonStorey322();
			_003CShowTryGunIfPossible_003Ec__AnonStorey2.maximumCoinBank = UpperCoinsBankBound();
			Dictionary<CategoryNames, List<List<string>>> tryGunsTable = WeaponManager.tryGunsTable;
			if (_003C_003Ef__am_0024cacheB9 == null)
			{
				_003C_003Ef__am_0024cacheB9 = _003CShowTryGunIfPossible_003Em__4D2;
			}
			IEnumerable<string> source = tryGunsTable.SelectMany(_003C_003Ef__am_0024cacheB9);
			if (_003C_003Ef__am_0024cacheBA == null)
			{
				_003C_003Ef__am_0024cacheBA = _003CShowTryGunIfPossible_003Em__4D3;
			}
			IEnumerable<ItemRecord> source2 = source.Select(_003C_003Ef__am_0024cacheBA);
			if (_003C_003Ef__am_0024cacheBB == null)
			{
				_003C_003Ef__am_0024cacheBB = _003CShowTryGunIfPossible_003Em__4D4;
			}
			IEnumerable<ItemRecord> source3 = source2.Where(_003C_003Ef__am_0024cacheBB);
			if (_003C_003Ef__am_0024cacheBC == null)
			{
				_003C_003Ef__am_0024cacheBC = _003CShowTryGunIfPossible_003Em__4D5;
			}
			List<ItemRecord> source4 = source3.Where(_003C_003Ef__am_0024cacheBC).ToList();
			if (_003C_003Ef__am_0024cacheBD == null)
			{
				_003C_003Ef__am_0024cacheBD = _003CShowTryGunIfPossible_003Em__4D6;
			}
			List<ItemRecord> source5 = source4.Where(_003C_003Ef__am_0024cacheBD).Where(_003CShowTryGunIfPossible_003Ec__AnonStorey2._003C_003Em__4D7).Randomize()
				.ToList();
			string text = null;
			if (source5.Any())
			{
				text = source5.First().Tag;
			}
			else
			{
				_003CShowTryGunIfPossible_003Ec__AnonStorey323 _003CShowTryGunIfPossible_003Ec__AnonStorey3 = new _003CShowTryGunIfPossible_003Ec__AnonStorey323();
				_003CShowTryGunIfPossible_003Ec__AnonStorey3.maximumGemBank = UpperGemsBankBound();
				if (_003C_003Ef__am_0024cacheBE == null)
				{
					_003C_003Ef__am_0024cacheBE = _003CShowTryGunIfPossible_003Em__4D8;
				}
				List<ItemRecord> source6 = source4.Where(_003C_003Ef__am_0024cacheBE).Where(_003CShowTryGunIfPossible_003Ec__AnonStorey3._003C_003Em__4D9).Randomize()
					.ToList();
				text = ((!source6.Any()) ? TryGunForCategoryWithMaxUnbought() : source6.First().Tag);
			}
			if (text != null)
			{
				ShowAddTryGun(text, point, layer, onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction);
			}
		}
		catch (Exception ex2)
		{
			Debug.LogError("Exception in giving: " + ex2);
		}
	}

	private static int UpperCoinsBankBound()
	{
		int value = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
		value = Mathf.Clamp(value, 0, ExperienceController.addCoinsFromLevels.Length - 1);
		return Storager.getInt("Coins", false) + 30 + ExperienceController.addCoinsFromLevels[value];
	}

	private static int UpperGemsBankBound()
	{
		int value = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
		value = Mathf.Clamp(value, 0, ExperienceController.addGemsFromLevels.Length - 1);
		return Storager.getInt("GemsCurrency", false) + ExperienceController.addGemsFromLevels[value];
	}

	private static string TryGunForCategoryWithMaxUnbought()
	{
		List<CategoryNames> list = new List<CategoryNames>();
		list.Add(CategoryNames.PrimaryCategory);
		list.Add(CategoryNames.BackupCategory);
		list.Add(CategoryNames.MeleeCategory);
		list.Add(CategoryNames.SpecilCategory);
		list.Add(CategoryNames.SniperCategory);
		list.Add(CategoryNames.PremiumCategory);
		IEnumerable<CategoryNames> source = list.Randomize();
		if (_003C_003Ef__am_0024cacheBF == null)
		{
			_003C_003Ef__am_0024cacheBF = _003CTryGunForCategoryWithMaxUnbought_003Em__4DA;
		}
		List<CategoryNames> list2 = source.OrderBy(_003C_003Ef__am_0024cacheBF).ToList();
		string result = null;
		for (int i = 0; i < list2.Count; i++)
		{
			_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey325 _003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey = new _003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey325();
			_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey.cat = list2[i];
			UnityEngine.Object[] weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
			if (_003C_003Ef__am_0024cacheC0 == null)
			{
				_003C_003Ef__am_0024cacheC0 = _003CTryGunForCategoryWithMaxUnbought_003Em__4DB;
			}
			IEnumerable<WeaponSounds> source2 = weaponsInGame.Select(_003C_003Ef__am_0024cacheC0).Where(_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey._003C_003Em__4DC);
			if (_003C_003Ef__am_0024cacheC1 == null)
			{
				_003C_003Ef__am_0024cacheC1 = _003CTryGunForCategoryWithMaxUnbought_003Em__4DD;
			}
			IEnumerable<WeaponSounds> source3 = source2.Where(_003C_003Ef__am_0024cacheC1);
			if (_003C_003Ef__am_0024cacheC2 == null)
			{
				_003C_003Ef__am_0024cacheC2 = _003CTryGunForCategoryWithMaxUnbought_003Em__4DE;
			}
			IEnumerable<WeaponSounds> source4 = source3.Where(_003C_003Ef__am_0024cacheC2).Where(_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey._003C_003Em__4DF);
			if (_003C_003Ef__am_0024cacheC3 == null)
			{
				_003C_003Ef__am_0024cacheC3 = _003CTryGunForCategoryWithMaxUnbought_003Em__4E0;
			}
			List<WeaponSounds> source5 = source4.Where(_003C_003Ef__am_0024cacheC3).Randomize().ToList();
			if (source5.Count() != 0)
			{
				result = ItemDb.GetByPrefabName(source5.First().name).Tag;
				break;
			}
		}
		return result;
	}

	public static void ShowTempItemExpiredIfPossible(Transform point, string layer, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
		List<string> list = new List<string>();
		foreach (string expiredItem in TempItemsController.sharedController.ExpiredItems)
		{
			if (TempItemsController.sharedController.CanShowExpiredBannerForTag(expiredItem))
			{
				ShowRentScreen(expiredItem, point, layer, LocalizationStore.Get("Key_1156"), LocalizationStore.Get("Key_1157"), onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction);
				list.Add(expiredItem);
				break;
			}
		}
		foreach (string item in list)
		{
			TempItemsController.sharedController.ExpiredItems.Remove(item);
		}
	}

	public static bool ShowPremimAccountExpiredIfPossible(Transform point, string layer, string header = "", bool showOnlyIfExpired = true)
	{
		if (showOnlyIfExpired && (!PremiumAccountController.AccountHasExpired || !Defs2.CanShowPremiumAccountExpiredWindow))
		{
			return false;
		}
		if (point != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("PremiumAccount"));
			gameObject.transform.parent = point;
			Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer(layer ?? "Default"));
			gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			PremiumAccountScreenController component = gameObject.GetComponent<PremiumAccountScreenController>();
			component.Header = header;
			PremiumAccountController.AccountHasExpired = false;
			return true;
		}
		return false;
	}

	public static void GiveArmorArmy1OrNoviceArmor()
	{
		ProvideShopItemOnStarterPackBoguht(CategoryNames.ArmorCategory, (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1) ? "Armor_Army_1" : "Armor_Novice", 1, false, 0, null, null, true, Storager.getInt("Training.NoviceArmorUsedKey", false) == 1, false);
	}

	private void setInArmorCategoryNotOnArmorRemovedNoviceArmor()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = trainingStateRemoveNoviceArmor;
			if (trainingState == TrainingState.OnArmor)
			{
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
			}
			trainingStateRemoveNoviceArmor = TrainingState.InArmorCategoryNotOnArmor;
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setInArmorCategoryNotOnArmorRemovedNoviceArmor: " + ex);
		}
	}

	private void setNotInArmorCategoryRemovedNoviceArmor()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = trainingStateRemoveNoviceArmor;
			if (trainingState == TrainingState.OnArmor)
			{
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
			}
			trainingStateRemoveNoviceArmor = TrainingState.NotInArmorCategory;
			toBlink = category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "green_btn", "trans_btn" });
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setNotInArmorCategoryRemovedNoviceArmor: " + ex);
		}
	}

	private void setOnArmorRemovedNoviceArmor()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = trainingStateRemoveNoviceArmor;
			if (trainingState == TrainingState.NotInArmorCategory)
			{
				category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
				category.buttons[7].offButton.normalSprite = "trans_btn";
			}
			trainingStateRemoveNoviceArmor = TrainingState.OnArmor;
			toBlink = equips[1].tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "or_btn", "green_btn" });
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setOnArmorRemovedNoviceArmor: " + ex);
		}
	}

	private void setNotInSniperCategory()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = this.trainingState;
			if (trainingState == TrainingState.OnSniperRifle)
			{
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
			}
			this.trainingState = TrainingState.NotInSniperCategory;
			toBlink = category.buttons[4].offButton.tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "green_btn", "trans_btn" });
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setNotInSniperCategory: " + ex);
		}
	}

	private void setOnSniperRifle()
	{
		try
		{
			StopCoroutine("Blink");
			if (trainingState == TrainingState.NotInSniperCategory)
			{
				category.buttons[4].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
				category.buttons[4].offButton.normalSprite = "trans_btn";
			}
			trainingState = TrainingState.OnSniperRifle;
			toBlink = equips[1].tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "or_btn", "green_btn" });
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setOnSniperRifle: " + ex);
		}
	}

	private void setInSniperCategoryNotOnSniperRifle()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = this.trainingState;
			if (trainingState == TrainingState.OnSniperRifle)
			{
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
			}
			this.trainingState = TrainingState.InSniperCategoryNotOnSniperRifle;
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setInSniperCategoryNotOnSniperRifle: " + ex);
		}
	}

	private void setNotInArmorCategory()
	{
		try
		{
			StopCoroutine("Blink");
			switch (trainingState)
			{
			case TrainingState.OnSniperRifle:
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
				break;
			case TrainingState.OnArmor:
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
				break;
			}
			trainingState = TrainingState.NotInArmorCategory;
			toBlink = category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "green_btn", "trans_btn" });
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setNotInArmorCategory: " + ex);
		}
	}

	private void setOnArmor()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = this.trainingState;
			if (trainingState == TrainingState.NotInArmorCategory)
			{
				category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
				category.buttons[7].offButton.normalSprite = "trans_btn";
			}
			this.trainingState = TrainingState.OnArmor;
			toBlink = equips[1].tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "or_btn", "green_btn" });
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setOnArmor: " + ex);
		}
	}

	private void setInArmorCategoryNotOnArmor()
	{
		try
		{
			StopCoroutine("Blink");
			TrainingState trainingState = this.trainingState;
			if (trainingState == TrainingState.OnArmor)
			{
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
			}
			this.trainingState = TrainingState.InArmorCategoryNotOnArmor;
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setInArmorCategoryNotOnArmor: " + ex);
		}
	}

	private void setBackBlinking()
	{
		try
		{
			StopCoroutine("Blink");
			switch (trainingState)
			{
			case TrainingState.OnArmor:
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
				break;
			case TrainingState.OnSniperRifle:
				equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				equips[1].normalSprite = "or_btn";
				break;
			}
			trainingState = TrainingState.BackBlinking;
			toBlink = backButton.tweenTarget.GetComponent<UISprite>();
			StartCoroutine("Blink", new string[2] { "yell_btn", "green_btn" });
			trainingColliders.SetActive(false);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShopNGUIController.setBackBlinking: " + ex);
		}
	}

	private void ForceResetTrainingState()
	{
		try
		{
			if (_setTrainingStateMethods == null)
			{
				_setTrainingStateMethods = new Dictionary<TrainingState, Action>
				{
					{
						TrainingState.NotInSniperCategory,
						setNotInSniperCategory
					},
					{
						TrainingState.OnSniperRifle,
						setOnSniperRifle
					},
					{
						TrainingState.InSniperCategoryNotOnSniperRifle,
						setInSniperCategoryNotOnSniperRifle
					},
					{
						TrainingState.NotInArmorCategory,
						setNotInArmorCategory
					},
					{
						TrainingState.OnArmor,
						setOnArmor
					},
					{
						TrainingState.InArmorCategoryNotOnArmor,
						setInArmorCategoryNotOnArmor
					},
					{
						TrainingState.BackBlinking,
						setBackBlinking
					}
				};
			}
			_setTrainingStateMethods[trainingState]();
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ForceResetTrainingState: " + ex);
		}
	}

	private void HideAllTrainingInterface()
	{
		try
		{
			foreach (GameObject trainingTip in trainingTips)
			{
				trainingTip.SetActive(false);
			}
			trainingColliders.SetActive(false);
			StopCoroutine("Blink");
			equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
			equips[1].normalSprite = "or_btn";
			category.buttons[4].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
			category.buttons[4].offButton.normalSprite = "trans_btn";
			category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
			category.buttons[7].offButton.normalSprite = "trans_btn";
			backButton.tweenTarget.GetComponent<UISprite>().spriteName = "yell_btn";
			backButton.normalSprite = "yell_btn";
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in HideAllTrainingInterface: " + ex);
		}
	}

	private void HideAllTrainingInterfaceRemovedNoviceArmor()
	{
		try
		{
			foreach (GameObject item in trainingTipsRemovedNoviceArmor)
			{
				item.SetActive(false);
			}
			trainingColliders.SetActive(false);
			trainingRemoveNoviceArmorCollider.SetActive(false);
			StopCoroutine("Blink");
			equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
			equips[1].normalSprite = "or_btn";
			category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
			category.buttons[7].offButton.normalSprite = "trans_btn";
			backButton.tweenTarget.GetComponent<UISprite>().spriteName = "yell_btn";
			backButton.normalSprite = "yell_btn";
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in HideAllTrainingInterfaceRemovedNoviceArmor: " + ex);
		}
	}

	private IEnumerator Blink(string[] images)
	{
		while (true)
		{
			try
			{
				toBlink.spriteName = ((!toBlink.spriteName.Equals(images[0])) ? images[0] : images[1]);
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Debug.LogError("Exception in ShopNGUIController.Blink: " + e);
			}
			yield return StartCoroutine(MyWaitForSeconds(0.5f));
		}
	}

	public static void FireWeaponOrArmorBought()
	{
		Action gunOrArmorBought = ShopNGUIController.GunOrArmorBought;
		if (gunOrArmorBought != null)
		{
			gunOrArmorBought();
		}
	}

	public void SetOfferID(string oid)
	{
		offerID = oid;
	}

	public void HandleFacebookButton()
	{
		_isFromPromoActions = false;
		if (_003C_003Ef__am_0024cacheC4 == null)
		{
			_003C_003Ef__am_0024cacheC4 = _003CHandleFacebookButton_003Em__4E1;
		}
		Action action = _003C_003Ef__am_0024cacheC4;
		if (_003C_003Ef__am_0024cacheC5 == null)
		{
			_003C_003Ef__am_0024cacheC5 = _003CHandleFacebookButton_003Em__4E2;
		}
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(action, _003C_003Ef__am_0024cacheC5);
	}

	public void HandleProfileButton()
	{
		_003CHandleProfileButton_003Ec__AnonStorey326 _003CHandleProfileButton_003Ec__AnonStorey = new _003CHandleProfileButton_003Ec__AnonStorey326();
		_003CHandleProfileButton_003Ec__AnonStorey.mainMenu = GameObject.Find("MainMenuNGUI");
		if ((bool)_003CHandleProfileButton_003Ec__AnonStorey.mainMenu)
		{
			_003CHandleProfileButton_003Ec__AnonStorey.mainMenu.SetActive(false);
		}
		_003CHandleProfileButton_003Ec__AnonStorey.inGameGui = GameObject.FindWithTag("InGameGUI");
		if ((bool)_003CHandleProfileButton_003Ec__AnonStorey.inGameGui)
		{
			_003CHandleProfileButton_003Ec__AnonStorey.inGameGui.SetActive(false);
		}
		_003CHandleProfileButton_003Ec__AnonStorey.networkTable = GameObject.FindWithTag("NetworkStartTableNGUI");
		if ((bool)_003CHandleProfileButton_003Ec__AnonStorey.networkTable)
		{
			_003CHandleProfileButton_003Ec__AnonStorey.networkTable.SetActive(false);
		}
		GuiActive = false;
		if (_003C_003Ef__am_0024cacheC6 == null)
		{
			_003C_003Ef__am_0024cacheC6 = _003CHandleProfileButton_003Em__4E3;
		}
		Action action = _003C_003Ef__am_0024cacheC6;
		ProfileController.Instance.DesiredWeaponTag = _assignedWeaponTag;
		ProfileController.Instance.ShowInterface(action, _003CHandleProfileButton_003Ec__AnonStorey._003C_003Em__4E4);
	}

	public static bool IsWeaponCategory(CategoryNames c)
	{
		return c < CategoryNames.HatsCategory;
	}

	public static bool IsWearCategory(CategoryNames c)
	{
		return Wear.wear.Keys.Contains(c);
	}

	private static string[] _CurrentWeaponSetIDs()
	{
		string[] array = new string[6];
		WeaponManager sharedManager = WeaponManager.sharedManager;
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (num >= sharedManager.playerWeapons.Count)
			{
				array[i] = null;
				continue;
			}
			Weapon weapon = sharedManager.playerWeapons[num] as Weapon;
			if (weapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == i)
			{
				num++;
				array[i] = ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
			}
			else
			{
				array[i] = null;
			}
		}
		return array;
	}

	public static void ShowAddTryGun(string gunTag, Transform point, string lr, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitCoinsShopAdditional = null, Action<string> customEquipWearAction = null, bool expiredTryGun = false)
	{
		try
		{
			GameObject original = Resources.Load<GameObject>("TryGunScreen");
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			TryGunScreenController component = gameObject.GetComponent<TryGunScreenController>();
			Player_move_c.SetLayerRecursively(component.gameObject, LayerMask.NameToLayer(lr));
			component.transform.parent = point;
			component.transform.localPosition = new Vector3(0f, 0f, -130f);
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			if (expiredTryGun)
			{
				WeaponManager.sharedManager.AddTryGunPromo(gunTag);
			}
			component.ItemTag = gunTag;
			component.onPurchaseCustomAction = onPurchase;
			component.onEnterCoinsShopAdditionalAction = onEnterCoinsShopAdditional;
			component.onExitCoinsShopAdditionalAction = onExitCoinsShopAdditional;
			component.customEquipWearAction = customEquipWearAction;
			component.ExpiredTryGun = expiredTryGun;
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in ShowAddTryGun: " + ex);
		}
	}

	public static void ShowRentScreen(string itemTag, Transform point, string lr, string hdr, string rentText, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitCoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
	}

	public void HandleRentButton()
	{
	}

	public void HandlePropertiesInfoButton()
	{
		if (WeaponCategory)
		{
			if (Defs.isSoundFX)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			infoScreen.Show(currentCategory == CategoryNames.MeleeCategory);
		}
	}

	internal void ReloadCarousel(string idToChoose = null)
	{
		_003CReloadCarousel_003Ec__AnonStorey328 _003CReloadCarousel_003Ec__AnonStorey = new _003CReloadCarousel_003Ec__AnonStorey328();
		_003CReloadCarousel_003Ec__AnonStorey.idToChoose = idToChoose;
		_003CReloadCarousel_003Ec__AnonStorey._003C_003Ef__this = this;
		ShopCarouselElement[] componentsInChildren = wrapContent.GetComponentsInChildren<ShopCarouselElement>(true);
		ShopCarouselElement[] array = componentsInChildren;
		foreach (ShopCarouselElement shopCarouselElement in array)
		{
			UnityEngine.Object.Destroy(shopCarouselElement.gameObject);
			shopCarouselElement.transform.parent = null;
		}
		wrapContent.Reposition();
		List<GameObject> list = FillModelsList(currentCategory);
		string[] array2 = null;
		if (currentCategory == CategoryNames.SkinsCategory)
		{
			array2 = new string[list.Count];
			List<string> list2 = SkinsController.skinsForPers.Keys.ToList();
			if (!ShowLockedFacebookSkin())
			{
				list2.Remove("61");
			}
			if (_003C_003Ef__am_0024cacheC7 == null)
			{
				_003C_003Ef__am_0024cacheC7 = _003CReloadCarousel_003Em__4E5;
			}
			list2.Sort(_003C_003Ef__am_0024cacheC7);
			if (_003C_003Ef__am_0024cacheC8 == null)
			{
				_003C_003Ef__am_0024cacheC8 = _003CReloadCarousel_003Em__4E6;
			}
			int num = list2.FindIndex(_003C_003Ef__am_0024cacheC8);
			int num2 = 0;
			if (num >= 0 && num < list2.Count)
			{
				List<string> list3 = new List<string>();
				list3.AddRange(list2.GetRange(num, list2.Count - num));
				list3.Reverse();
				list3.CopyTo(array2);
				num2 = list3.Count;
				array2[num2] = "CustomSkinID";
				num2++;
				list2.CopyTo(0, array2, num2, num);
			}
			else
			{
				array2[0] = "CustomSkinID";
				num2++;
				list2.CopyTo(array2, 1);
			}
		}
		if (EnableConfigurePos)
		{
			List<string> list4 = new List<string>();
			List<GameObject> list5 = new List<GameObject>();
			for (int j = 0; j < list.Count; j++)
			{
				List<string> list6 = null;
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (upgrade.Contains((!WeaponCategory) ? list[j].tag : ItemDb.GetByPrefabName(list[j].name.Replace("(Clone)", string.Empty)).Tag))
					{
						list6 = upgrade;
						break;
					}
				}
				if (list6 == null)
				{
					list5.Add(list[j]);
					continue;
				}
				for (int k = 0; k < list6.Count; k++)
				{
					UnityEngine.Object[] weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
					for (int l = 0; l < weaponsInGame.Length; l++)
					{
						GameObject gameObject = (GameObject)weaponsInGame[l];
						if (ItemDb.GetByPrefabName(gameObject.name).Tag.Equals(list6[k]))
						{
							list5.Add(gameObject);
							break;
						}
					}
				}
			}
			list = list5;
		}
		if (_003CReloadCarousel_003Ec__AnonStorey.idToChoose == null)
		{
			_003CReloadCarousel_003Ec__AnonStorey.idToChoose = viewedId;
		}
		int num3 = 10000;
		if (_003CReloadCarousel_003Ec__AnonStorey.idToChoose != null)
		{
			if (WeaponCategory)
			{
				_003CReloadCarousel_003Ec__AnonStorey327 _003CReloadCarousel_003Ec__AnonStorey2 = new _003CReloadCarousel_003Ec__AnonStorey327();
				_003CReloadCarousel_003Ec__AnonStorey2.itemRecord = ItemDb.GetByTag(_003CReloadCarousel_003Ec__AnonStorey.idToChoose);
				num3 = ((_003CReloadCarousel_003Ec__AnonStorey2.itemRecord == null) ? (-1) : list.FindIndex(_003CReloadCarousel_003Ec__AnonStorey2._003C_003Em__4E7));
			}
			else if (currentCategory != CategoryNames.SkinsCategory)
			{
				num3 = list.FindIndex(_003CReloadCarousel_003Ec__AnonStorey._003C_003Em__4E8);
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			_003CReloadCarousel_003Ec__AnonStorey329 _003CReloadCarousel_003Ec__AnonStorey3 = new _003CReloadCarousel_003Ec__AnonStorey329();
			_003CReloadCarousel_003Ec__AnonStorey3._003C_003Ef__this = this;
			GameObject original = Resources.Load<GameObject>("ShopCarouselElement");
			GameObject gameObject2 = UnityEngine.Object.Instantiate(original);
			gameObject2.transform.parent = wrapContent.transform;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			_003CReloadCarousel_003Ec__AnonStorey3.pref = list[m];
			gameObject2.name = m.ToString("D7");
			if (WeaponCategory)
			{
				gameObject2.name = gameObject2.name + "_" + int.Parse(_003CReloadCarousel_003Ec__AnonStorey3.pref.name.Substring("Weapon".Length)).ToString("D5");
			}
			_003CReloadCarousel_003Ec__AnonStorey3.sce = gameObject2.GetComponent<ShopCarouselElement>();
			_003CReloadCarousel_003Ec__AnonStorey3.itenID = (WeaponCategory ? ItemDb.GetByPrefabName(_003CReloadCarousel_003Ec__AnonStorey3.pref.name.Replace("(Clone)", string.Empty)).Tag : ((currentCategory != CategoryNames.SkinsCategory) ? ItemIDForPrefabReverse(_003CReloadCarousel_003Ec__AnonStorey3.pref.name, currentCategory) : array2[m]));
			_003CReloadCarousel_003Ec__AnonStorey3.sce.itemID = _003CReloadCarousel_003Ec__AnonStorey3.itenID;
			if (currentCategory == CategoryNames.GearCategory)
			{
				_003CReloadCarousel_003Ec__AnonStorey3.sce.showQuantity = true;
				_003CReloadCarousel_003Ec__AnonStorey3.sce.SetQuantity();
			}
			_003CReloadCarousel_003Ec__AnonStorey3.isBought = false;
			if (WeaponCategory && WeaponManager.tagToStoreIDMapping.ContainsKey(_003CReloadCarousel_003Ec__AnonStorey3.sce.itemID) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[_003CReloadCarousel_003Ec__AnonStorey3.sce.itemID]))
			{
				_003CReloadCarousel_003Ec__AnonStorey3.isBought = Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[_003CReloadCarousel_003Ec__AnonStorey3.sce.itemID]], true) > 0;
			}
			if (currentCategory == CategoryNames.SkinsCategory)
			{
				_003CReloadCarousel_003Ec__AnonStorey3.sce.readableName = (_003CReloadCarousel_003Ec__AnonStorey3.itenID.Equals("CustomSkinID") ? LocalizationStore.Get("Key_1090") : ((!SkinsController.skinsNamesForPers.ContainsKey(_003CReloadCarousel_003Ec__AnonStorey3.itenID)) ? string.Empty : SkinsController.skinsNamesForPers[_003CReloadCarousel_003Ec__AnonStorey3.itenID]));
			}
			if (PromoActionsManager.sharedManager.topSellers.Contains(_003CReloadCarousel_003Ec__AnonStorey3.itenID))
			{
				_003CReloadCarousel_003Ec__AnonStorey3.sce.showTS = true;
			}
			if (PromoActionsManager.sharedManager.news.Contains(_003CReloadCarousel_003Ec__AnonStorey3.itenID))
			{
				_003CReloadCarousel_003Ec__AnonStorey3.sce.showNew = true;
			}
			Action<GameObject, CategoryNames> action = _003CReloadCarousel_003Ec__AnonStorey3._003C_003Em__4E9;
			int num4 = -1;
			if (WeaponCategory)
			{
				_003CReloadCarousel_003Ec__AnonStorey32B _003CReloadCarousel_003Ec__AnonStorey32B = new _003CReloadCarousel_003Ec__AnonStorey32B();
				_003CReloadCarousel_003Ec__AnonStorey32B.itemRecord = ItemDb.GetByTag(_003CReloadCarousel_003Ec__AnonStorey3.itenID);
				num4 = ((_003CReloadCarousel_003Ec__AnonStorey32B.itemRecord == null) ? (-1) : list.FindIndex(_003CReloadCarousel_003Ec__AnonStorey32B._003C_003Em__4EA));
			}
			else if (currentCategory != CategoryNames.SkinsCategory)
			{
				num4 = list.FindIndex(_003CReloadCarousel_003Ec__AnonStorey3._003C_003Em__4EB);
			}
			if ((num4 < 0 || num3 < 0 || Mathf.Abs(num3 - num4) > 2) && currentCategory != CategoryNames.SkinsCategory && !EnableConfigurePos)
			{
				CoroutineRunner.Instance.StartCoroutine(LoadModelAsync(action, _003CReloadCarousel_003Ec__AnonStorey3.pref, currentCategory));
			}
			else
			{
				action(WeaponCategory ? WeaponManager.InnerPrefabForWeaponSync(_003CReloadCarousel_003Ec__AnonStorey3.pref.nameNoClone()) : ((currentCategory == CategoryNames.SkinsCategory) ? _003CReloadCarousel_003Ec__AnonStorey3.pref : ItemDb.GetWearFromResources(_003CReloadCarousel_003Ec__AnonStorey3.pref.nameNoClone(), currentCategory)), currentCategory);
			}
		}
		wrapContent.Reposition();
		ChooseCarouselItem(_003CReloadCarousel_003Ec__AnonStorey.idToChoose, true);
	}

	private IEnumerator LoadModelAsync(Action<GameObject, CategoryNames> onLoad, GameObject prototype, CategoryNames category)
	{
		ResourceRequest request = ((!IsWeaponCategory(category)) ? ItemDb.GetWearFromResourcesAsync(prototype.nameNoClone(), category) : WeaponManager.InnerPrefabForWeaponAsync(prototype.nameNoClone()));
		numberOfLoadingModels++;
		yield return request;
		numberOfLoadingModels--;
		try
		{
			GameObject model = request.asset as GameObject;
			if (currentCategory == category)
			{
				WeaponManager.cachedInnerPrefabsForCurrentShopCategory.Add(model);
				onLoad(model, category);
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Exception in LoadModelAsync prototype = " + prototype.name + "  e: " + e);
		}
	}

	public static void AddModel(GameObject pref, Action7<GameObject, Vector3, Vector3, string, float, int, int> act, CategoryNames c, bool isButtonInGameGui = false, WeaponSounds wsForPos = null)
	{
		float arg = 150f;
		Vector3 arg2 = Vector3.zero;
		Vector3 arg3 = Vector3.zero;
		GameObject gameObject = null;
		int arg4 = 0;
		int arg5 = 0;
		string arg6 = null;
		if (IsWeaponCategory(c))
		{
			arg = wsForPos.scaleShop;
			arg2 = wsForPos.positionShop;
			arg3 = wsForPos.rotationShop;
			gameObject = pref.GetComponent<InnerWeaponPars>().bonusPrefab;
			try
			{
				_003CAddModel_003Ec__AnonStorey32C _003CAddModel_003Ec__AnonStorey32C = new _003CAddModel_003Ec__AnonStorey32C();
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(wsForPos.name.Replace("(Clone)", string.Empty));
				string text = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
				_003CAddModel_003Ec__AnonStorey32C.firstRec = ItemDb.GetByTag(text);
				arg6 = WeaponManager.AllWrapperPrefabs().First(_003CAddModel_003Ec__AnonStorey32C._003C_003Em__4EC).shopName;
			}
			catch (Exception ex)
			{
				Debug.LogError("Error in getting shop name of first upgrade: " + ex);
				arg6 = wsForPos.shopName;
			}
			arg4 = wsForPos.tier;
		}
		else
		{
			switch (c)
			{
			case CategoryNames.SkinsCategory:
			{
				gameObject = UnityEngine.Object.Instantiate(pref);
				ShopPositionParams component = pref.GetComponent<ShopPositionParams>();
				arg3 = component.rotationShop;
				arg = component.scaleShop;
				arg2 = component.positionShop;
				arg4 = component.tier;
				break;
			}
			case CategoryNames.HatsCategory:
			case CategoryNames.ArmorCategory:
			case CategoryNames.CapesCategory:
			case CategoryNames.BootsCategory:
			case CategoryNames.GearCategory:
			case CategoryNames.MaskCategory:
			{
				gameObject = pref.transform.GetChild(0).gameObject;
				ShopPositionParams infoForNonWeaponItem = ItemDb.GetInfoForNonWeaponItem(pref.nameNoClone(), c);
				arg3 = infoForNonWeaponItem.rotationShop;
				arg = infoForNonWeaponItem.scaleShop;
				arg2 = infoForNonWeaponItem.positionShop;
				arg6 = infoForNonWeaponItem.shopName;
				arg4 = infoForNonWeaponItem.tier;
				arg5 = infoForNonWeaponItem.League;
				break;
			}
			}
		}
		Vector3 localPosition = Vector3.zero;
		GameObject gameObject2 = null;
		if (c == CategoryNames.SkinsCategory)
		{
			gameObject2 = gameObject;
			localPosition = new Vector3(0f, -1f, 0f);
		}
		else if (gameObject != null)
		{
			Material[] array = null;
			Mesh mesh = null;
			SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer == null)
			{
				SkinnedMeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				if (componentsInChildren != null && componentsInChildren.Length > 0)
				{
					skinnedMeshRenderer = componentsInChildren[0];
				}
			}
			if (skinnedMeshRenderer != null)
			{
				array = skinnedMeshRenderer.sharedMaterials;
				mesh = skinnedMeshRenderer.sharedMesh;
			}
			else
			{
				MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
				MeshRenderer component3 = gameObject.GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					mesh = component2.sharedMesh;
				}
				if (component3 != null)
				{
					array = component3.sharedMaterials;
				}
			}
			if (array != null && mesh != null)
			{
				gameObject2 = new GameObject();
				gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
				MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
				meshRenderer.materials = array;
				localPosition = -meshRenderer.bounds.center;
			}
		}
		try
		{
			DisableLightProbesRecursively(gameObject2);
		}
		catch (Exception ex2)
		{
			Debug.LogError("Exception DisableLightProbesRecursively: " + ex2);
		}
		GameObject gameObject3 = new GameObject();
		gameObject3.name = gameObject2.name;
		gameObject2.transform.localPosition = localPosition;
		gameObject2.transform.parent = gameObject3.transform;
		Player_move_c.SetLayerRecursively(gameObject3, LayerMask.NameToLayer("NGUIShop"));
		if (act != null)
		{
			act(gameObject3, arg2, arg3, arg6, arg, arg4, arg5);
		}
	}

	public void ChooseCarouselItem(string itemID, bool moveCarousel = false, bool setManuallyToChosen = false)
	{
		if (itemID == null)
		{
			if (WeaponCategory)
			{
				UpdatePersWithNewItem();
			}
			return;
		}
		ShopCarouselElement[] array = wrapContent.GetComponentsInChildren<ShopCarouselElement>(true);
		if (array == null)
		{
			array = new ShopCarouselElement[0];
		}
		ShopCarouselElement[] array2 = array;
		foreach (ShopCarouselElement shopCarouselElement in array2)
		{
			if (!shopCarouselElement.itemID.Equals(itemID))
			{
				continue;
			}
			if (moveCarousel || setManuallyToChosen)
			{
				SpringPanel component = scrollViewPanel.GetComponent<SpringPanel>();
				if (component != null)
				{
					UnityEngine.Object.Destroy(component);
				}
				if (scrollViewPanel.gameObject.activeInHierarchy)
				{
					scrollViewPanel.GetComponent<UIScrollView>().MoveRelative(new Vector3(0f - shopCarouselElement.transform.localPosition.x - scrollViewPanel.transform.localPosition.x, scrollViewPanel.transform.localPosition.y, scrollViewPanel.transform.localPosition.z));
				}
				wrapContent.Reposition();
			}
			viewedId = itemID;
			UpdatePersWithNewItem();
			UpdateButtons();
			UpdateItemParameters();
			caption.text = shopCarouselElement.readableName ?? string.Empty;
			caption.applyGradient = TempItemsController.PriceCoefs.ContainsKey(itemID);
			try
			{
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					if (trainingState == TrainingState.OnSniperRifle && viewedId != null && viewedId != WeaponTags.HunterRifleTag)
					{
						setInSniperCategoryNotOnSniperRifle();
					}
					else if (trainingState == TrainingState.InSniperCategoryNotOnSniperRifle && viewedId != null && viewedId == WeaponTags.HunterRifleTag)
					{
						setOnSniperRifle();
					}
					else if (trainingState == TrainingState.OnArmor && viewedId != null && viewedId != (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
					{
						setInArmorCategoryNotOnArmor();
					}
					else if (trainingState == TrainingState.InArmorCategoryNotOnArmor && viewedId != null && viewedId == (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
					{
						setOnArmor();
					}
				}
				if (InTrainingAfterNoviceArmorRemoved)
				{
					if (trainingStateRemoveNoviceArmor == TrainingState.OnArmor && viewedId != null && viewedId != (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
					{
						setInArmorCategoryNotOnArmorRemovedNoviceArmor();
					}
					else if (trainingStateRemoveNoviceArmor == TrainingState.InArmorCategoryNotOnArmor && viewedId != null && viewedId == (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
					{
						setOnArmorRemovedNoviceArmor();
					}
				}
				break;
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in training in ChooseCarouselItem: " + ex);
				break;
			}
		}
	}

	private void SetUpUpgradesAndTiers(bool bought, ref bool buyActive, ref bool upgradeActive, ref bool saleActive, ref bool needTierActive, ref bool rentActive, ref bool saleRentActive)
	{
		bool flag = TempItemsController.PriceCoefs.ContainsKey(viewedId);
		bool maxUpgrade = false;
		int num = ((viewedId == null) ? (-1) : _CurrentNumberOfWearUpgrades(viewedId, out maxUpgrade, currentCategory));
		bool flag2 = maxUpgrade;
		if ((!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted) || InTrainingAfterNoviceArmorRemoved)
		{
			buyActive = false;
			upgradeActive = false;
		}
		else
		{
			buyActive = viewedId != null && !flag2 && num == 0 && !viewedId.Equals("cape_Custom") && !flag;
			upgradeActive = viewedId != null && !flag2 && num != 0 && !flag;
			rentActive = false;
		}
		if (!flag2)
		{
			int num2 = Wear.TierForWear(WeaponManager.FirstUnboughtTag(viewedId));
			needTierActive = upgradeActive && ExpController.Instance != null && ExpController.Instance.OurTier < num2 && !flag && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
			if (needTierActive)
			{
				int num3 = ((num2 < 0 || num2 >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[num2]);
				string text = string.Format("{0} {1} {2}", LocalizationStore.Key_0226, num3, LocalizationStore.Get("Key_1022"));
				needTierLabel.text = text;
			}
			upgrade.isEnabled = (!upgradeActive || !(ExpController.Instance != null) || ExpController.Instance.OurTier >= num2) && !flag;
		}
		string text2 = WeaponManager.FirstUnboughtTag(viewedId);
		bool onlyServerDiscount;
		int num4 = DiscountFor(text2, out onlyServerDiscount);
		if (!flag2 && !flag && !needTierActive && num4 > 0 && (text2 == null || !text2.Equals("cape_Custom") || !inGame) && (!(text2 == "cape_Custom") || !Defs.isDaterRegim))
		{
			saleActive = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted) && !InTrainingAfterNoviceArmorRemoved;
			salePerc.text = num4 + "%";
		}
		else
		{
			saleActive = false;
		}
		saleRentActive = false;
	}

	private void UpdateTryGunDiscountTime()
	{
		try
		{
			tryGunDiscountTime.text = TempItemsController.TempItemTimeRemainsStringRepresentation(WeaponManager.sharedManager.StartTimeForTryGunDiscount(viewedId) + (long)WeaponManager.TryGunPromoDuration() - PromoActionsManager.CurrentUnixTime);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in tryGunDiscountPanelActive.text: " + ex);
		}
	}

	public void UpdateButtons()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		bool buyActive = false;
		bool rentActive = false;
		bool flag9 = false;
		bool flag10 = false;
		wholePrice2Gear.SetActive(false);
		bool upgradeActive = false;
		bool[] array = new bool[2];
		bool saleActive = false;
		bool flag11 = false;
		bool needTierActive = false;
		bool flag12 = viewedId != null && TempItemsController.PriceCoefs.ContainsKey(viewedId);
		rentProperties.SetActive(viewedId != null && TempItemsController.IsCategoryContainsTempItems(currentCategory) && TempItemsController.PriceCoefs.ContainsKey(viewedId));
		prolongateRentText.SetActive(viewedId != null && TempItemsController.PriceCoefs.ContainsKey(viewedId) && TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(viewedId));
		bool saleRentActive = false;
		upgrade.isEnabled = true;
		upgradeGear.isEnabled = true;
		if (WeaponCategory)
		{
			WeaponSounds weaponSounds = null;
			WeaponSounds weaponSounds2 = null;
			string text = null;
			string text2 = null;
			if (viewedId != null)
			{
				text = WeaponManager.FirstUnboughtTag(viewedId) ?? viewedId;
				string text3 = WeaponManager.FirstTagForOurTier(viewedId);
				List<string> list = WeaponUpgrades.ChainForTag(viewedId);
				if (text3 != null && list != null && list.IndexOf(text3) > list.IndexOf(text))
				{
					text = text3;
				}
				text2 = WeaponManager.LastBoughtTag(viewedId);
				foreach (WeaponSounds item in WeaponManager.AllWrapperPrefabs())
				{
					if (!ItemDb.GetByPrefabName(item.name).Tag.Equals(text2 ?? viewedId))
					{
						continue;
					}
					weaponSounds = item;
					foreach (WeaponSounds item2 in WeaponManager.AllWrapperPrefabs())
					{
						if (ItemDb.GetByPrefabName(item2.name).Tag.Equals(text))
						{
							weaponSounds2 = item2;
							break;
						}
					}
					break;
				}
			}
			bool flag13 = false;
			bool maxUpgrade = false;
			int num = ((viewedId == null) ? (-1) : _CurrentNumberOfUpgrades(viewedId, out maxUpgrade, currentCategory));
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(viewedId))
			{
				num = 0;
			}
			flag13 = maxUpgrade && (!(WeaponManager.sharedManager != null) || !WeaponManager.sharedManager.IsAvailableTryGun(viewedId));
			buyActive = viewedId != null && !flag13 && num == 0 && !flag12 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
			upgradeActive = viewedId != null && !flag13 && num != 0 && weaponSounds2.tier < 100 && !flag12 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
			rentActive = false;
			if (WeaponManager.sharedManager != null && viewedId != null)
			{
				flag = WeaponManager.sharedManager.IsAvailableTryGun(viewedId);
				if (flag)
				{
					try
					{
						tryGunMatchesCount.text = ((SaltedInt)WeaponManager.sharedManager.TryGuns[viewedId]["NumberOfMatchesKey"]).Value.ToString();
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception in tryGunMatchesCount.text: " + ex);
					}
				}
				flag2 = WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(viewedId);
				if (flag2)
				{
					UpdateTryGunDiscountTime();
				}
			}
			needTierActive = upgradeActive && weaponSounds2 != null && ExpController.Instance != null && ExpController.Instance.OurTier < weaponSounds2.tier && !flag12 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
			if (weaponSounds2 != null && viewedId == ItemDb.GetByPrefabName(weaponSounds.name).Tag)
			{
				needTierActive = false;
			}
			if (needTierActive)
			{
				int num2 = ((weaponSounds2.tier < 0 || weaponSounds2.tier >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[weaponSounds2.tier]);
				string text4 = string.Format("{0} {1} {2}", LocalizationStore.Key_0226, num2, LocalizationStore.Get("Key_1022"));
				needTierLabel.text = text4;
			}
			upgrade.isEnabled = !upgradeActive || !(weaponSounds2 != null) || !(ExpController.Instance != null) || ExpController.Instance.OurTier >= weaponSounds2.tier;
			string text5 = null;
			if (viewedId != null)
			{
				text5 = WeaponManager.LastBoughtTag(viewedId);
			}
			if (text5 == null && viewedId != null && WeaponManager.sharedManager.IsAvailableTryGun(viewedId))
			{
				text5 = viewedId;
			}
			bool flag14 = text5 != null && viewedId != null && (chosenId == null || !chosenId.Equals(text5)) && viewedId != null && (num > 0 || WeaponManager.sharedManager.IsAvailableTryGun(viewedId)) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || viewedId == WeaponTags.HunterRifleTag) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
			bool flag15 = !string.IsNullOrEmpty(viewedId) && ((_CurrentWeaponSetIDs()[(int)currentCategory] != null && _CurrentWeaponSetIDs()[(int)currentCategory].Equals(WeaponManager.LastBoughtTag(viewedId) ?? string.Empty)) || (WeaponManager.sharedManager.IsAvailableTryGun(viewedId) && _CurrentWeaponSetIDs()[(int)currentCategory].Equals(viewedId))) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || viewedId == WeaponTags.HunterRifleTag) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
			if ((upgradeActive || flag12 || WeaponManager.sharedManager.IsAvailableTryGun(viewedId)) && viewedId != null && text != null && (flag12 || !text.Equals(viewedId) || WeaponManager.sharedManager.IsAvailableTryGun(viewedId)) && flag14)
			{
				flag6 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || viewedId == WeaponTags.HunterRifleTag) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				flag7 = false;
			}
			if ((upgradeActive || flag12 || WeaponManager.sharedManager.IsAvailableTryGun(viewedId)) && viewedId != null && text != null && (flag12 || !text.Equals(viewedId) || WeaponManager.sharedManager.IsAvailableTryGun(viewedId)) && flag15)
			{
				array[0] = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || viewedId == WeaponTags.HunterRifleTag) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				array[1] = false;
			}
			if (!upgradeActive && !flag12 && !buyActive && flag14)
			{
				flag6 = false;
				flag7 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || viewedId == WeaponTags.HunterRifleTag) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
			}
			if (!upgradeActive && !flag12 && !buyActive && flag15)
			{
				array[0] = false;
				array[1] = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || viewedId == WeaponTags.HunterRifleTag) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
			}
			bool onlyServerDiscount;
			int num3 = DiscountFor(text, out onlyServerDiscount);
			if (!flag13 && !flag12 && weaponSounds != null && weaponSounds.tier < 100 && !needTierActive && num3 > 0)
			{
				saleActive = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
				salePerc.text = num3 + "%";
			}
			else
			{
				saleActive = false;
			}
			saleRentActive = false;
			if (text != null)
			{
				int num4 = 1;
				int num5 = 0;
				if (num4 == 1 && text2 != null && text.Equals(text2))
				{
					num5 = 1;
				}
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (!upgrade.Contains(text))
					{
						continue;
					}
					num4 = upgrade.Count;
					if (text2 != null)
					{
						num5 = upgrade.IndexOf(text2) + 1;
						break;
					}
					string text6 = WeaponManager.FirstTagForOurTier(text);
					if (text6 != null && upgrade.IndexOf(text6) > 0)
					{
						num5 = upgrade.IndexOf(text6);
					}
					break;
				}
				bool flag16 = (!weaponSounds.isMelee || weaponSounds.isShotMelee) && viewedId != null;
				if (weaponProperties.activeSelf != flag16)
				{
					weaponProperties.SetActive(flag16);
				}
				bool flag17 = weaponSounds.isMelee && !weaponSounds.isShotMelee && viewedId != null;
				if (meleeProperties.activeSelf != flag17)
				{
					meleeProperties.SetActive(flag17);
				}
				if (upgradesAnchor.activeSelf != (viewedId != null && !flag12))
				{
					upgradesAnchor.SetActive(viewedId != null && !flag12);
				}
				bool flag18 = num4 == 1 && !flag12;
				if (upgrade_1.activeSelf != flag18)
				{
					upgrade_1.SetActive(flag18);
				}
				bool flag19 = num4 == 2 && !flag12;
				if (upgrade_2.activeSelf != flag19)
				{
					upgrade_2.SetActive(flag19);
				}
				bool flag20 = num4 == 3 && !flag12;
				if (upgrade_3.activeSelf != flag20)
				{
					upgrade_3.SetActive(flag20);
				}
				if (num4 > 0)
				{
					GameObject obj;
					switch (num4)
					{
					case 3:
						obj = upgrade_3;
						break;
					case 2:
						obj = upgrade_2;
						break;
					default:
						obj = upgrade_1;
						break;
					}
					GameObject gameObject = obj;
					GameObject[] array2 = upgradeSprites1;
					if (num4 == 3)
					{
						array2 = upgradeSprites3;
					}
					if (num4 == 2)
					{
						array2 = upgradeSprites2;
					}
					if (array2 == null)
					{
						array2 = new GameObject[0];
					}
					GameObject[] array3 = array2;
					if (_003C_003Ef__am_0024cacheC9 == null)
					{
						_003C_003Ef__am_0024cacheC9 = _003CUpdateButtons_003Em__4ED;
					}
					Array.Sort(array3, _003C_003Ef__am_0024cacheC9);
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].SetActive((num5 >= i) ? true : false);
						if (num5 == i)
						{
							array2[i].GetComponent<TweenColor>().enabled = true;
							continue;
						}
						array2[i].GetComponent<TweenColor>().enabled = false;
						array2[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
					}
				}
				UpdateTempItemTime();
				if (weaponSounds2 == null)
				{
					weaponSounds2 = weaponSounds;
				}
				int[] array4 = null;
				array4 = ((!weaponSounds.isMelee || weaponSounds.isShotMelee) ? new int[4]
				{
					(!flag12) ? weaponSounds.damageShop : ((int)weaponSounds.DPS),
					weaponSounds.fireRateShop,
					weaponSounds.CapacityShop,
					weaponSounds.mobilityShop
				} : new int[3]
				{
					(!flag12) ? weaponSounds.damageShop : ((int)weaponSounds.DPS),
					weaponSounds.fireRateShop,
					weaponSounds.mobilityShop
				});
				int[] array5 = null;
				array5 = ((!weaponSounds.isMelee || weaponSounds.isShotMelee) ? new int[4]
				{
					(!flag12) ? weaponSounds2.damageShop : ((int)weaponSounds2.DPS),
					weaponSounds2.fireRateShop,
					weaponSounds2.CapacityShop,
					weaponSounds2.mobilityShop
				} : new int[3]
				{
					(!flag12) ? weaponSounds2.damageShop : ((int)weaponSounds2.DPS),
					weaponSounds2.fireRateShop,
					weaponSounds2.mobilityShop
				});
				bool flag21 = text2 != null && text != null && text2 != text && viewedId != null && text2 == viewedId;
				int[] array6 = ((!flag21) ? array5 : array4);
				if (weaponSounds.isMelee && !weaponSounds.isShotMelee)
				{
					damageMelee.text = GetWeaponStatText(array4[0], array6[0]);
					fireRateMElee.text = GetWeaponStatText(array4[1], array6[1]);
					mobilityMelee.text = GetWeaponStatText(array4[2], array6[2]);
				}
				else
				{
					damage.text = GetWeaponStatText(array4[0], array6[0]);
					fireRate.text = GetWeaponStatText(array4[1], array6[1]);
					capacity.text = GetWeaponStatText(array4[2], array6[2]);
					mobility.text = GetWeaponStatText(array4[3], array6[3]);
				}
				if (!SpecialParams.activeSelf)
				{
					SpecialParams.SetActive(true);
				}
				float num6 = 0.81500006f;
				if (weaponSounds2 == null)
				{
					weaponSounds2 = weaponSounds;
				}
				WeaponSounds weaponSounds3 = ((!flag21) ? weaponSounds2 : weaponSounds);
				if (weaponSounds3 != null)
				{
					if (!FriendsController.SandboxEnabled && weaponSounds3.InShopEffects.Contains(WeaponSounds.Effects.ForSandbox))
					{
						weaponSounds3.InShopEffects.Remove(WeaponSounds.Effects.ForSandbox);
					}
					float num7 = 90f / (float)weaponSounds3.InShopEffects.Count;
					for (int j = 0; j < effectsLabels.Count; j++)
					{
						if (effectsLabels[j].gameObject.activeSelf != j < weaponSounds3.InShopEffects.Count)
						{
							effectsLabels[j].gameObject.SetActive(j < weaponSounds3.InShopEffects.Count);
						}
						if (j >= weaponSounds3.InShopEffects.Count)
						{
							continue;
						}
						Transform transform = effectsLabels[j].transform;
						float num8 = 0f;
						if (weaponSounds3.InShopEffects.Count == 3)
						{
							if (j == 0)
							{
								num8 = 1f;
							}
							if (j == 2)
							{
								num8 = -3f;
							}
						}
						else if (weaponSounds3.InShopEffects.Count == 2)
						{
							if (j == 0)
							{
								num8 = -6f;
							}
							if (j == 1)
							{
								num8 = 6f;
							}
						}
						transform.localPosition = new Vector3(transform.localPosition.x, 39f - num7 * ((float)j + 0.5f) + num8, transform.localPosition.z);
						effectsLabels[j].text = ((weaponSounds3.InShopEffects[j] != WeaponSounds.Effects.Zoom) ? string.Empty : (weaponSounds3.zoomShop + "X ")) + LocalizationStore.Get(WeaponSounds.keysAndSpritesForEffects[weaponSounds3.InShopEffects[j]].Value);
						effectsSprites[j].spriteName = WeaponSounds.keysAndSpritesForEffects[weaponSounds3.InShopEffects[j]].Key;
					}
				}
			}
		}
		else
		{
			if (weaponProperties.activeSelf)
			{
				weaponProperties.SetActive(false);
			}
			if (meleeProperties.activeSelf)
			{
				meleeProperties.SetActive(false);
			}
			if (upgradesAnchor.activeSelf)
			{
				upgradesAnchor.SetActive(false);
			}
			if (SpecialParams.activeSelf)
			{
				SpecialParams.SetActive(false);
			}
			switch (currentCategory)
			{
			case CategoryNames.HatsCategory:
			case CategoryNames.ArmorCategory:
			case CategoryNames.CapesCategory:
			case CategoryNames.BootsCategory:
			case CategoryNames.MaskCategory:
			{
				string text7 = WeaponManager.LastBoughtTag(viewedId);
				bool flag28 = text7 != null;
				bool flag29 = flag28 && _CurrentEquippedWear.Equals(text7);
				string text8 = WeaponManager.FirstUnboughtTag(viewedId);
				SetUpUpgradesAndTiers(flag28, ref buyActive, ref upgradeActive, ref saleActive, ref needTierActive, ref rentActive, ref saleRentActive);
				bool flag30 = viewedId != "Armor_Novice" && flag28 && !flag29 && text7 != null && text7.Equals(viewedId) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || trainingState >= TrainingState.OnArmor) && (!InTrainingAfterNoviceArmorRemoved || viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				flag8 = viewedId != "Armor_Novice" && flag28 && flag29 && text7 != null && text7.Equals(viewedId) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
				bool flag31 = viewedId == "Armor_Novice" || (text7 != null && _CurrentEquippedWear.Equals(text7) && !upgradeActive && !flag12);
				if (!flag28 && viewedId != null && viewedId.Equals("cape_Custom"))
				{
					flag3 = !inGame && !Defs.isDaterRegim && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
				}
				if (!inGame && flag28 && viewedId != null && viewedId.Equals("cape_Custom"))
				{
					flag4 = !Defs.isDaterRegim && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
				}
				if (upgradeActive || flag12)
				{
					flag6 = flag30 && (flag12 || (viewedId != null && text8 != null && (text7 == null || text7.Equals(text8) || !text8.Equals(viewedId))));
					flag7 = false;
				}
				else
				{
					flag6 = false;
					flag7 = flag30 && viewedId != null && text8 != null && (text7 == null || text7.Equals(text8) || !text8.Equals(viewedId));
				}
				if (viewedId == "cape_Custom" && flag28)
				{
					array[1] = flag31;
					array[0] = false;
				}
				else
				{
					array[(!upgradeActive) ? 1 : 0] = flag31 && (flag12 || (viewedId != null && text8 != null && (text7 == null || text7.Equals(text8) || !text8.Equals(viewedId))));
					array[upgradeActive ? 1 : 0] = false;
				}
				try
				{
					if (currentCategory == CategoryNames.ArmorCategory)
					{
						armorCountLabel.text = Wear.armorNum[viewedId].ToString();
						armorCountLabel.gameObject.SetActive(viewedId != "Armor_Novice");
						armorWearDescription.text = LocalizationStore.Get("Key_0354");
					}
					else
					{
						nonArmorWearDEscription.text = LocalizationStore.Get(Wear.descriptionLocalizationKeys[viewedId]);
					}
				}
				catch (Exception ex2)
				{
					Debug.LogError("Exception in setting desciption for wear: " + ex2);
				}
				UpdateTempItemTime();
				break;
			}
			case CategoryNames.SkinsCategory:
			{
				flag3 = false;
				bool flag22 = false;
				bool flag23 = false;
				bool flag24 = false;
				bool flag25 = viewedId == "61";
				bool flag26 = false;
				if (!viewedId.Equals("CustomSkinID"))
				{
					bool isForMoneySkin = false;
					flag26 = SkinsController.IsSkinBought(viewedId, out isForMoneySkin);
					buyActive = isForMoneySkin && !flag26 && !flag25 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					upgradeActive = false;
					flag23 = (!isForMoneySkin || flag26) && !viewedId.Equals(SkinsController.currentSkinNameForPers) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					flag8 = false;
					flag22 = (!isForMoneySkin || flag26) && viewedId.Equals(SkinsController.currentSkinNameForPers);
					flag10 = flag25 && !flag26 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					bool flag27 = false;
					long result;
					flag27 = long.TryParse(viewedId, out result) && result >= 1000000;
					flag4 = !inGame && flag27 && !Defs.isDaterRegim && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					flag5 = flag27 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					bool onlyServerDiscount2;
					int num9 = DiscountFor(viewedId, out onlyServerDiscount2);
					if (!flag26 && !flag25 && !needTierActive && num9 > 0)
					{
						saleActive = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
						salePerc.text = num9 + "%";
					}
					else
					{
						saleActive = false;
					}
				}
				else
				{
					flag24 = Storager.getInt(Defs.SkinsMakerInProfileBought, true) > 0;
					flag3 = !inGame && !flag24 && !Defs.isDaterRegim && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					flag4 = false;
					flag11 = !inGame && flag24 && !Defs.isDaterRegim && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
					bool onlyServerDiscount3;
					int num10 = DiscountFor(viewedId, out onlyServerDiscount3);
					if (!flag24 && !needTierActive && num10 > 0 && (viewedId == null || !viewedId.Equals("CustomSkinID") || !inGame) && !inGame && !Defs.isDaterRegim)
					{
						saleActive = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
						salePerc.text = num10 + "%";
					}
					else
					{
						saleActive = false;
					}
				}
				flag6 = false;
				flag7 = flag23 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
				array[0] = false;
				array[1] = flag22 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && !InTrainingAfterNoviceArmorRemoved;
				foreach (Transform item3 in skinProperties.transform)
				{
					if (viewedId != null && viewedId.Equals("CustomSkinID"))
					{
						if (!flag24)
						{
							item3.gameObject.SetActive(item3.gameObject.name.Equals("Custom_Skin"));
						}
						else
						{
							item3.gameObject.SetActive(item3.gameObject.name.Equals("Custom1_Skin"));
						}
					}
					else if (flag25 && !flag26)
					{
						item3.gameObject.SetActive(item3.gameObject.name.Equals("Facebook_Skin"));
					}
					else
					{
						item3.gameObject.SetActive(item3.gameObject.name.Equals("Usual_Skin"));
					}
				}
				break;
			}
			}
		}
		if (tryGunPanel != null && tryGunPanel.activeSelf != flag)
		{
			tryGunPanel.SetActive(flag);
		}
		if (tryGunDiscountPanel != null && tryGunDiscountPanel.activeSelf != flag2)
		{
			tryGunDiscountPanel.SetActive(flag2);
		}
		if (edit != null && edit.gameObject.activeSelf != flag4)
		{
			edit.gameObject.SetActive(flag4);
		}
		if (enable != null && enable.gameObject.activeSelf != flag3)
		{
			enable.gameObject.SetActive(flag3);
		}
		if (delete.gameObject.activeSelf != flag5)
		{
			delete.gameObject.SetActive(flag5);
		}
		if (buy.gameObject.activeSelf != buyActive)
		{
			buy.gameObject.SetActive(buyActive);
		}
		if (rent.gameObject.activeSelf != rentActive)
		{
			rent.gameObject.SetActive(rentActive);
		}
		if (equips[0].gameObject.activeSelf != flag6)
		{
			equips[0].gameObject.SetActive(flag6);
		}
		if (equips[1].gameObject.activeSelf != flag7)
		{
			equips[1].gameObject.SetActive(flag7);
		}
		if (unequip.gameObject.activeSelf != flag8)
		{
			unequip.gameObject.SetActive(flag8);
		}
		if (buyGear.gameObject.activeSelf != flag9)
		{
			buyGear.gameObject.SetActive(flag9);
		}
		if (this.upgrade.gameObject.activeSelf != upgradeActive)
		{
			this.upgrade.gameObject.SetActive(upgradeActive);
		}
		if (facebookLoginLockedSkinButton.gameObject.activeSelf != flag10)
		{
			facebookLoginLockedSkinButton.gameObject.SetActive(flag10);
		}
		if (equippeds[0].gameObject.activeSelf != array[0])
		{
			equippeds[0].gameObject.SetActive(array[0]);
		}
		if (equippeds[1].gameObject.activeSelf != array[1])
		{
			equippeds[1].gameObject.SetActive(array[1]);
		}
		if (sale.gameObject.activeSelf != saleActive)
		{
			sale.gameObject.SetActive(saleActive);
		}
		if (saleRent.gameObject.activeSelf != saleRentActive)
		{
			saleRent.gameObject.SetActive(saleRentActive);
		}
		if (create.gameObject.activeSelf != flag11)
		{
			create.gameObject.SetActive(flag11);
		}
		if (needTier.gameObject.activeSelf != needTierActive)
		{
			needTier.gameObject.SetActive(needTierActive);
		}
	}

	private void UpdateTempItemTime()
	{
		if (TempItemsController.sharedController != null)
		{
			bool flag = TempItemsController.PriceCoefs.ContainsKey(viewedId) && !TempItemsController.sharedController.ContainsItem(viewedId);
			if (notRented.activeInHierarchy != flag)
			{
				notRented.SetActive(flag);
			}
			string text = TempItemsController.sharedController.TimeRemainingForItemString(viewedId);
			bool flag2 = TempItemsController.sharedController.ContainsItem(viewedId) && text.Length < 5;
			if (daysLeftLabel.gameObject.activeInHierarchy != flag2)
			{
				daysLeftLabel.gameObject.SetActive(flag2);
			}
			if (daysLeftValueLabel.gameObject.activeInHierarchy != flag2)
			{
				daysLeftValueLabel.gameObject.SetActive(flag2);
			}
			if (flag2)
			{
				daysLeftValueLabel.text = text;
			}
			bool flag3 = TempItemsController.sharedController.ContainsItem(viewedId) && text.Length >= 5;
			if (timeLeftLabel.gameObject.activeInHierarchy != flag3)
			{
				timeLeftLabel.gameObject.SetActive(flag3);
			}
			if (timeLeftValueLabel.gameObject.activeInHierarchy != flag3)
			{
				timeLeftValueLabel.gameObject.SetActive(flag3);
			}
			if (flag3)
			{
				timeLeftValueLabel.text = text;
			}
			bool flag4 = flag3 && TempItemsController.sharedController.TimeRemainingForItems(viewedId) <= 3600;
			if (redBackForTime.activeInHierarchy != flag4)
			{
				redBackForTime.SetActive(flag4);
			}
		}
	}

	public static int DiscountFor(string itemTag, out bool onlyServerDiscount)
	{
		//Discarded unreachable code: IL_0182, IL_01a5
		try
		{
			if (itemTag == null)
			{
				Debug.LogError("DiscountFor: itemTag == null");
				onlyServerDiscount = false;
				return 0;
			}
			bool flag = false;
			bool flag2 = false;
			float num = 100f;
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(itemTag))
			{
				long num2 = WeaponManager.sharedManager.DiscountForTryGun(itemTag);
				num -= (float)num2;
				num = Math.Max(1f, num);
				num = Math.Min(100f, num);
				flag2 = true;
			}
			num /= 100f;
			float num3 = 100f;
			if (!flag2 && PromoActionsManager.sharedManager.discounts.ContainsKey(itemTag) && PromoActionsManager.sharedManager.discounts[itemTag].Count > 0)
			{
				num3 -= (float)PromoActionsManager.sharedManager.discounts[itemTag][0].Value;
				num3 = Math.Max(10f, num3);
				num3 = Math.Min(100f, num3);
				flag = true;
			}
			num3 /= 100f;
			onlyServerDiscount = !flag2 && flag;
			if (!flag2 && !flag)
			{
				return 0;
			}
			float value = num * num3;
			value = Mathf.Clamp(value, 0.01f, 1f);
			float f = (1f - value) * 100f;
			int num4 = Mathf.RoundToInt(f);
			if (onlyServerDiscount && num4 % 5 != 0)
			{
				num4 = 5 * (num4 / 5 + 1);
			}
			return Math.Min(num4, 99);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in DiscountFor: " + ex);
			onlyServerDiscount = false;
			return 0;
		}
	}

	public static ItemPrice currentPrice(string itemId, CategoryNames currentCategory, bool upgradeNotBuy = false, bool useDiscounts = true)
	{
		//Discarded unreachable code: IL_0186, IL_01b0
		try
		{
			if (itemId == null)
			{
				return new ItemPrice(0, "Coins");
			}
			string text = itemId;
			if (itemId != null && WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
			{
				text = WeaponManager.tagToStoreIDMapping[WeaponManager.FirstUnboughtOrForOurTier(itemId)];
			}
			if (currentCategory == CategoryNames.SkinsCategory && SkinsController.shopKeyFromNameSkin.ContainsKey(text))
			{
				text = SkinsController.shopKeyFromNameSkin[text];
			}
			if (currentCategory == CategoryNames.GearCategory)
			{
				text = ((!upgradeNotBuy) ? GearManager.OneItemIDForGear(GearManager.HolderQuantityForID(text), GearManager.CurrentNumberOfUphradesForGear(text)) : GearManager.UpgradeIDForGear(GearManager.HolderQuantityForID(text), GearManager.CurrentNumberOfUphradesForGear(text) + 1));
			}
			if (IsWearCategory(currentCategory))
			{
				text = WeaponManager.FirstUnboughtTag(text);
			}
			string itemTag = ((!IsWeaponCategory(currentCategory) && !IsWearCategory(currentCategory)) ? itemId : WeaponManager.FirstUnboughtOrForOurTier(itemId));
			ItemPrice itemPrice = ItemDb.GetPriceByShopId(text) ?? new ItemPrice(10, "Coins");
			int num = itemPrice.Price;
			if (useDiscounts)
			{
				bool onlyServerDiscount;
				int num2 = DiscountFor(itemTag, out onlyServerDiscount);
				if (num2 > 0)
				{
					float num3 = num2;
					num = Math.Max((int)((float)num * 0.01f), Mathf.RoundToInt((float)num * (1f - num3 / 100f)));
					if (onlyServerDiscount)
					{
						num = ((num % 5 >= 3) ? (num + (5 - num % 5)) : (num - num % 5));
					}
				}
			}
			if (currentCategory == CategoryNames.GearCategory && !upgradeNotBuy)
			{
				num *= GearManager.ItemsInPackForGear(GearManager.HolderQuantityForID(text));
			}
			return new ItemPrice(num, itemPrice.Currency);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in currentPrice: " + ex);
			return new ItemPrice(0, "Coins");
		}
	}

	public static int PriceIfGunWillBeTryGun(string tg)
	{
		return Mathf.RoundToInt((float)currentPrice(tg, (CategoryNames)ItemDb.GetItemCategory(tg), false, false).Price * ((float)WeaponManager.BaseTryGunDiscount() / 100f));
	}

	public void UpdateItemParameters()
	{
		wholePrice.gameObject.SetActive(buy.gameObject.activeInHierarchy || upgrade.gameObject.activeInHierarchy || enable.gameObject.activeInHierarchy);
		wholePriceUpgradeGear.gameObject.SetActive(false);
		wholePrice2Gear.gameObject.SetActive(buyGear.gameObject.activeInHierarchy);
		bool onlyServerDiscount;
		bool flag = viewedId != null && DiscountFor(WeaponManager.FirstUnboughtOrForOurTier(viewedId), out onlyServerDiscount) > 0;
		wholePriceBG.gameObject.SetActive(!flag);
		wholePriceBG_Discount.gameObject.SetActive(flag);
		bool flag2 = viewedId != null && DiscountFor(viewedId, out onlyServerDiscount) > 0;
		wholePriceBG2Gear.gameObject.SetActive(!flag2);
		wholePriceBG2Gear_Discount.gameObject.SetActive(flag2);
		wholePriceUpgradeBG2Gear.gameObject.SetActive(!flag2);
		wholePriceUpgradeBG2Gear_Discount.gameObject.SetActive(flag2);
		if (viewedId != null)
		{
			ItemPrice itemPrice = currentPrice(viewedId, currentCategory, currentCategory == CategoryNames.GearCategory);
			if (currentCategory == CategoryNames.GearCategory)
			{
				priceUpgradeGear.text = itemPrice.Price.ToString();
				currencyImagePriceUpgradeGear.spriteName = ((!itemPrice.Currency.Equals("Coins")) ? "gem_znachek" : "ingame_coin");
				currencyImagePriceUpgradeGear.width = ((!itemPrice.Currency.Equals("Coins")) ? 34 : 30);
				currencyImagePriceUpgradeGear.height = ((!itemPrice.Currency.Equals("Coins")) ? 24 : 30);
			}
			else
			{
				price.text = itemPrice.Price.ToString();
				currencyImagePrice.spriteName = ((!itemPrice.Currency.Equals("Coins")) ? "gem_znachek" : "ingame_coin");
				currencyImagePrice.width = ((!itemPrice.Currency.Equals("Coins")) ? 34 : 30);
				currencyImagePrice.height = ((!itemPrice.Currency.Equals("Coins")) ? 24 : 30);
			}
		}
	}

	private static int _CurrentNumberOfWearUpgrades(string id, out bool maxUpgrade, CategoryNames c)
	{
		_003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D _003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D = new _003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D();
		_003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D.id = id;
		if (_003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D.id == "Armor_Novice")
		{
			maxUpgrade = NoviceArmorAvailable;
			return NoviceArmorAvailable ? 1 : 0;
		}
		List<string> list = Wear.wear[c].FirstOrDefault(_003C_CurrentNumberOfWearUpgrades_003Ec__AnonStorey32D._003C_003Em__4EE);
		if (list == null)
		{
			maxUpgrade = false;
			return 0;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (Storager.getInt(list[i], true) == 0)
			{
				maxUpgrade = false;
				return i;
			}
		}
		maxUpgrade = true;
		return list.Count;
	}

	public static int _CurrentNumberOfUpgrades(string itemId, out bool maxUpgrade, CategoryNames c, bool countTryGunsAsUpgrade = true)
	{
		List<string> list = new List<string>();
		int num = 0;
		if (IsWeaponCategory(c))
		{
			List<string> list2 = WeaponUpgrades.ChainForTag(itemId);
			if (list2 == null)
			{
				List<string> list3 = new List<string>();
				list3.Add(itemId);
				list2 = list3;
			}
			list = list2;
			num = list.Count;
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
			{
				int num2 = list.Count - 1;
				while (num2 >= 0)
				{
					string defName = itemId;
					bool flag = ItemDb.IsTemporaryGun(itemId);
					if (!flag)
					{
						defName = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list[num2]]];
					}
					bool flag2 = HasBoughtGood(defName, flag);
					if (!flag2 && countTryGunsAsUpgrade && WeaponManager.sharedManager != null)
					{
						flag2 = WeaponManager.sharedManager.IsAvailableTryGun(list[num2]);
					}
					if (!flag2)
					{
						num--;
						num2--;
						continue;
					}
					break;
				}
			}
		}
		else if (IsWearCategory(c))
		{
			num = (HasBoughtGood(itemId, TempItemsController.PriceCoefs.ContainsKey(itemId)) ? 1 : 0);
		}
		if (itemId.Equals(StoreKitEventListener.elixirID) && Defs.NumberOfElixirs > 0)
		{
			num++;
		}
		maxUpgrade = num == ((list.Count <= 0) ? 1 : list.Count);
		return num;
	}

	private static bool HasBoughtGood(string defName, bool tempGun = false)
	{
		bool flag = ((!tempGun) ? (Storager.getInt(defName, true) == 0) : (!TempItemsController.sharedController.ContainsItem(defName)));
		return !flag;
	}

	public void UpdatePersWithNewItem()
	{
		if (WeaponCategory)
		{
			string text = viewedId;
			if (text == null && WeaponManager.sharedManager.playerWeapons.Count > 0)
			{
				text = ItemDb.GetByPrefabName((WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
			}
			SetWeapon(text);
			return;
		}
		switch (currentCategory)
		{
		case CategoryNames.HatsCategory:
			UpdatePersHat(viewedId);
			break;
		case CategoryNames.SkinsCategory:
			if (!viewedId.Equals("CustomSkinID"))
			{
				UpdatePersSkin(viewedId);
			}
			break;
		case CategoryNames.CapesCategory:
			UpdatePersCape(viewedId);
			break;
		case CategoryNames.MaskCategory:
			UpdatePersMask(viewedId);
			break;
		case CategoryNames.BootsCategory:
			UpdatePersBoots(viewedId);
			break;
		case CategoryNames.ArmorCategory:
			UpdatePersArmor(viewedId);
			break;
		case CategoryNames.GearCategory:
			break;
		}
	}

	public void UpdatePersHat(string hat)
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < hatPoint.transform.childCount; i++)
		{
			list.Add(hatPoint.transform.GetChild(i));
		}
		foreach (Transform item in list)
		{
			item.parent = null;
			item.position = new Vector3(0f, -10000f, 0f);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		if (!hat.Equals(Defs.HatNoneEqupped))
		{
			string @string = Storager.getString(Defs.VisualHatArmor, false);
			if (!string.IsNullOrEmpty(@string) && Wear.wear[CategoryNames.HatsCategory][0].IndexOf(hat) >= 0 && Wear.wear[CategoryNames.HatsCategory][0].IndexOf(hat) < Wear.wear[CategoryNames.HatsCategory][0].IndexOf(@string))
			{
				hat = @string;
			}
			GameObject gameObject = Resources.Load("Hats/" + hat) as GameObject;
			if (!(gameObject == null))
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
				DisableLightProbesRecursively(gameObject2);
				gameObject2.transform.parent = hatPoint.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
				SetPersHatVisible(hatPoint);
			}
		}
	}

	public void UpdatePersArmor(string armor)
	{
		if (armorPoint.childCount > 0)
		{
			Transform child = armorPoint.GetChild(0);
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
				child.position = new Vector3(0f, -10000f, 0f);
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (armor.Equals(Defs.ArmorNewNoneEqupped))
		{
			return;
		}
		string @string = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			armor = @string;
		}
		if (!(weapon != null))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Armor_Shop/" + armor) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		DisableLightProbesRecursively(gameObject2);
		ArmorRefs component2 = gameObject2.transform.GetChild(0).GetComponent<ArmorRefs>();
		if (component2 != null)
		{
			WeaponSounds component3 = weapon.GetComponent<WeaponSounds>();
			gameObject2.transform.parent = armorPoint.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
			if (component2.leftBone != null && component3.LeftArmorHand != null)
			{
				component2.leftBone.parent = component3.LeftArmorHand;
				component2.leftBone.localPosition = Vector3.zero;
				component2.leftBone.localRotation = Quaternion.identity;
				component2.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component2.rightBone != null && component3.RightArmorHand != null)
			{
				component2.rightBone.parent = component3.RightArmorHand;
				component2.rightBone.localPosition = Vector3.zero;
				component2.rightBone.localRotation = Quaternion.identity;
				component2.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		SetPersArmorVisible(armorPoint);
	}

	public void UpdatePersMask(string mask)
	{
		for (int i = 0; i < maskPoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(maskPoint.transform.GetChild(i).gameObject);
		}
		if (!mask.Equals("MaskNoneEquipped"))
		{
			GameObject gameObject = Resources.Load("Masks/" + mask) as GameObject;
			if (gameObject == null)
			{
				Debug.LogWarning("ShopNGUIController UpdatePersMask: maskPrefab == null  mask = " + (mask ?? "(null)"));
				return;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			DisableLightProbesRecursively(gameObject2);
			gameObject2.transform.parent = maskPoint.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
		}
	}

	public void UpdatePersCape(string cape)
	{
		for (int i = 0; i < capePoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(capePoint.transform.GetChild(i).gameObject);
		}
		if (!cape.Equals(Defs.CapeNoneEqupped))
		{
			GameObject gameObject = Resources.Load<GameObject>("Capes/" + cape);
			if (!(gameObject == null))
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
				DisableLightProbesRecursively(gameObject2);
				gameObject2.transform.parent = capePoint.transform;
				gameObject2.transform.localPosition = new Vector3(0f, -0.8f, 0f);
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
			}
		}
	}

	public void UpdatePersSkin(string skinId)
	{
		if (skinId == null)
		{
			Debug.LogError("Skin id should not be null!");
		}
		else
		{
			SetSkinOnPers(SkinsController.skinsForPers[skinId]);
		}
	}

	public void SetSkinOnPers(Texture skin)
	{
		WeaponSounds weaponSounds = ((body.transform.childCount <= 0) ? null : body.transform.GetChild(0).GetComponent<WeaponSounds>());
		GameObject gameObject = ((!(weaponSounds != null)) ? null : weaponSounds.bonusPrefab);
		GameObject gameObject2 = null;
		GameObject gameObject3 = null;
		if (gameObject != null)
		{
			Transform leftArmorHand = weaponSounds.LeftArmorHand;
			Transform rightArmorHand = weaponSounds.RightArmorHand;
			if (leftArmorHand != null)
			{
				gameObject2 = leftArmorHand.gameObject;
			}
			if (rightArmorHand != null)
			{
				gameObject3 = rightArmorHand.gameObject;
			}
		}
		List<GameObject> list = new List<GameObject>();
		list.Add(capePoint.gameObject);
		list.Add(hatPoint.gameObject);
		list.Add(bootsPoint.gameObject);
		list.Add(armorPoint.gameObject);
		list.Add(maskPoint.gameObject);
		List<GameObject> list2 = list;
		if (weaponSounds != null && weaponSounds.grenatePoint != null)
		{
			list2.Add(weaponSounds.grenatePoint.gameObject);
		}
		if (gameObject != null)
		{
			list2.Add(gameObject);
		}
		if (gameObject2 != null)
		{
			list2.Add(gameObject2);
		}
		if (gameObject3 != null)
		{
			list2.Add(gameObject3);
		}
		if (weaponSounds != null)
		{
			List<GameObject> listWeaponAnimEffects = weaponSounds.GetListWeaponAnimEffects();
			if (listWeaponAnimEffects != null)
			{
				list2.AddRange(listWeaponAnimEffects);
			}
		}
		Player_move_c.SetTextureRecursivelyFrom(MainMenu_Pers.gameObject, skin, list2.ToArray());
	}

	public void UpdatePersBoots(string bs)
	{
		foreach (Transform item in bootsPoint.transform)
		{
			if (item.gameObject.name.Equals(bs))
			{
				item.gameObject.SetActive(true);
			}
			else
			{
				item.gameObject.SetActive(false);
			}
		}
	}

	public void ReloadCategoryTempItemsRemoved(List<string> expired)
	{
		if (currentCategory != CategoryNames.HatsCategory && expired.Contains("hat_Adamant_3"))
		{
			UpdatePersHat(Defs.HatNoneEqupped);
		}
		if (currentCategory != CategoryNames.ArmorCategory && expired.Contains("Armor_Adamant_3"))
		{
			UpdatePersArmor(Defs.ArmorNewNoneEqupped);
		}
		if (GuiActive && TempItemsController.IsCategoryContainsTempItems(currentCategory))
		{
			CategoryChosen(currentCategory, (expired.Count <= 0 || !TempItemsController.GunsMappingFromTempToConst.ContainsKey(expired[0])) ? viewedId : TempItemsController.GunsMappingFromTempToConst[expired[0]]);
			UpdateIcons();
		}
	}

	public void SimulateCategoryChoose(int num)
	{
		if (num >= 0 && num < category.buttons.Length && num != 0)
		{
			category.buttons[0].IsChecked = false;
			category.buttons[num].IsChecked = true;
		}
	}

	public void CategoryChosen(CategoryNames i, string idToSet = null, bool initial = false)
	{
		WeaponManager.ClearCachedInnerPrefabs();
		if (!initial)
		{
			switch (currentCategory)
			{
			case CategoryNames.SkinsCategory:
				if (SkinsController.currentSkinNameForPers != null)
				{
					viewedId = SkinsController.currentSkinNameForPers;
				}
				else
				{
					if (SkinsController.skinsForPers == null || SkinsController.skinsForPers.Keys.Count <= 0)
					{
						break;
					}
					using (Dictionary<string, Texture2D>.KeyCollection.Enumerator enumerator = SkinsController.skinsForPers.Keys.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							string text = (viewedId = enumerator.Current);
						}
					}
				}
				break;
			case CategoryNames.HatsCategory:
			case CategoryNames.ArmorCategory:
			case CategoryNames.CapesCategory:
			case CategoryNames.BootsCategory:
			case CategoryNames.MaskCategory:
				viewedId = _CurrentEquippedWear;
				break;
			}
			if (WearCategory || currentCategory == CategoryNames.SkinsCategory)
			{
				UpdatePersWithNewItem();
			}
		}
		currentCategory = i;
		if (highlightedCarouselObject != null)
		{
		}
		highlightedCarouselObject = null;
		if (WeaponCategory)
		{
			chosenId = _CurrentWeaponSetIDs()[(int)i];
			viewedId = idToSet;
			if (viewedId != null && chosenId != null && viewedId.Equals(chosenId) && WeaponManager.sharedManager != null && WeaponManager.sharedManager.FilteredShopLists.Count > (int)i && !WeaponManager.sharedManager.FilteredShopLists[(int)i].Find(_003CCategoryChosen_003Em__4EF))
			{
				viewedId = null;
			}
			viewedId = chosenId;
			if (viewedId == null)
			{
				CategoryNames cn;
				string text2 = TempGunOrHighestDPSGun(i, out cn);
				if (i == cn)
				{
					viewedId = text2;
				}
				else
				{
					viewedId = TemppOrHighestDPSGunInCategory((int)i);
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
			{
				if (trainingState > TrainingState.OnArmor && currentCategory == CategoryNames.ArmorCategory)
				{
					viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
					idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
				}
				else if (currentCategory == CategoryNames.SniperCategory)
				{
					viewedId = WeaponTags.HunterRifleTag;
					idToSet = WeaponTags.HunterRifleTag;
				}
			}
			if (InTrainingAfterNoviceArmorRemoved && currentCategory == CategoryNames.ArmorCategory)
			{
				viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
				idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
			}
		}
		else
		{
			switch (currentCategory)
			{
			case CategoryNames.HatsCategory:
			{
				_003CCategoryChosen_003Ec__AnonStorey32E _003CCategoryChosen_003Ec__AnonStorey32E = new _003CCategoryChosen_003Ec__AnonStorey32E();
				_003CCategoryChosen_003Ec__AnonStorey32E._003C_003Ef__this = this;
				string text3 = null;
				try
				{
					Dictionary<Wear.LeagueItemState, List<string>> dictionary = Wear.LeagueItems();
					text3 = dictionary[Wear.LeagueItemState.Open].Union(dictionary[Wear.LeagueItemState.Purchased]).OrderBy(_003CCategoryChosen_003Ec__AnonStorey32E._003C_003Em__4F0).FirstOrDefault();
				}
				catch (Exception ex)
				{
					Debug.LogError("CategoryChoosen: exception in getting firstLeagueItem: " + ex);
				}
				_003CCategoryChosen_003Ec__AnonStorey32E.fu = text3 ?? WeaponManager.FirstUnboughtTag("hat_Headphones");
				int num3 = hats.FindIndex(_003CCategoryChosen_003Ec__AnonStorey32E._003C_003Em__4F1);
				viewedId = ((_CurrentEquippedWear == null || _CurrentNoneEquipped == null || _CurrentEquippedWear.Equals(_CurrentNoneEquipped) || WeaponManager.LastBoughtTag(_CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(_CurrentEquippedWear).Equals(_CurrentEquippedWear)) ? hats[(num3 <= -1) ? (hats.Count - 1) : num3].name : _CurrentEquippedWear);
				break;
			}
			case CategoryNames.ArmorCategory:
				viewedId = ((_CurrentEquippedWear == null || _CurrentNoneEquipped == null || _CurrentEquippedWear.Equals(_CurrentNoneEquipped) || WeaponManager.LastBoughtTag(_CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(_CurrentEquippedWear).Equals(_CurrentEquippedWear)) ? WeaponManager.FirstUnboughtTag(Wear.wear[CategoryNames.ArmorCategory][0][0]) : _CurrentEquippedWear);
				scrollViewPanel.transform.localPosition = Vector3.zero;
				scrollViewPanel.clipOffset = new Vector2(0f, 0f);
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
					idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
				}
				if (InTrainingAfterNoviceArmorRemoved)
				{
					viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
					idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
				}
				break;
			case CategoryNames.SkinsCategory:
				if (SkinsController.currentSkinNameForPers != null)
				{
					viewedId = SkinsController.currentSkinNameForPers;
				}
				else
				{
					if (SkinsController.skinsForPers == null || SkinsController.skinsForPers.Keys.Count <= 0)
					{
						break;
					}
					using (Dictionary<string, Texture2D>.KeyCollection.Enumerator enumerator2 = SkinsController.skinsForPers.Keys.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							string text4 = (viewedId = enumerator2.Current);
						}
					}
				}
				break;
			case CategoryNames.CapesCategory:
				viewedId = ((_CurrentEquippedWear == null || _CurrentNoneEquipped == null || _CurrentEquippedWear.Equals(_CurrentNoneEquipped) || WeaponManager.LastBoughtTag(_CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(_CurrentEquippedWear).Equals(_CurrentEquippedWear)) ? (WeaponManager.LastBoughtTag(capes[1].name) ?? capes[1].name) : _CurrentEquippedWear);
				break;
			case CategoryNames.BootsCategory:
				viewedId = ((_CurrentEquippedWear == null || _CurrentNoneEquipped == null || _CurrentEquippedWear.Equals(_CurrentNoneEquipped) || WeaponManager.LastBoughtTag(_CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(_CurrentEquippedWear).Equals(_CurrentEquippedWear)) ? (WeaponManager.LastBoughtTag(boots[0].name) ?? boots[0].name) : _CurrentEquippedWear);
				break;
			case CategoryNames.MaskCategory:
				viewedId = ((_CurrentEquippedWear == null || _CurrentNoneEquipped == null || _CurrentEquippedWear.Equals(_CurrentNoneEquipped) || WeaponManager.LastBoughtTag(_CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(_CurrentEquippedWear).Equals(_CurrentEquippedWear)) ? (WeaponManager.LastBoughtTag(masks[0].name) ?? masks[0].name) : _CurrentEquippedWear);
				break;
			case CategoryNames.GearCategory:
			{
				int num = GearManager.CurrentNumberOfUphradesForGear(GearManager.InvisibilityPotion);
				int num2 = ((num >= GearManager.NumOfGearUpgrades) ? num : num);
				viewedId = GearManager.NameForUpgrade(GearManager.InvisibilityPotion, num2);
				break;
			}
			}
		}
		armorWearProperties.SetActive(currentCategory == CategoryNames.ArmorCategory);
		nonArmorWearProperties.SetActive(IsWearCategory(currentCategory) && currentCategory != CategoryNames.ArmorCategory);
		gearProperties.SetActive(currentCategory == CategoryNames.GearCategory);
		skinProperties.SetActive(currentCategory == CategoryNames.SkinsCategory);
		border.SetActive(currentCategory != CategoryNames.SkinsCategory);
		ReloadCarousel(idToSet);
		SetCamera();
		if (!IsWeaponCategory(i) && weapon == null)
		{
			SetWeapon(_CurrentWeaponSetIDs()[0] ?? WeaponManager._initialWeaponName);
		}
		if (currentCategory == CategoryNames.SkinsCategory)
		{
			shopCarouselCollider.center = new Vector3(0f, -40f, 0f);
			shopCarouselCollider.size = new Vector3(shopCarouselCollider.size.x, 363f, shopCarouselCollider.size.z);
		}
		else
		{
			shopCarouselCollider.center = new Vector3(0f, 0f, 0f);
			shopCarouselCollider.size = new Vector3(shopCarouselCollider.size.x, 252f, shopCarouselCollider.size.z);
		}
	}

	private void HandleCarouselCentering()
	{
		HandleCarouselCentering(carouselCenter.centeredObject);
	}

	private void HandleCarouselCentering(GameObject centeredObj)
	{
		if (centeredObj != null && centeredObj != _lastSelectedItem)
		{
			_lastSelectedItem = centeredObj;
			if (highlightedCarouselObject != null)
			{
			}
			highlightedCarouselObject = centeredObj.transform;
			if (highlightedCarouselObject != null)
			{
			}
			ShopCarouselElement component = centeredObj.GetComponent<ShopCarouselElement>();
			ChooseCarouselItem(component.itemID);
		}
		if (EnableConfigurePos && centeredObj != null)
		{
			centeredObj.GetComponent<ShopCarouselElement>().SetPos(1f, 0f);
		}
	}

	private void CheckCenterItemChanging()
	{
		if (!scrollViewPanel.cachedGameObject.activeInHierarchy)
		{
			return;
		}
		Transform cachedTransform = scrollViewPanel.cachedTransform;
		itemIndex = -1;
		int num = (int)wrapContent.cellWidth;
		int childCount = wrapContent.transform.childCount;
		if (cachedTransform.localPosition.x > 0f)
		{
			itemIndex = 0;
		}
		else if (cachedTransform.localPosition.x < (float)(-1 * num * childCount))
		{
			itemIndex = childCount - 1;
		}
		else
		{
			itemIndex = -1 * Mathf.RoundToInt((cachedTransform.localPosition.x - (float)(Mathf.CeilToInt(cachedTransform.localPosition.x / (float)num / (float)childCount) * num * childCount)) / (float)num);
		}
		itemIndex = Mathf.Clamp(itemIndex, 0, childCount - 1);
		if (itemIndex >= 0 && itemIndex < wrapContent.transform.childCount)
		{
			GameObject centeredObj = wrapContent.transform.GetChild(itemIndex).gameObject;
			if (!EnableConfigurePos)
			{
				HandleCarouselCentering(centeredObj);
			}
		}
	}

	public static void SetPersArmorVisible(Transform armorPoint)
	{
		SetRenderersVisibleFromPoint(armorPoint, ShowArmor);
		if (armorPoint.childCount <= 0)
		{
			return;
		}
		Transform child = armorPoint.GetChild(0);
		ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null)
		{
			if (component.leftBone != null)
			{
				SetRenderersVisibleFromPoint(component.leftBone, ShowArmor);
			}
			if (component.rightBone != null)
			{
				SetRenderersVisibleFromPoint(component.rightBone, ShowArmor);
			}
		}
	}

	public static void SetPersHatVisible(Transform hatPoint)
	{
	}

	public static void SetRenderersVisibleFromPoint(Transform pt, bool showArmor)
	{
		_003CSetRenderersVisibleFromPoint_003Ec__AnonStorey32F _003CSetRenderersVisibleFromPoint_003Ec__AnonStorey32F = new _003CSetRenderersVisibleFromPoint_003Ec__AnonStorey32F();
		_003CSetRenderersVisibleFromPoint_003Ec__AnonStorey32F.showArmor = showArmor;
		Player_move_c.PerformActionRecurs(pt.gameObject, _003CSetRenderersVisibleFromPoint_003Ec__AnonStorey32F._003C_003Em__4F2);
	}

	private void Awake()
	{
		_003CAwake_003Ec__AnonStorey330 _003CAwake_003Ec__AnonStorey = new _003CAwake_003Ec__AnonStorey330();
		_003CAwake_003Ec__AnonStorey._003C_003Ef__this = this;
		showHatButton.gameObject.SetActive(false);
		_ShowArmor = PlayerPrefs.GetInt("ShowArmorKeySetting", 1) == 1;
		_ShowHat = PlayerPrefs.GetInt("ShowHatKeySetting", 1) == 1;
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		timeToUpdateTempGunTime = Time.realtimeSinceStartup;
		if (category != null)
		{
			category.Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4F3;
		}
		if (buy != null)
		{
			buy.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4F4;
		}
		if (buyGear != null)
		{
			buyGear.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4F5;
		}
		if (upgrade != null)
		{
			upgrade.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4F6;
		}
		if (upgradeGear != null)
		{
			upgradeGear.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4F7;
		}
		showArmorButton.IsChecked = !ShowArmor;
		showHatButton.IsChecked = !ShowHat;
		showArmorButtonTempArmor.IsChecked = !ShowArmor;
		showHatButtonTempHat.IsChecked = !ShowHat;
		_003CAwake_003Ec__AnonStorey.toggleShowArmor = _003CAwake_003Ec__AnonStorey._003C_003Em__4F8;
		_003CAwake_003Ec__AnonStorey.toggleShowHat = _003CAwake_003Ec__AnonStorey._003C_003Em__4F9;
		showArmorButton.Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4FA;
		showHatButton.Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4FB;
		showArmorButtonTempArmor.Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4FC;
		showHatButtonTempHat.Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4FD;
		sharedShop = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ActiveObject.SetActive(false);
		if (coinShopButton != null)
		{
			coinShopButton.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4FE;
		}
		if (backButton != null)
		{
			backButton.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__4FF;
		}
		_003CAwake_003Ec__AnonStorey332 _003CAwake_003Ec__AnonStorey2 = new _003CAwake_003Ec__AnonStorey332();
		_003CAwake_003Ec__AnonStorey2._003C_003Ef__this = this;
		UIButton[] array = equips;
		for (int i = 0; i < array.Length; i++)
		{
			_003CAwake_003Ec__AnonStorey2.ee = array[i];
			_003CAwake_003Ec__AnonStorey2.ee.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey2._003C_003Em__500;
		}
		unequip.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__501;
		enable.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__502;
		create.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__503;
		edit.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__504;
		delete.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Ec__AnonStorey._003C_003Em__505;
		hats.AddRange(Resources.LoadAll<ShopPositionParams>("Hats_Info"));
		sort(hats, CategoryNames.HatsCategory);
		armor.AddRange(Resources.LoadAll<ShopPositionParams>("Armor_Info"));
		sort(armor, CategoryNames.ArmorCategory);
		capes.AddRange(Resources.LoadAll<ShopPositionParams>("Capes_Info"));
		sort(capes, CategoryNames.CapesCategory);
		masks.AddRange(Resources.LoadAll<ShopPositionParams>("Masks_Info"));
		sort(masks, CategoryNames.MaskCategory);
		boots.AddRange(Resources.LoadAll<ShopPositionParams>("Shop_Boots_Info"));
		sort(boots, CategoryNames.BootsCategory);
		pixlMan = Resources.Load<GameObject>("PixlManForSkins");
		if (!Device.IsLoweMemoryDevice)
		{
			_onPersArmorRefs = Resources.LoadAll<GameObject>("Armor_Shop");
		}
		if (Device.isPixelGunLow)
		{
			_refOnLowPolyArmor = Resources.Load<GameObject>("Armor_Low");
			_refsOnLowPolyArmorMaterials = Resources.LoadAll<Material>("LowPolyArmorMaterials");
		}
	}

	public void goToSM()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("SkinEditorController"));
		SkinEditorController component = gameObject.GetComponent<SkinEditorController>();
		if (component != null)
		{
			_003CgoToSM_003Ec__AnonStorey336 _003CgoToSM_003Ec__AnonStorey = new _003CgoToSM_003Ec__AnonStorey336();
			_003CgoToSM_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CgoToSM_003Ec__AnonStorey.backHandler = null;
			_003CgoToSM_003Ec__AnonStorey.backHandler = _003CgoToSM_003Ec__AnonStorey._003C_003Em__506;
			SkinEditorController.ExitFromSkinEditor += _003CgoToSM_003Ec__AnonStorey.backHandler;
			SkinEditorController.currentSkinName = ((!viewedId.Equals("CustomSkinID")) ? viewedId : null);
			SkinEditorController.modeEditor = ((currentCategory != CategoryNames.SkinsCategory) ? SkinEditorController.ModeEditor.Cape : SkinEditorController.ModeEditor.SkinPers);
			mainPanel.SetActive(false);
		}
	}

	public IEnumerator ReloadAfterEditing(string n, bool shouldReload = true)
	{
		yield return null;
		if (shouldReload)
		{
			ReloadCarousel(n ?? "cape_Custom");
		}
		PlayWeaponAnimation();
		UpdateIcon(currentCategory);
	}

	private static List<Camera> BankRelatedCameras()
	{
		List<Camera> list = BankController.Instance.GetComponentsInChildren<Camera>(true).ToList();
		if (FreeAwardController.Instance != null && FreeAwardController.Instance.renderCamera != null)
		{
			list.Add(FreeAwardController.Instance.renderCamera);
		}
		return list;
	}

	private static void SetBankCamerasEnabled()
	{
		List<Camera> list = BankRelatedCameras();
		foreach (Camera item in list)
		{
			if ((!(ExpController.Instance != null) || !ExpController.Instance.IsRenderedWithCamera(item)) && !item.gameObject.tag.Equals("CamTemp") && !sharedShop.ourCameras.Contains(item))
			{
				item.rect = new Rect(0f, 0f, 1f, 1f);
			}
		}
	}

	private void BuyOrUpgradeWeapon(bool upgradeNotBuy = false)
	{
		_003CBuyOrUpgradeWeapon_003Ec__AnonStorey337 _003CBuyOrUpgradeWeapon_003Ec__AnonStorey = new _003CBuyOrUpgradeWeapon_003Ec__AnonStorey337();
		_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.upgradeNotBuy = upgradeNotBuy;
		_003CBuyOrUpgradeWeapon_003Ec__AnonStorey._003C_003Ef__this = this;
		_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id = ((currentCategory != CategoryNames.GearCategory) ? viewedId : ((!_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.upgradeNotBuy) ? IDForCurrentGear : NextUpgradeIDForCurrentGear));
		_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.tg = _003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.tg))
		{
			_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id = WeaponManager.tagToStoreIDMapping[WeaponManager.FirstUnboughtOrForOurTier(_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.tg)];
		}
		if (WearCategory)
		{
			_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id = WeaponManager.FirstUnboughtTag(_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id);
			_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.tg = _003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id;
		}
		if (_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.id != null)
		{
			_003CBuyOrUpgradeWeapon_003Ec__AnonStorey.price = currentPrice(viewedId, currentCategory, _003CBuyOrUpgradeWeapon_003Ec__AnonStorey.upgradeNotBuy);
			GameObject obj = mainPanel;
			ItemPrice itemPrice = _003CBuyOrUpgradeWeapon_003Ec__AnonStorey.price;
			Action onSuccess = _003CBuyOrUpgradeWeapon_003Ec__AnonStorey._003C_003Em__507;
			Action onFailure = _003CBuyOrUpgradeWeapon_003Ec__AnonStorey._003C_003Em__508;
			Action onReturnFromBank = _003CBuyOrUpgradeWeapon_003Ec__AnonStorey._003C_003Em__509;
			if (_003C_003Ef__am_0024cacheCA == null)
			{
				_003C_003Ef__am_0024cacheCA = _003CBuyOrUpgradeWeapon_003Em__50A;
			}
			TryToBuy(obj, itemPrice, onSuccess, onFailure, null, onReturnFromBank, _003C_003Ef__am_0024cacheCA, _003CBuyOrUpgradeWeapon_003Ec__AnonStorey._003C_003Em__50B);
		}
	}

	public static void ProvideShopItemOnStarterPackBoguht(CategoryNames c, string sourceTg, int gearCount = 1, bool buyArmorUpToSourceTg = false, int timeForRentIndex = 0, Action<string> contextSpecificAction = null, Action<string> customEquipWearAction = null, bool equipSkin = true, bool equipWear = true, bool doAndroidCloudSync = true)
	{
		_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey338 _003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey = new _003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey338();
		_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey.customEquipWearAction = customEquipWearAction;
		_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey.equipWear = equipWear;
		_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey.c = c;
		_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey.equipSkin = equipSkin;
		string text = ((_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey.c != CategoryNames.GearCategory) ? sourceTg : GearManager.HolderQuantityForID(sourceTg));
		string text2 = text;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(text2))
		{
			text = WeaponManager.tagToStoreIDMapping[text2];
		}
		if (text != null)
		{
			ProvdeShopItemWithRightId(_003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey.c, text, text2, null, _003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey._003C_003Em__50C, contextSpecificAction, _003CProvideShopItemOnStarterPackBoguht_003Ec__AnonStorey._003C_003Em__50D, true, gearCount, buyArmorUpToSourceTg, timeForRentIndex, doAndroidCloudSync);
		}
	}

	public static void ProvideAllTypeShopItem(CategoryNames category, string sourceTag, int gearCount, int timeForRent)
	{
		int num = 0;
		if (timeForRent != -1)
		{
			int days = timeForRent / 24;
			num = TempItemsController.RentIndexFromDays(days);
		}
		int timeForRentIndex = num;
		if (_003C_003Ef__am_0024cacheCB == null)
		{
			_003C_003Ef__am_0024cacheCB = _003CProvideAllTypeShopItem_003Em__50E;
		}
		ProvideShopItemOnStarterPackBoguht(category, sourceTag, gearCount, false, timeForRentIndex, null, _003C_003Ef__am_0024cacheCB);
		TempItemsController.sharedController.ExpiredItems.Remove(sourceTag);
	}

	private static int AddedNumberOfGearWhenBuyingPack(string id)
	{
		int num = GearManager.ItemsInPackForGear(GearManager.HolderQuantityForID(id));
		if (Storager.getInt(id, false) + num > GearManager.MaxCountForGear(id))
		{
			num = GearManager.MaxCountForGear(id) - Storager.getInt(id, false);
		}
		return num;
	}

	private static void ProvdeShopItemWithRightId(CategoryNames c, string id, string tg, Action UNUSED_DO_NOT_SET_onTrainingAction, Action<string> onEquipWearAction, Action<string> contextSpecificAction, Action<string> onSkinBoughtAction, bool giveOneItemOfGear = false, int gearCount = 1, bool buyArmorAndHatsUpToTg = false, int timeForRentIndex = 0, bool doAndroidCloudSync = true)
	{
		if (ShopNGUIController.GunBought != null)
		{
			ShopNGUIController.GunBought();
		}
		if (IsWearCategory(c))
		{
			if (buyArmorAndHatsUpToTg && Wear.wear.ContainsKey(c))
			{
				List<List<string>> list = Wear.wear[c];
				List<string> list2 = null;
				foreach (List<string> item in list)
				{
					if (item.Contains(tg))
					{
						list2 = item;
						break;
					}
				}
				if (list2 != null)
				{
					for (int i = 0; i < list2.Count; i++)
					{
						Storager.setInt(list2[i], 1, true);
						if (list2[i].Equals(tg))
						{
							break;
						}
					}
				}
			}
			else if (TempItemsController.PriceCoefs.ContainsKey(tg))
			{
				int tm = TempItemsController.RentTimeForIndex(timeForRentIndex);
				TempItemsController.sharedController.AddTemporaryItem(tg, tm);
			}
			else
			{
				Storager.setInt(tg, 1, true);
			}
			if ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && doAndroidCloudSync)
			{
				SynchronizeAndroidPurchases("Wear: " + tg);
			}
			if (onEquipWearAction != null)
			{
				onEquipWearAction(tg);
			}
		}
		if (IsWeaponCategory(c) && !WeaponManager.FirstUnboughtTag(tg).Equals(tg))
		{
			List<string> list3 = WeaponUpgrades.ChainForTag(tg);
			if (list3 != null)
			{
				int num = list3.IndexOf(tg) - 1;
				if (num >= 0)
				{
					for (int j = 0; j <= num; j++)
					{
						try
						{
							Storager.setInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list3[j]]], 1, true);
						}
						catch
						{
							Debug.LogError("Error filling chain in indexOfWeaponBeforeCurrentTg");
						}
					}
				}
			}
		}
		WeaponManager.sharedManager.AddMinerWeapon(id, timeForRentIndex);
		if (WeaponManager.sharedManager != null)
		{
			try
			{
				bool flag = WeaponManager.sharedManager.IsAvailableTryGun(WeaponManager.LastBoughtTag(tg));
				bool flag2 = WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(WeaponManager.LastBoughtTag(tg));
				WeaponManager.RemoveGunFromAllTryGunRelated(tg);
				if (flag2)
				{
					string empty = string.Empty;
					string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(tg), empty, c);
					AnalyticsStuff.LogWEaponsSpecialOffers_Conversion(false, itemNameNonLocalized);
				}
				if (flag2 || flag)
				{
					Action<string> tryGunBought = ShopNGUIController.TryGunBought;
					if (tryGunBought != null)
					{
						tryGunBought(WeaponManager.LastBoughtTag(tg));
					}
					if (FriendsController.useBuffSystem)
					{
						BuffSystem.instance.OnTryGunBuyed(ItemDb.GetByTag(tg).PrefabName);
					}
					else
					{
						KillRateCheck.OnTryGunBuyed();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in removeing TryGun structures: " + ex);
			}
		}
		if (c == CategoryNames.GearCategory)
		{
			if (id.Contains(GearManager.UpgradeSuffix))
			{
				string key = GearManager.NameForUpgrade(GearManager.HolderQuantityForID(id), GearManager.CurrentNumberOfUphradesForGear(GearManager.HolderQuantityForID(id)) + 1);
				Storager.setInt(key, 1, false);
			}
			else
			{
				int num2 = AddedNumberOfGearWhenBuyingPack(id);
				Storager.setInt(id, Storager.getInt(id, false) + ((!giveOneItemOfGear) ? num2 : gearCount), false);
			}
		}
		if (contextSpecificAction != null)
		{
			contextSpecificAction(id);
		}
		if (c != CategoryNames.SkinsCategory)
		{
			return;
		}
		if (id != null && SkinsController.shopKeyFromNameSkin.ContainsKey(id))
		{
			string text = SkinsController.shopKeyFromNameSkin[id];
			if (Array.IndexOf(StoreKitEventListener.skinIDs, text) >= 0)
			{
				foreach (KeyValuePair<string, string> value in InAppData.inAppData.Values)
				{
					if (value.Key != null && value.Key.Equals(text))
					{
						Storager.setInt(value.Value, 1, true);
						if (doAndroidCloudSync)
						{
							SynchronizeAndroidPurchases("Skin: " + text);
						}
						break;
					}
				}
			}
		}
		if (onSkinBoughtAction != null)
		{
			onSkinBoughtAction(id);
		}
	}

	public void FireBuyAction(string item)
	{
		if (buyAction != null)
		{
			buyAction(item);
		}
	}

	private void LogShopPurchasesTotalAndPayingNonPaying(string itemName)
	{
		try
		{
			string text = currentCategory.ToString();
			string eventName = string.Format("Shop Purchases {0}", "Total");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("All Categories", text);
			dictionary.Add(text, itemName);
			dictionary.Add("Item", itemName);
			Dictionary<string, string> dictionary2 = dictionary;
			if (currentCategory != CategoryNames.GearCategory)
			{
				dictionary2.Add("Without Quick Shop", itemName);
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, dictionary2);
			string payingSuffix = FlurryPluginWrapper.GetPayingSuffix();
			string eventName2 = string.Format("Shop Purchases {0}", "Total" + payingSuffix);
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName2, dictionary2);
		}
		catch (Exception ex)
		{
			Debug.LogError("LogShopPurchasesTotalAndPayingNonPaying exception: " + ex);
		}
	}

	private void ActualBuy(string id, string tg, ItemPrice itemPrice)
	{
		_003CActualBuy_003Ec__AnonStorey339 _003CActualBuy_003Ec__AnonStorey = new _003CActualBuy_003Ec__AnonStorey339();
		_003CActualBuy_003Ec__AnonStorey.id = id;
		_003CActualBuy_003Ec__AnonStorey._003C_003Ef__this = this;
		if (currentCategory == CategoryNames.ArmorCategory || IsWeaponCategory(currentCategory))
		{
			FireWeaponOrArmorBought();
		}
		_003CActualBuy_003Ec__AnonStorey.c = currentCategory;
		int num = AddedNumberOfGearWhenBuyingPack(_003CActualBuy_003Ec__AnonStorey.id);
		ProvdeShopItemWithRightId(_003CActualBuy_003Ec__AnonStorey.c, _003CActualBuy_003Ec__AnonStorey.id, tg, null, _003CActualBuy_003Ec__AnonStorey._003C_003Em__50F, _003CActualBuy_003Ec__AnonStorey._003C_003Em__510, _003CActualBuy_003Ec__AnonStorey._003C_003Em__511);
		if (WeaponManager.tagToStoreIDMapping.ContainsValue(_003CActualBuy_003Ec__AnonStorey.id))
		{
			IEnumerable<KeyValuePair<string, string>> source = WeaponManager.tagToStoreIDMapping.Where(_003CActualBuy_003Ec__AnonStorey._003C_003Em__512);
			if (_003C_003Ef__am_0024cacheCC == null)
			{
				_003C_003Ef__am_0024cacheCC = _003CActualBuy_003Em__513;
			}
			IEnumerable<string> source2 = source.Select(_003C_003Ef__am_0024cacheCC);
			SynchronizeAndroidPurchases("Weapon: " + source2.FirstOrDefault());
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(_003CActualBuy_003Ec__AnonStorey.id);
			int? num2 = null;
			if (priceByShopId != null)
			{
				num2 = priceByShopId.Price;
			}
			if (num2.HasValue && num2.Value >= PlayerPrefs.GetInt(Defs.MostExpensiveWeapon, 0))
			{
				PlayerPrefs.SetInt(Defs.MostExpensiveWeapon, num2.Value);
				PlayerPrefs.SetString(Defs.MenuPersWeaponTag, (source2.Count() <= 0) ? string.Empty : source2.ElementAt(0));
				PlayerPrefs.Save();
			}
		}
		string text = _003CActualBuy_003Ec__AnonStorey.id;
		try
		{
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(_003CActualBuy_003Ec__AnonStorey.id))
			{
				text = WeaponManager.tagToStoreIDMapping[_003CActualBuy_003Ec__AnonStorey.id];
			}
			if (currentCategory == CategoryNames.SkinsCategory && text != null && SkinsController.shopKeyFromNameSkin.ContainsKey(text))
			{
				text = SkinsController.shopKeyFromNameSkin[text];
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in setting shopId: " + ex);
		}
		try
		{
			string text2 = WeaponManager.LastBoughtTag(viewedId) ?? WeaponManager.FirstUnboughtTag(viewedId);
			string text3 = ItemDb.GetItemNameNonLocalized(text2, text, currentCategory);
			try
			{
				if (currentCategory == CategoryNames.SkinsCategory)
				{
					text3 = LocalizationStore.GetByDefault(SkinsController.skinsLocalizeKey[int.Parse(_003CActualBuy_003Ec__AnonStorey.id)]);
				}
			}
			catch (Exception ex2)
			{
				Debug.LogError("Shop: ActualBuy: get readable skin name: " + ex2);
			}
			FlurryPluginWrapper.LogPurchaseByModes(currentCategory, (currentCategory != CategoryNames.GearCategory) ? text3 : GearManager.HolderQuantityForID(text), (currentCategory != CategoryNames.GearCategory) ? 1 : num, false);
			if (currentCategory != CategoryNames.GearCategory)
			{
				FlurryPluginWrapper.LogPurchasesPoints(IsWeaponCategory(currentCategory));
				FlurryPluginWrapper.LogPurchaseByPoints(currentCategory, text3, 1);
			}
			else
			{
				FlurryPluginWrapper.LogGearPurchases(GearManager.HolderQuantityForID(text), num, false);
				if (Application.loadedLevelName == Defs.MainMenuScene)
				{
					FlurryPluginWrapper.LogPurchaseByPoints(currentCategory, GearManager.HolderQuantityForID(text), num);
				}
			}
			try
			{
				bool isDaterWeapon = false;
				if (IsWeaponCategory(currentCategory))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(text2);
					isDaterWeapon = weaponInfo != null && weaponInfo.IsAvalibleFromFilter(3);
				}
				string text4 = ((!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(currentCategory)) ? currentCategory.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[currentCategory]);
				AnalyticsStuff.LogSales(text3, text4, isDaterWeapon);
				AnalyticsFacade.InAppPurchase(text3, text4, 1, itemPrice.Price, itemPrice.Currency);
				if (_isFromPromoActions && _promoActionsIdClicked != null && text2 != null && _promoActionsIdClicked == text2)
				{
					AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "Buy", text4 ?? "Unknown", text3);
				}
				_isFromPromoActions = false;
			}
			catch (Exception ex3)
			{
				Debug.LogError("Exception in LogSales block in Shop: " + ex3);
			}
			bool onlyServerDiscount;
			int num3 = DiscountFor(WeaponManager.LastBoughtTag(viewedId) ?? WeaponManager.FirstUnboughtTag(viewedId), out onlyServerDiscount);
			if (num3 > 0)
			{
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(viewedId) ?? WeaponManager.FirstUnboughtTag(viewedId), text, currentCategory, "Unknown");
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add(num3.ToString(), itemNameNonLocalized);
				Dictionary<string, string> parameters = dictionary;
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Offers Sale", parameters);
			}
			if (currentCategory == CategoryNames.GearCategory && text3 != null && !text3.Contains(GearManager.UpgradeSuffix) && GearManager.AllGear.Contains(text3))
			{
				text3 = GearManager.AnalyticsIDForOneItemOfGear(text3);
			}
			LogShopPurchasesTotalAndPayingNonPaying(text3);
			if (ExperienceController.sharedController != null)
			{
				int currentLevel = ExperienceController.sharedController.currentLevel;
				int num4 = (currentLevel - 1) / 9;
				string arg = string.Format("[{0}, {1})", num4 * 9 + 1, (num4 + 1) * 9 + 1);
				string eventName = string.Format("Shop Purchases On Level {0} ({1}){2}", arg, FlurryPluginWrapper.GetPayingSuffix().Trim(), string.Empty);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Level " + currentLevel, text3);
				Dictionary<string, string> parameters2 = dictionary;
				FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, parameters2);
			}
			LogPurchaseAfterPaymentAnalyticsEvent(text3);
		}
		catch (Exception ex4)
		{
			Debug.LogError("Exception in Shop Logging: " + ex4);
		}
		chosenId = WeaponManager.LastBoughtTag(viewedId);
		viewedId = ((currentCategory != CategoryNames.GearCategory) ? chosenId : GearManager.NameForUpgrade(GearManager.HolderQuantityForID(viewedId), GearManager.CurrentNumberOfUphradesForGear(GearManager.HolderQuantityForID(viewedId))));
		UpdateIcon(currentCategory, true);
		ReloadCarousel();
		ChooseCarouselItem(viewedId, false, true);
		Resources.UnloadUnusedAssets();
		if (!inGame && currentCategory == CategoryNames.CapesCategory && viewedId.Equals("cape_Custom"))
		{
			FlurryPluginWrapper.LogEvent("Enable_Custom Cape");
			wholePrice.gameObject.SetActive(false);
			goToSM();
		}
	}

	private static void SaveSkinAndSendToServer(string id)
	{
		SkinsController.SetCurrentSkin(id);
		byte[] array = SkinsController.currentSkinForPers.EncodeToPNG();
		if (array != null)
		{
			string text = Convert.ToBase64String(array);
			if (text != null)
			{
				FriendsController.sharedController.skin = text;
				FriendsController.sharedController.SendOurData(true);
			}
		}
	}

	private void FireOnEquipSkin(string id)
	{
		if (onEquipSkinAction != null)
		{
			onEquipSkinAction(id);
		}
	}

	public void SetSkinAsCurrent(string id)
	{
		SaveSkinAndSendToServer(id);
		FireOnEquipSkin(id);
	}

	public static void SetAsEquippedAndSendToServer(string tg, CategoryNames c)
	{
		Storager.setString(SnForWearCategory(c), tg, false);
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("FriendsController.sharedController == null");
		}
		else
		{
			FriendsController.sharedController.SendAccessories();
		}
	}

	public IEnumerator BackAfterDelay()
	{
		_isFromPromoActions = false;
		yield return null;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShopCompleted;
			HintController.instance.StartShow();
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Back_Shop);
		}
		if (resumeAction != null)
		{
			resumeAction();
		}
		else
		{
			GuiActive = false;
		}
		if (wearResumeAction != null)
		{
			wearResumeAction();
		}
		if (InTrainingAfterNoviceArmorRemoved)
		{
			trainingColliders.SetActive(false);
			trainingRemoveNoviceArmorCollider.SetActive(false);
		}
		InTrainingAfterNoviceArmorRemoved = false;
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
	}

	public static string SnForWearCategory(CategoryNames c)
	{
		object result;
		switch (c)
		{
		case CategoryNames.CapesCategory:
			result = Defs.CapeEquppedSN;
			break;
		case CategoryNames.BootsCategory:
			result = Defs.BootsEquppedSN;
			break;
		case CategoryNames.ArmorCategory:
			result = Defs.ArmorNewEquppedSN;
			break;
		case CategoryNames.MaskCategory:
			result = "MaskEquippedSN";
			break;
		default:
			result = Defs.HatEquppedSN;
			break;
		}
		return (string)result;
	}

	public static string NoneEquippedForWearCategory(CategoryNames c)
	{
		object result;
		switch (c)
		{
		case CategoryNames.CapesCategory:
			result = Defs.CapeNoneEqupped;
			break;
		case CategoryNames.BootsCategory:
			result = Defs.BootsNoneEqupped;
			break;
		case CategoryNames.ArmorCategory:
			result = Defs.ArmorNewNoneEqupped;
			break;
		case CategoryNames.MaskCategory:
			result = "MaskNoneEquipped";
			break;
		default:
			result = Defs.HatNoneEqupped;
			break;
		}
		return (string)result;
	}

	public string WearForCat(CategoryNames c)
	{
		string result;
		switch (c)
		{
		case CategoryNames.CapesCategory:
			result = _currentCape;
			break;
		case CategoryNames.BootsCategory:
			result = _currentBoots;
			break;
		case CategoryNames.ArmorCategory:
			result = _currentArmor;
			break;
		case CategoryNames.HatsCategory:
			result = _currentHat;
			break;
		case CategoryNames.MaskCategory:
			result = _currentMask;
			break;
		default:
			result = string.Empty;
			break;
		}
		return result;
	}

	private void SetWearForCategory(CategoryNames cat, string wear)
	{
		switch (cat)
		{
		case CategoryNames.CapesCategory:
			_currentCape = wear;
			break;
		case CategoryNames.HatsCategory:
			_currentHat = wear;
			break;
		case CategoryNames.BootsCategory:
			_currentBoots = wear;
			break;
		case CategoryNames.ArmorCategory:
			_currentArmor = wear;
			break;
		case CategoryNames.MaskCategory:
			_currentMask = wear;
			break;
		case CategoryNames.SkinsCategory:
		case CategoryNames.GearCategory:
			break;
		}
	}

	public void LoadCurrentWearToVars()
	{
		_currentCape = Storager.getString(Defs.CapeEquppedSN, false);
		_currentHat = Storager.getString(Defs.HatEquppedSN, false);
		_currentBoots = Storager.getString(Defs.BootsEquppedSN, false);
		_currentArmor = Storager.getString(Defs.ArmorNewEquppedSN, false);
		_currentMask = Storager.getString("MaskEquippedSN", false);
	}

	private void HandleActionsUUpdated()
	{
		UpdateButtons();
		UpdateItemParameters();
	}

	private void OnDestroy()
	{
		if (profile != null)
		{
			Resources.UnloadAsset(profile);
			profile = null;
		}
	}

	private void Start()
	{
		StartCoroutine(TryToShowExpiredBanner());
	}

	private IEnumerator TryToShowExpiredBanner()
	{
		while (FriendsController.sharedController == null || TempItemsController.sharedController == null)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			try
			{
				if (!GuiActive || rentScreenPoint.childCount != 0)
				{
					continue;
				}
				if (Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1)
				{
					GameObject window = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/WeRemoveNoviceArmorBanner"));
					window.transform.parent = rentScreenPoint;
					Player_move_c.SetLayerRecursively(window, LayerMask.NameToLayer("NGUIShop"));
					window.transform.localPosition = new Vector3(0f, 0f, -130f);
					window.transform.localRotation = Quaternion.identity;
					window.transform.localScale = new Vector3(1f, 1f, 1f);
					UpdatePersArmor(Defs.ArmorNewNoneEqupped);
					sharedShop.trainingColliders.SetActive(true);
					sharedShop.trainingRemoveNoviceArmorCollider.SetActive(true);
					trainingStateRemoveNoviceArmor = TrainingState.NotInArmorCategory;
					InTrainingAfterNoviceArmorRemoved = true;
					if (HintController.instance != null)
					{
						HintController.instance.HideHintByName("shop_remove_novice_armor");
					}
				}
				else if ((_shouldShowRewardWindowSkin || _shouldShowRewardWindowCape) && !Device.isPixelGunLow)
				{
					PlayerPrefs.SetInt((!_shouldShowRewardWindowSkin) ? Defs.ShownRewardWindowForCape : Defs.ShownRewardWindowForSkin, 1);
					if (FacebookController.FacebookSupported || TwitterController.TwitterSupported)
					{
						_isFromPromoActions = false;
						GameObject window2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/CreateNewItemNGUI"));
						RewardWindowBase rwb = window2.GetComponent<RewardWindowBase>();
						bool skin = _shouldShowRewardWindowSkin;
						FacebookController.StoryPriority priority = (rwb.priority = FacebookController.StoryPriority.Green);
						rwb.shareAction = ((_003CTryToShowExpiredBanner_003Ec__Iterator1AB)(object)this)._003C_003Em__529;
						rwb.HasReward = false;
						if (skin)
						{
							if (_003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheB == null)
							{
								_003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheB = _003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Em__52A;
							}
							rwb.twitterStatus = _003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheB;
							rwb.EventTitle = "Painted Skin";
						}
						else
						{
							if (_003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheC == null)
							{
								_003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheC = _003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Em__52B;
							}
							rwb.twitterStatus = _003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheC;
							rwb.EventTitle = "Painted Cape";
						}
						window2.transform.parent = rentScreenPoint;
						Player_move_c.SetLayerRecursively(window2, LayerMask.NameToLayer("NGUIShop"));
						window2.transform.localPosition = new Vector3(0f, 0f, -130f);
						window2.transform.localRotation = Quaternion.identity;
						window2.transform.localScale = new Vector3(1f, 1f, 1f);
						DrawItemRewardWindow dirw = window2.GetComponent<DrawItemRewardWindow>();
						dirw.skin.SetActive(_shouldShowRewardWindowSkin);
						dirw.cape.SetActive(!_shouldShowRewardWindowSkin);
					}
					if (_shouldShowRewardWindowSkin)
					{
						_shouldShowRewardWindowSkin = false;
					}
					else if (_shouldShowRewardWindowCape)
					{
						_shouldShowRewardWindowCape = false;
					}
				}
				else if (Storager.getInt(Defs.PremiumEnabledFromServer, false) != 1 || !(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.myPlayerMoveC == null) || !ShowPremimAccountExpiredIfPossible(rentScreenPoint, "NGUIShop", string.Empty))
				{
					Transform point = rentScreenPoint;
					Action<string> onPurchase = ((_003CTryToShowExpiredBanner_003Ec__Iterator1AB)(object)this)._003C_003Em__52C;
					if (_003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheD == null)
					{
						_003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheD = _003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Em__52D;
					}
					ShowTempItemExpiredIfPossible(point, "NGUIShop", onPurchase, _003CTryToShowExpiredBanner_003Ec__Iterator1AB._003C_003Ef__am_0024cacheD, ((_003CTryToShowExpiredBanner_003Ec__Iterator1AB)(object)this)._003C_003Em__52E, ((_003CTryToShowExpiredBanner_003Ec__Iterator1AB)(object)this)._003C_003Em__52F);
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning("exception in Shop  TryToShowExpiredBanner: " + e);
			}
		}
	}

	private void Update()
	{
		if (!ActiveObject.activeInHierarchy)
		{
			return;
		}
		ExperienceController.sharedController.isShowRanks = rentScreenPoint.childCount == 0 && SkinEditorController.sharedController == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled);
		if (Time.realtimeSinceStartup - timeToUpdateTempGunTime >= 1f)
		{
			timeToUpdateTempGunTime = Time.realtimeSinceStartup;
			if (GuiActive && viewedId != null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(viewedId))
			{
				UpdateTryGunDiscountTime();
			}
		}
		string showHatTag = ((hatPoint.transform.childCount > 1) ? hatPoint.transform.GetChild(1).gameObject.tag : ((hatPoint.transform.childCount <= 0) ? "none" : hatPoint.transform.GetChild(0).gameObject.tag));
		bool flag = currentCategory == CategoryNames.HatsCategory && !Wear.NonArmorHat(showHatTag);
		bool flag2 = currentCategory == CategoryNames.ArmorCategory && viewedId != null && !TempItemsController.PriceCoefs.ContainsKey(viewedId);
		if (showArmorButton.gameObject.activeSelf != flag2)
		{
			showArmorButton.gameObject.SetActive(flag2);
		}
		bool flag3 = false;
		if (showHatButton.gameObject.activeSelf != flag3)
		{
			showHatButton.gameObject.SetActive(flag3);
		}
		bool flag4 = currentCategory == CategoryNames.ArmorCategory && viewedId != null && TempItemsController.PriceCoefs.ContainsKey(viewedId);
		if (showArmorButtonTempArmor.gameObject.activeSelf != flag4)
		{
			showArmorButtonTempArmor.gameObject.SetActive(flag4);
		}
		bool flag5 = flag && viewedId != null && TempItemsController.PriceCoefs.ContainsKey(viewedId);
		if (showHatButtonTempHat.gameObject.activeSelf != flag5)
		{
			showHatButtonTempHat.gameObject.SetActive(flag5);
		}
		if (Time.realtimeSinceStartup - _timePurchaseSuccessfulShown >= 2f)
		{
			purchaseSuccessful.SetActive(false);
		}
		if (Time.realtimeSinceStartup - _timePurchaseRentSuccessfulShown >= 2f)
		{
			purchaseSuccessfulRent.SetActive(false);
		}
		if (mainPanel.activeInHierarchy && !HOTween.IsTweening(MainMenu_Pers))
		{
			float num = -120f;
			num *= ((BuildSettings.BuildTargetPlatform == RuntimePlatform.Android) ? 2f : ((!Application.isEditor) ? 0.5f : 100f));
			Rect rect = new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height);
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Moved && rect.Contains(touch.position))
				{
					idleTimerLastTime = Time.realtimeSinceStartup;
					MainMenu_Pers.Rotate(Vector3.up, touch.deltaPosition.x * num * 0.5f * (Time.realtimeSinceStartup - lastTime));
				}
			}
			if (Application.isEditor)
			{
				float num2 = Input.GetAxis("Mouse ScrollWheel") * 3f * num * (Time.realtimeSinceStartup - lastTime);
				MainMenu_Pers.Rotate(Vector3.up, num2);
				if (num2 != 0f)
				{
					idleTimerLastTime = Time.realtimeSinceStartup;
				}
			}
			lastTime = Time.realtimeSinceStartup;
		}
		if (currentCategory != CategoryNames.CapesCategory && Time.realtimeSinceStartup - idleTimerLastTime > IdleTimeoutPers)
		{
			SetCamera();
		}
		ActivityIndicator.IsActiveIndicator = StoreKitEventListener.restoreInProcess;
		CheckCenterItemChanging();
	}

	private void LateUpdate()
	{
		float num = scrollViewPanel.GetViewSize().x / 2f;
		ShopCarouselElement[] componentsInChildren = wrapContent.GetComponentsInChildren<ShopCarouselElement>(false);
		ShopCarouselElement[] array = componentsInChildren;
		foreach (ShopCarouselElement shopCarouselElement in array)
		{
			Transform transform = shopCarouselElement.transform;
			float x = scrollViewPanel.clipOffset.x;
			float num2 = Mathf.Abs(transform.localPosition.x - x);
			float num3 = scaleCoef + (1f - scaleCoef) * (1f - num2 / num);
			float num4 = 0.65f;
			num3 = ((!(num2 <= num / 3f)) ? (scaleCoef + (num4 - scaleCoef) * (1f - (num2 - num / 3f) / (num * 2f / 3f))) : (num4 + (1f - num4) * (1f - num2 / (num / 3f))));
			if (num2 >= num * 0.9f)
			{
				num3 = 0f;
			}
			float num5 = transform.localPosition.x - x;
			float num6 = 0f;
			float num7 = ((num5 <= 0f) ? 1 : (-1));
			if (num5 != 0f)
			{
				num6 = ((Mathf.Abs(num5) <= wrapContent.cellWidth) ? (firstOFfset * (Mathf.Abs(num5) / wrapContent.cellWidth)) : ((!(Mathf.Abs(num5) <= 2f * wrapContent.cellWidth)) ? (secondOffset * (1f - (Mathf.Abs(num5) - 2f * wrapContent.cellWidth) / wrapContent.cellWidth)) : (firstOFfset + (secondOffset - firstOFfset) * ((Mathf.Abs(num5) - wrapContent.cellWidth) / wrapContent.cellWidth))));
			}
			num6 *= num7;
			if (!EnableConfigurePos || scrollViewPanel.GetComponent<UIScrollView>().isDragging || scrollViewPanel.GetComponent<UIScrollView>().currentMomentum.x > 0f)
			{
				shopCarouselElement.SetPos(num3, num6);
			}
			shopCarouselElement.topSeller.gameObject.SetActive(shopCarouselElement.showTS && Mathf.Abs(num2) <= wrapContent.cellWidth / 10f);
			shopCarouselElement.newnew.gameObject.SetActive(shopCarouselElement.showNew && Mathf.Abs(num2) <= wrapContent.cellWidth / 10f);
			shopCarouselElement.quantity.gameObject.SetActive(shopCarouselElement.showQuantity && Mathf.Abs(num2) <= wrapContent.cellWidth / 10f);
		}
		if (_escapeRequested)
		{
			StartCoroutine(BackAfterDelay());
			_escapeRequested = false;
		}
	}

	private void HandleEscape()
	{
		if ((BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled) || (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled))
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			if (Application.isEditor)
			{
				Debug.Log("Ignoring [Escape] since Tutorial is not completed.");
			}
		}
		else if (InTrainingAfterNoviceArmorRemoved)
		{
			if (Application.isEditor)
			{
				Debug.Log("Ignoring [Escape] since Tutorial after removing Novice Armor is not completed.");
			}
		}
		else if (!GuiActive)
		{
			if (Application.isEditor)
			{
				Debug.Log(GetType().Name + ".LateUpdate():    Ignoring Escape because Shop GUI is not active.");
			}
		}
		else
		{
			_escapeRequested = true;
		}
	}

	public void SetInGame(bool e)
	{
		inGame = e;
	}

	private IEnumerator DisableStub()
	{
		for (int i = 0; i < 3; i++)
		{
			yield return null;
		}
		stub.SetActive(false);
	}

	public void MakeACtiveAfterDelay(string idToSet, CategoryNames cn)
	{
		Light[] array = UnityEngine.Object.FindObjectsOfType<Light>();
		if (array == null)
		{
			array = new Light[0];
		}
		Light[] array2 = array;
		foreach (Light light in array2)
		{
			if (!mylights.Contains(light))
			{
				light.cullingMask &= ~(1 << LayerMask.NameToLayer("NGUIShop"));
			}
		}
		sharedShop.ActiveObject.SetActive(true);
		wrapContent.Reposition();
		if (ExperienceController.sharedController != null && ExpController.Instance != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
			ExpController.Instance.InterfaceEnabled = true;
		}
		UpdatePersHat(_currentHat);
		UpdatePersCape(_currentCape);
		UpdatePersArmor(_currentArmor);
		UpdatePersBoots(_currentBoots);
		UpdatePersMask(_currentMask);
		UpdatePersSkin(SkinsController.currentSkinNameForPers);
		MyCenterOnChild myCenterOnChild = carouselCenter;
		myCenterOnChild.onFinished = (SpringPanel.OnFinished)Delegate.Combine(myCenterOnChild.onFinished, new SpringPanel.OnFinished(HandleCarouselCentering));
		PromoActionsManager.ActionsUUpdated += HandleActionsUUpdated;
		PlayWeaponAnimation();
		idleTimerLastTime = Time.realtimeSinceStartup;
		if (idToSet != null)
		{
			sharedShop.ChooseCarouselItem(idToSet, false, true);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			ForceResetTrainingState();
		}
		sharedShop.carouselCenter.enabled = true;
		AdjustCategoryButtonsForFilterMap();
	}

	private void AdjustCategoryButtonsForFilterMap()
	{
		List<int> list = new List<int>();
		if (SceneLoader.ActiveSceneName.Equals("Sniper"))
		{
			List<int> list2 = new List<int>();
			list2.Add(0);
			list2.Add(3);
			list2.Add(5);
			list = list2;
		}
		else if (SceneLoader.ActiveSceneName.Equals("Knife"))
		{
			List<int> list2 = new List<int>();
			list2.Add(0);
			list2.Add(1);
			list2.Add(3);
			list2.Add(5);
			list2.Add(4);
			list = list2;
		}
		else if (Defs.isHunger)
		{
			List<int> list2 = new List<int>();
			list2.Add(7);
			list = list2;
		}
		for (int i = 0; i < category.buttons.Length; i++)
		{
			category.buttons[i].onButton.GetComponent<BoxCollider>().enabled = !list.Contains(i) && category.buttons[i].onButton.GetComponent<BoxCollider>().enabled;
			category.buttons[i].offButton.GetComponent<BoxCollider>().enabled = !list.Contains(i);
		}
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	private static string TemppOrHighestDPSGunInCategory(int cInt)
	{
		string text = null;
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.FilteredShopLists != null && WeaponManager.sharedManager.FilteredShopLists.Count > cInt)
		{
			List<GameObject> list = WeaponManager.sharedManager.FilteredShopLists[cInt];
			if (_003C_003Ef__am_0024cacheCD == null)
			{
				_003C_003Ef__am_0024cacheCD = _003CTemppOrHighestDPSGunInCategory_003Em__514;
			}
			GameObject gameObject = list.Find(_003C_003Ef__am_0024cacheCD);
			if (gameObject != null)
			{
				text = ItemDb.GetByPrefabName(gameObject.name.Replace("(Clone)", string.Empty)).Tag;
			}
			if (text == null && list.Count > 0)
			{
				for (int num = list.Count - 1; num >= 0; num--)
				{
					string text2 = ItemDb.GetByPrefabName(list[num].name.Replace("(Clone)", string.Empty)).Tag;
					if (!ItemDb.IsTemporaryGun(text2) && ExpController.Instance != null && list[num].GetComponent<WeaponSounds>().tier <= ExpController.Instance.OurTier)
					{
						text = text2;
						break;
					}
				}
			}
		}
		return text;
	}

	public static string TempGunOrHighestDPSGun(CategoryNames c, out CategoryNames cn)
	{
		cn = c;
		string text = null;
		text = TemppOrHighestDPSGunInCategory((int)c);
		if (text == null && WeaponManager.sharedManager.playerWeapons.Count > 0)
		{
			int num = (WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
			text = TemppOrHighestDPSGunInCategory(num);
			cn = (CategoryNames)num;
		}
		return text;
	}

	private void OnLevelWasLoaded(int level)
	{
		if (GuiActive)
		{
			_storedAmbientLight = RenderSettings.ambientLight;
			_storedFogEnabled = RenderSettings.fog;
			RenderSettings.ambientLight = Defs.AmbientLightColorForShop();
			RenderSettings.fog = false;
		}
	}

	public void SetOtherCamerasEnabled(bool e)
	{
		List<Camera> list = (Camera.allCameras ?? new Camera[0]).ToList();
		List<Camera> collection = ProfileController.Instance.GetComponentsInChildren<Camera>(true).ToList();
		list.AddRange(collection);
		list.AddRange(BankRelatedCameras());
		foreach (Camera item in list)
		{
			if ((!(ExpController.Instance != null) || !ExpController.Instance.IsRenderedWithCamera(item)) && !item.gameObject.tag.Equals("CamTemp") && !sharedShop.ourCameras.Contains(item))
			{
				item.rect = new Rect(0f, 0f, e ? 1 : 0, e ? 1 : 0);
			}
		}
	}

	private static void SetIconChosen(CategoryNames cn)
	{
		for (int i = 0; i < sharedShop.category.buttons.Length; i++)
		{
			sharedShop.category.buttons[i].SetCheckedImage(i == (int)cn);
			if (i == (int)cn)
			{
				sharedShop.category.buttons[i].onButton.GetComponent<BoxCollider>().enabled = false;
			}
		}
	}

	public void IsInShopFromPromoPanel(bool isFromPromoACtions, string tg)
	{
		_isFromPromoActions = isFromPromoACtions;
		_promoActionsIdClicked = tg;
	}

	public static void DisableLightProbesRecursively(GameObject w)
	{
		if (_003C_003Ef__am_0024cacheCE == null)
		{
			_003C_003Ef__am_0024cacheCE = _003CDisableLightProbesRecursively_003Em__515;
		}
		Player_move_c.PerformActionRecurs(w, _003C_003Ef__am_0024cacheCE);
	}

	public void SetWeapon(string tg)
	{
		animationCoroutineRunner.StopAllCoroutines();
		if (WeaponManager.sharedManager == null)
		{
			return;
		}
		if (armorPoint.childCount > 0)
		{
			ArmorRefs component = armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					Vector3 position = component.leftBone.position;
					Quaternion rotation = component.leftBone.rotation;
					component.leftBone.parent = armorPoint.GetChild(0).GetChild(0);
					component.leftBone.position = position;
					component.leftBone.rotation = rotation;
				}
				if (component.rightBone != null)
				{
					Vector3 position2 = component.rightBone.position;
					Quaternion rotation2 = component.rightBone.rotation;
					component.rightBone.parent = armorPoint.GetChild(0).GetChild(0);
					component.rightBone.position = position2;
					component.rightBone.rotation = rotation2;
				}
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (Transform item in body.transform)
		{
			list.Add(item);
		}
		foreach (Transform item2 in list)
		{
			item2.parent = null;
			item2.position = new Vector3(0f, -10000f, 0f);
			UnityEngine.Object.Destroy(item2.gameObject);
		}
		if (tg == null)
		{
			return;
		}
		if (profile != null)
		{
			Resources.UnloadAsset(profile);
			profile = null;
		}
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag == null || string.IsNullOrEmpty(byTag.PrefabName))
		{
			Debug.Log("rec == null || string.IsNullOrEmpty(rec.PrefabName)");
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>("Weapons/" + byTag.PrefabName);
		if (gameObject == null)
		{
			Debug.Log("pref==null");
			return;
		}
		profile = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		DisableLightProbesRecursively(gameObject2);
		Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
		gameObject2.transform.parent = body.transform;
		weapon = gameObject2;
		weapon.transform.localScale = new Vector3(1f, 1f, 1f);
		weapon.transform.position = body.transform.position;
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.localRotation = Quaternion.identity;
		WeaponSounds component2 = weapon.GetComponent<WeaponSounds>();
		if (armorPoint.childCount > 0 && component2 != null)
		{
			ArmorRefs component3 = armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component3 != null)
			{
				if (component3.leftBone != null && component2.LeftArmorHand != null)
				{
					component3.leftBone.parent = component2.LeftArmorHand;
					component3.leftBone.localPosition = Vector3.zero;
					component3.leftBone.localRotation = Quaternion.identity;
					component3.leftBone.localScale = new Vector3(1f, 1f, 1f);
				}
				if (component3.rightBone != null && component2.RightArmorHand != null)
				{
					component3.rightBone.parent = component2.RightArmorHand;
					component3.rightBone.localPosition = Vector3.zero;
					component3.rightBone.localRotation = Quaternion.identity;
					component3.rightBone.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		PlayWeaponAnimation();
		DisableGunflashes(weapon);
		if (SkinsController.currentSkinForPers != null)
		{
			SetSkinOnPers(SkinsController.currentSkinForPers);
		}
		_assignedWeaponTag = tg;
	}

	internal static void SynchronizeAndroidPurchases(string comment)
	{
		_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A = new _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A();
		_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A.comment = comment;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B = new _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B();
			_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B._003C_003Ef__ref_0024826 = _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A;
			Debug.LogFormat("Trying to synchronize purchases to cloud ({0})", _003CSynchronizeAndroidPurchases_003Ec__AnonStorey33A.comment);
			if (_003C_003Ef__am_0024cacheCF == null)
			{
				_003C_003Ef__am_0024cacheCF = _003CSynchronizeAndroidPurchases_003Em__516;
			}
			_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B.ResetWeaponManager = _003C_003Ef__am_0024cacheCF;
			switch (Defs.AndroidEdition)
			{
			case Defs.RuntimeAndroidEdition.Amazon:
				PurchasesSynchronizer.Instance.SynchronizeAmazonPurchases();
				_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B.ResetWeaponManager();
				break;
			case Defs.RuntimeAndroidEdition.GoogleLite:
				PlayerPrefs.SetInt("PendingGooglePlayGamesSync", 1);
				PurchasesSynchronizer.Instance.AuthenticateAndSynchronize(_003CSynchronizeAndroidPurchases_003Ec__AnonStorey33B._003C_003Em__517, true);
				break;
			}
		}
	}

	[CompilerGenerated]
	private static void _003Csort_003Em__4C4(List<ShopPositionParams> prefabs, CategoryNames c)
	{
		_003Csort_003Ec__AnonStorey31B _003Csort_003Ec__AnonStorey31B = new _003Csort_003Ec__AnonStorey31B();
		_003Csort_003Ec__AnonStorey31B.c = c;
		Comparison<ShopPositionParams> comparison = _003Csort_003Ec__AnonStorey31B._003C_003Em__518;
		prefabs.Sort(comparison);
	}

	[CompilerGenerated]
	private static Comparison<GameObject> _003CFillModelsList_003Em__4C8(CategoryNames cn)
	{
		_003CFillModelsList_003Ec__AnonStorey31E _003CFillModelsList_003Ec__AnonStorey31E = new _003CFillModelsList_003Ec__AnonStorey31E();
		_003CFillModelsList_003Ec__AnonStorey31E.cn = cn;
		return _003CFillModelsList_003Ec__AnonStorey31E._003C_003Em__519;
	}

	[CompilerGenerated]
	private void _003CSetCamera_003Em__4C9()
	{
		idleTimerLastTime = Time.realtimeSinceStartup;
	}

	[CompilerGenerated]
	private static void _003CUpdateIcon_003Em__4CC(Transform ch)
	{
		if (ch.gameObject.name.Equals("Sprite"))
		{
			ch.gameObject.SetActive(true);
		}
	}

	[CompilerGenerated]
	private static void _003CUpdateIcon_003Em__4CD(Transform ch)
	{
		if (ch.gameObject.name.Equals("ShopIcon"))
		{
			ch.GetComponent<UITexture>().mainTexture = null;
		}
	}

	[CompilerGenerated]
	private static void _003CUpdateIcon_003Em__4CF(Transform ch)
	{
		if (ch.gameObject.name.Equals("Sprite"))
		{
			ch.gameObject.SetActive(true);
		}
		else if (ch.gameObject.name.Equals("ShopIcon"))
		{
			ch.GetComponent<UITexture>().mainTexture = null;
		}
	}

	[CompilerGenerated]
	private static IEnumerable<string> _003CShowTryGunIfPossible_003Em__4D2(KeyValuePair<CategoryNames, List<List<string>>> kvp)
	{
		return kvp.Value[ExpController.OurTierForAnyPlace()];
	}

	[CompilerGenerated]
	private static ItemRecord _003CShowTryGunIfPossible_003Em__4D3(string prefabName)
	{
		return ItemDb.GetByPrefabName(prefabName);
	}

	[CompilerGenerated]
	private static bool _003CShowTryGunIfPossible_003Em__4D4(ItemRecord rec)
	{
		return rec.StorageId != null && Storager.getInt(rec.StorageId, true) == 0;
	}

	[CompilerGenerated]
	private static bool _003CShowTryGunIfPossible_003Em__4D5(ItemRecord rec)
	{
		return !WeaponManager.sharedManager.IsAvailableTryGun(rec.Tag) && !WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(rec.Tag);
	}

	[CompilerGenerated]
	private static bool _003CShowTryGunIfPossible_003Em__4D6(ItemRecord rec)
	{
		return rec.Price.Currency == "Coins";
	}

	[CompilerGenerated]
	private static bool _003CShowTryGunIfPossible_003Em__4D8(ItemRecord rec)
	{
		return rec.Price.Currency == "GemsCurrency";
	}

	[CompilerGenerated]
	private static int _003CTryGunForCategoryWithMaxUnbought_003Em__4DA(CategoryNames cat)
	{
		_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey324 _003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey = new _003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey324();
		_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey.cat = cat;
		UnityEngine.Object[] weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
		if (_003C_003Ef__am_0024cacheD0 == null)
		{
			_003C_003Ef__am_0024cacheD0 = _003CTryGunForCategoryWithMaxUnbought_003Em__51A;
		}
		IEnumerable<WeaponSounds> source = weaponsInGame.Select(_003C_003Ef__am_0024cacheD0).Where(_003CTryGunForCategoryWithMaxUnbought_003Ec__AnonStorey._003C_003Em__51B);
		if (_003C_003Ef__am_0024cacheD1 == null)
		{
			_003C_003Ef__am_0024cacheD1 = _003CTryGunForCategoryWithMaxUnbought_003Em__51C;
		}
		List<WeaponSounds> list = source.Where(_003C_003Ef__am_0024cacheD1).ToList();
		return list.Count;
	}

	[CompilerGenerated]
	private static WeaponSounds _003CTryGunForCategoryWithMaxUnbought_003Em__4DB(UnityEngine.Object w)
	{
		return ((GameObject)w).GetComponent<WeaponSounds>();
	}

	[CompilerGenerated]
	private static bool _003CTryGunForCategoryWithMaxUnbought_003Em__4DD(WeaponSounds ws)
	{
		List<string> list = WeaponUpgrades.ChainForTag(ItemDb.GetByPrefabName(ws.name).Tag);
		return list == null || (list.Count > 0 && list[0] == ItemDb.GetByPrefabName(ws.name).Tag);
	}

	[CompilerGenerated]
	private static bool _003CTryGunForCategoryWithMaxUnbought_003Em__4DE(WeaponSounds ws)
	{
		return WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(ws.name).Tag) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]], true) == 0;
	}

	[CompilerGenerated]
	private static bool _003CTryGunForCategoryWithMaxUnbought_003Em__4E0(WeaponSounds ws)
	{
		return !WeaponManager.sharedManager.IsAvailableTryGun(ItemDb.GetByPrefabName(ws.name).Tag) && !WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(ItemDb.GetByPrefabName(ws.name).Tag);
	}

	[CompilerGenerated]
	private static void _003CHandleFacebookButton_003Em__4E1()
	{
		if (_003C_003Ef__am_0024cacheD2 == null)
		{
			_003C_003Ef__am_0024cacheD2 = _003CHandleFacebookButton_003Em__51D;
		}
		FacebookController.Login(_003C_003Ef__am_0024cacheD2, null, "Shop");
	}

	[CompilerGenerated]
	private static void _003CHandleFacebookButton_003Em__4E2()
	{
		FacebookController.Login(null, null, "Shop");
	}

	[CompilerGenerated]
	private static void _003CHandleProfileButton_003Em__4E3()
	{
	}

	[CompilerGenerated]
	private static int _003CReloadCarousel_003Em__4E5(string kvp1, string kvp2)
	{
		//Discarded unreachable code: IL_00bd, IL_00cb
		try
		{
			if (kvp1 == "61" || kvp2 == "61")
			{
				string text = 4.ToString();
				string text2 = ((!(kvp1 == "61")) ? kvp1 : kvp2);
				if (text2 == text)
				{
					text2 = 6.ToString();
				}
				if (kvp1 == "61")
				{
					return long.Parse(text).CompareTo(long.Parse(text2));
				}
				return long.Parse(text2).CompareTo(long.Parse(text));
			}
			return long.Parse(kvp1).CompareTo(long.Parse(kvp2));
		}
		catch
		{
			return 0;
		}
	}

	[CompilerGenerated]
	private static bool _003CReloadCarousel_003Em__4E6(string kvp)
	{
		long result;
		if (long.TryParse(kvp, out result))
		{
			return result >= 1000000;
		}
		return false;
	}

	[CompilerGenerated]
	private static int _003CUpdateButtons_003Em__4ED(GameObject sp1, GameObject sp2)
	{
		return Defs.CompareAlphaNumerically(sp1.gameObject.name, sp2.gameObject.name);
	}

	[CompilerGenerated]
	private bool _003CCategoryChosen_003Em__4EF(GameObject go)
	{
		return go.name.Equals(viewedId);
	}

	[CompilerGenerated]
	private static void _003CBuyOrUpgradeWeapon_003Em__50A()
	{
		SetBankCamerasEnabled();
	}

	[CompilerGenerated]
	private static void _003CProvideAllTypeShopItem_003Em__50E(string tg)
	{
		if (!(WeaponManager.sharedManager != null) || WeaponManager.sharedManager.weaponsInGame == null)
		{
			return;
		}
		if (GuiActive && sharedShop != null)
		{
			int num = PromoActionsGUIController.CatForTg(tg);
			if (num != -1)
			{
				EquipWearInCategoryIfNotEquiped(tg, (CategoryNames)num, WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null);
			}
		}
		else
		{
			int num2 = PromoActionsGUIController.CatForTg(tg);
			if (num2 != -1)
			{
				SetAsEquippedAndSendToServer(tg, (CategoryNames)num2);
			}
		}
	}

	[CompilerGenerated]
	private static string _003CActualBuy_003Em__513(KeyValuePair<string, string> kv)
	{
		return kv.Key;
	}

	[CompilerGenerated]
	private static bool _003CTemppOrHighestDPSGunInCategory_003Em__514(GameObject w)
	{
		return ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(w.name.Replace("(Clone)", string.Empty)).Tag);
	}

	[CompilerGenerated]
	private static void _003CDisableLightProbesRecursively_003Em__515(Transform t)
	{
		MeshRenderer component = t.GetComponent<MeshRenderer>();
		SkinnedMeshRenderer component2 = t.GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			component.useLightProbes = false;
		}
		if (component2 != null)
		{
			component2.useLightProbes = false;
		}
	}

	[CompilerGenerated]
	private static void _003CSynchronizeAndroidPurchases_003Em__516()
	{
		PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
		if (WeaponManager.sharedManager != null)
		{
			int currentWeaponIndex = WeaponManager.sharedManager.CurrentWeaponIndex;
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0);
			WeaponManager.sharedManager.CurrentWeaponIndex = currentWeaponIndex;
		}
		if (GuiActive)
		{
			sharedShop.UpdateIcons();
		}
	}

	[CompilerGenerated]
	private static WeaponSounds _003CTryGunForCategoryWithMaxUnbought_003Em__51A(UnityEngine.Object w)
	{
		return ((GameObject)w).GetComponent<WeaponSounds>();
	}

	[CompilerGenerated]
	private static bool _003CTryGunForCategoryWithMaxUnbought_003Em__51C(WeaponSounds ws)
	{
		return WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(ws.name).Tag) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]], true) == 1;
	}

	[CompilerGenerated]
	private static void _003CHandleFacebookButton_003Em__51D()
	{
		if (GuiActive)
		{
			sharedShop.UpdateButtons();
		}
	}
}
