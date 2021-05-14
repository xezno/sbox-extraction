using System.Collections.Generic;
using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	// This should all happen server-side and the client should never really know about it
	public class HealthStationTrigger : BaseTrigger
	{
		// TODO: Perhaps attach this to each player instead?
		private Dictionary<Player, TimeSince> playerTimers = new Dictionary<Player, TimeSince>();
		private List<Player> currentlyInProximity = new List<Player>();
		
		public override void Spawn()
		{
			SetupPhysicsFromAABB( PhysicsMotionType.Static, new Vector3( -75, -75, -75 ), new Vector3( 75, 75, 75 ) );
			CollisionGroup = CollisionGroup.Trigger;
		}

		[Event( "server.tick" )]
		public void OnServerTick()
		{
			foreach ( var player in currentlyInProximity )
			{
				if ( playerTimers.TryGetValue( player, out var timeSince ) )
				{
					if ( timeSince.Relative > 1 )
					{
						// TODO: Limit player health
						player.Health += 15;
						playerTimers[player] = 0;
					}
				}
				else
				{
					playerTimers.Add( player, 0 );
				}
			}
		}

		public override void StartTouch( Entity other )
		{
			if ( !IsServer ) return;
			if ( other is ExtractionPlayer player )
			{
				Log.Info( "Something entered health station proximity..." );
				if ( !currentlyInProximity.Contains( player ) )
					currentlyInProximity.Add( player );
			}
			base.StartTouch( other );
		}

		public override void EndTouch( Entity other )
		{
			if ( !IsServer ) return;
			if ( other is ExtractionPlayer player )
			{
				Log.Info( "Something exited health station proximity..." );
				if ( currentlyInProximity.Contains( player ) )
					currentlyInProximity.Remove( player );
			}
			base.EndTouch( other );
		}
	}
}
