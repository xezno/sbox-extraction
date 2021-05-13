using System;
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

		// Called when the player decides they want to switch to a different hero
		public void ChangeHero( string newHeroId )
		{
			if ( !HeroCollection.HeroDatas.ContainsKey( newHeroId ) )
				return;
			
			// We want to change hero; access Hero directly, change the ID there
			ChatPanel.AddInformation( this, $"Hero {newHeroId} selected; this will change when you respawn" );
			WishHeroId = newHeroId;
		}
		
		// Called when the player actually spawns in as a new hero
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
				ChatPanel.AddInformation( Player.All, $"{Name} respawned as {WishHeroId} (was {HeroId})" );

			HeroId = WishHeroId;
			
			// I want to bang my head against a wall.
			SetHeroControllerProperties();
		}

		private void SetHeroControllerProperties()
		{
			if ( Controller is ExtractionController controller )
			{
				// Do we even need walking??
				// TODO: Remove walking when the new pawn stuff gets pushed
				
				controller.WalkSpeed = HeroData.Speed * .5f;
				controller.DefaultSpeed = HeroData.Speed * .5f;
				controller.SprintSpeed = HeroData.Speed;
				Log.Info( $"Setting walk speed to {controller.WalkSpeed} {IsServer}" );
			}
		}
	}
}
