﻿using Sandbox;
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
	internal partial class BaseExtractionWeapon : BaseWeapon
	{
		public virtual int Slot => 0;
		public virtual int ClipSize => 16;
		public virtual float ReloadTime => 3.0f;

		[NetPredicted] public int AmmoClip { get; set; }
		[NetPredicted] public TimeSince TimeSinceReload { get; set; }
		[NetPredicted] public bool IsReloading { get; set; }
		[NetPredicted] public TimeSince TimeSinceDeployed { get; set; }

		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

		public int AvailableAmmo()
		{
			return int.MaxValue; // TODO: Player ammo
		}

		public override void ActiveStart( Entity ent )
		{
			base.ActiveStart( ent );
			TimeSinceDeployed = 0;
		}

		public override void Reload()
		{
			if ( IsReloading )
			{
				return;
			}

			if ( AmmoClip >= ClipSize )
			{
				return;
			}

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
			AmmoClip = ClipSize;
		}

		[ClientRpc]
		public virtual void StartReloadEffects()
		{
			ViewModelEntity?.SetAnimParam( "reload", true );

			// TODO - player third person model reload
		}

		public override void AttackPrimary()
		{
			TimeSincePrimaryAttack = 0;
			TimeSinceSecondaryAttack = 0;

			//
			// Tell the clients to play the shoot effects
			//
			ShootEffects();

			//
			// ShootBullet is coded in a way where we can have bullets pass through shit
			// or bounce off shit, in which case it'll return multiple results
			//
			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward * 5000 ) )
			{
				tr.Surface.DoBulletImpact( tr );

				if ( !IsServer )
				{
					continue;
				}

				if ( !tr.Entity.IsValid() )
				{
					continue;
				}

				//
				// We turn predictiuon off for this, so aany exploding effects don't get culled etc
				//
				using ( Prediction.Off() )
				{
					var damage = DamageInfo.FromBullet( tr.EndPos, Owner.EyeRot.Forward * 100, 15 )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damage );
				}
			}
		}

		[ClientRpc]
		protected virtual void ShootEffects()
		{
			Host.AssertClient();

			Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );

			if ( Owner == Player.Local )
			{
				new Perlin();
			}

			ViewModelEntity?.SetAnimParam( "fire", true );
			CrosshairPanel?.OnEvent( "fire" );
		}

		public new virtual bool CanPrimaryAttack()
		{
			return base.CanPrimaryAttack() && Owner.Health > 0;
		}

		/// <summary>
		///     Shoot a single bullet
		/// </summary>
		public virtual void ShootBullet( float spread, float force, float damage, float bulletSize )
		{
			var forward = Owner.EyeRot.Forward;
			forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
			forward = forward.Normal;

			//
			// ShootBullet is coded in a way where we can have bullets pass through shit
			// or bounce off shit, in which case it'll return multiple results
			//
			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * 5000, bulletSize ) )
			{
				tr.Surface.DoBulletImpact( tr );

				if ( !IsServer )
				{
					continue;
				}

				if ( !tr.Entity.IsValid() )
				{
					continue;
				}

				//
				// We turn predictiuon off for this, so any exploding effects don't get culled etc
				//
				using ( Prediction.Off() )
				{
					var damageInfo = DamageInfo.FromBullet( tr.EndPos, forward * 100 * force, damage )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damageInfo );
				}
			}
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
			// TODO: Stop this repeating on weapons with auto fire
			PlaySound( "pistol-dryfire" );
		}

		public override void CreateViewModel()
		{
			Host.AssertClient();

			if ( string.IsNullOrEmpty( ViewModelPath ) )
			{
				return;
			}

			ViewModelEntity = new BaseViewModel();
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
