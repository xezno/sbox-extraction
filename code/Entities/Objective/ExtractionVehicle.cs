using Sandbox;

namespace Extraction.Entities.Objective
{
	[Library( "obj_extraction_vehicle" )]
	public partial class ExtractionVehicle : ModelEntity
	{
		[HammerProp("model")]
		public string ModelPath { get; set; }
		
		[HammerProp("health")]
		public float MaxHealth { get; set; }
		
		[HammerProp("startRepaired")]
		public bool StartRepaired { get; set; }
		
		[HammerProp("speed")]
		public float Speed { get; set; }
		
		[HammerProp("target")]
		public string Target { get; set; }

		[Net] public float Health { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			SetModel( ModelPath );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			
			Log.Info( $"Next target: {Target}" );
			
			if ( StartRepaired )
			{
				Health = MaxHealth;
			}
		}

		public override void OnKilled()
		{
			if (this.LifeState != LifeState.Alive)
				return;
			this.LifeState = LifeState.Dead;
			
			// TODO: Switch to 'destroyed' model, allow repairs
		}

		public override void Simulate( Client cl )
		{
		// 	this.Position = Vector3.Lerp( this.Position, Target.Position, 1f * Time.Delta );
			base.Simulate( cl );
		}
	}
}
