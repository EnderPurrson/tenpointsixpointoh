using System;
using UnityEngine;

public class SetChatLabelController : MonoBehaviour
{
	public UILabel killerLabel;

	public UILabel killedLabel;

	public UISprite reasonSprite;

	public UISprite reasonSprite2;

	public SetChatLabelController()
	{
	}

	public void SetChatLabelText(Player_move_c.SystemMessage message)
	{
		this.SetChatLabelText(message.nick1, message.message2, message.nick2, message.message, message.textColor);
	}

	public void SetChatLabelText(string _nameKiller, string _reasonSpriteName2, string _nameKilled, string _reasonSpriteName, Color color)
	{
		this.killerLabel.text = _nameKiller;
		this.killerLabel.color = color;
		int num = this.killerLabel.width;
		if (this.reasonSprite != null && !string.IsNullOrEmpty(_reasonSpriteName))
		{
			if (!this.reasonSprite.gameObject.activeSelf)
			{
				this.reasonSprite.gameObject.SetActive(true);
			}
			this.reasonSprite.transform.localPosition = new Vector3((float)num + 33f, 0f, 0f);
			num += 66;
			this.reasonSprite.spriteName = this.SubstituteWeaponImageIfNeeded(_reasonSpriteName);
		}
		else if (this.reasonSprite != null && this.reasonSprite.gameObject.activeSelf)
		{
			this.reasonSprite.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(_reasonSpriteName2))
		{
			if (!this.reasonSprite2.gameObject.activeSelf)
			{
				this.reasonSprite2.gameObject.SetActive(true);
			}
			this.reasonSprite2.transform.localPosition = new Vector3((float)num + 23f, 0f, 0f);
			num += 46;
			this.reasonSprite2.spriteName = this.SubstituteWeaponImageIfNeeded(_reasonSpriteName2);
		}
		else if (this.reasonSprite2.gameObject.activeSelf)
		{
			this.reasonSprite2.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(_nameKilled))
		{
			if (!this.killedLabel.gameObject.activeSelf)
			{
				this.killedLabel.gameObject.SetActive(true);
			}
			this.killedLabel.transform.localPosition = new Vector3((float)num, 0f, 0f);
			this.killedLabel.text = _nameKilled;
			this.killedLabel.color = color;
		}
		else if (this.killedLabel.gameObject.activeSelf)
		{
			this.killedLabel.gameObject.SetActive(false);
		}
	}

	private string SubstituteWeaponImageIfNeeded(string source)
	{
		if (source == null)
		{
			return null;
		}
		ItemRecord byPrefabName = ItemDb.GetByPrefabName(source);
		if (byPrefabName != null && byPrefabName.UseImagesFromFirstUpgrade && byPrefabName.Tag != null)
		{
			string str = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
			if (str != null && !str.Equals(byPrefabName.Tag))
			{
				ItemRecord byTag = ItemDb.GetByTag(str);
				if (byTag != null && byTag.PrefabName != null)
				{
					return byTag.PrefabName;
				}
			}
		}
		return source;
	}
}