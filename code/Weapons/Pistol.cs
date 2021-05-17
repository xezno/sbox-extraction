using Sandbox;

// TODO: Move weapons into JSON files or something
namespace Extraction.Weapons
{
	[Library( "ext_pistol", Title = "Pistol" )]
	internal class Pistol : ExtractionWeapon
	{
		public override string WeaponName => "Pistol";
		public override string ShotSound => "rust_pistol.shoot";
		public override float Spread => 0.05f;
		public override float Force => 1.5f;
		public override float Damage => 20.0f;
		public override float BulletSize => 3.0f;
		public override int ShotCount => 1;

		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";
		public override string WorldModelPath => "weapons/rust_pistol/rust_pistol.vmdl";

		public override int ClipSize => 9;

		public override float PrimaryRate => 5.5f;
		public override float SecondaryRate => 1.0f;
		public override float ReloadTime => 3.0f;

		public override bool AutoFire => false;
		public override HoldType WeaponHoldType => HoldType.Pistol;

		public override int Slot => 0;

		public override Vector3 AdsOffset => new( -16.73f, -5.25f, 3f );
	}
}
