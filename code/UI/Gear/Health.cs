using Extraction.Actor;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class Health : Panel
	{
		public Label healthText;
		public Panel healthIcon;

		public Health()
		{
			SetClass( "health", true );

			healthIcon = Add.Panel( "health-icon" );
			healthText = Add.Label( "0", "current-health" );
		}

		public override void Tick()
		{
			var player = Local.Pawn as ExtractionPlayer;
			if ( player == null ) return;

			healthText.Text = player.Health.ToString("D0");
			healthText.SetClass( "danger", player.Health < 33.3f );
		}
	}
}
