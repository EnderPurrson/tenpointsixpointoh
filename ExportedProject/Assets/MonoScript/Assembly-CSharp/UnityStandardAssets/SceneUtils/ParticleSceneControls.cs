using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

namespace UnityStandardAssets.SceneUtils
{
	public class ParticleSceneControls : MonoBehaviour
	{
		public ParticleSceneControls.DemoParticleSystemList demoParticles;

		public float spawnOffset = 0.5f;

		public float multiply = 1f;

		public bool clearOnChange;

		public Text titleText;

		public Transform sceneCamera;

		public Text instructionText;

		public Button previousButton;

		public Button nextButton;

		public GraphicRaycaster graphicRaycaster;

		public EventSystem eventSystem;

		private ParticleSystemMultiplier m_ParticleMultiplier;

		private List<Transform> m_CurrentParticleList = new List<Transform>();

		private Transform m_Instance;

		private static int s_SelectedIndex;

		private Vector3 m_CamOffsetVelocity = Vector3.zero;

		private Vector3 m_LastPos;

		private static ParticleSceneControls.DemoParticleSystem s_Selected;

		static ParticleSceneControls()
		{
		}

		public ParticleSceneControls()
		{
		}

		private void Awake()
		{
			this.Select(ParticleSceneControls.s_SelectedIndex);
			this.previousButton.onClick.AddListener(new UnityAction(this.Previous));
			this.nextButton.onClick.AddListener(new UnityAction(this.Next));
		}

		private bool CheckForGuiCollision()
		{
			PointerEventData pointerEventDatum = new PointerEventData(this.eventSystem)
			{
				pressPosition = Input.mousePosition,
				position = Input.mousePosition
			};
			List<RaycastResult> raycastResults = new List<RaycastResult>();
			this.graphicRaycaster.Raycast(pointerEventDatum, raycastResults);
			return raycastResults.Count > 0;
		}

		public void Next()
		{
			ParticleSceneControls.s_SelectedIndex++;
			if (ParticleSceneControls.s_SelectedIndex == (int)this.demoParticles.items.Length)
			{
				ParticleSceneControls.s_SelectedIndex = 0;
			}
			this.Select(ParticleSceneControls.s_SelectedIndex);
		}

		private void OnDisable()
		{
			this.previousButton.onClick.RemoveListener(new UnityAction(this.Previous));
			this.nextButton.onClick.RemoveListener(new UnityAction(this.Next));
		}

		private void Previous()
		{
			ParticleSceneControls.s_SelectedIndex--;
			if (ParticleSceneControls.s_SelectedIndex == -1)
			{
				ParticleSceneControls.s_SelectedIndex = (int)this.demoParticles.items.Length - 1;
			}
			this.Select(ParticleSceneControls.s_SelectedIndex);
		}

		private void Select(int i)
		{
			ParticleSceneControls.s_Selected = this.demoParticles.items[i];
			this.m_Instance = null;
			ParticleSceneControls.DemoParticleSystem[] demoParticleSystemArray = this.demoParticles.items;
			for (int num = 0; num < (int)demoParticleSystemArray.Length; num++)
			{
				ParticleSceneControls.DemoParticleSystem demoParticleSystem = demoParticleSystemArray[num];
				if (demoParticleSystem != ParticleSceneControls.s_Selected && demoParticleSystem.mode == ParticleSceneControls.Mode.Activate)
				{
					demoParticleSystem.transform.gameObject.SetActive(false);
				}
			}
			if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
			{
				ParticleSceneControls.s_Selected.transform.gameObject.SetActive(true);
			}
			this.m_ParticleMultiplier = ParticleSceneControls.s_Selected.transform.GetComponent<ParticleSystemMultiplier>();
			this.multiply = 1f;
			if (this.clearOnChange)
			{
				while (this.m_CurrentParticleList.Count > 0)
				{
					UnityEngine.Object.Destroy(this.m_CurrentParticleList[0].gameObject);
					this.m_CurrentParticleList.RemoveAt(0);
				}
			}
			this.instructionText.text = ParticleSceneControls.s_Selected.instructionText;
			this.titleText.text = ParticleSceneControls.s_Selected.transform.name;
		}

		private void Update()
		{
			RaycastHit raycastHit;
			this.sceneCamera.localPosition = Vector3.SmoothDamp(this.sceneCamera.localPosition, Vector3.forward * (float)(-ParticleSceneControls.s_Selected.camOffset), ref this.m_CamOffsetVelocity, 1f);
			if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
			{
				return;
			}
			if (this.CheckForGuiCollision())
			{
				return;
			}
			bool flag = (!Input.GetMouseButtonDown(0) ? false : ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Instantiate);
			bool flag1 = (!Input.GetMouseButton(0) ? false : ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail);
			if ((flag || flag1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
			{
				Quaternion quaternion = Quaternion.LookRotation(raycastHit.normal);
				if (ParticleSceneControls.s_Selected.align == ParticleSceneControls.AlignMode.Up)
				{
					quaternion = Quaternion.identity;
				}
				Vector3 vector3 = raycastHit.point + (raycastHit.normal * this.spawnOffset);
				if ((vector3 - this.m_LastPos).magnitude > ParticleSceneControls.s_Selected.minDist)
				{
					if (ParticleSceneControls.s_Selected.mode != ParticleSceneControls.Mode.Trail || this.m_Instance == null)
					{
						this.m_Instance = (Transform)UnityEngine.Object.Instantiate(ParticleSceneControls.s_Selected.transform, vector3, quaternion);
						if (this.m_ParticleMultiplier != null)
						{
							this.m_Instance.GetComponent<ParticleSystemMultiplier>().multiplier = this.multiply;
						}
						this.m_CurrentParticleList.Add(this.m_Instance);
						if (ParticleSceneControls.s_Selected.maxCount > 0 && this.m_CurrentParticleList.Count > ParticleSceneControls.s_Selected.maxCount)
						{
							if (this.m_CurrentParticleList[0] != null)
							{
								UnityEngine.Object.Destroy(this.m_CurrentParticleList[0].gameObject);
							}
							this.m_CurrentParticleList.RemoveAt(0);
						}
					}
					else
					{
						this.m_Instance.position = vector3;
						this.m_Instance.rotation = quaternion;
					}
					if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail)
					{
						this.m_Instance.transform.GetComponent<ParticleSystem>().enableEmission = false;
						this.m_Instance.transform.GetComponent<ParticleSystem>().Emit(1);
					}
					this.m_Instance.parent = raycastHit.transform;
					this.m_LastPos = vector3;
				}
			}
		}

		public enum AlignMode
		{
			Normal,
			Up
		}

		[Serializable]
		public class DemoParticleSystem
		{
			public Transform transform;

			public ParticleSceneControls.Mode mode;

			public ParticleSceneControls.AlignMode align;

			public int maxCount;

			public float minDist;

			public int camOffset;

			public string instructionText;

			public DemoParticleSystem()
			{
			}
		}

		[Serializable]
		public class DemoParticleSystemList
		{
			public ParticleSceneControls.DemoParticleSystem[] items;

			public DemoParticleSystemList()
			{
			}
		}

		public enum Mode
		{
			Activate,
			Instantiate,
			Trail
		}
	}
}