using UnityEngine;
using System.Collections;

public class IA {

	private ItemsCollection itemsCollection;
	private LinesCollection linesCollection;
	private Map map;
	private GameController gameController = GameController.getGameController();

	public IA (ItemsCollection itemsCollection, LinesCollection linesCollection, Map map)
	{
		this.itemsCollection = itemsCollection;
		this.linesCollection = linesCollection;
		this.map = map;

		ItemController itemPlayer = new ItemController ();
		ItemController itemEnemy = new ItemController ();
		ItemController itemNeutral = new ItemController();

		foreach (ItemController item in itemsCollection.GetAllItemsController()) {
			if (item.owner == 0) {
				itemNeutral = item;
			}
			else if (item.owner == 1) {
				itemPlayer = item;
			}
			else {
				itemEnemy = item;
			}
		}

		gameController.MakeAShootAtB (itemPlayer,itemEnemy);
		gameController.MakeAShootAtB (itemEnemy,itemNeutral);

	}

		
}
