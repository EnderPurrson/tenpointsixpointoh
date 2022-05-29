using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class EditorShopItem : MonoBehaviour
{
	public UILabel itemName;

	public UIToggle topCheckbox;

	public UIToggle newCheckbox;

	public UIInput discountInput;

	private EditorShopItemData _itemData;

	public string prefabName
	{
		get;
		private set;
	}

	public EditorShopItem()
	{
	}

	public void SetData(EditorShopItemData data)
	{
		this._itemData = data;
		StringBuilder stringBuilder = new StringBuilder();
		string byDefault = LocalizationStore.GetByDefault(data.localizeKey);
		stringBuilder.AppendLine(string.Format("Name: {0}", byDefault));
		if (!string.IsNullOrEmpty(data.prefabName))
		{
			this.prefabName = data.prefabName;
			stringBuilder.AppendLine(string.Format("Prefab: {0}", data.prefabName));
		}
		stringBuilder.Append(string.Format("Tag: {0}", data.tag));
		this.itemName.text = stringBuilder.ToString();
		this.topCheckbox.@value = data.isTop;
		this.newCheckbox.@value = data.isNew;
		this.discountInput.label.text = data.discount.ToString();
	}

	public void SetDiscount()
	{
		int.TryParse(this.discountInput.label.text, out this._itemData.discount);
	}

	public void SetNewState()
	{
		this._itemData.isNew = this.newCheckbox.@value;
	}

	public void SetTopState()
	{
		this._itemData.isTop = this.topCheckbox.@value;
	}
}