using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	// HUD for game logs (chatbox, kill log)
	
	[Library]
	public partial class GamePanel : Panel
	{
		public GamePanel()
		{
			AddChild<ChatPanel>();
			AddChild<InteractPanel>();
		}	
	}
}
