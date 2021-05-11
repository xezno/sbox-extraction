using Extraction.Camera;
using Extraction.Weapons;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		[ServerCmd( "togglethirdperson", Help = "Toggles the third person camera" )]
		public static void SwitchCamera()
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);
			Log.Info( "Toggled third person mode" );
			if ( player.Camera.GetType() == typeof( PlayerCamera ) )
			{
				player.Camera = new ThirdPersonPlayerCamera();
			}
			else
			{
				player.Camera = new PlayerCamera();
			}
		}

		[ServerCmd( "sethero", Help = "Set Hero" )]
		public static void SetHero( string hero = "duke" )
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);
			player.ChangeHero( hero );
			Log.Info( "Set player hero" );
		}
		
		[ServerCmd( "fov", Help = "Change the field of view" )]
		public static void FovCommand(float newFov)
		{
			if ( newFov > 179 || newFov < 0 )
			{
				Log.Error( "Bad FOV value!" );
				return;
			}
			ExtractionConfig.FieldOfView = newFov;
			Log.Info( $"Set FOV to {ExtractionConfig.FieldOfView}" );
		}

		[ServerCmd( "setammo", Help = "Sets player reserve ammo amount" )]
		public static void SetAmmo(int newAmmo)
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);
			if ( player.Inventory.Active is BaseExtractionWeapon weapon )
			{
				weapon.ReserveAmmo = newAmmo;
			}
			Log.Info( $"Set reserve ammo to {newAmmo}" );
		}

		[ServerCmd( "give", Help = "giv wepon :DDD" )]
		public static void SetPrimary(string primaryId)
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);
			player.Inventory.DropActive();
			player.Inventory.Add( Entity.Create( primaryId ) );
			Log.Info( $"gave playr {primaryId}" );
		}

		[ServerCmd( "damage", Help = "Damage the player" )]
		public static void DoDamage(int damage = 10)
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);
			player.TakeDamage(new DamageInfo() { Damage = damage });
			Log.Info( $"Damaged the player for {damage}" );
		}

		[ServerCmd( "kill2", Help = "Kill the player" )]
		public static void KillPlayer()
		{
			var target = ConsoleSystem.Caller;
			(Game.Current as Game)?.DoPlayerSuicide( target );
			Log.Info( $"killed lol" );
		}
	}
}
