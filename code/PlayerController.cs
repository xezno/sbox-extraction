using Sandbox;

namespace Extraction
{
	public class PlayerController : WalkController
	{
		public PlayerController() : base()
		{
			Duck = new ExtractionDuck( this );
		}
	}
}
