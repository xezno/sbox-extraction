using Sandbox;

namespace Extraction
{
	/// <summary>
	/// Static class containing config parameters.
	/// </summary>
	public static class ExtractionConfig
	{
		// Should probably store these in a JSON file instead but whatever
		
		#region Camera
		// FPS cam FOV and stuff
		
		public static float FieldOfView { get; set; } = 100f;

		private static float sprintFovMultiplier = 1.1f;
		private static float adsFovMultiplier = 0.75f;
		public static float SprintFieldOfView { get => FieldOfView * sprintFovMultiplier; set => sprintFovMultiplier = value; }
		public static float AdsFieldOfView  { get => FieldOfView * adsFovMultiplier; set => adsFovMultiplier = value; }
		#endregion
		
		#region Heroes
		// Ideally we'll have separate models for each hero in future, but for prototyping we'll just use Terry
		
		public static string PlayerModel => "models/citizen/citizen.vmdl";
		
		// Hero to load in as (we'll get rid of this when we have a lobby system or something)
		public static string DefaultHero => "debug-slow";
		#endregion
		
		#region Controls
		// This is kind of a shit way of doing controls, but it's good enough for now & means that if stuff changes
		// then it should be pretty easy to fix things
		
		public static float RespawnTimer => 3.0f;
		public static InputButton Fire => InputButton.Weapon1;
		public static InputButton AimDownSights => InputButton.Weapon2;
		public static InputButton Use => InputButton.Use;
		public static InputButton Respawn => InputButton.Jump;
		public static InputButton InventorySlot1 => InputButton.Slot1;
		public static InputButton InventorySlot2 => InputButton.Slot2;
		public static InputButton InventorySlot3 => InputButton.Slot3;
		#endregion
	}
}
