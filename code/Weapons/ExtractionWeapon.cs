using Extraction.Actor;
using Sandbox;
using Sandbox.ScreenShake;

/* TODO:
 * - ADS
 * - Damage ranges
 * - Player hit(marker) SFX
 * - Ammo stuff
 * - Weapon sway
 */
namespace Extraction.Weapons
{
	internal partial class ExtractionWeapon : BaseWeapon
	{
		public enum HoldType
		{
			None,
			Pistol,
			Rifle
		}
		
		public virtual int Slot => 0;
		public virtual int ClipSize => 16;
		public virtual float ReloadTime => 3.0f;
		public virtual string WeaponName { get; set; }
		public virtual string ShotSound => "rust_pistol.shoot";
		public virtual string DryFireSound => "pistol-dryfire";
		public virtual string ShootParticles => "particles/pistol_muzzleflash.vpcf";
		public virtual float Spread => 0.05f;
		public virtual float Force => 1.5f;
		public virtual float Damage => 9.0f;
		public virtual float BulletSize => 3.0f;
		public virtual int ShotCount => 1; // How many bullets to shoot per shot - e.g. 1 for pistol, 8 for shotty
		public virtual float TimeBetweenShots => 0; // For burst fire n stuff
		public virtual string WorldModelPath => "weapons/rust_smg/rust_smg.vmdl";

		public virtual bool AutoFire => false;

		public virtual HoldType WeaponHoldType => HoldType.Pistol;

		[NetPredicted] public int ReserveAmmo { get; set; }
		[NetPredicted] public int AmmoClip { get; set; }
		[NetPredicted] public TimeSince TimeSinceReload { get; set; }
		[NetPredicted] public bool IsReloading { get; set; }
		[NetPredicted] public TimeSince TimeSinceDeployed { get; set; }
		
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";
		
		public override void Spawn()
		{
			base.Spawn();

			SetModel( WorldModelPath );
			AmmoClip = ClipSize;
			ReserveAmmo = AmmoClip * 4;
		}
		
		public override bool CanPrimaryAttack()
		{
			bool canAutoFire;
			{
				bool hasAmmo = AmmoClip > 0;
				canAutoFire = AutoFire && hasAmmo; // Prevent the gun from spamming dry sounds
			}
			
			bool playerIsShooting =
				(canAutoFire && Owner.Input.Down( InputButton.Attack1 )) || Owner.Input.Pressed( InputButton.Attack1 );

			bool playerIsAlive = Owner.Health > 0;
			
			bool gunCanFire = (PrimaryRate <= 0) || (TimeSincePrimaryAttack > (1 / PrimaryRate));


			return playerIsShooting && gunCanFire && playerIsAlive;
		}
		
		
		public override void AttackPrimary()
		{
			TimeSincePrimaryAttack = 0;
			TimeSinceSecondaryAttack = 0;

			if ( !TakeAmmo( 1 ) )
			{
				Log.Info( AmmoClip.ToString() );
				DryFire();
				return;
			}
			
			ShootEffects();
			PlaySound( ShotSound );

			for ( int i = 0; i < ShotCount; ++i )
			{
				ShootBullet( Spread, Force, Damage, BulletSize, i == 0 );
			}
		}

		public int AvailableAmmo()
		{
			return ReserveAmmo;
		}

		public override void ActiveStart( Entity ent )
		{
			base.ActiveStart( ent );
			TimeSinceDeployed = 0;
		}

		public override void Reload()
		{
			if ( IsReloading )
				return;

			if ( AmmoClip >= ClipSize )
				return;

			if ( ReserveAmmo <= 0 )
				return;
			
			TimeSinceReload = 0;

			IsReloading = true;
			Owner.SetAnimParam( "b_reload", true );
			StartReloadEffects();
		}

		public override void OnPlayerControlTick( Player owner )
		{
			if ( TimeSinceDeployed < 0.6f )
			{
				return;
			}

			if ( !IsReloading )
			{
				base.OnPlayerControlTick( owner );
			}

			if ( IsReloading && TimeSinceReload > ReloadTime )
			{
				OnReloadFinish();
			}
		}

		public virtual void OnReloadFinish()
		{
			IsReloading = false;
			
			if (ReserveAmmo > ClipSize - AmmoClip)
			{
				ReserveAmmo -= ClipSize - AmmoClip;
				AmmoClip = ClipSize;
			}
			else
			{
				AmmoClip += ReserveAmmo;
				ReserveAmmo = 0;
			}
		}
		
		public override void TickPlayerAnimator( PlayerAnimator anim )
		{
			anim.SetParam( "holdtype", (int)WeaponHoldType );
			anim.SetParam( "aimat_weight", 1.0f );
		}

		[ClientRpc]
		public virtual void StartReloadEffects()
		{
			ViewModelEntity?.SetAnimParam( "reload", true );

			// TODO - player third person model reload
		}

		public override bool CanSecondaryAttack()
		{
			return base.CanSecondaryAttack() && Owner.Health > 0;
		}

		public override void AttackSecondary()
		{
			base.AttackSecondary();
			return;
			// TODO: ADS
			Log.Info( "Secondary Attack" );
			
			AimDownSights();
		}

		[ClientRpc]
		public virtual void AimDownSights()
		{
			Log.Info( ViewModelEntity.LocalPos.ToString() );
			ViewModelEntity.LocalPos = new Vector3( 0, 0.5f, 0 );
		}

		[ClientRpc]
		protected virtual void ShootEffects()
		{
			Host.AssertClient();

			Particles.Create( ShootParticles, EffectEntity, "muzzle" );

			if ( Owner == Player.Local )
			{
				_ = new Perlin();
			}

			ViewModelEntity?.SetAnimParam( "fire", true );
			CrosshairPanel?.OnEvent( "fire" );
		}

		/// <summary>
		///     Shoot a single bullet
		/// </summary>
		public virtual void ShootBullet( float spread, float force, float damage, float bulletSize, bool playAudio )
		{
			Vector3 forward = Owner.EyeRot.Forward;
			forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
			forward = forward.Normal;

			// ShootBullet is coded in a way where we can have bullets pass through shit
			// or bounce off shit, in which case it'll return multiple results
			foreach ( var traceResult in TraceBullet( Owner.EyePos, Owner.EyePos + forward * 5000, bulletSize ) )
			{
				traceResult.Surface.DoBulletImpact( traceResult );

				if ( !IsServer )
					continue;

				if ( !traceResult.Entity.IsValid() )
					continue;

				// We turn prediction off for this, so any exploding effects don't get culled etc
				using ( Prediction.Off() )
				{
					var damageInfo = DamageInfo.FromBullet( traceResult.EndPos, forward * 100 * force, damage )
						.UsingTraceResult( traceResult )
						.WithAttacker( Owner )
						.WithWeapon( this );
					
					traceResult.Entity.TakeDamage( damageInfo );
					
					if (!traceResult.Entity.IsWorld && playAudio)
						PlayHitmarkerSound();
				}
			}
		}

		[ClientRpc]
		public virtual void PlayHitmarkerSound()
		{
			PlaySound( "hitmarker-temp" );
		}

		public bool TakeAmmo( int amount )
		{
			if ( AmmoClip < amount )
			{
				return false;
			}

			AmmoClip -= amount;
			return true;
		}

		[ClientRpc]
		public virtual void DryFire()
		{
			PlaySound( DryFireSound );
		}

		public override void CreateViewModel()
		{
			Host.AssertClient();

			if ( string.IsNullOrEmpty( ViewModelPath ) )
			{
				return;
			}

			ViewModelEntity = new DynamicViewModel();
			ViewModelEntity.WorldPos = WorldPos;
			ViewModelEntity.Owner = Owner;
			ViewModelEntity.EnableViewmodelRendering = true;
			ViewModelEntity.SetModel( ViewModelPath );
		}

		public bool IsUsable()
		{
			if ( AmmoClip > 0 )
			{
				return true;
			}

			return AvailableAmmo() > 0;
		}
	}
}
