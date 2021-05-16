using Sandbox;

namespace Extraction.Weapons
{
	[Library( "ext_smg", Title = "SMG" )]
	internal class SMG : ExtractionWeapon
	{
		public override string WeaponName => "MP5SD";
		public override string ShotSound => "mp5sd-shot";
		public override float Spread => 0.05f;
		public override float Force => 1.5f;
		public override float Damage => 9.0f;
		public override float BulletSize => 3.0f;
		public override int ShotCount => 1;

		public override string ViewModelPath => "weapons/mp5sd/v_mp5sd.vmdl";
		public override string WorldModelPath => "weapons/mp5sd/mp5sd.vmdl";

		public override string UiIcon => "weapons/mp5sd/mp5sd-icon.png";

		public override int ClipSize => 30;

		public override float PrimaryRate => 10.1f; // 202
		public override float SecondaryRate => 1.0f;
		public override float ReloadTime => 2.75f;

		public override bool AutoFire => true;
		public override HoldType WeaponHoldType => HoldType.Rifle;

		public override int Slot => 0;

		public override Vector3 AdsOffset => new( -11.23f, -5.25f, 3.5f );
	}
}
