using Sandbox;

namespace Extraction
{
	public class PlayerController : WalkController
	{
		// TODO: Replace with custom controller for wall jumping etc.
		public PlayerController() : base()
		{
			Duck = new ExtractionDuck( this );
		}
	}
}
