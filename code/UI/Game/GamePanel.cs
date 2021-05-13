using Sandbox;
using Sandbox.UI;

// HUD for game logs (chatbox, kill log)
namespace Extraction.UI
{
	[Library]
	public partial class GamePanel : Panel
	{
		public GamePanel()
		{
			AddChild<ChatPanel>();
			AddChild<InteractPanel>();
			// AddChild<PingPanel>();
		}	
	}
}
