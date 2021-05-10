using Extraction.Weapons;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class Ammo : Panel
	{
		public Label AmmoText;
		public Label ReserveAmmoText;

		public Ammo()
		{
			SetClass( "ammo", true );
			
			AmmoText = Add.Label( "30", "ammo-current" );
			ReserveAmmoText = Add.Label( "90", "ammo-reserve" );
		}

		public override void Tick()
		{
			var player = Player.Local;
			if ( player == null ) return;

			// if ( player.Inventory.Active is BaseExtractionWeapon weapon )
			// {
			// 	AmmoText.Text = weapon.AmmoClip.ToString();
			// }
		}
	}
}
