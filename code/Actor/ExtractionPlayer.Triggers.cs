using System;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		[Net]
		public TriggerType CurrentTriggerType { get; set; }
	
		
		// TODO: This enum should probably get moved to somewhere that makes snese (like ExtractionTrigger or whatever)
		[Flags]	
		public enum TriggerType
		{
			None = 0,
			Objective = 1,
			NoGadgets = 2,
		}

		private void DrawDebugShit()
		{
			DebugOverlay.ScreenText( new Vector2( 100, 100  ), $"Current trigger state: {CurrentTriggerType}" );
		}
	}
}
