using Extraction.Hero;
using Sandbox;
using Sandbox.UI;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		// Current hero ID
		[Net] public string HeroId { get; set; }

		// Hero ID to respawn as
		[Net] public string WishHeroId { get; set; } = ExtractionConfig.DefaultHero;

		public HeroData HeroData => HeroCollection.HeroDatas[HeroId];

		public void ChangeHero( string newHeroId )
		{
			if ( !HeroCollection.HeroDatas.ContainsKey( newHeroId ) )
				return;
			
			// We want to change hero; access Hero directly, change the ID there
			ExtractionChatPanel.AddInformation( this, $"Hero {newHeroId} selected; this will change when you respawn" );
			WishHeroId = newHeroId;
		}
		
		public void SetupHero()
		{
			// Now that we have the ID, lets set up the hero data
			if ( WishHeroId == HeroId )
				return;

			// This shouldn't happen, but if we receive a bad hero ID, then revert it to default
			if ( !HeroCollection.HeroDatas.ContainsKey( WishHeroId ) )
				WishHeroId = ExtractionConfig.DefaultHero;

			// Don't print the message if we've just spawned in (which is the only case where HeroId should be empty)
			if ( !string.IsNullOrEmpty(HeroId) )
				ExtractionChatPanel.AddInformation( Player.All, $"{Name} respawned as {WishHeroId} (was {HeroId})" ); 
			
			HeroId = WishHeroId;
		}
	}
}
