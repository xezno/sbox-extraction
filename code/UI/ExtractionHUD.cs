using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	[Library]
	public partial class ExtractionHUD : Hud
	{
		public ExtractionHUD()
		{
			if (!IsClient) return;
			
			RootPanel.AddChild<Crosshair>();
			
			RootPanel.AddChild<GearPanel>();
			RootPanel.AddChild<GamePanel>();	
		}	
	}
}
