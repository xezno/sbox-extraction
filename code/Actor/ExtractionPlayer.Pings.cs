﻿using System;
using Extraction.UI;
using Extraction.Util;
using Sandbox;
using Sandbox.UI;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		private Entity currentPingEntity;

		private ControlledRandom pingRandom = new ControlledRandom();

		void RemovePing()
		{
			if ( currentPingEntity != null )
			{
				currentPingEntity.Delete();
			}
		}
		
		[ClientRpc]
		void SetPing()
		{
			if ( IsServer )
				return;
			
			RemovePing(); // Remove ping if one exists
			
			// Trace to a super far distance - then create a ping object if we hit
			var traceResult = Trace.Ray( EyePos, EyePos + EyeRot.Forward * 8000 ).WorldOnly().Run();
			if ( traceResult.Hit )
			{
				currentPingEntity = Entity.Create( "ent_ping" );
				Log.Info( "Pinging point" );
				currentPingEntity.Owner = this;
				currentPingEntity.WorldPos = traceResult.EndPos;
			}
		}

		void SendPingMessage()
		{
			if ( !IsServer )
				return;
			
			string[] pingMessages = new string[]
			{
				"Let's go here.", "This looks like a good spot.", "Heading in that direction."
			};
			
			ChatPanel.AddChatEntry( Player.All, $"{Name} (PING)", pingRandom.FromArray(pingMessages) );
		}
	}
}
