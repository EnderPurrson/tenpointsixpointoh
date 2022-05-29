using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class MaterialInfo
	{
		private readonly HashSet<Renderer> _renderers = new HashSet<Renderer>();

		public MaterialInfo()
		{
		}

		public bool AddRenderer(Renderer renderer)
		{
			return this._renderers.Add(renderer);
		}

		public IList<Renderer> GetRenderers()
		{
			return this._renderers.ToList<Renderer>();
		}
	}
}