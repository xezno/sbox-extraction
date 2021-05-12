using System;
using System.Collections.Generic;
using System.Linq;
using Extraction.Entities;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI
{
	public class BasePing : Panel
	{
		public enum PingType
		{
			Default
		}

		private Player player;
		private Label nameLabel;
		public PingType type;
		private Panel pingImage;

		public BasePing( Player player )
		{
			this.player = player;
			type = PingType.Default;
			
			nameLabel = Add.Label( player.Name.ToString() );
			pingImage = Add.Panel( "pingimage" );
		}
	}

	public class Pings : Panel
	{
		Dictionary<PingEntity, BasePing> ActivePings = new();

		public Pings()
		{
			StyleSheet.Load( "/ui/Pings.scss" );
		}

		public override void Tick()
		{
			base.Tick();

			var deleteList = new List<PingEntity>();
			deleteList.AddRange( ActivePings.Keys );

			int count = 0;
			foreach ( var player in Entity.All.Where( x => x.GetType() == typeof(PingEntity) )
				.OrderBy( x => Vector3.DistanceBetween( x.WorldPos, Sandbox.Camera.LastPos ) ) )
			{
				if ( UpdateNameTag( player as PingEntity ) )
				{
					deleteList.Remove( player as PingEntity );
					count++;
				}
			}

			foreach( var player in deleteList )
			{
				ActivePings[player].Delete();
				ActivePings.Remove( player );
			}
		}

		public virtual BasePing CreatePing( PingEntity ping )
		{
			var tag = new BasePing( ping.Owner );
			tag.Parent = this;
			return tag;
		}

		public bool UpdateNameTag( PingEntity pingEntity )
		{
			var labelPos = pingEntity.Pos;
			
			var lookDir = (labelPos - Sandbox.Camera.LastPos).Normal;
			if ( Sandbox.Camera.LastRot.Forward.Dot( lookDir ) < 0.5 )
				return false;

			if ( !ActivePings.TryGetValue( pingEntity, out var ping ) )
			{
				ping = CreatePing( pingEntity );
				ActivePings[pingEntity] = ping;
			}

			var screenPos = labelPos.ToScreen();

			ping.Style.Left = Length.Fraction( screenPos.x );
			ping.Style.Top = Length.Fraction( screenPos.y );
			ping.Style.Opacity = 0.5f;

			var transform = new PanelTransform();
			transform.AddTranslateY( Length.Fraction( -1.0f ) );
			transform.AddScale( 0.5f );
			transform.AddTranslateX( Length.Fraction( -0.5f ) );

			ping.Style.Transform = transform;
			ping.Style.Dirty();

			return true;
		}
	}
}
