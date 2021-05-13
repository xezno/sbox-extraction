using Sandbox;

namespace Extraction.Weapons
{
	[Library( "ext_shotgun", Title = "Shotgun" )]
	internal class Shotgun : ExtractionWeapon
	{
		public override string WeaponName => "shotgun";
		public override string ShotSound => "rust_pumpshotgun.shoot";
		public override float Spread => 0.2f;
		public override float Force => 1.5f;
		public override float Damage => 60.0f;
		public override float BulletSize => 3.0f;
		public override int ShotCount => 8;

		public override string ViewModelPath => "weapons/rust_pumpshotgun/v_rust_pumpshotgun.vmdl";
		public override string WorldModelPath => "weapons/rust_pumpshotgun/rust_pumpshotgun.vmdl";

		public override int ClipSize => 6;

		public override float PrimaryRate => 1.5f;
		public override float SecondaryRate => 1.0f;
		public override float ReloadTime => 1.0f;

		public override bool AutoFire => false;
		public override HoldType WeaponHoldType => HoldType.Rifle;

		public override int Slot => 0;
	}
}
