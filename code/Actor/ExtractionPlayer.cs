﻿using Sandbox;
using Extraction.Camera;
using Extraction.Weapons;

namespace Extraction.Actor
{
	partial class ExtractionPlayer : BasePlayer
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
			
			Inventory = new ExtractionInventory( this ); // Clear

			foreach ( string item in HeroData.Loadout )
			{
				Inventory.Add( Entity.Create( item ) );
			}
			
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

		protected override void Tick()
		{
			if ( LifeState == LifeState.Dead )
			{
				if ( Input.Pressed( ExtractionConfig.Respawn ) && IsServer )
				{
					Respawn();
				}
			}

			TickActiveChild();
			CheckTrace();
			
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
				if ( CurrentTraceType == TraceType.Objective )
				{
					Log.Info( "TODO: Objective" );
				}
			}
		}
	}
}
