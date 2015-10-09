using UnityEngine;
using System.Collections;

public class IA {

	private ItemsCollection itemsCollection;
	private LinesCollection linesCollection;
	private Map map;
	private GameController gameController = GameController.getGameController();

	private List<ItemController> neutralList;
	private List<ItemController> playerList;
	private List<ItemController> enemyList;

	public IA (ItemsCollection itemsCollection, LinesCollection linesCollection, Map map)
	{
		this.itemsCollection = itemsCollection;
		this.linesCollection = linesCollection;
		this.map = map;

		ItemController itemPlayer = null;
		ItemController itemEnemy = null;
		ItemController itemNeutral = null;

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

        if (itemPlayer != null && itemEnemy != null)
        {
            gameController.MakeAShootAtB(itemPlayer, itemEnemy);
        }
		    
        if (itemNeutral != null && itemEnemy != null)
        {
            gameController.MakeAShootAtB(itemEnemy, itemNeutral);
        }
	}

		
}
