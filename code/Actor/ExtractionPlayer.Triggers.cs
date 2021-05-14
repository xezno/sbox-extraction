using System;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		[Net]
		public TraceType CurrentTraceType { get; set; }
	
		[Flags]	
		public enum TraceType
		{
			None = 0,
			Objective = 1,
			NoGadgets = 2,
		}

		private void DrawDebugShit()
		{
			DebugOverlay.ScreenText( new Vector2( 100, 100  ), $"Current trigger state: {CurrentTraceType}" );
		}
	}
}
