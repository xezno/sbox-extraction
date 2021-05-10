using Sandbox;

namespace Extraction.Weapons
{
	// Versatile generic gun which other weapons can derive from without having to make their own logic
	internal class GenericGun : BaseExtractionWeapon
	{
		public virtual string ShotSound => "rust_pistol.shoot";
		public virtual float Spread => 0.05f;
		public virtual float Force => 1.5f;
		public virtual float Damage => 9.0f;
		public virtual float BulletSize => 3.0f;
		public virtual int ShotCount => 1; // How many bullets to shoot per shot - e.g. 1 for pistol, 8 for shotty
		public virtual string WorldModelPath => "weapons/rust_smg/rust_smg.vmdl";

		public virtual bool AutoFire => false;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( WorldModelPath );
			AmmoClip = ClipSize;
		}

		public override bool CanPrimaryAttack()
		{
			var playerIsShooting =
				AutoFire ? Owner.Input.Down( InputButton.Attack1 ) : Owner.Input.Pressed( InputButton.Attack1 );

			return base.CanPrimaryAttack() && playerIsShooting;
		}

		public override void AttackPrimary()
		{
			TimeSincePrimaryAttack = 0;
			TimeSinceSecondaryAttack = 0;

			if ( !TakeAmmo( ShotCount ) )
			{
				Log.Info( AmmoClip.ToString() );
				DryFire();
				return;
			}

			ShootEffects();
			PlaySound( ShotSound );

			for ( var i = 0; i < ShotCount; ++i )
			{
				ShootBullet( Spread, Force, Damage, BulletSize );
			}
		}
	}
}
