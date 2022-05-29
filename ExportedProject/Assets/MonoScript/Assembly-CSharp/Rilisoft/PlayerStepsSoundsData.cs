using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class PlayerStepsSoundsData
	{
		public AudioClip Walk;

		public AudioClip Jump;

		public AudioClip JumpDown;

		public AudioClip WalkMech;

		public AudioClip WalkMechBear;

		public PlayerStepsSoundsData()
		{
		}

		public static PlayerStepsSoundsData Create(SkinName data)
		{
			PlayerStepsSoundsData playerStepsSoundsDatum = new PlayerStepsSoundsData()
			{
				Walk = data.walkAudio,
				Jump = data.jumpAudio,
				JumpDown = data.jumpDownAudio,
				WalkMech = data.walkMech,
				WalkMechBear = data.walkMechBear
			};
			return playerStepsSoundsDatum;
		}

		public bool IsSettedTo(SkinName data)
		{
			return (!(data.walkAudio == this.Walk) || !(data.jumpAudio == this.Jump) || !(data.jumpDownAudio == this.JumpDown) || !(data.walkMech == this.WalkMech) ? false : data.walkMechBear == this.WalkMechBear);
		}

		public void SetTo(SkinName data)
		{
			data.walkAudio = this.Walk;
			data.jumpAudio = this.Jump;
			data.jumpDownAudio = this.JumpDown;
			data.walkMech = this.WalkMech;
			data.walkMechBear = this.WalkMechBear;
		}
	}
}