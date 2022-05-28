using UnityEngine;

public class RocketSettings : MonoBehaviour
{
	public enum TypeFlyRocket
	{
		Rocket,
		Grenade,
		Bullet,
		MegaBullet,
		Autoaim,
		Bomb,
		AutoaimBullet,
		Ball,
		GravityRocket,
		Lightning,
		AutoTarget,
		StickyBomb,
		Ghost
	}

	public WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.explosion;

	public TypeFlyRocket typeFly;

	public Player_move_c.TypeKills typeKilsIconChat = Player_move_c.TypeKills.explosion;

	public Vector3 sizeBoxCollider = new Vector3(0.15f, 0.15f, 0.75f);

	public TrailRenderer trail;

	public int countJumpLightning = 2;

	public float raduisDetectTargetLightning = 5f;

	public float raduisDetectTarget = 5f;

	public float startForce = 190f;

	public float autoRocketForce = 15f;

	public float lifeTime = 7f;
}
