using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	// Ping wheel so that people can specify the type of shit they want
	public class PingPanel : Menu
	{
		public PingPanel()
		{
			StyleSheet.Load( "/UI/Game/PingPanel.scss" );
			Add.Label( "What the fuck" );
		}

		public override void Tick()
		{
			base.Tick();
		}
	}
}
