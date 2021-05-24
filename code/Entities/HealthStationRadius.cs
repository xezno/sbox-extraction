using System.Runtime.InteropServices.ComTypes;
using Sandbox;

namespace Extraction.Entities
{
	public class HealthStationRadius : RenderEntity
	{
		public Material Material = Material.Load( "materials/entities/healthstationradius.vmat" );
		public bool ShouldDraw = false;

		private const float Radius = 60f;

		public override void Spawn()
		{
			Scale = Radius;
			base.Spawn();
			// I would love to use a decal for this, but there's no way of moving them dynamically at the minute
			// (and I'm not sure if S2 supports that, either)
			// Decals.Place( Material, this, 0, WorldPos, Vector3.One * 50, Rotation.Identity );
		}

		public override void DoRender( SceneObject obj )
		{
			if ( !ShouldDraw )
				return;

			Render.SetLighting( obj );
			var vertexBuffer = Render.GetDynamicVB( true );
			// vertexBuffer.AddCube( WorldPos, Vector3.One * 100, Rotation.Identity ); - We can use this with a decal material to 'hack' together some decal shit
			vertexBuffer.AddQuad( new Ray( Position, Vector3.Up ), Vector3.Forward * Radius, Vector3.Left * Radius );
			vertexBuffer.Draw( Material );
		}
	}
}
