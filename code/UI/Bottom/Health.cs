using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class Health : Panel
	{
		public Label HealthText;

		public Health()
		{
			SetClass( "health", true );
			
			HealthText = Add.Label( "100", "current-health" );
		}

		public override void Tick()
		{
			var player = Player.Local;
			if ( player == null ) return;

			HealthText.Text = $"{player.Health:n0}";
			HealthText.SetClass( "danger", player.Health < 40.0f );
		}
	}
}
