using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		public TraceType CurrentTraceType { get; private set; }
		
		public enum TraceType
		{
			Objective,
			NoGadgets,
			
			None
		}
		
		// Do a trace for whatever's at the player's feet
		public TraceResult TracePlayerFeet( Vector3 start, Vector3 end, CollisionLayer layer )
		{
			Vector3 mins = Vector3.One * -1;
			Vector3 maxs = Vector3.One * 4;

			start += (Vector3.Up * 1f);

			var tr = Trace.Ray( start, end )
				.Size( mins, maxs )
				.HitLayer( layer )
				.Ignore( Player.Local )
				.Run();
			
			return tr;
		}
		
		// Do a trace for stuff like interactions etc.
		private bool CheckTrace()
		{
			if ( Health <= 0 )
				return false;
			
			// Essentially, each 'tool' type has its own unique surface & surface name, and we just check for those.
			// This probably isn't the best way to check for stuff.
			// I couldn't find a better way of doing this - hopefully we can do this nicer in the future
			var traceResult = TracePlayerFeet( Controller.Pos, Controller.Pos + (Vector3.Down * 100), CollisionLayer.STATIC_LEVEL );
			
			if ( traceResult.Hit )
			{
				switch ( traceResult.Surface.Name )
				{
					case "no-gadgets":
						CurrentTraceType = TraceType.NoGadgets;
						return true;
					case "objective":
						CurrentTraceType = TraceType.Objective;
						return true;
				}
			}

			CurrentTraceType = TraceType.None;
			return false;
		}
	}
}
