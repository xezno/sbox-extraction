using System.Collections.Generic;
using Extraction.Actor;
using Extraction.Weapons;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class Loadout : Panel
	{
		private static Loadout Current;
		public List<Label> loadoutTexts = new List<Label>();

		public Loadout()
		{
			SetClass( "health", true );

			OnLoadoutUpdate();
			
			Current = this;
		}

		public static void OnLoadoutUpdate()
		{
			Current?.UpdateLoadout();
		}

		private void UpdateLoadout()
		{
			foreach ( var label in loadoutTexts )
				label.Delete();
			
			loadoutTexts.Clear();

			var inventory = Player.Local.Inventory;

			for ( int i = 0; i < 3; ++i )
			{
				var inventoryItem = inventory.GetSlot( i );

				if ( inventoryItem is BaseExtractionWeapon weapon )
				{
					Log.Info( weapon.EntityName );
					loadoutTexts.Add( Add.Label( weapon.EntityName ) );	
				}
			}
		}

		public override void Tick()
		{
			var player = Player.Local as ExtractionPlayer;
			if ( player == null ) return;
		}
	}
}
