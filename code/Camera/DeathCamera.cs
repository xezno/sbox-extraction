using Sandbox;
using Trace = Sandbox.Trace;

namespace Extraction.Camera
{
	public class DeathCamera : BaseCamera
	{
		Vector3 FocusPoint;
		private float boxSize = 5;
		
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
			FocusPoint = Vector3.Lerp( FocusPoint, GetSpectatePoint(), Time.Delta * 50.0f );

			Vector3 targetPos = FocusPoint + GetViewOffset();
			
			// Col: cast ray from focus point to target
			var trace = Trace.Ray( FocusPoint, targetPos ).Radius( boxSize ).WorldOnly();
			var traceResult = trace.Run();
			if ( traceResult.Hit )
			{
				// Hit? Shift camera away from wall
				targetPos = traceResult.EndPos;
			}
			
			// Set positions
			Pos = targetPos;
			Rot = player.EyeRot;

			FieldOfView = ExtractionConfig.FieldOfView;
			Viewer = null;
		}
		
		public Vector3 GetSpectatePoint()
		{
			var player = Player.Local as BasePlayer;
			if ( player == null ) return LastPos;
			if ( !player.Corpse.IsValid() ) return player.WorldPos;

			return player.Corpse.PhysicsGroup.MassCenter;
		}

		public Vector3 GetViewOffset()
		{
			var player = Player.Local as BasePlayer;
			if ( player == null ) return Vector3.Zero;
			return player.EyeRot.Forward * (-130 * player.WorldScale) +
			       Vector3.Up * (20 * player.WorldScale);
		}
	}
}
