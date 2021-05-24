using Sandbox;
using Trace = Sandbox.Trace;

namespace Extraction.Camera
{
	// Spectator death camera
	public class DeathCamera : Sandbox.Camera
	{
		Vector3 FocusPoint;
		private float boxSize = 5;
		
		public override void Activated()
		{			
			FocusPoint = CurrentView.Position - GetViewOffset();
			FieldOfView = ExtractionConfig.FieldOfView;
		}

		public override void Update()
		{
			var player = Local.Pawn as Player;
			if ( player == null ) return;

			// lerp the focus point
			FocusPoint = Vector3.Lerp( FocusPoint, GetSpectatePoint(), Time.Delta * 50.0f );

			Vector3 targetPos = FocusPoint + GetViewOffset();
			
			// Col: cast ray from focus point to target
			var traceResult = Trace.Ray( FocusPoint, targetPos ).Radius( boxSize ).WorldOnly().Run();
			targetPos = traceResult.EndPos; // Shift camera away from wall
			
			// Set positions
			Pos = targetPos;
			Rot = player.EyeRot;

			FieldOfView = ExtractionConfig.FieldOfView;
			Viewer = null;
		}
		
		public Vector3 GetSpectatePoint()
		{
			var player = Local.Pawn as Player;
			if ( player == null ) return CurrentView.Position;
			if ( !player.Corpse.IsValid() ) return player.Position;

			return player.Corpse.PhysicsGroup.MassCenter;
		}

		public Vector3 GetViewOffset()
		{
			var player = Local.Pawn as Player;
			if ( player == null ) return Vector3.Zero;
			return player.EyeRot.Forward * (-130 * player.Scale) +
			       Vector3.Up * (20 * player.Scale);
		}
	}
}
