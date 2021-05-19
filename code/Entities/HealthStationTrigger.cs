using System.Collections.Generic;
using Extraction.Actor;
using Sandbox;

namespace Extraction.Entities
{
	// This should all happen server-side and the client should never really know about it
	public class HealthStationTrigger : BaseTrigger
	{
		private Dictionary<Player, TimeSince> playerTimers = new Dictionary<Player, TimeSince>();
		private List<Player> currentlyInProximity = new List<Player>();

		private int HealAmt => 2; // How much to heal
		private float HealInterval => 0.1f; // How often to heal

		private const float Radius = 75;
		
		public override void Spawn()
		{
			SetupPhysicsFromAABB( PhysicsMotionType.Static, new Vector3( -Radius, -Radius, 0 ),
				new Vector3( Radius, Radius, Radius ) );
			CollisionGroup = CollisionGroup.Trigger;
		}

		[Event( "server.tick" )]
		public void OnServerTick()
		{
			foreach ( var player in currentlyInProximity )
			{
				if ( playerTimers.TryGetValue( player, out var timeSince ) )
				{
					if ( timeSince.Relative > HealInterval )
					{
						// TODO: Limit player health
						player.Health += HealAmt;
						playerTimers[player] = 0; // Reset player's timer so we don't heal them again for a while
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
				if ( currentlyInProximity.Contains( player ) )
					currentlyInProximity.Remove( player );
			}
			base.EndTouch( other );
		}
	}
}
