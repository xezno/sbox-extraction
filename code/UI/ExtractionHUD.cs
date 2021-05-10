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
			
			// RootPanel.AddChild<ExtractionChatPanel>();
			RootPanel.AddChild<GearPanel>();
		}	
	}
}
