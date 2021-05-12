using Extraction.Actor;
using Sandbox.UI.Construct;
using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	// TODO
	public class HeroPortrait : Panel
	{
		public Label heroText;
		private Image heroImage;

		public HeroPortrait()
		{
			Style.FlexWrap = Wrap.Wrap;
			Style.JustifyContent = Justify.Center; 
			Style.AlignItems = Align.Center;
			Style.AlignContent = Align.Center;
			StyleSheet.Parse( "image { width: 128px; height: 128px; justify-content: center; align-items: center; }" );

			Add.Image( "ui/extraction/hero-portrait-placeholder.png", "hero-image" );
			
			heroText = Add.Label( "HeroName", "current-hero" );
			heroText.Style.Set( "font-size: 18px" );
		}

		public override void Tick()
		{
			base.Tick();
			
			var player = Player.Local as ExtractionPlayer;
			if ( player == null ) return;
			
			heroText.Text = $"{player.HeroData.Name}";
		}

		[Event( "hero_change" )] // TODO
		public void UpdateHeroPortrait()
		{
			heroImage = Add.Image( "ui/extraction/hero-portrait-placeholder.png", "hero-image" );
		}
	}
}
