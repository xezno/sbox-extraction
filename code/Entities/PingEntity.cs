using Sandbox;

namespace Extraction.Entities
{
	[Library( "ent_ping" )]
	public class PingEntity : ModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/arrow.vmdl" );
			PhysicsEnabled = false;
		}
	}
}
