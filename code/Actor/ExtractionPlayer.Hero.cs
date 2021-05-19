using System;
using Extraction.Hero;
using Extraction.UI;
using Sandbox;
using Sandbox.UI;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		// Current hero ID
		[Net] public string HeroId { get; set; } // TODO: VERIFY: Does this need networking to all clients?

		// Hero ID to respawn as
		[Net] public string WishHeroId { get; set; } = ExtractionConfig.DefaultHero; // TODO: VERIFY: Does this need networking to all clients?

		public HeroData HeroData => HeroCollection.HeroDatas[HeroId];

		/// Called when the player decides they want to switch to a different hero
		public void ChangeHero( string newHeroId )
		{
			if ( !HeroCollection.HeroDatas.ContainsKey( newHeroId ) )
				return;
			
			// We want to change hero; access Hero directly, change the ID there
			ChatPanel.AddInformation( To.Single( Local.Pawn ), $"Hero {newHeroId} selected; this will change when you respawn" );
			WishHeroId = newHeroId;
		}
		
		/// Called when the player actually spawns in as a new hero
		public void SetupHero()
		{
			if ( WishHeroId == HeroId )
				return;

			// This shouldn't happen, but if we receive a bad hero ID, then revert it to default
			if ( !HeroCollection.HeroDatas.ContainsKey( WishHeroId ) )
				WishHeroId = ExtractionConfig.DefaultHero;

			// Don't print the message if we've just spawned in (which is the only case where HeroId should be empty)
			if ( !string.IsNullOrEmpty( HeroId ) )
			{
				string heroName = HeroCollection.HeroDatas[WishHeroId].Name;
				string oldHeroName = HeroCollection.HeroDatas[HeroId].Name;
				ChatPanel.AddInformation( To.Everyone, $"{Local.DisplayName} respawned as {heroName} (was {oldHeroName})" );
			}

			HeroId = WishHeroId;
			
			SetHeroControllerProperties();
			Dress();
		}

		/// Set up new hero stuff
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
