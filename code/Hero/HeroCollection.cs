using System.Collections.Generic;
using System.IO;
using Sandbox;

namespace Extraction.Hero
{
	public class HeroCollection
	{
		public static Dictionary<string, HeroData> HeroDatas = new ();

		public static void Load()
		{
			HeroDatas = new();
			foreach ( var file in FileSystem.Mounted.FindFile( "/data/heroes", "*.json" ) )
			{
				string rawFileName = Path.GetFileNameWithoutExtension( file );
				Log.Info( $"Loading {rawFileName}" );
				HeroDatas.Add( rawFileName, FileSystem.Mounted.ReadJson<HeroData>( $"data/heroes/{rawFileName}.json" ) );
			}
		}

		[Event( "client.hotloaded" )]
		public static void OnHotload()
		{
			Load();
		}
	}
}
