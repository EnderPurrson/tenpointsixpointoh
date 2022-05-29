using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class BankIncrementSoundIndicator : MonoBehaviour
	{
		public float PlayDelay = 0.1f;

		public AudioClip ClipCoinAdded;

		public AudioClip ClipCoinsAdded;

		public AudioClip ClipGemAdded;

		public AudioClip ClipGemsAdded;

		public BankIncrementSoundIndicator()
		{
		}

		private void OnCurrencyGetted(bool isGems, int count)
		{
			float single = (Defs.isMulti || Defs.IsSurvival || !TrainingController.TrainingCompleted ? this.PlayDelay : 0f);
			base.StartCoroutine(this.PlaySounds(isGems, count < 2, single));
		}

		private void OnDisable()
		{
			CoinsMessage.CoinsLabelDisappeared -= new CoinsMessage.CoinsLabelDisappearedDelegate(this.OnCurrencyGetted);
		}

		private void OnEnable()
		{
			CoinsMessage.CoinsLabelDisappeared += new CoinsMessage.CoinsLabelDisappearedDelegate(this.OnCurrencyGetted);
		}

		[DebuggerHidden]
		private IEnumerator PlaySounds(bool isGems, bool oneCoin, float delay)
		{
			BankIncrementSoundIndicator.u003cPlaySoundsu003ec__Iterator108 variable = null;
			return variable;
		}
	}
}