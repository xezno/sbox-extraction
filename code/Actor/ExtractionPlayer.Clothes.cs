using System.Collections.Generic;
using Sandbox;

namespace Extraction.Actor
{
	public partial class ExtractionPlayer
	{
		private bool dressed;

		private List<ModelEntity> clothes = new List<ModelEntity>();

		public void Undress()
		{
			// 😳
			foreach ( ModelEntity clothingItem in clothes )
			{
				clothingItem.Delete();
			}
				
			clothes.Clear();

			dressed = false;
		}
		
		public void SetHeroClothing()
		{
			if ( dressed )
				Undress();

			foreach ( string clothingItem in HeroData.Clothing )
			{
				var temp = new ModelEntity();
				temp.SetModel( clothingItem );
				temp.SetParent( this, true );
				temp.EnableShadowInFirstPerson = true;
				temp.EnableHideInFirstPerson = true;

				clothes.Add( temp );
			}

			dressed = true;
		}
	}
}
