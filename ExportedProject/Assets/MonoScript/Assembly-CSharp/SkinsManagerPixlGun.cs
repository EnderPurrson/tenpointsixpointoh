using System;
using System.Collections;
using UnityEngine;

internal sealed class SkinsManagerPixlGun : MonoBehaviour
{
	public Hashtable skins = new Hashtable();

	public static SkinsManagerPixlGun sharedManager;

	public SkinsManagerPixlGun()
	{
	}

	private void OnDestroy()
	{
		SkinsManagerPixlGun.sharedManager = null;
	}

	private void OnLevelWasLoaded(int idx)
	{
		string str;
		if (this.skins.Count > 0)
		{
			this.skins.Clear();
		}
		if (!Defs.isMulti || !Defs.isCOOP || Defs.isCompany)
		{
			if (Defs.isMulti || Defs.isCOOP || Defs.isCompany)
			{
				return;
			}
			str = (!Defs.IsSurvival ? string.Concat("EnemySkins/Level", (!TrainingController.TrainingCompleted ? "3" : CurrentCampaignGame.currentLevel.ToString())) : Defs.SurvSkinsPath);
		}
		else
		{
			str = "EnemySkins/COOP/";
		}
		UnityEngine.Object[] objArray = Resources.LoadAll(str);
		try
		{
			UnityEngine.Object[] objArray1 = objArray;
			for (int i = 0; i < (int)objArray1.Length; i++)
			{
				Texture texture = (Texture)objArray1[i];
				this.skins.Add(texture.name, texture);
			}
		}
		catch (Exception exception)
		{
			Debug.Log(string.Concat("Exception in SkinsManagerPixlGun: ", exception));
		}
	}

	private void Start()
	{
		SkinsManagerPixlGun.sharedManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}