using UnityEngine;

public class ABTestInfoInConsol : MonoBehaviour
{
	private void Update()
	{
		UILabel component = GetComponent<UILabel>();
		string text = string.Empty;
		if (Defs.isActivABTestStaticBank)
		{
			text = text + "А/Б тест статического банка:" + ((!Defs.isActivABTestStaticBank) ? "Не активен." : "Активен.") + ((!Defs.isActivABTestStaticBank) ? string.Empty : ("Когорта:" + ((!FriendsController.isShowStaticBank) ? "Scroll" : "Static"))) + "\n";
		}
		if (Defs.isActivABTestBuffSystem)
		{
			text = text + "А/Б тест системы бафов" + ((!Defs.isActivABTestBuffSystem) ? "Не активен." : "Активен.") + ((!Defs.isActivABTestBuffSystem) ? string.Empty : ("Когорта:" + ((!FriendsController.useBuffSystem) ? "NonBuf" : "UseBuf"))) + "\n";
		}
		text = (component.text = text + "А/Б тест системы кубков и бафов:" + ((!Defs.isActivABTestRatingSystem) ? "Не активен." : "Активен.") + ((!Defs.isActivABTestRatingSystem) ? string.Empty : ("Когорта:" + ((!FriendsController.isCohortUseRatingSystem) ? "NoneRating" : "UseRating"))) + " UseRating=" + FriendsController.isUseRatingSystem + " UseBuff=" + FriendsController.useBuffSystem + "\n");
	}
}
