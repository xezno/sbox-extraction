using Sandbox;
using System;
using System.Diagnostics;

namespace Extraction
{
	partial class ExtractionPlayer : BasePlayer
	{
		public ExtractionPlayer()
		{
			Log.Info("Extraction Player");
		}

		public override void Respawn()
		{
			SetModel("models/citizen/citizen.vmdl"); // If you have your own model, you can place it here instead.
			Controller = new PlayerController();
			Animator = new StandardPlayerAnimator();
			Camera = new PlayerCamera();
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			Dress();
			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			BecomeRagdollOnClient( Vector3.Zero, 0 );

			Controller = null;
			Camera = new DeathCamera();

			EnableAllCollisions = false;
			EnableDrawing = false;
		}

		[ServerCmd( "togglethirdperson", Help = "Toggles the third person camera" )]
		public static void SwitchCamera()
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);
			Log.Info( "ToggleThirdPerson" );
			if ( player.Camera.GetType() == typeof( PlayerCamera ) )
			{
				player.Camera = new ThirdPersonPlayerCamera();
			}
			else
			{
				player.Camera = new PlayerCamera();
			}
		}
	}
}
