using System;

namespace RilisoftBot
{
	public class MeleeBossBot : MeleeBot
	{
		public MeleeBossBot()
		{
		}

		protected override void Initialize()
		{
			this.isMobChampion = true;
			base.Initialize();
		}
	}
}