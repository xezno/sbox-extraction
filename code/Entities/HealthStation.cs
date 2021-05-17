using Sandbox;

namespace Extraction.Entities
{
	[Library( "ent_health_station" )]
	public class HealthStation : ModelEntity
	{
		private HealthStationRadius radius;
		private HealthStationTrigger trigger;
		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/rust_props/black_bin/blackbin.vmdl" );
			trigger = new();
			trigger.WorldPos = WorldPos;

			WorldScale = 0.6f;

			SetupPhysicsFromModel( PhysicsMotionType.Static, false );

			Health = 25;
		}

		protected override void OnDestroy()
		{
			radius?.Delete();
			trigger?.Delete();
			base.OnDestroy();
		}

		[Event( "frame" )]
		public void OnFrame()
		{
			radius.WorldPos = WorldPos + (Vector3.Up * 0.1f);
			radius.ShouldDraw = true;
		}

		public override void OnNewModel(Model model)
		{
			if (Host.IsServer)
				return;

			if ( !radius.IsValid() )
				radius = new();
		}
	}
}
