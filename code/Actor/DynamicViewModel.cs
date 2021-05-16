using System;
using Extraction.Camera;
using Extraction.Weapons;
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
		protected float BobCycleTime => 14f;
		protected Vector3 BobDirection => new( 0.0f, 1.0f, 0.5f );

		private Vector3 swayPos;
		private Vector3 aimPos;

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

			swayPos = camera.Pos;
			if ( Player.Local is ExtractionPlayer player &&
			     player.Inventory.Active is ExtractionWeapon { IsAimingDownSights: true } weapon )
			{
				(( PlayerCamera ) camera).TargetFov = ExtractionConfig.AdsFieldOfView;
				camera.ViewModelFieldOfView = 55;
				
				DebugOverlay.ScreenText(new Vector2(500, 490), aimPos.ToString());

				var targetAimTransform = WorldRot.Right * weapon.AdsOffset.x + WorldRot.Forward * weapon.AdsOffset.y +
				                     WorldRot.Up * weapon.AdsOffset.z;
				
				aimPos = Vector3.Lerp( aimPos, targetAimTransform, 25f * Time.Delta, true);
				DebugOverlay.ScreenText(new Vector2(500, 500), aimPos.ToString());
			}
			else
			{
				aimPos = Vector3.Lerp( aimPos, Vector3.Zero, 25f * Time.Delta, true);

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

				swayPos += WorldRot * offset;

				lastPitch = newPitch;
				lastYaw = newYaw;
			
				camera.ViewModelFieldOfView = 65;
			}

			WorldPos = camera.Pos + (aimPos);
			WorldRot = camera.Rot;
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
