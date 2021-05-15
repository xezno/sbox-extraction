using System.Collections.Generic;
using System.Threading;
using Extraction.Actor;
using Sandbox.UI.Construct;
using Sandbox;
using Sandbox.UI;

namespace Extraction.UI
{
	public class HeroPortrait : Panel
	{
		private Angles CamAngles;
		private SceneCapture sceneCapture;
		private AnimSceneObject playerPreview;
		private float startTime;
		
		private Label heroText;
		private Image heroImage;
		
		public HeroPortrait()
		{
			Style.FlexWrap = Wrap.Wrap;
			Style.JustifyContent = Justify.Center; 
			Style.AlignItems = Align.Center;
			Style.AlignContent = Align.Center;
			StyleSheet.Parse( "image { width: 100%; height: 128px; justify-content: center; align-items: center; }" );

			CamAngles = new Angles( 10, 0.0f, 0 );
			
			LoadWorld();

			heroImage = Add.Image( "scene:portrait", "hero-image" );
			
			heroText = Add.Label( "HeroName", "current-hero" );
			heroText.Style.Set( "font-size: 18px" );
		}

		private void DeleteScene()
		{
			sceneCapture?.Delete();
			sceneCapture = null;
		}

		private void LoadWorld()
		{
			DeleteScene();
			
			using ( SceneWorld.SetCurrent( new SceneWorld() ) )
			{
				playerPreview = new AnimSceneObject( Model.Load( ExtractionConfig.PlayerModel ), Transform.Zero );

				Light.Point( Vector3.Up * 150.0f, 200.0f, Color.White * 5000.0f );
				Light.Point( Vector3.Up * 100.0f + Vector3.Forward * 100.0f, 200, Color.White * 15000.0f );

				sceneCapture = SceneCapture.Create( "portrait", 256, 256 );

				sceneCapture.AmbientColor = new Color( 0.8f, 0.8f, 0.8f );
				sceneCapture.SetCamera( Vector3.Up * 100 + CamAngles.Direction * -50, CamAngles, 45 );
				
				// TODO: Clothes
				
				startTime = Time.Now;
			}
		}

		public override void OnDeleted()
		{
			base.OnDeleted();

			// If we don't delete this, it persists until s&box restart
			DeleteScene();
		}

		public override void Tick()
		{
			base.Tick();
			
			var player = Player.Local as ExtractionPlayer;
			if ( player == null ) return;
			
			heroText.Text = $"{player.HeroData.Name}";
			CamAngles.yaw = 180;

			if ( player.GetActiveAnimator() is ExtractionPlayerAnimator animator )
			{
				// Animation overrides
				animator.Params["lookat_pos"] = new Vector3( 10, 0, 0 );
				foreach ( var animParam in animator.Params )
				{
					if ( animParam.Value is int intAnimValue )
						playerPreview.SetAnimParam( animParam.Key, intAnimValue );
					else if ( animParam.Value is bool boolAnimValue )
						playerPreview.SetAnimParam( animParam.Key, boolAnimValue );
					else if ( animParam.Value is float floatAnimValue )
						playerPreview.SetAnimParam( animParam.Key, floatAnimValue );
					else if ( animParam.Value is Vector3 vector3AnimValue )
						playerPreview.SetAnimParam( animParam.Key, vector3AnimValue );
				}
				
				CamAngles.yaw = 160;
			}
			
			playerPreview.Update( Time.Now - startTime );
			sceneCapture?.SetCamera( Vector3.Up * 60 + CamAngles.Direction * -50, CamAngles, 30 );
		}
	}
}
