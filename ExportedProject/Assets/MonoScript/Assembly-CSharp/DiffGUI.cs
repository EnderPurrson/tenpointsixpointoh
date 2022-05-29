using System;
using UnityEngine;

public class DiffGUI : MonoBehaviour
{
	public GUIStyle buttons;

	public Texture fon;

	public Texture[] instr = new Texture[0];

	private int _curInd = 1;

	public DiffGUI()
	{
	}

	private void OnGUI()
	{
		GUI.depth = -100;
		Rect rect = new Rect((float)Screen.width - (float)this.fon.width * Defs.Coef, (float)Screen.height - (float)this.fon.height * Defs.Coef, (float)this.fon.width * Defs.Coef, (float)this.fon.height * Defs.Coef);
		GUI.DrawTexture(rect, this.fon);
		float coef = (float)this.buttons.normal.background.width * Defs.Coef;
		float single = (float)this.buttons.normal.background.height * Defs.Coef;
		float coef1 = 14f * Defs.Coef;
		Rect rect1 = new Rect(rect.x + rect.width - coef1 - coef - Defs.BottomOffs * Defs.Coef, rect.y + rect.height - coef1 - single * 3f - Defs.BottomOffs * Defs.Coef, coef, single * 3f);
		int num = GUI.SelectionGrid(rect1, this._curInd, new string[] { "Easy", "Medium", "Hard" }, 1, this.buttons);
		if (num != this._curInd)
		{
			ButtonClickSound.Instance.PlayClick();
			this._curInd = num;
			PlayerPrefs.SetInt(Defs.DiffSett, this._curInd);
			Defs.diffGame = this._curInd;
		}
		Rect rect2 = new Rect(rect1.x - (float)this.instr[this._curInd].width * Defs.Coef, rect.y, (float)this.instr[this._curInd].width * Defs.Coef, (float)this.instr[this._curInd].height * Defs.Coef);
		GUI.DrawTexture(rect2, this.instr[this._curInd]);
	}

	private void Start()
	{
		this.buttons.fontSize = Mathf.RoundToInt(16f * Defs.Coef);
		this._curInd = PlayerPrefs.GetInt(Defs.DiffSett, 1);
	}
}