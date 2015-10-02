using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	/**********************************************************/
	/*************** STATIC METHODS & VARIABLES ***************/


	public static GameController getGameController() {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		GameController gameController = null;
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

		return gameController;
	}


	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	public GameObject itemPrefab;
	public GameObject linePrefab;
	public SelectionManager selectionManager;
	public DragDropManager dragDropManager;

	private ItemsCollection itemsCollection;
	private LinesCollection linesCollection;
	private Map map;
	private IA ia;
	private GameObject[] items;


	void Start() {
		itemsCollection = new ItemsCollection();
		linesCollection = new LinesCollection (itemsCollection);
		map = new Map (itemsCollection.GetCollectionSize ());
		this.dragDropManager.SendMessage ("SetLines", this.linesCollection);
	}

	void Update() {
		if (ia == null) {
			ia = new IA (itemsCollection, linesCollection, map);
		}
	}

	public void onItemSelect(ItemController itemController) {
		selectionManager.newSelection (itemController);
	}

	public void onItemDeselect(ItemController item) {
		selectionManager.deselect (item);
	}

	public void selectionValidated(ItemController itemFrom, ItemController itemTo) {
		if (this.isInvertNecessary (itemFrom, itemTo)) {
			ItemController itemTemp = itemFrom;
			itemFrom = itemTo;
			itemTo = itemTemp;
		}
		if ((itemFrom.owner == itemTo.owner) && itemTo.IsShootingAt(itemFrom.GetId())) {
			itemTo.StopShootingAt(itemFrom.GetId());
			map.AstopsShootingAtB(itemFrom.GetId(), itemTo.GetId ());
		}

		if (!itemTo.IsFull()) {
			MakeAShootAtB (itemFrom, itemTo);
		}
	}

	public void MakeAShootAtB(ItemController itemFrom, ItemController itemTo) {
		Vector3 fromPosition = itemFrom.gameObject.transform.position;
		Vector3 toPosition = itemTo.gameObject.transform.position;
		Vector3 relativePos = toPosition - fromPosition;
		linesCollection.GetLineControllerBetween (itemFrom.GetId (), itemTo.GetId ()).Activate();
		itemFrom.StartShootingAt (Quaternion.LookRotation(relativePos), itemTo.GetId());
		map.AshootsAtB (itemFrom.GetId (), itemTo.GetId ());
	}

	public void onShotItem(ItemController itemShot, int shooter) {
		itemShot.AlterNrjBy (1, shooter);
	}

	public void onZeroNrj(ItemController item) {
		List<int> targetsId = map.AtWhoAIsShootingAt (item.GetId());
		ItemController targetController;

		foreach (int targetId in targetsId) {
			targetController = itemsCollection.GetItemController(targetId);
			if (targetController.getOwner() == item.getOwner()) {
				linesCollection.GetLineControllerBetween (item.GetId (), targetId).Deactivate();
			}
			else {
				MakeAShootAtB(targetController, item);
			}

		}

		map.AstopsShooting (item.GetId ());
		item.SetOwner (0);
	}

	public void onFullNrj(ItemController fullItem) {
		List<int> shooterIdList = map.ByWhoAIsShooting (fullItem.GetId ());

		foreach (int shooterId in shooterIdList) {
			itemsCollection.GetItemController(shooterId).SuspendShootingAt(fullItem.GetId());
		}
	}

	public void onNoMoreFullNrj(ItemController noFullItem) {
		List<int> shooterIdList = map.ByWhoAIsShooting (noFullItem.GetId ());
		
		foreach (int shooterId in shooterIdList) {
			itemsCollection.GetItemController(shooterId).ResumeShootingAt(noFullItem.GetId());
		}
	}

	public void OnItemAcquiredBy(ItemController itemController, int newOwner) {
		itemController.SetOwner (newOwner);
	}

	public void onCutLines(List<Indexes> cutLines) {
		ItemController itemController1, itemController2;
		foreach (Indexes line in cutLines) {
			itemController1 = itemsCollection.GetItemController(line.i);
			itemController2 = itemsCollection.GetItemController(line.j);

			if ((itemController1.owner == 1) && map.IsAShootingAtB(line.i, line.j)) {
				itemController1.StopShootingAt(line.j);
				map.AstopsShootingAtB(line.i,line.j);
				linesCollection.GetLineControllerBetween(line.i,line.j).Deactivate();
			}
			else if ((itemController1.owner == 1) && map.IsAShootingAtB(line.j, line.i)) {
				itemController2.StopShootingAt(line.i);
				map.AstopsShootingAtB(line.j,line.i);
				linesCollection.GetLineControllerBetween(line.i,line.j).Deactivate();
			}
			else if ((itemController1.owner > 1) && (itemController2.owner == 1) && map.IsAShootingAtB(line.i,line.j)) {
				itemController1.StopShootingAt(line.j);
				map.AstopsShootingAtB(line.i,line.j);
				linesCollection.GetLineControllerBetween(line.i,line.j).Deactivate();
			}
			else if ((itemController2.owner > 1) && (itemController1.owner == 1) && map.IsAShootingAtB(line.j,line.i)) {
				itemController2.StopShootingAt(line.i);
				map.AstopsShootingAtB(line.j,line.i);
				linesCollection.GetLineControllerBetween(line.i,line.j).Deactivate();
			}
		}
	}

	public GameObject InstantiateLine() {
		return Instantiate (linePrefab, transform.position, transform.rotation) as GameObject;
	}

	public ItemController GetItemController(int id) {
		return itemsCollection.GetItemController (id);
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/


	private bool isInvertNecessary(ItemController itemFrom, ItemController itemTo) {
		return (isPlayer (itemFrom) && isEnemy (itemTo)) || isNeutral (itemFrom);  
	}

	private bool isPlayer(ItemController item) {
		return item.owner == 1;
	}

	private bool isEnemy(ItemController item) {
		return item.owner != 0 && item.owner != 1;
	}

	private bool isNeutral(ItemController item) {
		return item.owner == 0;
	}
}
