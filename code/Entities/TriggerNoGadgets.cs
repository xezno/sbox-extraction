using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	[Library( "trigger_no_gadgets" )]
	public class TriggerNoGadgets : ExtractionTrigger
	{
		public override ExtractionPlayer.TraceType TraceType => ExtractionPlayer.TraceType.NoGadgets;
	}
}
