using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook.Unity
{
	internal class GraphResult : ResultBase, IGraphResult, IResult
	{
		public IList<object> ResultList
		{
			get
			{
				return JustDecompileGenerated_get_ResultList();
			}
			set
			{
				JustDecompileGenerated_set_ResultList(value);
			}
		}

		private IList<object> JustDecompileGenerated_ResultList_k__BackingField;

		public IList<object> JustDecompileGenerated_get_ResultList()
		{
			return this.JustDecompileGenerated_ResultList_k__BackingField;
		}

		private void JustDecompileGenerated_set_ResultList(IList<object> value)
		{
			this.JustDecompileGenerated_ResultList_k__BackingField = value;
		}

		public Texture2D Texture
		{
			get
			{
				return JustDecompileGenerated_get_Texture();
			}
			set
			{
				JustDecompileGenerated_set_Texture(value);
			}
		}

		private Texture2D JustDecompileGenerated_Texture_k__BackingField;

		public Texture2D JustDecompileGenerated_get_Texture()
		{
			return this.JustDecompileGenerated_Texture_k__BackingField;
		}

		private void JustDecompileGenerated_set_Texture(Texture2D value)
		{
			this.JustDecompileGenerated_Texture_k__BackingField = value;
		}

		internal GraphResult(WWW result) : base(result.text, result.error, false)
		{
			this.Init(this.RawResult);
			if (result.error == null)
			{
				this.Texture = result.texture;
			}
		}

		private void Init(string rawResult)
		{
			if (string.IsNullOrEmpty(rawResult))
			{
				return;
			}
			object obj = Json.Deserialize(this.RawResult);
			IDictionary<string, object> strs = obj as IDictionary<string, object>;
			if (strs != null)
			{
				this.ResultDictionary = strs;
				return;
			}
			IList<object> objs = obj as IList<object>;
			if (objs == null)
			{
				return;
			}
			this.ResultList = objs;
		}
	}
}