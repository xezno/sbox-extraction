using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	public class ExtractionTrigger : BaseTrigger
	{
		public virtual ExtractionPlayer.TraceType TraceType => ExtractionPlayer.TraceType.None;
		
		public override void StartTouch( Entity other )
		{
			if ( !IsServer ) return;
			if ( other is ExtractionPlayer player )
			{
				Log.Info( $"Player {player.Name} entered {TraceType} zone" );
				player.CurrentTraceType |= TraceType;
			}
			base.StartTouch( other );
		}

		public override void EndTouch( Entity other )
		{
			if ( !IsServer ) return;
			// POTENTIAL BUG: Nested entities (of same type) will cause this to fuck up
			if ( other is ExtractionPlayer player )
			{
				Log.Info( $"Player {player.Name} left {TraceType} zone" );	
				player.CurrentTraceType &= ~TraceType;
			}
			base.EndTouch( other );
		}
	}
}
