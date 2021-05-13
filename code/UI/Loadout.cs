using System.Collections.Generic;
using Extraction.Actor;
using Extraction.Weapons;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

// TODO: Improvements - make dirty flag work, make UI dynamic

namespace Extraction.UI
{
	public class Loadout : Panel
	{
		public static Loadout Current { get; private set; }
		public List<Label> loadoutTexts = new List<Label>();

		private bool inventoryDirty = false;

		public Loadout()
		{
			SetClass( "loadout", true );

			inventoryDirty = true;
			Current = this;
			
			Event.Register( this );
		}

		private void UpdateLoadout()
		{
			if (!inventoryDirty)
				return;
			
			foreach ( var label in loadoutTexts )
				label.Delete();

			loadoutTexts = new();

			var inventory = Player.Local.Inventory;

			for ( int i = 0; i < 3; ++i )
			{
				var inventoryItem = inventory.GetSlot( i );

				if ( inventoryItem is ExtractionWeapon weapon )
				{
					var weaponStr = $"{i+1} {weapon.WeaponName ?? "Shit"}";
					
					var label = Add.Label( weaponStr );
					loadoutTexts.Add( label );	
				}
			}

			inventoryDirty = false;
		}

		[Event( "extraction.player.loadoutChange" )]
		public void InventoryChange()
		{
			Log.Info( "Marked inventory as dirty" );
			inventoryDirty = true;
		}

		public override void Tick()
		{
			var player = Player.Local as ExtractionPlayer;
			if ( player == null ) return;
			
			UpdateLoadout();
		}
	}
}
