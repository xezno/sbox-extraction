using Sandbox;
using System;
using System.Linq;

namespace Extraction.Actor
{
	partial class ExtractionPlayer
	{
		// TODO - make ragdolls one per entity
		static EntityLimit RagdollLimit = new EntityLimit { MaxTotal = 10 }; // This works now

		[ClientRpc]
		void BecomeRagdollOnClient( Vector3 force, int forceBone )
		{
			// TODO - lets not make everyone write this shit out all the time
			// maybe a CreateRagdoll<T>() on ModelEntity?
			ModelEntity ent = new();
			ent.Position = Position;
			ent.Rotation = Rotation;
			ent.MoveType = MoveType.Physics;
			ent.UsePhysicsCollision = true;
			ent.SetInteractsAs( CollisionLayer.Debris );
			ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

			ent.SetModel( GetModelName() );
			ent.CopyBonesFrom( this );
			ent.TakeDecalsFrom( this );
			ent.SetRagdollVelocityFrom( this );
			ent.DeleteAsync( 20.0f );

			// Copy the clothes over
			foreach ( Entity child in Children )
			{
				if ( child is ModelEntity e )
				{
					string model = e.GetModelName();
					if ( model != null && !model.Contains( "clothes" ) ) // Uck we're better than this, entity tags, entity type or something?
						continue;

					ModelEntity clothing = new();
					clothing.SetModel( model );
					clothing.SetParent( ent, true );
				}
			}

			ent.PhysicsGroup.AddVelocity( force );

			if ( forceBone >= 0 )
			{
				var body = ent.GetBonePhysicsBody( forceBone );
				if ( body != null )
				{
					body.ApplyForce( force * 1000 );
				}
				else
				{
					ent.PhysicsGroup.AddVelocity( force );
				}
			}

			Corpse = ent;

			RagdollLimit.Watch( ent );
		}
	}
}
