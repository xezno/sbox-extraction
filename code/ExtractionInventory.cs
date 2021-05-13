using Extraction.UI;
using Sandbox;

namespace Extraction
{
	public class ExtractionInventory : BaseInventory
	{
		public ExtractionInventory( Entity owner ) : base( owner ) { }

		public override bool SetActiveSlot( int i, bool evenIfEmpty = false )
		{
			bool val = base.SetActiveSlot( i, evenIfEmpty );
			Event.Run( "extraction.player.loadoutChange" );
			return val;
		}

		public override bool SetActive( Entity ent )
		{
			bool val = base.SetActive( ent );
			Event.Run( "extraction.player.loadoutChange" );
			return val;
		}
	}
}
