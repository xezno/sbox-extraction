using Sandbox;

namespace Extraction
{
	public class ExtractionCamera : FirstPersonCamera
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

			Pos = Vector3.Lerp( player.EyePos.WithZ( lastPos.z ), player.EyePos, 100.0f * Time.Delta );
			Rot = player.EyeRot;

			Viewer = player;
			lastPos = Pos;
		}
	}
}
