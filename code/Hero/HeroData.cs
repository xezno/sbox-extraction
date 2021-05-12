namespace Extraction.Hero
{
	// Container class for all hero data
	// Read from JSON at runtime
	public class HeroData
	{
		public string Name { get; set; } = "";
		public string Country { get; set; } = "";
		public string BodyType { get; set; } = "";
		public long Health { get; set; } = 100;
		public long Speed { get; set; } = 320;
		public string Class { get; set; } = "";
		public string Description { get; set; } = "";
		public string Portrait { get; set; } = "/ui/extraction/placeholder.png";
		public string[] Clothing { get; set; } = new string[0];
	} 
}
