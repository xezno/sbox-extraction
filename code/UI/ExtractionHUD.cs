using Extraction.UI.Menus;
using Sandbox;

namespace Extraction.UI
{
	[Library]
	public partial class ExtractionHUD : Hud
	{
		private static ExtractionHUD current;

		private HeroSelect heroSelect;
		
		public ExtractionHUD()
		{
			if ( !IsClient ) return;

			current = this;

			RootPanel.AddChild<Pings>();
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
