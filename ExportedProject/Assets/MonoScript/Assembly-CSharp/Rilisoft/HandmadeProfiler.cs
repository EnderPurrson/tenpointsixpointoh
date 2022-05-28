using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public class HandmadeProfiler : MonoBehaviour
	{
		[Serializable]
		private struct SampleMemento : IEquatable<SampleMemento>
		{
			[SerializeField]
			private int frame;

			[SerializeField]
			private double dt;

			[SerializeField]
			private string scene;

			private List<string> components;

			public int Frame
			{
				get
				{
					return frame;
				}
				set
				{
					frame = value;
				}
			}

			public double Dt
			{
				get
				{
					return dt;
				}
				set
				{
					dt = value;
				}
			}

			public string Scene
			{
				get
				{
					return scene ?? string.Empty;
				}
				set
				{
					scene = value ?? string.Empty;
				}
			}

			public List<string> Components
			{
				get
				{
					return components ?? new List<string>();
				}
				set
				{
					components = value ?? new List<string>();
				}
			}

			public bool Equals(SampleMemento other)
			{
				if (Frame != other.Frame)
				{
					return false;
				}
				if (Dt != other.Dt)
				{
					return false;
				}
				if (Scene != other.Scene)
				{
					return false;
				}
				if (!Components.SequenceEqual(Components))
				{
					return false;
				}
				return true;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is SampleMemento))
				{
					return false;
				}
				SampleMemento other = (SampleMemento)obj;
				return Equals(other);
			}

			public override int GetHashCode()
			{
				return Frame.GetHashCode() ^ Dt.GetHashCode() ^ Scene.GetHashCode() ^ Components.GetHashCode();
			}

			public override string ToString()
			{
				return JsonUtility.ToJson(this);
			}
		}

		private float _dtThreshold = 1f;

		[CompilerGenerated]
		private static Func<MonoBehaviour, bool> _003C_003Ef__am_0024cache1;

		[CompilerGenerated]
		private static Func<MonoBehaviour, string> _003C_003Ef__am_0024cache2;

		[CompilerGenerated]
		private static Func<string, string> _003C_003Ef__am_0024cache3;

		private void Awake()
		{
			_dtThreshold = ((!Application.isEditor) ? 2f : 1f);
		}

		private void LateUpdate()
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			float num = Time.realtimeSinceStartup - Time.unscaledTime;
			if (!(num > _dtThreshold))
			{
				return;
			}
			MonoBehaviour[] source = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CLateUpdate_003Em__2D9;
			}
			IEnumerable<MonoBehaviour> source2 = source.Where(_003C_003Ef__am_0024cache1);
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003CLateUpdate_003Em__2DA;
			}
			IEnumerable<string> source3 = source2.Select(_003C_003Ef__am_0024cache2).Distinct();
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CLateUpdate_003Em__2DB;
			}
			List<string> list = source3.OrderBy(_003C_003Ef__am_0024cache3).ToList();
			SampleMemento sampleMemento = default(SampleMemento);
			sampleMemento.Dt = Math.Round(num, 3);
			sampleMemento.Frame = Time.frameCount;
			sampleMemento.Scene = SceneManager.GetActiveScene().name;
			SampleMemento sample = sampleMemento;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && list.Count > 32767)
			{
				int num2 = 0;
				while (list.Count > 0)
				{
					List<string> list2 = new List<string>(32769);
					if (num2 > 0)
					{
						list2.Add("...");
					}
					list2.AddRange(list.Take(32767));
					list.RemoveRange(0, Math.Min(32767, list.Count));
					if (list.Count > 0)
					{
						list2.Add("...");
					}
					sample.Components = list2;
					LogSample(sample);
					num2++;
				}
			}
			else
			{
				sample.Components = list;
				LogSample(sample);
			}
		}

		private void LogSample(SampleMemento sample)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "Frame rate drop: {0}", sample);
			string message = ((!Application.isEditor) ? text : ("<color=orange><b>" + text + "</b></color>"));
			Debug.LogWarning(message);
		}

		[CompilerGenerated]
		private static bool _003CLateUpdate_003Em__2D9(MonoBehaviour c)
		{
			return c.gameObject.activeInHierarchy;
		}

		[CompilerGenerated]
		private static string _003CLateUpdate_003Em__2DA(MonoBehaviour c)
		{
			return c.GetType().Name;
		}

		[CompilerGenerated]
		private static string _003CLateUpdate_003Em__2DB(string s)
		{
			return s;
		}
	}
}
