using Sandbox;

namespace Extraction.Weapons
{
	[Library( "ext_shotgun", Title = "Shotgun" )]
	internal class Shotgun : ExtractionWeapon
	{
		public override string WeaponName => "Shotgun";
		public override string ShotSound => "rust_pumpshotgun.shoot";
		public override float Spread => 0.2f;
		public override float Force => 1.5f;
		public override float Damage => 7.0f; // Per pellet
		public override float BulletSize => 3.0f;
		public override int ShotCount => 12;

		public override string ViewModelPath => "weapons/rust_pumpshotgun/v_rust_pumpshotgun.vmdl";
		public override string WorldModelPath => "weapons/rust_pumpshotgun/rust_pumpshotgun.vmdl";

		public override int ClipSize => 5;

		public override float PrimaryRate => 1.5f;
		public override float SecondaryRate => 1.0f;
		public override float ReloadTime => 0.7f;

		public override bool AutoFire => true;
		public override HoldType WeaponHoldType => HoldType.Rifle;

		public override int Slot => 0;

		//
		// Shotgun-specific reload.
		// Loads one shell at a time. Lets you pause the reload to shoot in-between shells.
		//
		public override void OnReloadFinish()
		{
			IsReloading = false;
			TimeSincePrimaryAttack = 0;
			
			// Local functions are ass but this is super useful
			void Finished() => ViewModelEntity?.SetAnimParam( "reload_finished", true ); 

			// Do we have a full mag?
			if ( AmmoClip >= ClipSize )
			{
				Finished(); // Yeah, fuck off
				return;
			}

			// Do we even have ammo?
			if ( AvailableAmmo() != 0 )
			{
				// There's ammo, take it
				AmmoClip++;
				ReserveAmmo--;
			}
			else
			{
				// No ammo, fuck off
				Finished();
				return;
			}
			
			// Janky way to pause reloads if we're between shells
			if ( Owner.Input.Down( InputButton.Attack1 ) )
			{
				Finished();
				return;
			}

			// Have we finished reloading?
			if ( AmmoClip < ClipSize && AvailableAmmo() > 0 )
			{
				// No, let's take another shell
				Reload();
			}
			else
			{
				// Yeah, fuck off
				Finished();
				return;
			}
		}
	}
}
