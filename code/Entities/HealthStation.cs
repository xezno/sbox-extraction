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

			SetModel( "models/citizen_props/crowbar01.vmdl" );
			PhysicsEnabled = false;
			trigger = new();
			trigger.WorldPos = WorldPos;
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
