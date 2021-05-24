using Sandbox;

namespace Extraction.Camera
{
	// Third person camera for debugging
	public class ThirdPersonPlayerCamera : ThirdPersonCamera
	{
		public override void Activated()
		{
			base.Activated();
			FieldOfView = ExtractionConfig.FieldOfView;
		}
	}
}
