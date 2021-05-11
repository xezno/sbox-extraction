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
			ReserveAmmo = AmmoClip * 4;
		}

		public override bool CanPrimaryAttack()
		{
			var hasAmmo = AmmoClip > 0;
			var canAutoFire = AutoFire && hasAmmo; // Prevent the gun from spamming dry sounds
			
			var playerIsShooting =
				(canAutoFire && Owner.Input.Down( InputButton.Attack1 )) || Owner.Input.Pressed( InputButton.Attack1 );

			var playerIsAlive = Owner.Health > 0;
			
			var gunCanFire = (PrimaryRate <= 0) || (TimeSincePrimaryAttack > (1 / PrimaryRate));


			return playerIsShooting && gunCanFire && playerIsAlive;
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

			if ( AmmoClip <= 3 && ClipSize > 3 )
			{
				// Nearing end of clip - alert player
				PlaySound( "pistol-dryfire" );	
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
