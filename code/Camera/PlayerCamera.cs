﻿using System;
using System.Threading.Tasks;
using Extraction.Actor;
using Sandbox;

namespace Extraction.Camera
{
	public class PlayerCamera : FirstPersonCamera
	{
		private Vector3 lastPos;
		private int FovThreshold = 20;
		
		public override void Activated()
		{
			base.Activated();
			
			FieldOfView = ExtractionConfig.FieldOfView;
		}

		public override void Update()
		{			
			var player = Player.Local;
			if ( player == null ) return;

			Pos = Vector3.Lerp( player.EyePos.WithZ( lastPos.z ), player.EyePos, 25.0f * Time.Delta );
			Rot = player.EyeRot;

			Viewer = player;
			lastPos = Pos;

			ApplyFov();
		}

		private void ApplyFov()
		{
			var player = Player.Local as ExtractionPlayer;
			var playerController = player.GetActiveController() as ExtractionController;
			
			if ( player == null ) return;
			if ( playerController == null ) return;
			
			float targetFov = ExtractionConfig.FieldOfView;
			// Log.Info( $"{player.Velocity.WithZ( 0 ).Length}" );
			
			if ( player.Velocity.WithZ( 0 ).Length >= player.HeroData.Speed - FovThreshold ) // TODO: Length^2 is faster than Length
			{
				targetFov = ExtractionConfig.SprintFieldOfView;
			}

			FieldOfView = FieldOfView.LerpTo( targetFov , 10.0f * Time.Delta, false );
		}
	}
}
