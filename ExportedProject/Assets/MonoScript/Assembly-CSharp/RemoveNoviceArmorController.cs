using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RemoveNoviceArmorController : MonoBehaviour
{
	private IDisposable _escapeSubscription;

	public RemoveNoviceArmorController()
	{
	}

	private void Awake()
	{
		this._escapeSubscription = BackSystem.Instance.Register(() => this.Hide(), "RemoveNoviceArmorController");
		Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 0, false);
		ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
		ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.ArmorCategory, "Armor_Army_1", 1, false, 0, null, null, true, false, false);
	}

	public void Hide()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		this._escapeSubscription.Dispose();
	}
}