using Extraction.Actor;
using Extraction.Hero;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI.Menus
{
	public class HeroCard : Panel
	{
		// TODO: These suck and need re-doing
		
		public Label heroText;
		public Label heroDescription;
		private Image heroImage;

		private string heroId;
		public string HeroId { get => heroId; set { heroId = value; Recreate(); } }

		public HeroCard()
		{ 
			StyleSheet.Load( "/UI/ExtractionHud.scss" );
			StyleSheet.Load( "/UI/Menus/HeroSelect/HeroSelect.scss" );
		}

		private void Recreate()
		{
			if ( heroImage != null ) heroImage.Delete();
			if ( heroText != null ) heroText.Delete();
			
			var heroData = HeroCollection.HeroDatas[heroId];

			heroImage = Add.Image( heroData.Portrait, "hero-portrait" );
			var heroInfo = Add.Panel( "hero-info" );
			heroText = heroInfo.Add.Label( heroData.Name, "hero-text" );
			heroDescription = heroInfo.Add.Label( $"{heroData.Description}\n({heroData.Health}hp | {heroData.Speed}u/s | {heroData.BodyType} | {heroData.Class})", "hero-description" );
			
			AddEvent( "onclick", () =>
			{
				// Set hero
				// This feels like a hacky solution but I think it's the only way to run stuff server-side
				ConsoleSystem.Run( "sethero", heroId ); 
				ConsoleSystem.Run( "kill" );
				ConsoleSystem.Run( "changehero" ); // Toggle menu (hide)
			});
		}
	}
}
