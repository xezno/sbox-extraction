using Extraction.Actor;
using Extraction.Game;
using Extraction.Hero;
using Sandbox;
using Extraction.UI;
using Sandbox.UI;

namespace Extraction
{
	[Library("extraction", Title = "Extraction")]
	partial class ExtractionGame : Sandbox.Game
	{
		private BaseGameState currentGameState;
		
		private static HudEntity<RootPanel> extractionHud;
		public ExtractionGame()
		{
			HeroCollection.Load();
			
			Log.Info( "Game Started" );

			if ( IsServer )
			{
				extractionHud = new ExtractionHud();
				currentGameState = new BaseGameState();
			}
		}

		// TODO: VERIFY: Does this command work?
		[ServerCmd( "recreatehud", Help = "Recreate hud object" )]
		public static void RecreateHud()
		{
			extractionHud.Delete();
			extractionHud = new ExtractionHud();
			
			Log.Info( "Recreated HUD" );
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new ExtractionPlayer();
			cl.Pawn = player;

			player.Respawn();
		}
	}
}
