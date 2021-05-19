using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	[Library( "trigger_no_gadgets" )]
	public class TriggerNoGadgets : ExtractionTrigger
	{
		public override ExtractionPlayer.TriggerType TriggerType => ExtractionPlayer.TriggerType.NoGadgets;
	}
}
