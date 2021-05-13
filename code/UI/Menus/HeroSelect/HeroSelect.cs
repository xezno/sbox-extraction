using System.Collections.Generic;
using Extraction.Hero;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Extraction.UI.Menus
{
	public class HeroSelect : Menu
	{
		public Label titleText; // TODO: Generic extraction menu class
		public List<HeroCard> heroCards;
		
		public HeroSelect()
		{
			StyleSheet.Load( "/UI/Menus/HeroSelect/HeroSelect.scss" );
			
			heroCards = new List<HeroCard>();
			
			titleText = Add.Label( "Select a Hero", "title");
			
			foreach ( var hero in HeroCollection.HeroDatas )
			{
				var heroCard = AddChild<HeroCard>();
				heroCard.HeroId = hero.Key;
			}
		}
	}
}
