using Extraction.Actor;
using Extraction.Hero;
using Sandbox;
using Extraction.UI;

namespace Extraction
{
	[Library("extraction", Title = "Addon")]
	partial class ExtractionGame : Game
	{
		private static Hud extractionHud;
		public ExtractionGame()
		{
			HeroCollection.Load();
			
			Log.Info( "Game Started" );
			
			if (IsServer)
				extractionHud = new ExtractionHud();
		}

		[ServerCmd( "recreatehud", Help = "Recreate hud object" )]
		public static void RecreateHud()
		{
			extractionHud.Delete();
			extractionHud = new ExtractionHud();
			
			Log.Info( "Recreated HUD" );
		}
		
		public override void DoPlayerSuicide( Player player )
		{
			if ( player.LifeState != LifeState.Alive ) 
				return;

			DamageInfo damage = DamageInfo.Generic( 1000.0f )
				.WithAttacker( player );

			player.TakeDamage( damage );
		}

		public override Player CreatePlayer() => new ExtractionPlayer();
	}
}
