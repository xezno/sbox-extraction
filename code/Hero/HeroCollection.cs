using System.Collections.Generic;
using System.IO;
using Sandbox;

namespace Extraction.Hero
{
	public class HeroCollection
	{
		public static Dictionary<string, HeroData> HeroDatas = new Dictionary<string, HeroData>();

		public static void Load()
		{
			foreach ( var file in FileSystem.Mounted.FindFile( "/data/heroes", "*.json" ) )
			{
				var rawFileName = Path.GetFileNameWithoutExtension( file );
				Log.Info( $"Loading {rawFileName}" );
				HeroDatas.Add( rawFileName, FileSystem.Mounted.ReadJson<HeroData>( $"data/heroes/{rawFileName}.json" ) );
			}
		}
	}
}
