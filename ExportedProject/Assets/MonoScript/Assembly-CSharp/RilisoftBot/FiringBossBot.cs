using System;

namespace RilisoftBot
{
	public class FiringBossBot : FiringShotBot
	{
		public FiringBossBot()
		{
		}

		protected override void Initialize()
		{
			this.isMobChampion = true;
			base.Initialize();
		}
	}
}