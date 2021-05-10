using Sandbox;

namespace Extraction
{
	public class ExtractionDuck : Duck
	{
		public ExtractionDuck( BasePlayerController controller ) : base( controller ) { }
		
		public override void PreTick() 
		{
			bool wants = Controller.Input.Down( InputButton.Duck );

			if ( wants != IsActive ) 
			{
				if ( wants ) TryDuck();
				else TryUnDuck();
			}

			if ( IsActive )
			{
				Controller.SetTag( "ducked" );
				Controller.ViewOffset *= 0.7f;
			}
		}
		
		void TryDuck()
		{
			IsActive = true;
		}

		// Uck, saving off the bbox kind of sucks
		// and we should probably be changing the bbox size in PreTick
		Vector3 originalMins;
		Vector3 originalMaxs;
		
		void TryUnDuck()
		{
			var pm = Controller.TraceBBox( Controller.Pos, Controller.Pos, originalMins, originalMaxs );
			if ( pm.StartedSolid ) return;

			IsActive = false;
		}
	}
}
