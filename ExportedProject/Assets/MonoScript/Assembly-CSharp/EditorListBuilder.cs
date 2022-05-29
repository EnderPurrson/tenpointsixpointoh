using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EditorListBuilder : MonoBehaviour
{
	public GameObject itemPrefab;

	public UIScrollView scrollView;

	public UIGrid grid;

	public UIToggle defaultFilter;

	public UploadShopItemDataToServer applyWindow;

	public UIInput generateTextFile;

	public UIWidget promoActionPanel;

	public UIWidget x3Panel;

	private Dictionary<string, int> _discountsData = new Dictionary<string, int>();

	private List<string> _topSellersData = new List<string>();

	private List<string> _newsData = new List<string>();

	private EditorShopItemsType _currentFilter;

	private bool _isStart;

	private List<EditorShopItemData> _shopItemsData;

	public EditorListBuilder()
	{
	}

	public void ChangeCurrentFilter(UIToggle toggle)
	{
		int num;
		if (toggle == null || !toggle.@value)
		{
			return;
		}
		this.ClearShopItemList();
		string str = toggle.name;
		if (str != null)
		{
			if (EditorListBuilder.u003cu003ef__switchu0024map9 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(10)
				{
					{ "OnlyWeaponCheckbox", 0 },
					{ "OnlySkinsCheckbox", 1 },
					{ "OnlyHatsCheckbox", 2 },
					{ "OnlyArmorCheckbox", 3 },
					{ "OnlyCapesCheckbox", 4 },
					{ "OnlyBootsCheckbox", 5 },
					{ "AllCheckbox", 6 },
					{ "OnlyNewCheckbox", 7 },
					{ "OnlyTopsCheckbox", 8 },
					{ "OnlyDiscountCheckbox", 9 }
				};
				EditorListBuilder.u003cu003ef__switchu0024map9 = strs;
			}
			if (EditorListBuilder.u003cu003ef__switchu0024map9.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						this._currentFilter = EditorShopItemsType.Weapon;
						break;
					}
					case 1:
					{
						this._currentFilter = EditorShopItemsType.Skins;
						break;
					}
					case 2:
					{
						this._currentFilter = EditorShopItemsType.Hats;
						break;
					}
					case 3:
					{
						this._currentFilter = EditorShopItemsType.Armor;
						break;
					}
					case 4:
					{
						this._currentFilter = EditorShopItemsType.Capes;
						break;
					}
					case 5:
					{
						this._currentFilter = EditorShopItemsType.Boots;
						break;
					}
					case 6:
					{
						this._currentFilter = EditorShopItemsType.All;
						break;
					}
					case 7:
					{
						this._currentFilter = EditorShopItemsType.OnlyNew;
						break;
					}
					case 8:
					{
						this._currentFilter = EditorShopItemsType.OnlyTop;
						break;
					}
					case 9:
					{
						this._currentFilter = EditorShopItemsType.OnlyDiscount;
						break;
					}
				}
			}
		}
		if (!this._isStart)
		{
			this.FillShopItemList();
		}
		else
		{
			base.StartCoroutine(this.GetPromoActionsData());
		}
	}

	public void CheckAllNewState(UIToggle newAllCheck)
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].newCheckbox.@value = newAllCheck.@value;
			componentsInChildren[i].SetNewState();
		}
	}

	public void CheckAllTopState(UIToggle topAllCheck)
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].topCheckbox.@value = topAllCheck.@value;
			componentsInChildren[i].SetTopState();
		}
	}

	private void ClearPromoActionsData()
	{
		this._discountsData.Clear();
		this._topSellersData.Clear();
		this._newsData.Clear();
	}

	private void ClearShopItemList()
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			NGUITools.Destroy(componentsInChildren[i].gameObject);
		}
	}

	public static void CopyTextInClipboard(string text)
	{
		TextEditor textEditor = new TextEditor()
		{
			content = new GUIContent(text)
		};
		textEditor.SelectAll();
		textEditor.Copy();
	}

	public void DownloadDataClick()
	{
		this._isStart = true;
		this.applyWindow.Show(UploadShopItemDataToServer.TypeWindow.ChangePlatform);
	}

	private void FillShopItemList()
	{
		EditorShopItem component = null;
		GameObject gameObject = null;
		if (this._shopItemsData == null || this._isStart)
		{
			this._shopItemsData = this.GetItemsList(this._currentFilter);
			this.applyWindow.itemsData = this._shopItemsData;
		}
		this.ClearShopItemList();
		for (int i = 0; i < this._shopItemsData.Count; i++)
		{
			if (this.IsItemEqualFilter(this._shopItemsData[i]))
			{
				gameObject = NGUITools.AddChild(this.grid.gameObject, this.itemPrefab);
				gameObject.name = string.Format("{0:00}", i);
				component = gameObject.GetComponent<EditorShopItem>();
				if (component != null)
				{
					component.SetData(this._shopItemsData[i]);
				}
				gameObject.gameObject.SetActive(true);
			}
		}
		if (this._currentFilter != EditorShopItemsType.Weapon)
		{
			this.grid.sorting = UIGrid.Sorting.Alphabetic;
		}
		else
		{
			this.grid.onCustomSort = new Comparison<Transform>(this.SortingWeaponByOrder);
			this.grid.sorting = UIGrid.Sorting.Custom;
		}
		this.grid.Reposition();
		this.scrollView.ResetPosition();
	}

	public void GenerateUploadTextButtonClick()
	{
		this.generateTextFile.@value = this.applyWindow.GenerateTextForUploadFile();
		EditorListBuilder.CopyTextInClipboard(this.generateTextFile.@value);
	}

	private EditorShopItemData GetEditorItemDataByTag(string tag)
	{
		EditorShopItemData editorShopItemDatum = new EditorShopItemData();
		if (this._discountsData.ContainsKey(tag))
		{
			editorShopItemDatum.discount = this._discountsData[tag];
		}
		editorShopItemDatum.tag = tag;
		editorShopItemDatum.isTop = this._topSellersData.Contains(tag);
		editorShopItemDatum.isNew = this._newsData.Contains(tag);
		return editorShopItemDatum;
	}

	public List<EditorShopItemData> GetItemsList(EditorShopItemsType filter)
	{
		switch (filter)
		{
			case EditorShopItemsType.Weapon:
			{
				return this.GetWeaponsData();
			}
			case EditorShopItemsType.Skins:
			{
				return this.GetSkinsData();
			}
			case EditorShopItemsType.Hats:
			{
				return this.GetWearData(EditorShopItemsType.Hats);
			}
			case EditorShopItemsType.Armor:
			{
				return this.GetWearData(EditorShopItemsType.Armor);
			}
			case EditorShopItemsType.Capes:
			{
				return this.GetWearData(EditorShopItemsType.Capes);
			}
			case EditorShopItemsType.Boots:
			{
				return this.GetWearData(EditorShopItemsType.Boots);
			}
			case EditorShopItemsType.All:
			{
				List<EditorShopItemData> wearData = this.GetWearData(EditorShopItemsType.Hats);
				wearData.AddRange(this.GetWearData(EditorShopItemsType.Armor));
				wearData.AddRange(this.GetWearData(EditorShopItemsType.Capes));
				wearData.AddRange(this.GetWearData(EditorShopItemsType.Boots));
				wearData.AddRange(this.GetWeaponsData());
				wearData.AddRange(this.GetSkinsData());
				return wearData;
			}
		}
		return null;
	}

	[DebuggerHidden]
	public IEnumerator GetPromoActionsData()
	{
		EditorListBuilder.u003cGetPromoActionsDatau003ec__Iterator132 variable = null;
		return variable;
	}

	private List<EditorShopItemData> GetSkinsData()
	{
		List<EditorShopItemData> editorShopItemDatas = new List<EditorShopItemData>();
		foreach (KeyValuePair<string, string> keyValuePair in SkinsController.shopKeyFromNameSkin)
		{
			EditorShopItemData editorItemDataByTag = this.GetEditorItemDataByTag(keyValuePair.Key);
			editorItemDataByTag.localizeKey = SkinsController.skinsLocalizeKey[int.Parse(keyValuePair.Key)];
			editorItemDataByTag.type = EditorShopItemsType.Skins;
			editorShopItemDatas.Add(editorItemDataByTag);
		}
		return editorShopItemDatas;
	}

	private List<EditorShopItemData> GetWeaponsData()
	{
		WeaponSounds[] weaponSoundsArray = Resources.LoadAll<WeaponSounds>("Weapons/");
		List<EditorShopItemData> editorShopItemDatas = new List<EditorShopItemData>();
		for (int i = 0; i < (int)weaponSoundsArray.Length; i++)
		{
			EditorShopItemData editorItemDataByTag = this.GetEditorItemDataByTag(ItemDb.GetByPrefabName(weaponSoundsArray[i].name).Tag);
			editorItemDataByTag.localizeKey = weaponSoundsArray[i].localizeWeaponKey;
			editorItemDataByTag.type = EditorShopItemsType.Weapon;
			editorItemDataByTag.prefabName = weaponSoundsArray[i].name;
			editorShopItemDatas.Add(editorItemDataByTag);
		}
		return editorShopItemDatas;
	}

	private List<EditorShopItemData> GetWearData(EditorShopItemsType type)
	{
		string empty = string.Empty;
		switch (type)
		{
			case EditorShopItemsType.Hats:
			{
				empty = "Hats_Info/";
				break;
			}
			case EditorShopItemsType.Armor:
			{
				empty = "Armor_Info/";
				break;
			}
			case EditorShopItemsType.Capes:
			{
				empty = "Capes_Info/";
				break;
			}
			case EditorShopItemsType.Boots:
			{
				empty = "Shop_Boots_Info/";
				break;
			}
		}
		ShopPositionParams[] shopPositionParamsArray = Resources.LoadAll<ShopPositionParams>(empty);
		List<EditorShopItemData> editorShopItemDatas = new List<EditorShopItemData>();
		for (int i = 0; i < (int)shopPositionParamsArray.Length; i++)
		{
			EditorShopItemData editorItemDataByTag = this.GetEditorItemDataByTag(shopPositionParamsArray[i].name);
			editorItemDataByTag.localizeKey = shopPositionParamsArray[i].localizeKey;
			editorItemDataByTag.type = type;
			editorItemDataByTag.prefabName = shopPositionParamsArray[i].name;
			editorShopItemDatas.Add(editorItemDataByTag);
		}
		return editorShopItemDatas;
	}

	private bool IsItemEqualFilter(EditorShopItemData itemData)
	{
		if (this._currentFilter == EditorShopItemsType.All)
		{
			return true;
		}
		if (this._currentFilter == EditorShopItemsType.OnlyNew && itemData.isNew)
		{
			return true;
		}
		if (this._currentFilter == EditorShopItemsType.OnlyTop && itemData.isTop)
		{
			return true;
		}
		if (this._currentFilter == EditorShopItemsType.OnlyDiscount && itemData.discount > 0)
		{
			return true;
		}
		if (this._currentFilter == itemData.type)
		{
			return true;
		}
		return false;
	}

	public void SendDataToServerClick()
	{
		this.applyWindow.Show(UploadShopItemDataToServer.TypeWindow.UploadFileToServer);
	}

	public void SetAllDiscounts(UIInput inputAllDiscount)
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].discountInput.label.text = inputAllDiscount.label.text;
			componentsInChildren[i].SetDiscount();
		}
	}

	public void ShowPromoActionsPanel()
	{
		this.promoActionPanel.alpha = 1f;
		this.x3Panel.alpha = 0f;
	}

	public void ShowX3ActionsPanel()
	{
		this.promoActionPanel.alpha = 0f;
		this.x3Panel.alpha = 1f;
	}

	private int SortingWeaponByOrder(Transform left, Transform right)
	{
		EditorShopItem component = left.GetComponent<EditorShopItem>();
		EditorShopItem editorShopItem = right.GetComponent<EditorShopItem>();
		string str = component.prefabName.Replace("Weapon", string.Empty);
		int num = 0;
		int.TryParse(str, out num);
		string str1 = editorShopItem.prefabName.Replace("Weapon", string.Empty);
		int num1 = 0;
		int.TryParse(str1, out num1);
		if (num > num1)
		{
			return -1;
		}
		if (num < num1)
		{
			return 1;
		}
		return 0;
	}

	private void Start()
	{
		string currentLanguage = LocalizationStore.CurrentLanguage;
		this._currentFilter = EditorShopItemsType.All;
		this._isStart = true;
	}
}