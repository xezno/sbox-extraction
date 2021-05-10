using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	public class Crosshair : Panel
	{
		public Crosshair()
		{
			StyleSheet.Load( "/ui/Crosshair.scss" );
			SetClass( "crosshair", true );
			SetClass( "crosshair-cross", true );

			Add.Panel( "crosshair-n" );
			Add.Panel( "crosshair-s" );
			Add.Panel( "crosshair-e" );
			Add.Panel( "crosshair-w" );
		}

		public override void Tick()
		{
			// Test values
			// TODO: Hook up to weapon
			float size = 32.0f + (64.0f * (Player.Local.Velocity.Length / 320f));
			
			Style.Width = Length.Pixels(size);
			Style.Height = Length.Pixels(size);

			Style.MarginLeft = Length.Pixels( -size / 2.0f );
			Style.MarginTop = Length.Pixels( -size / 2.0f );
			
			Style.Dirty();
		}
	}
}
