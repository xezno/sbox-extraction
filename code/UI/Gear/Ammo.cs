using Extraction.Actor;
using Extraction.Weapons;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class Ammo : Panel
	{
		public Panel ammoImage;
		public Label ammoText;
		public Label reserveAmmoText;
		public Label weaponNameText;
		public Panel ammoIcon;

		public Ammo()
		{
			SetClass( "ammo", true );

			ammoIcon = Add.Panel( "ammo-icon" );
			ammoText = Add.Label( "30", "ammo-current" );
			reserveAmmoText = Add.Label( "90", "ammo-reserve" );
			weaponNameText = Add.Label( "MP5SD", "weapon-name" );
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			if ( player.Inventory.Active is not ExtractionWeapon weapon )
			{
				// Hide this if we're not holding a weapon
				Style.Display = DisplayMode.None;
				Style.Dirty();
				return;
			}
			
			Style.Display = DisplayMode.Flex;
			Style.Dirty();
			
			var ammoInfo = GetAmmoInfo();
			
			var currentAmmo = ammoInfo.Item1;
			var reserveAmmo = ammoInfo.Item2;
			
			ammoText.Text = currentAmmo.ToString("D3");
			reserveAmmoText.Text = reserveAmmo.ToString( "D3" );
			
			weaponNameText.Text = weapon.WeaponName;
		}

		/// <returns>Current ammo, reserve ammo</returns>
		private (int, int) GetAmmoInfo()
		{
			var player = Local.Pawn;
			if ( player == null ) return (-1, -1);
			if ( player.Inventory?.Active == null ) return (-1, -1);
			
			if ( player.Inventory.Active is ExtractionWeapon weapon )
			{
				return (weapon.AmmoClip, weapon.AvailableAmmo());
			}
			else
			{
				return (-1, -1);
			}
		}
	}
}
