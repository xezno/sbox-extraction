using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	// HUD for player gear stuff (health, ammo, loadout...)
	
	[Library]
	public partial class GearPanel : Panel
	{
		public GearPanel()
		{
			StyleSheet.Load( "/ui/ExtractionHud.scss" );
			
			SetClass( "bottom", true );
			
			AddChild<HeroPortrait>();
			AddChild<Health>();
			AddChild<Ammo>();
		}	
	}
}
