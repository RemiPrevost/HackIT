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

		gameController.MakeAShootAtB (itemsCollection.GetItemController(2),itemsCollection.GetItemController(1));
		gameController.MakeAShootAtB (itemsCollection.GetItemController(0),itemsCollection.GetItemController(2));

	}

		
}
