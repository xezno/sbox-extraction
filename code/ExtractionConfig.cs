using Sandbox;

namespace Extraction
{
	public static class ExtractionConfig
	{
		#region Camera
		public static float FieldOfView { get; set; } = 100f;

		private static float sprintFovMultiplier = 1.1f;
		public static float SprintFieldOfView { get => FieldOfView * sprintFovMultiplier; set => sprintFovMultiplier = value; }
		#endregion
		
		#region Heroes
		// Ideally we'll have separate models for each hero in future, but for prototyping we'll just use Terry
		public static string PlayerModel => "models/citizen/citizen.vmdl";
		public static string DefaultHero => "duke";
		#endregion
		
		#region Controls
		// This is kind of a shit way of doing controls, but it's good enough for now & means that if stuff changes
		// then it should be pretty easy to fix things
		
		public static float RespawnTimer => 3.0f;
		public static InputButton Ping => InputButton.View;
		public static InputButton Fire => InputButton.Weapon1;
		public static InputButton AimDownSights => InputButton.Weapon2;
		public static InputButton Ability1 => InputButton.Menu;
		public static InputButton Ability2 => InputButton.Use;
		public static InputButton Respawn => InputButton.Jump;
		public static InputButton InventorySlot1 => InputButton.Slot1;
		public static InputButton InventorySlot2 => InputButton.Slot2;
		public static InputButton InventorySlot3 => InputButton.Slot3;
		public static InputButton QuickMenu => InputButton.Grenade1;

		#endregion
	}
}
