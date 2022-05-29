using RilisoftBot;
using System;
using System.Collections;
using UnityEngine;

public sealed class BotChangeDamageMaterial : MonoBehaviour
{
	private Texture _mainTexture;

	private Texture _damageTexture;

	public BotChangeDamageMaterial()
	{
	}

	public void ResetMainMaterial()
	{
		base.GetComponent<Renderer>().material.mainTexture = this._mainTexture;
	}

	public void ShowDamageEffect()
	{
		base.GetComponent<Renderer>().material.mainTexture = this._damageTexture;
	}

	private void Start()
	{
		string child = base.transform.root.GetChild(0).name;
		Texture texture = null;
		if (child.Contains("Enemy"))
		{
			string str = string.Concat(child, "_Level", CurrentCampaignGame.currentLevel);
			Texture item = SkinsManagerPixlGun.sharedManager.skins[str] as Texture;
			texture = item;
			if (!item)
			{
				Debug.Log(string.Concat("No skin: ", str));
			}
		}
		if (texture == null)
		{
			this._mainTexture = base.GetComponent<Renderer>().material.mainTexture;
		}
		else
		{
			this._mainTexture = texture;
			this.ResetMainMaterial();
		}
		BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(base.transform.root);
		if (botScriptForObject != null)
		{
			this._damageTexture = botScriptForObject.flashDeadthTexture;
		}
	}
}