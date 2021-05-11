using Extraction.Actor;
using Sandbox;
using Extraction.UI;

namespace Extraction
{
	[Library("extraction", Title = "Addon")]
	partial class ExtractionGame : Game
	{
		public ExtractionGame()
		{
			Log.Info("Game Started");
			if (IsServer)
				new ExtractionHUD();
		}

		public override Player CreatePlayer() => new ExtractionPlayer();
	}
}
