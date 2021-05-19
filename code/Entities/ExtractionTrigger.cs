using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	/// <summary>
	/// Basic trigger code that we can attach to brushes/meshes in Hammer for stuff like
	/// objective locations, no-gadget zones, etc.
	/// </summary>
	public class ExtractionTrigger : BaseTrigger
	{
		public virtual ExtractionPlayer.TriggerType TriggerType => ExtractionPlayer.TriggerType.None;
		
		public override void StartTouch( Entity other )
		{
			if ( !IsServer ) return;
			if ( other is ExtractionPlayer player )
			{
				player.CurrentTriggerType |= TriggerType;
			}
			base.StartTouch( other );
		}

		public override void EndTouch( Entity other )
		{
			if ( !IsServer ) return;
			// POTENTIAL BUG: Nested entities (of same type) will cause this to fuck up
			if ( other is ExtractionPlayer player )
			{	
				player.CurrentTriggerType &= ~TriggerType;
			}
			base.EndTouch( other );
		}
	}
}
