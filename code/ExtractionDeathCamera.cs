using Sandbox;

namespace Extraction
{
	// TODO: Polish later
	public class ExtractionDeathCamera : SpectateRagdollCamera
	{
		Vector3 FocusPoint;
		private Vector3 boxSize = Vector3.One * 10;
		
		public override void Activated()
		{			
			FocusPoint = LastPos - GetViewOffset();
			FieldOfView = ExtractionConfig.FieldOfView;
		}

		public override void Update()
		{
			var player = Player.Local as BasePlayer;
			if ( player == null ) return;

			// lerp the focus point
			FocusPoint = Vector3.Lerp( FocusPoint, GetSpectatePoint(), Time.Delta * 5.0f );

			// Cast ray from focus to target
			Vector3 targetPos = FocusPoint + GetViewOffset();
			var trace = Trace.Ray( FocusPoint, targetPos + boxSize );
			var traceResult = trace.Run();
			if ( traceResult.Hit )
			{
				// Hit? Move camera away from wall
				targetPos = traceResult.EndPos - boxSize;
			}
			
			// Set positions
			Pos = targetPos;
			Rot = player.EyeRot;

			FieldOfView = ExtractionConfig.FieldOfView;
			Viewer = null;
		}
	}
}
