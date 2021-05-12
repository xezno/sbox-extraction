using Extraction.Camera;
using Extraction.Weapons;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		private static ExtractionPlayer GetCommandCaller()
		{
			var target = ConsoleSystem.Caller;
			var player = ((ExtractionPlayer)target);

			return player;
		}
		
		[ServerCmd( "togglethirdperson", Help = "Toggles the third person camera" )]
		public static void SwitchCamera()
		{
			var player = GetCommandCaller();
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

		[ServerCmd( "sethero", Help = "Set player hero" )]
		public static void SetHero( string hero = "duke" )
		{
			var player = GetCommandCaller();
			player.ChangeHero( hero );
			Log.Info( "Set player hero" );
		}
		
		[ClientCmd( "fov", Help = "Change the field of view", CanBeCalledFromServer = false )]
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
		
		[ClientCmd( "fov_sprint", Help = "Change the field of view", CanBeCalledFromServer = false )]
		public static void SprintFovCommand(float newMultiplier)
		{
			if ( newMultiplier > 2 || newMultiplier < 1 )
			{
				Log.Error( "Bad FOV value!" );
				return;
			}
			ExtractionConfig.SprintFieldOfView = newMultiplier;
			Log.Info( $"Set sprint FOV multiplier to {ExtractionConfig.SprintFieldOfView}" );
		}

		[ServerCmd( "setammo", Help = "Sets player reserve ammo amount" )]
		public static void SetAmmo(int newAmmo)
		{
			var player = GetCommandCaller();
			if ( player.Inventory.Active is BaseExtractionWeapon weapon )
			{
				weapon.ReserveAmmo = newAmmo;
			}
			Log.Info( $"Set reserve ammo to {newAmmo}" );
		}

		[ServerCmd( "damage", Help = "Damage the player" )]
		public static void DoDamage(int damage = 10)
		{
			var player = GetCommandCaller();
			player.TakeDamage(new DamageInfo() { Damage = damage });
			Log.Info( $"Damaged the player for {damage}" );
		}
	}
}
