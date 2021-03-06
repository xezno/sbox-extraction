using Extraction.Actor;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class InteractPanel : Menu
	{
		private Label text;
		
		public InteractPanel()
		{
			StyleSheet.Load( "/UI/Game/InteractPanel.scss" );
			text = Add.Label( "Press USE to interact" );
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null ) return;
			
			if ( player is ExtractionPlayer extractionPlayer )
			{
				if ( extractionPlayer.CurrentTriggerType.HasFlag(ExtractionPlayer.TriggerType.Objective) )
					text.Style.Display = DisplayMode.Flex;
				else
					text.Style.Display = DisplayMode.None;
				
				text.Style.Dirty();
			}
		}
	}
}
