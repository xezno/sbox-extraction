using Sandbox;

namespace Extraction.Actor
{
	public class Controller : WalkController
	{
		// TODO: Replace with custom controller for wall jumping etc.
		public Controller() : base()
		{
			Duck = new Duck( this );
		}
	}
}
