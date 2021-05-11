using Extraction.Hero;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		// Current hero ID
		[NetLocal] public string HeroId { get; set; }

		// Hero ID to respawn as
		[NetLocal] public string WishHeroId { get; set; } = "duke";

		public HeroData HeroData { get; set; } = new HeroData();

		public void SetupHero()
		{
			// Now that we have the ID, lets set up the hero data
			HeroData = FileSystem.Mounted.ReadJson<HeroData>( $"data/heroes/{WishHeroId}.json" );
			Log.Info( $"Extraction player spawned; current hero: {HeroData.Name}" );

			HeroId = WishHeroId;
		}
	}
}
