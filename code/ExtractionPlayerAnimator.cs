using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace Extraction
{
	public class ExtractionPlayerAnimator : StandardPlayerAnimator
	{
		// Keep track of state so that we can use it in multiple places at once (i.e. hero portrait)
		public Dictionary<string, object> Params { get; } = new();
		
		public override void SetParam( string name, bool val )
		{
			base.SetParam( name, val );
			if ( !Params.TryAdd( name, val ) )
				Params[name] = val;
		}

		public override void SetParam( string name, float val )
		{
			base.SetParam( name, val );
			if ( !Params.TryAdd( name, val ) )
				Params[name] = val;
		}

		public override void SetParam( string name, int val )
		{
			base.SetParam( name, val );
			if ( !Params.TryAdd( name, val ) )
				Params[name] = val;
		}

		public override void SetParam( string name, Vector3 val )
		{
			base.SetParam( name, val );
			if ( !Params.TryAdd( name, val ) )
				Params[name] = val;
		}
		
		public override void Trigger( string name )
		{
			// SetParam( name, true );
			base.SetParam( name, true ); // HACK: Don't go thru setParam otherwise jump looks weird in portrait 
		}
	}
}
