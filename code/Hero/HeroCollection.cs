using System.Collections.Generic;
using System.IO;
using Sandbox;

namespace Extraction.Hero
{
	/// <summary>
	/// This is just a container for all hero data stuff
	/// </summary>
	public class HeroCollection
	{
		public static Dictionary<string, HeroData> HeroDatas = new ();

		/// <summary>
		/// Loads all hero data json files from the filesystem
		/// </summary>
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

		/// <summary>
		/// Reloads whenever the client is hotloaded
		/// </summary>
		[Event( "client.hotloaded" )]
		public static void OnHotload()
		{
			Load();
		}
	}
}
