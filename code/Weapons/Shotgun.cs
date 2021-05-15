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

		public override void OnReloadFinish()
		{
			IsReloading = false;
			TimeSincePrimaryAttack = 0;
			
			void Finished() => ViewModelEntity?.SetAnimParam( "reload_finished", true ); 

			if ( AmmoClip >= ClipSize )
			{
				Finished();
				return;
			}

			if ( AvailableAmmo() != 0 )
			{
				AmmoClip++;
				ReserveAmmo--;
			}
			else
			{
				Finished();
				return;
			}

			if ( Owner.Input.Down( InputButton.Attack1 ) ) // Janky way to pause reloads if we're between shells
			{
				Finished();
				return;
			}

			if ( AmmoClip < ClipSize && AvailableAmmo() > 0 )
			{
				Reload();
			}
			else
			{
				Finished();
			}
		}
	}
}
