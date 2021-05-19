using Extraction.Camera;
using Extraction.Weapons;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		// TODO: VERIFY: Do all of these commands still work after Pain Day?
		
		private static ExtractionPlayer GetCommandCaller()
		{
			ExtractionPlayer target = ConsoleSystem.Caller.Pawn as ExtractionPlayer;
			return target;
		}
		
		#region Debug
		[ServerCmd( "togglethirdperson", Help = "(DEBUG) Toggles the third person camera" )]
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

		[ServerCmd( "setammo", Help = "(DEBUG) Sets player reserve ammo amount" )]
		public static void SetAmmo(int newAmmo)
		{	
			var player = GetCommandCaller();
			
			if ( player.Inventory.Active is ExtractionWeapon weapon )
			{
				weapon.ReserveAmmo = newAmmo;
			}
			Log.Info( $"Set reserve ammo to {newAmmo}" );
		}

		[ServerCmd( "damage", Help = "(DEBUG) Damage the player" )]
		public static void DoDamage(int damage = 10)
		{
			var player = GetCommandCaller();
			
			player.TakeDamage(new DamageInfo() { Damage = damage });
			Log.Info( $"Damaged the player for {damage}" );
		}
		#endregion

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
	}
}
