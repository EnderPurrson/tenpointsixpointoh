using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class MeshInfo
	{
		private readonly HashSet<MeshFilter> _meshFilters = new HashSet<MeshFilter>();

		public MeshInfo()
		{
		}

		public bool AddMeshFilter(MeshFilter meshFilter)
		{
			return this._meshFilters.Add(meshFilter);
		}

		public IList<MeshFilter> GetRenderers()
		{
			return this._meshFilters.ToList<MeshFilter>();
		}
	}
}