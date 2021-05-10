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

		public override void Update()
		{			
			var player = Player.Local;
			if ( player == null ) return;

			Pos = Vector3.Lerp( player.EyePos.WithZ( lastPos.z ), player.EyePos, 25.0f * Time.Delta );
			Rot = player.EyeRot;

			Viewer = player;
			lastPos = Pos;
		}
	}
}
