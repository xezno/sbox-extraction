using Sandbox;
using System;

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
			Controller = new ExtractionController();
			Animator = new StandardPlayerAnimator();
			Camera = new ExtractionCamera();
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			BecomeRagdollOnClient( Vector3.Zero, 0 );

			Controller = null;
			Camera = new ExtractionDeathCamera();

			EnableAllCollisions = false;
			EnableDrawing = false;
		}
	}
}
