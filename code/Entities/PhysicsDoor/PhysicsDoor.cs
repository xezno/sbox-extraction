using System;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.ScreenShake;

namespace Extraction.Entities
{
	/// <summary>
	/// This is my own implementation of a door.
	/// It uses "prop_door_rotating_physics" because it's sorta physics-based, and s&box doesn't have an implementation
	/// of this.
	/// </summary>
	[Library( "prop_door_rotating_physics" )]
	public partial class PhysicsDoor : KeyframeEntity, IUse
	{
		public enum Flags
		{
			RotateBackwards = 2,
			NonSolidToPlayer = 4,
			Passable = 8,
			OneWay = 16,
			NoAutoReturn = 32,
			Roll = 64,
			Pitch= 128,
			Use = 256,
			NoNpcs = 512,
			Touch = 1024,
			StartLocked = 2048,
			Silent = 4096,
			UseCloses = 8192,
			SilentToNpcs = 16384,
			IgnoreUse = 32768,
			StartUnbreakable = 524288,
		}
		
		public enum OpenDirection
		{
			Closed,
			Front,
			Back
		}

		public enum DoorState
		{
			Open,
			Closed,
			Opening,
			Closing
		}

		#region Hammer Props
		[HammerProp( "movedir" )]
		public Angles MoveDir { get; set; }

		[HammerProp( "movedir_islocal" )]
		public bool MoveDirIsLocal { get; set; }

		[HammerProp( "spawnpos" )]
		public bool SpawnOpen { get; set; }

		[HammerProp( "lip" )]
		public float Lip { get; set; }

		[HammerProp( "speed" )]
		public float Speed { get; set; }

		[HammerProp( "wait" )]
		public float TimeBeforeReset { get; set; }
		
		[HammerProp( "distance" )]
		public float RotationDegrees { get; set; }
		#endregion
		
		#region Angles
		Angles RotationClosed;
		Angles RotationBack;
		Angles RotationFront;
		#endregion
		
		Vector3 PositionA;
		Vector3 PositionB;
		
		public DoorState State { get; protected set; } = DoorState.Open;
		
		private PhysicsDoorTrigger trigger;

		public override void Spawn()
		{
			base.Spawn();

			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );

			// Setup run-thru trigger
			trigger = new();
			trigger.WorldPos = WorldPos;
			trigger.WorldRot = WorldRot;
			trigger.Owner = this;
			trigger.SetupPhysics();

			PositionA = WorldPos;
			// Get the direction we want to move
			var dir = Rotation.From( MoveDir ).Forward;
			if ( MoveDirIsLocal ) dir = Transform.NormalToWorld( dir );

			// Open position is the size of the bbox in the direction minus the lip size
			var boundSize = OOBBox.Size;
			PositionB = WorldPos + dir * (MathF.Abs( boundSize.Dot( dir ) ) - Lip);

			State = DoorState.Closed;
			SpawnFlags.Add( Flags.Use );

			if ( SpawnOpen )
			{
				WorldPos = PositionB;
				State = DoorState.Open;
			}
			
			RotationClosed = WorldRot.Angles();

			var degrees = RotationDegrees - Lip;

			if ( SpawnFlags.Has( Flags.RotateBackwards ) ) degrees *= -1.0f;

			// Setup back rotation
			if ( SpawnFlags.Has( Flags.Pitch ) ) RotationBack = RotationClosed + new Angles( degrees, 0, 0 );
			else if ( SpawnFlags.Has( Flags.Roll ) ) RotationBack = RotationClosed + new Angles( 0, 0, degrees );
			else RotationBack = RotationClosed + new Angles( 0, degrees, 0 );
			
			// Setup front rotation
			if ( SpawnFlags.Has( Flags.Pitch ) ) RotationFront = RotationClosed - new Angles( degrees, 0, 0 );
			else if ( SpawnFlags.Has( Flags.Roll ) ) RotationFront = RotationClosed - new Angles( 0, 0, degrees );
			else RotationFront = RotationClosed - new Angles( 0, degrees, 0 );
		}
		
		public bool OnUse( Entity user )
		{
			if ( SpawnFlags.Has( Flags.Use ) )
			{
				Toggle( user );
			}

			Log.Info( "Door Used" );
			return false;
		}

		public virtual bool IsUsable( Entity user ) => SpawnFlags.Has( Flags.Use ) && !SpawnFlags.Has( Flags.IgnoreUse );

		public new void UpdateState( OpenDirection open )
		{
			Angles rot = open switch
			{
				OpenDirection.Back => RotationBack,
				OpenDirection.Front => RotationFront,
				_ => RotationClosed
			};

			if ( Speed <= 0 )
				Speed = 0.1f;

			var dfference = (WorldRot.Angles() - rot).Normal;
			var distance = dfference.Length;
			var seconds = distance / Speed;

			_ = DoMove( Rotation.From( rot ), seconds, open );
		}

		int movement = 0;

		async Task DoMove( Rotation target, float timeToTake, OpenDirection open )
		{
			var startPos = WorldRot;
			int moveid = ++movement;

			for ( float f = 0; f < 1; )
			{
				await Task.NextPhysicsFrame();

				if ( moveid != movement )
					return;

				var eased = Easing.EaseOut( f );

				var newPos = Rotation.Lerp( startPos, target, eased );
				SetPositionAndUpdateVelocity( newPos );
				f += Time.Delta / timeToTake;
			}

			if ( open != OpenDirection.Closed && TimeBeforeReset >= 0 )
			{
				await Task.DelaySeconds( TimeBeforeReset );

				if ( moveid != movement )
					return;

				EnableAllCollisions = true;
				// Toggle();
			}
		}

		public void ForceOpen( Player player )
		{
			var prevSpeed = Speed;
			Speed = player.Velocity.Length * 2; // Open super fast

			EnableAllCollisions = false;

			if ( State == DoorState.Closed || State == DoorState.Closing )
			{
				ShakeScreen( To.Single(player) );
				Open( player );
			}

			Speed = prevSpeed;
		}

		[ClientRpc]
		private void ShakeScreen( )
		{
			_ = new Perlin( 1f, 2f, 5f );
		}

		void SetPositionAndUpdateVelocity( Rotation pos )
		{
			var oldPos = WorldRot;
			WorldRot = pos;
		}

		[Input]
		protected void Toggle( Entity entity )
		{
			if ( State == DoorState.Open || State == DoorState.Opening ) State = DoorState.Closing;
			else if ( State == DoorState.Closed || State == DoorState.Closing ) State = DoorState.Opening;

			Log.Info( Transform.ToLocal( entity.Transform ).Pos.ToString() );

			var direction = (Transform.ToLocal( entity.Transform ).Pos.y > 0)
				? OpenDirection.Front
				: OpenDirection.Back;

			UpdateState( State == DoorState.Opening ? direction : OpenDirection.Closed );
		}

		// TODO: Do we need this?
		[Input]
		protected void Open( Entity entity )
		{
			if ( State == DoorState.Closed || State == DoorState.Closing ) State = DoorState.Opening;
			
			var direction = (Transform.ToLocal( entity.Transform ).Pos.y > 0)
				? OpenDirection.Front
				: OpenDirection.Back;

			UpdateState( State == DoorState.Opening ? direction : OpenDirection.Closed );
		}

		// TODO: Do we need this?
		[Input]
		protected void Close()
		{
			if ( State == DoorState.Open || State == DoorState.Opening ) State = DoorState.Closing;

			UpdateState( State == DoorState.Opening ? OpenDirection.Back : OpenDirection.Closed );
		}
	}
}
