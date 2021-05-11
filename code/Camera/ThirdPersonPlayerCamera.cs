using Sandbox;

namespace Extraction.Camera
{
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
