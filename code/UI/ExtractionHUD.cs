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

			RootPanel.StyleSheet.Load("/ui/Hud.scss");
			RootPanel.AddChild<ChatBox>();
		}
	}
}
