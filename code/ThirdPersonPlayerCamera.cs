using Sandbox;

namespace Extraction
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
