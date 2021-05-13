namespace Extraction.Hero
{
	// Container class for all hero data
	// Read from JSON at runtime
	public class HeroData
	{
		public string Name { get; set; }
		public string Country { get; set; }
		public string BodyType { get; set; }
		public long Health { get; set; }
		public long Speed { get; set; }
		public string Class { get; set; }
		public string Description { get; set; }
		public string Portrait { get; set; }
		public bool DoesObjectivesFast { get; set; }
		public string[] Loadout { get; set; }
		// public Ability[] Abilities { get; set; }
		public string[] Clothing { get; set; }
	} 
}
