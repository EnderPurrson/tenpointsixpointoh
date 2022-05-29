using System;

namespace RilisoftBot
{
	public class ShootingBossBot : ShootingBot
	{
		public ShootingBossBot()
		{
		}

		protected override void Initialize()
		{
			this.isMobChampion = true;
			base.Initialize();
		}
	}
}