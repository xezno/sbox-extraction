using System;
using Sandbox;

namespace Extraction.Actor
{
	public class DynamicViewModel : BaseViewModel
	{
		private bool activated;
		private float bobAnim;
		private float lastPitch;
		private float lastYaw;

		private Vector3 swingOffset;
		protected float SwingInfluence => 0.05f;
		protected float ReturnSpeed => 5.0f;
		protected float MaxOffsetLength => 10.0f;
		protected float BobCycleTime => MathX.Clamp((Player.Local.Velocity.Length) / 22f, 1f, 20f);
		protected Vector3 BobDirection => new( 0.0f, 1.0f, 0.5f );

		public override void UpdateCamera( Sandbox.Camera camera )
		{
			if ( !Player.Local.IsValid() )
				return;

			if ( !activated )
			{
				lastPitch = camera.Rot.Pitch();
				lastYaw = camera.Rot.Yaw();

				activated = true;
			}

			WorldPos = camera.Pos;
			WorldRot = camera.Rot;

			camera.ViewModelFieldOfView = FieldOfView;

			var newPitch = WorldRot.Pitch();
			var newYaw = WorldRot.Yaw();

			var pitchDelta = Angles.NormalizeAngle( newPitch - lastPitch );
			var yawDelta = Angles.NormalizeAngle( lastYaw - newYaw );

			var playerVelocity = Player.Local.Velocity;
			var verticalDelta = playerVelocity.z * Time.Delta;
			var viewDown = Rotation.FromPitch( newPitch ).Up * -1.0f;
			verticalDelta *= 1.0f - MathF.Abs( viewDown.Cross( Vector3.Down ).y );
			pitchDelta -= verticalDelta * 1;

			var offset = CalcSwingOffset( pitchDelta, yawDelta );
			offset += CalcBobbingOffset( playerVelocity );

			WorldPos += WorldRot * offset;

			lastPitch = newPitch;
			lastYaw = newYaw;
		}

		protected Vector3 CalcSwingOffset( float pitchDelta, float yawDelta )
		{
			var swingVelocity = new Vector3( 0, yawDelta, pitchDelta );

			swingOffset -= swingOffset * ReturnSpeed * Time.Delta;
			swingOffset += swingVelocity * SwingInfluence;

			if ( swingOffset.Length > MaxOffsetLength )
			{
				swingOffset = swingOffset.Normal * MaxOffsetLength;
			}

			return swingOffset;
		}

		protected Vector3 CalcBobbingOffset( Vector3 velocity )
		{
			bobAnim += Time.Delta * BobCycleTime;

			var twoPI = MathF.PI * 2.0f;

			if ( bobAnim > twoPI )
			{
				bobAnim -= twoPI;
			}

			var speed = new Vector2( velocity.x, velocity.y ).Length;
			speed = speed > 10.0 ? speed : 0.0f;

			speed = MathX.Clamp( speed, 0, 400 );
			
			var offset = BobDirection * (speed * 0.005f) * MathF.Cos( bobAnim );
			offset = offset.WithZ( -MathF.Abs( offset.z ) );
			
			return offset;
		}
	}
}
