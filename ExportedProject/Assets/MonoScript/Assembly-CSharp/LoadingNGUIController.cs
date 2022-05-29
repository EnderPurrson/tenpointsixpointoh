using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class LoadingNGUIController : MonoBehaviour
{
	private string sceneToLoad = string.Empty;

	public UITexture loadingNGUITexture;

	public UILabel[] levelNameLabels;

	public UILabel recommendedForThisMap;

	public Transform gunsPoint;

	public string SceneToLoad
	{
		set
		{
			this.sceneToLoad = value;
		}
	}

	public LoadingNGUIController()
	{
	}

	public void Init()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("PromoForLoadings");
		if (textAsset == null)
		{
			return;
		}
		string str = textAsset.text;
		if (str == null)
		{
			return;
		}
		string[] strArrays = str.Split(new char[] { '\r', '\n' });
		Dictionary<string, List<string>> strs = new Dictionary<string, List<string>>();
		string[] strArrays1 = strArrays;
		for (int i = 0; i < (int)strArrays1.Length; i++)
		{
			string str1 = strArrays1[i];
			string[] tag = str1.Split(new char[] { '\t' });
			if ((int)tag.Length >= 2)
			{
				if (tag[0] != null && this.sceneToLoad != null && tag[0].Equals(this.sceneToLoad))
				{
					List<string> strs1 = new List<string>();
					for (int j = 1; j < (int)tag.Length; j++)
					{
						if (tag[j] != null && tag[j].Equals("armor"))
						{
							strs1.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
						}
						else if (tag[j] == null || !tag[j].Equals("hat"))
						{
							int num = 0;
							while (num < (int)WeaponManager.sharedManager.weaponsInGame.Length)
							{
								if (!WeaponManager.sharedManager.weaponsInGame[num].name.Equals(tag[j]))
								{
									num++;
								}
								else
								{
									tag[j] = ItemDb.GetByPrefabName(WeaponManager.sharedManager.weaponsInGame[num].name).Tag;
									tag[j] = PromoActionsGUIController.FilterForLoadings(tag[j], strs1) ?? string.Empty;
									break;
								}
							}
							if (!string.IsNullOrEmpty(tag[j]))
							{
								strs1.Add(tag[j]);
							}
						}
					}
					foreach (string str2 in PromoActionsGUIController.FilterPurchases(strs1, true, true, false, true))
					{
						strs1.Remove(str2);
					}
					if (!strs.ContainsKey(tag[0]))
					{
						strs.Add(tag[0], strs1);
					}
					else
					{
						strs[tag[0]] = strs1;
					}
				}
			}
		}
		if (this.sceneToLoad != null)
		{
			if (strs.ContainsKey(this.sceneToLoad ?? string.Empty))
			{
				List<string> item = strs[this.sceneToLoad ?? string.Empty];
				if (item != null)
				{
					for (int k = 0; k < item.Count; k++)
					{
						GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("PromoItemForLoading") as GameObject);
						vector3.transform.parent = this.gunsPoint;
						vector3.transform.localScale = new Vector3(1f, 1f, 1f);
						vector3.transform.localPosition = new Vector3(-256f * (float)item.Count / 2f + 128f + (float)k * 256f, 0f, 0f);
						PromoItemForLoading component = vector3.GetComponent<PromoItemForLoading>();
						int num1 = PromoActionsGUIController.CatForTg(item[k]);
						string str3 = PromoActionsGUIController.IconNameForKey(item[k], num1);
						Texture texture = Resources.Load<Texture>(string.Concat("OfferIcons/", str3));
						UITexture[] uITextureArray = component.texture;
						for (int l = 0; l < (int)uITextureArray.Length; l++)
						{
							UITexture uITexture = uITextureArray[l];
							if (texture != null)
							{
								uITexture.mainTexture = texture;
							}
						}
						UILabel[] uILabelArray = component.label;
						for (int m = 0; m < (int)uILabelArray.Length; m++)
						{
							UILabel uILabel = uILabelArray[m];
							uILabel.text = ItemDb.GetItemName(item[k], (ShopNGUIController.CategoryNames)num1).Trim().Replace(" -", "-");
						}
					}
				}
			}
		}
		this.recommendedForThisMap.gameObject.SetActive((this.sceneToLoad == null || !strs.ContainsKey(this.sceneToLoad) || strs[this.sceneToLoad] == null ? false : strs[this.sceneToLoad].Count > 0));
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.sceneToLoad);
		UILabel[] uILabelArray1 = this.levelNameLabels;
		for (int n = 0; n < (int)uILabelArray1.Length; n++)
		{
			UILabel upper = uILabelArray1[n];
			if (!(infoScene != null) || string.IsNullOrEmpty(this.sceneToLoad))
			{
				upper.gameObject.SetActive(false);
			}
			else
			{
				upper.gameObject.SetActive(true);
				string str4 = infoScene.TranslatePreviewName.Replace("\n", " ");
				upper.text = str4.Replace("\r", " ").ToUpper();
			}
		}
	}

	public void SetEnabledGunsScroll(bool enabled)
	{
		if (this.recommendedForThisMap != null)
		{
			this.recommendedForThisMap.gameObject.SetActive(enabled);
		}
		if (this.gunsPoint != null)
		{
			this.gunsPoint.gameObject.SetActive(enabled);
		}
	}

	public void SetEnabledMapName(bool enabled)
	{
		for (int i = 0; i < (int)this.levelNameLabels.Length; i++)
		{
			this.levelNameLabels[i].gameObject.SetActive(enabled);
		}
	}
}