using Sandbox;

namespace Extraction
{
	public class ExtractionController : WalkController
	{
		public ExtractionController() : base()
		{
			Duck = new ExtractionDuck( this );
		}
	}
}
