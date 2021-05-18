using System.Collections.Generic;
using Extraction.Actor;
using Sandbox;
using Sandbox.ScreenShake;

namespace Extraction.Entities
{
	// This should all happen server-side and the client should never really know about it
	public class PhysicsDoorTrigger : BaseTrigger
	{
		public void SetupPhysics()
		{
			var mins = 0 * Vector3.Forward + -25 * Vector3.Left + -40 * Vector3.Up;
			var maxs = 50 * Vector3.Forward + 25 * Vector3.Left + 40 * Vector3.Up;
			// SetModel( (OwnerEntity as PhysicsDoor).GetModel() );
			// SetupPhysicsFromModel( PhysicsMotionType.Static );

			SetupPhysicsFromOBB( PhysicsMotionType.Static, mins, maxs );
			
			CollisionGroup = CollisionGroup.Trigger;
		}

		public override void StartTouch( Entity other )
		{
			if ( !IsServer ) return;
			if ( other is ExtractionPlayer player )
			{
				Log.Info( "Player nearby" );
				if ( player.Velocity.Length > 250 )
				{
					(Owner as PhysicsDoor).ForceOpen( player );
				}
			}
			base.StartTouch( other );
		}

		public override void EndTouch( Entity other )
		{
			if ( !IsServer ) return;
			if ( other is ExtractionPlayer player )
			{
				Log.Info( "Player fucked off" );
			}
			base.EndTouch( other );
		}
	}
}
