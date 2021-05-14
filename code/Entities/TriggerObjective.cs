using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	[Library( "trigger_objective" )]
	public class TriggerObjective : ExtractionTrigger
	{
		public override ExtractionPlayer.TraceType TraceType => ExtractionPlayer.TraceType.Objective;
	}
}
