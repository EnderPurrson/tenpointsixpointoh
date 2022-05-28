using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	public class WindowBackgroundAnimation : MonoBehaviour
	{
		public GameObject[] Arrows;

		public GameObject[] ShineNodes;

		public bool PlayOnEnable = true;

		private int _currentBgArrowPrefabIndex = -1;

		private GameObject[] _bgArrowRows;

		private UIRoot interfaceHolderValue;

		private UIRoot interfaceHolder
		{
			get
			{
				if (interfaceHolderValue == null)
				{
					interfaceHolderValue = base.gameObject.GetComponentInParents<UIRoot>();
				}
				return interfaceHolderValue;
			}
		}

		private void OnEnable()
		{
			if (PlayOnEnable)
			{
				Play();
			}
		}

		public void Play()
		{
			_currentBgArrowPrefabIndex = -1;
			StartCoroutine(LoopBackgroundAnimation());
		}

		private IEnumerator LoopBackgroundAnimation()
		{
			GameObject arrowRowPrefab = Arrows[0];
			if (_bgArrowRows == null)
			{
				_bgArrowRows = new GameObject[8];
				for (int l = 0; l < _bgArrowRows.Length; l++)
				{
					GameObject newArrowRow = Object.Instantiate(arrowRowPrefab);
					newArrowRow.transform.parent = arrowRowPrefab.transform.parent;
					_bgArrowRows[l] = newArrowRow;
				}
			}
			for (int k = 0; k < Arrows.Length; k++)
			{
				Arrows[k].SetActive(false);
			}
			_currentBgArrowPrefabIndex = -1;
			while (true)
			{
				if (interfaceHolder != null && interfaceHolder.gameObject.activeInHierarchy)
				{
					for (int j = 0; j < ShineNodes.Length; j++)
					{
						GameObject shine = ShineNodes[j];
						if (shine != null && shine.activeInHierarchy)
						{
							shine.transform.Rotate(Vector3.forward, Time.deltaTime * 10f, Space.Self);
							if (j != _currentBgArrowPrefabIndex)
							{
								_currentBgArrowPrefabIndex = j;
								ResetBackgroundArrows(Arrows[j].transform);
							}
						}
					}
					for (int i = 0; i < _bgArrowRows.Length; i++)
					{
						if (!(_bgArrowRows[i] == null))
						{
							Transform t = _bgArrowRows[i].transform;
							float newLocalY = t.localPosition.y + Time.deltaTime * 60f;
							if (newLocalY > 474f)
							{
								newLocalY -= 880f;
							}
							t.localPosition = new Vector3(t.localPosition.x, newLocalY, t.localPosition.z);
						}
					}
				}
				yield return null;
			}
		}

		private void ResetBackgroundArrows(Transform target)
		{
			for (int i = 0; i < _bgArrowRows.Length; i++)
			{
				Transform transform = _bgArrowRows[i].transform;
				transform.parent = target.parent;
				transform.localScale = Vector3.one;
				transform.localPosition = new Vector3(target.localPosition.x + ((i % 2 != 1) ? 0f : 90f), target.localPosition.y - 110f * (float)i, target.localPosition.z);
				transform.localRotation = target.localRotation;
			}
		}
	}
}
