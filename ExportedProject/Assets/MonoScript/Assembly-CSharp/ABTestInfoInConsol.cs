using System;
using UnityEngine;

public class ABTestInfoInConsol : MonoBehaviour
{
	public ABTestInfoInConsol()
	{
	}

	private void Update()
	{
		UILabel component = base.GetComponent<UILabel>();
		string empty = string.Empty;
		if (Defs.isActivABTestStaticBank)
		{
			string[] strArrays = new string[] { empty, "А/Б тест статического банка:", null, null, null };
			strArrays[2] = (!Defs.isActivABTestStaticBank ? "Не активен." : "Активен.");
			strArrays[3] = (!Defs.isActivABTestStaticBank ? string.Empty : string.Concat("Когорта:", (!FriendsController.isShowStaticBank ? "Scroll" : "Static")));
			strArrays[4] = "\n";
			empty = string.Concat(strArrays);
		}
		if (Defs.isActivABTestBuffSystem)
		{
			string[] strArrays1 = new string[] { empty, "А/Б тест системы бафов", null, null, null };
			strArrays1[2] = (!Defs.isActivABTestBuffSystem ? "Не активен." : "Активен.");
			strArrays1[3] = (!Defs.isActivABTestBuffSystem ? string.Empty : string.Concat("Когорта:", (!FriendsController.useBuffSystem ? "NonBuf" : "UseBuf")));
			strArrays1[4] = "\n";
			empty = string.Concat(strArrays1);
		}
		object[] objArray = new object[] { empty, "А/Б тест системы кубков и бафов:", null, null, null, null, null, null, null };
		objArray[2] = (!Defs.isActivABTestRatingSystem ? "Не активен." : "Активен.");
		objArray[3] = (!Defs.isActivABTestRatingSystem ? string.Empty : string.Concat("Когорта:", (!FriendsController.isCohortUseRatingSystem ? "NoneRating" : "UseRating")));
		objArray[4] = " UseRating=";
		objArray[5] = FriendsController.isUseRatingSystem;
		objArray[6] = " UseBuff=";
		objArray[7] = FriendsController.useBuffSystem;
		objArray[8] = "\n";
		empty = string.Concat(objArray);
		component.text = empty;
	}
}