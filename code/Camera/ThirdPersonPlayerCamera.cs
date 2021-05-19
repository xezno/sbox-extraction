using Sandbox;

namespace Extraction.Camera
{
	// Third person camera for debugging
	public class ThirdPersonPlayerCamera : ThirdPersonCamera
	{
		private Vector3 lastPos;
		
		public override void Activated()
		{
			base.Activated();
			FieldOfView = ExtractionConfig.FieldOfView;
		}
	}
}
