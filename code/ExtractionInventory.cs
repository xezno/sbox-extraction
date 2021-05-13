using Extraction.UI;
using Sandbox;

namespace Extraction
{
	public class ExtractionInventory : BaseInventory
	{
		public ExtractionInventory( Entity owner ) : base( owner ) { }

		public override bool SetActiveSlot( int i, bool evenIfEmpty = false )
		{
			var val = base.SetActiveSlot( i, evenIfEmpty );
			Event.Run( "extraction.player.loadoutChange" );
			return val;
		}

		public override bool SetActive( Entity ent )
		{
			var val = base.SetActive( ent );
			Event.Run( "extraction.player.loadoutChange" );
			return val;
		}
	}
}
