using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public class HandmadeProfiler : MonoBehaviour
	{
		private float _dtThreshold = 1f;

		public HandmadeProfiler()
		{
		}

		private void Awake()
		{
			this._dtThreshold = (!Application.isEditor ? 2f : 1f);
		}

		private void LateUpdate()
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			float single = Time.realtimeSinceStartup - Time.unscaledTime;
			if (single > this._dtThreshold)
			{
				List<string> list = (
					from s in (
						from c in (IEnumerable<MonoBehaviour>)UnityEngine.Object.FindObjectsOfType<MonoBehaviour>()
						where c.gameObject.activeInHierarchy
						select c.GetType().Name).Distinct<string>()
					orderby s
					select s).ToList<string>();
				HandmadeProfiler.SampleMemento sampleMemento = new HandmadeProfiler.SampleMemento();
				HandmadeProfiler.SampleMemento activeScene = sampleMemento;
				activeScene.Dt = Math.Round((double)single, 3);
				activeScene.Frame = Time.frameCount;
				activeScene.Scene = SceneManager.GetActiveScene().name;
				sampleMemento = activeScene;
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || list.Count <= 32767)
				{
					sampleMemento.Components = list;
					this.LogSample(sampleMemento);
				}
				else
				{
					int num = 0;
					while (list.Count > 0)
					{
						List<string> strs = new List<string>(32769);
						if (num > 0)
						{
							strs.Add("...");
						}
						strs.AddRange(list.Take<string>(32767));
						list.RemoveRange(0, Math.Min(32767, list.Count));
						if (list.Count > 0)
						{
							strs.Add("...");
						}
						sampleMemento.Components = strs;
						this.LogSample(sampleMemento);
						num++;
					}
				}
			}
		}

		private void LogSample(HandmadeProfiler.SampleMemento sample)
		{
			string str = string.Format(CultureInfo.InvariantCulture, "Frame rate drop: {0}", new object[] { sample });
			Debug.LogWarning((!Application.isEditor ? str : string.Concat("<color=orange><b>", str, "</b></color>")));
		}

		[Serializable]
		private struct SampleMemento : IEquatable<HandmadeProfiler.SampleMemento>
		{
			[SerializeField]
			private int frame;

			[SerializeField]
			private double dt;

			[SerializeField]
			private string scene;

			private List<string> components;

			public List<string> Components
			{
				get
				{
					return this.components ?? new List<string>();
				}
				set
				{
					this.components = value ?? new List<string>();
				}
			}

			public double Dt
			{
				get
				{
					return this.dt;
				}
				set
				{
					this.dt = value;
				}
			}

			public int Frame
			{
				get
				{
					return this.frame;
				}
				set
				{
					this.frame = value;
				}
			}

			public string Scene
			{
				get
				{
					return this.scene ?? string.Empty;
				}
				set
				{
					this.scene = value ?? string.Empty;
				}
			}

			public bool Equals(HandmadeProfiler.SampleMemento other)
			{
				if (this.Frame != other.Frame)
				{
					return false;
				}
				if (this.Dt != other.Dt)
				{
					return false;
				}
				if (this.Scene != other.Scene)
				{
					return false;
				}
				if (!this.Components.SequenceEqual<string>(this.Components))
				{
					return false;
				}
				return true;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is HandmadeProfiler.SampleMemento))
				{
					return false;
				}
				return this.Equals((HandmadeProfiler.SampleMemento)obj);
			}

			public override int GetHashCode()
			{
				int hashCode = this.Frame.GetHashCode();
				double dt = this.Dt;
				return hashCode ^ dt.GetHashCode() ^ this.Scene.GetHashCode() ^ this.Components.GetHashCode();
			}

			public override string ToString()
			{
				return JsonUtility.ToJson(this);
			}
		}
	}
}