using Sandbox;

namespace Extraction.Actor
{
	/// <summary>
	/// TODO
	/// </summary>
	public class ExtractionController : WalkController
	{
		// TODO: Replace with custom controller for wall jumping etc.
		public ExtractionController() : base()
		{
			Duck = new Duck( this );
		}
	}
}
