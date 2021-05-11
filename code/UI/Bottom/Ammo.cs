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

		public Ammo()
		{
			SetClass( "ammo", true );

			ammoImage = Add.Panel( "ammo-icon" );
			ammoText = Add.Label( "30", "ammo-current" );
			reserveAmmoText = Add.Label( "90", "ammo-reserve" );
		}

		public override void Tick()
		{
			var ammoInfo = GetAmmoInfo();
			
			var currentAmmo = ammoInfo.Item1;
			var reserveAmmo = ammoInfo.Item2;
			
			if ( currentAmmo < 0 )
			{
				ammoText.Text = "??";
				// ammoText.Style.Display = DisplayMode.None;
			}
			else
			{
				ammoText.Text = currentAmmo.ToString();
				// ammoText.Style.Display = DisplayMode.Flex;
			}

			if ( reserveAmmo < 0 )
			{
				reserveAmmoText.Text = "??";
				// reserveAmmoText.Style.Display = DisplayMode.None;
			}
			else
			{
				reserveAmmoText.Text = reserveAmmo.ToString();
				// reserveAmmoText.Style.Display = DisplayMode.Flex;
			}
		}

		/// <returns>Current ammo, reserve ammo</returns>
		private (int, int) GetAmmoInfo()
		{
			var player = Player.Local;
			if ( player == null ) return (-1, -1);
			if ( player.Inventory.Active == null ) return (-1, -1);
			
			if ( player.Inventory.Active is BaseExtractionWeapon weapon )
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
