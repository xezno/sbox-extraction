using Sandbox;

namespace Extraction
{
	/*
	 * citizen_clothes/beards/beard_trucker_brown
	 * citizen_clothes/shirt/shirt_longsleeve.plain
	 * citizen_clothes/trousers/trousers.jeans
	 * citizen_clothes/shoes/shoes.workboots
	 */
	public partial class ExtractionPlayer
	{
		private bool dressed;
		private ModelEntity beard;
		private ModelEntity shirt;
		private ModelEntity pants;
		private ModelEntity shoes;
		
		public void Dress()
		{
			if ( dressed )
			{
				return;
			}

			dressed = true;
			pants = new ModelEntity();
			pants.SetModel( "models/citizen_clothes/trousers/trousers.smart.vmdl" );
			pants.SetParent( this, true );
			pants.EnableShadowInFirstPerson = true;
			pants.EnableHideInFirstPerson = true;
			
			shirt = new ModelEntity();
			shirt.SetModel( "models/citizen_clothes/jacket/jacket.tuxedo.vmdl" );
			shirt.SetParent( this, true );
			shirt.EnableShadowInFirstPerson = true;
			shirt.EnableHideInFirstPerson = true;
			
			shoes = new ModelEntity();
			shoes.SetModel( "models/citizen_clothes/shoes/shoes.smartbrown.vmdl" );
			shoes.SetParent( this, true );
			shoes.EnableShadowInFirstPerson = true;
			shoes.EnableHideInFirstPerson = true;
			
			beard = new ModelEntity();
			beard.SetModel( "models/citizen_clothes/beards/beard_trucker_brown.vmdl" );
			beard.SetParent( this, true );
			beard.EnableShadowInFirstPerson = true;
			beard.EnableHideInFirstPerson = true;
		}
	}
}
