using System;
using System.Threading.Tasks;
using Extraction.Actor;
using Sandbox;

namespace Extraction.Camera
{
	// First-person player camera
	public class PlayerCamera : FirstPersonCamera
	{
		private Vector3 lastPos;
		private int FovThreshold = 20;

		public float TargetFov { get; set; } = ExtractionConfig.FieldOfView;
		
		public override void Activated()
		{
			base.Activated();
			
			FieldOfView = ExtractionConfig.FieldOfView;
		}

		public override void Update()
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			Pos = Vector3.Lerp( player.EyePos.WithZ( lastPos.z ), player.EyePos, 25.0f * Time.Delta );
			Rot = player.EyeRot;

			Viewer = player;
			lastPos = Pos;

			ApplyFov();
		}

		private void ApplyFov()
		{
			var player = Local.Pawn as ExtractionPlayer;
			var playerController = player.GetActiveController() as ExtractionController;
			
			if ( player == null ) return;
			if ( playerController == null ) return;
			
			// TODO: Put this in movement
			//
			if ( player.Velocity.WithZ( 0 ).Length >= player.HeroData.Speed - FovThreshold )
			{
				TargetFov = ExtractionConfig.SprintFieldOfView;
			}

			FieldOfView = FieldOfView.LerpTo( TargetFov, 10.0f * Time.Delta, false );
			TargetFov = ExtractionConfig.FieldOfView;
		}
	}
}
