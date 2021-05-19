using Extraction.UI.Menus;
using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	[Library]
	public partial class ExtractionHud : HudEntity<RootPanel>
	{
		private static ExtractionHud current;

		private HeroSelect heroSelect;
		
		public ExtractionHud()
		{
			if ( !IsClient ) return;

			current = this;
			
			RootPanel.AddChild<Crosshair>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();

			RootPanel.AddChild<GearPanel>();
			RootPanel.AddChild<GamePanel>();
		}

		/// <summary>
		/// Command that displays the 'change hero' UI on the client
		/// Server can't call this right now, but probably should be able to in case we need to force a hero change
		/// </summary>
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
