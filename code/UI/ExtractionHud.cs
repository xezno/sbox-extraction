using Extraction.UI.Menus;
using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	[Library]
	public partial class ExtractionHud : Hud
	{
		private static ExtractionHud current;

		private HeroSelect heroSelect;
		
		public ExtractionHud()
		{
			if ( !IsClient ) return;

			current = this;
			
			RootPanel.AddChild<Crosshair>();

			RootPanel.AddChild<GearPanel>();
			RootPanel.AddChild<GamePanel>();
		}

		[ClientCmd( "changehero", CanBeCalledFromServer = false )]
		public static void CmdChangeHero()
		{
			if ( current.heroSelect == null )
			{
				current.heroSelect = current.RootPanel.AddChild<HeroSelect>();
			}
			else
			{
				current.heroSelect.Delete();
				current.heroSelect = null;
			}
		}
	}
}
