using System.Linq;
using Sandbox;
using Extraction.Camera;
using Extraction.UI;
using Extraction.Weapons;
using Sandbox.InternalTests;

namespace Extraction.Actor
{
	partial class ExtractionPlayer : Player
	{
		public ExtractionPlayer()
		{
			Inventory = new BaseInventory( this );
		}

		public override void Respawn()
		{
			SetModel( ExtractionConfig.PlayerModel );
			
			Controller = new ExtractionController();
			Animator = new ExtractionPlayerAnimator();
			Camera = new PlayerCamera();
			
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			
			SetupHero();
			SetupInventory();

			base.Respawn();
			
			Health = HeroData.Health;
		}
		
		private void SetupInventory()
		{			
			Inventory.DeleteContents();
			for ( int i = 0; i < HeroData.Loadout.Length; i++ )
			{
				string item = HeroData.Loadout[i];
				Inventory.Add( Entity.Create( item ), i == 0 );
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();

			Inventory.DeleteContents();
			
			BecomeRagdollOnClient( Vector3.Zero, 0 );

			Controller = null;
			Camera = new DeathCamera();

			EnableAllCollisions = false;
			EnableDrawing = false;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			
			if ( LifeState == LifeState.Dead )
			{
				if ( Input.Pressed( ExtractionConfig.Respawn ) && IsServer )
				{
					Respawn();
				}
			}

			if ( IsServer )
			{
				// Clamp health
				if ( Health > HeroData.Health )
					Health = HeroData.Health;
			}

			DrawDebugShit();
			SimulateActiveChild( cl, ActiveChild );
			TickPlayerUse();
			
			// gross
			if ( Input.Pressed( ExtractionConfig.InventorySlot1 ) )
			{
				Inventory.SetActiveSlot( 0, true );
			}
			else if ( Input.Pressed( ExtractionConfig.InventorySlot2 ) )
			{
				Inventory.SetActiveSlot( 1, true );
			}
			else if ( Input.Pressed( ExtractionConfig.InventorySlot3 ) )
			{
				Inventory.SetActiveSlot( 2, true );
			}

			if ( Input.Pressed( ExtractionConfig.Use ) && IsServer )
			{
				Log.Info( "Player used" );
				if ( CurrentTraceType.HasFlag( TraceType.Objective ) )
				{
					Log.Info( "TODO: Objective" );
				}
			}
		}
	}
}
