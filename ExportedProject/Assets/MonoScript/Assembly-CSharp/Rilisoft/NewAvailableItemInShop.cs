using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public class NewAvailableItemInShop : MonoBehaviour
	{
		public string _tag = string.Empty;

		public ShopNGUIController.CategoryNames category;

		public UITexture itemImage;

		public List<UILabel> itemName;

		public NewAvailableItemInShop()
		{
		}

		private void OnClick()
		{
			LevelUpWithOffers componentInParent = base.GetComponentInParent<LevelUpWithOffers>();
			if (componentInParent != null)
			{
				ExpController.Instance.HandleNewAvailableItem(componentInParent.gameObject, this);
			}
		}

		[ContextMenu("Set ref")]
		private void SetRef()
		{
			this.itemImage = base.GetComponentsInChildren<UITexture>()[1];
			this.itemName.Clear();
			this.itemName.AddRange(base.GetComponentsInChildren<UILabel>(true));
		}
	}
}