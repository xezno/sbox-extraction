using System;
using System.Threading.Tasks;
using Sandbox;

namespace Extraction.Camera
{
	public class PlayerCamera : FirstPersonCamera
	{
		private Vector3 lastPos;
		
		// High speed cutoff for FOV change
		private const float HighSpeed = 300f*300f;
		
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

			ApplyFov();
		}

		private void ApplyFov()
		{
			var player = Player.Local;
			
			float targetFov = ExtractionConfig.FieldOfView;
			if ( player.Velocity.WithZ( 0 ).LengthSquared > HighSpeed ) // Length^2 is faster than Length
				targetFov = ExtractionConfig.SprintFieldOfView;

			FieldOfView = FieldOfView.LerpTo( targetFov , 10.0f * Time.Delta, false );
		}
	}
}
