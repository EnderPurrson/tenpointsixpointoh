using System.Collections;
using UnityEngine;

public class ChooseBoxNGUIController : MonoBehaviour
{
	public MultipleToggleButton difficultyToggle;

	public UIButton backButton;

	public UIButton startButton;

	public GameObject grid;

	public Transform ScrollTransform;

	public GameObject SelectMapPanel;

	public int selectIndexMap;

	public int countMap;

	public float widthCell = 824f;

	private IEnumerator Start()
	{
		ScrollTransform.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, 760 * Screen.width / Screen.height, 736f);
		countMap = grid.transform.childCount;
		yield return null;
		Defs.diffGame = PlayerPrefs.GetInt(Defs.DiffSett, 1);
		if (difficultyToggle != null)
		{
			difficultyToggle.buttons[Defs.diffGame].IsChecked = true;
			MultipleToggleButton multipleToggleButton = difficultyToggle;
			if (_003CStart_003Ec__Iterator10._003C_003Ef__am_0024cache3 == null)
			{
				_003CStart_003Ec__Iterator10._003C_003Ef__am_0024cache3 = _003CStart_003Ec__Iterator10._003C_003Em__C;
			}
			multipleToggleButton.Clicked += _003CStart_003Ec__Iterator10._003C_003Ef__am_0024cache3;
		}
	}

	private void Update()
	{
		if (SelectMapPanel.activeInHierarchy)
		{
			if (ScrollTransform.localPosition.x > 0f)
			{
				selectIndexMap = Mathf.RoundToInt((ScrollTransform.localPosition.x - (float)Mathf.FloorToInt(ScrollTransform.localPosition.x / widthCell / (float)countMap) * widthCell * (float)countMap) / widthCell);
				selectIndexMap = countMap - selectIndexMap;
			}
			else
			{
				selectIndexMap = -1 * Mathf.RoundToInt((ScrollTransform.localPosition.x - (float)Mathf.CeilToInt(ScrollTransform.localPosition.x / widthCell / (float)countMap) * widthCell * (float)countMap) / widthCell);
			}
			if (selectIndexMap == countMap)
			{
				selectIndexMap = 0;
			}
		}
	}
}
