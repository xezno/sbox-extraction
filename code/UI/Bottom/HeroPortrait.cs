using Extraction.Actor;
using Sandbox.UI.Construct;
using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	// TODO
	public class HeroPortrait : Panel
	{
		SceneCapture sceneCapture;
		Angles CamAngles;
		public Label heroText;

		public HeroPortrait()
		{
			Style.FlexWrap = Wrap.Wrap;
			Style.JustifyContent = Justify.Center; 
			Style.AlignItems = Align.Center;
			Style.AlignContent = Align.Center;
			StyleSheet.Parse( "image { width: 128px; height: 128px; justify-content: center; align-items: center; }" );

			// CamAngles = new Angles( 25, 0.0f, 0 );
			//
			// using ( SceneWorld.SetCurrent( new SceneWorld() ) )
			// {
			// 	var playerPrevObj = SceneObject.CreateModel( "models/citizen/citizen.vmdl", Transform.Zero );
			// 	playerPrevObj.Transform = new Transform( default, Rotation.From(new Angles(0, 180, 0  )) );
			//
			// 	sceneCapture = SceneCapture.Create( "test", 512, 512 );
			// 	sceneCapture.ClearColor = Color.Transparent;
			// 	sceneCapture.AmbientColor = Color.White;
			//
			// 	sceneCapture.SetCamera( Vector3.Up * 50 + CamAngles.Direction * -50, CamAngles, 45 );
			// }
			//
			// Add.Image( "scene:test" );

			Add.Image( "ui/extraction/hero-portrait-placeholder.png", "hero-image" );
			
			heroText = Add.Label( "HeroName", "current-hero" );
			heroText.Style.Set( "font-size: 18px" );
		}

		public override void Tick()
		{
			base.Tick();
			
			var player = Player.Local as ExtractionPlayer;
			if ( player == null ) return;
			
			heroText.Text = $"{player.HeroData.Name}";
		}

		public override void OnDeleted()
		{
			base.OnDeleted();

			sceneCapture?.Delete();
			sceneCapture = null;
		}
	}
}
