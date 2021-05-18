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
		protected float SwingInfluence => 0.15f;
		protected float ReturnSpeed => 25.0f;
		protected float MaxOffsetLength => 10.0f;
		protected float BobCycleTime => 14f;
		protected Vector3 BobDirection => new( 0.0f, 1.0f, 0.5f );

		private Vector3 swayPos;
		private Vector3 aimPos;

		private float lerpRate = 25f; // How quick the procedural ADS animation should be

		public override void PostCameraSetup( ref CameraSetup camSetup )
		{
			base.PostCameraSetup( ref camSetup );
			
			if ( !Local.Pawn.IsValid() )
				return;

			if ( !activated )
			{
				lastPitch = camSetup.Rotation.Pitch();
				lastYaw = camSetup.Rotation.Yaw();

				activated = true;
			}

			swayPos = camSetup.Position;
			
			if ( Local.Pawn is ExtractionPlayer player &&
			     player.Inventory.Active is ExtractionWeapon { IsAimingDownSights: true } weapon )
			{
				var targetAimTransform = WorldRot.Right * weapon.AdsOffset.x + WorldRot.Forward * weapon.AdsOffset.y +
				                     WorldRot.Up * weapon.AdsOffset.z;
				
				aimPos = aimPos.LerpTo( targetAimTransform, lerpRate * Time.Delta);
				
				(Local.Pawn.Camera as PlayerCamera).TargetFov = ExtractionConfig.AdsFieldOfView;
				camSetup.ViewModel.FieldOfView = camSetup.ViewModel.FieldOfView.LerpTo( 45, lerpRate * Time.Delta, true);
			}
			else
			{
				aimPos = aimPos.LerpTo( Vector3.Zero, lerpRate * Time.Delta );

				var newPitch = WorldRot.Pitch();
				var newYaw = WorldRot.Yaw();

				var pitchDelta = Angles.NormalizeAngle( newPitch - lastPitch );
				var yawDelta = Angles.NormalizeAngle( lastYaw - newYaw );

				var playerVelocity = Local.Pawn.Velocity;
				var verticalDelta = playerVelocity.z * Time.Delta;
				var viewDown = Rotation.FromPitch( newPitch ).Up * -1.0f;
				verticalDelta *= 1.0f - MathF.Abs( viewDown.Cross( Vector3.Down ).y );
				pitchDelta -= verticalDelta * 1;

				var offset = CalcSwingOffset( pitchDelta, yawDelta );
				offset += CalcBobbingOffset( playerVelocity );

				swayPos += WorldRot * offset;

				lastPitch = newPitch;
				lastYaw = newYaw;
			
				camSetup.ViewModel.FieldOfView = camSetup.ViewModel.FieldOfView.LerpTo( 65, 25f * Time.Delta, true );
			}

			WorldPos = swayPos + aimPos;
			WorldRot = camSetup.Rotation;
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
