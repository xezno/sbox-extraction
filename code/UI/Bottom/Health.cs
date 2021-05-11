using Extraction.Actor;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class Health : Panel
	{
		public Label healthText;
		public Label heroText;

		public Health()
		{
			SetClass( "health", true );
			
			healthText = Add.Label( "🖤 100", "current-health" );
			heroText = Add.Label( "HeroName", "current-hero" );
		}

		public override void Tick()
		{
			var player = Player.Local as ExtractionPlayer;
			if ( player == null ) return;

			healthText.Text = $"🖤 {player.Health:n0}"; // TODO: Proper icons
			healthText.SetClass( "danger", player.Health < 40.0f );

			heroText.Text = $"Hero: {player.HeroId ?? "WTF"}";
		}
	}
}
