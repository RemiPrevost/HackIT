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

	public static Color getColorOfPlayer(int player_code) {
		return color_map [player_code];
	}

	private static Color[] color_map = new Color[] {
		Color.white,
		Color.green,
		Color.red
	};


	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	public GameObject itemPrefab;
	public GameObject line;
	public SelectionManager selectionManager;
	public DragDropManager dragDropManager;

	private ItemsCollection itemCollection;
	private GameObject[] items;
	private GameObject[,] lines;


	void Start() {
		itemCollection = new ItemsCollection();
		this.DrawLines ();
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
		}

		Vector3 fromPosition = itemFrom.gameObject.transform.position;
		Vector3 toPosition = itemTo.gameObject.transform.position;
		Vector3 relativePos = toPosition - fromPosition;
		lines [itemFrom.GetId (), itemTo.GetId ()].SendMessage ("Activate");
		itemFrom.StartShootingAt (Quaternion.LookRotation(relativePos), itemTo.GetId());

		lines [itemTo.GetId (), itemFrom.GetId()].SendMessage ("Activate");
	}

	public void onShotItem(ItemController itemShot, int shooter) {
		itemShot.AlterNrjBy (1, shooter);
	}

	public void onZeroNrj(ItemController item) {
		LineController lineController;

		foreach (GameObject line in lines) {
			if (line != null) {
				lineController = line.GetComponent<LineController>();
				if (lineController.GetOwnerIn() == item.GetId()) {
					lineController.Deactivate();
				}
			}
		}

		item.SetOwner (0);
	}

	public void onFullNrj(ItemController fullItem) {
		foreach (ItemController itemController in itemCollection.GetAllItemsController()) {
			if (!itemController.Equals(fullItem)) {
				itemController.SuspendShootingAt(fullItem.GetId());
			}
		}
	}

	public void onNoMoreFullNrj(ItemController noFullItem) {
		foreach (ItemController itemController in itemCollection.GetAllItemsController()) {
			if (!itemController.Equals(noFullItem)) {
				itemController.ResumeShootingAt(noFullItem.GetId());
			}
		}
	}

	public void OnItemAcquiredBy(ItemController itemController, int newOwner) {
		itemController.SetOwner (newOwner);
	}

	public GameObject[,] GetLines() {
		print (lines);
		return this.lines;
	}

	public void onCutLines(List<Indexes> cutLines) {
		ItemController itemController1, itemController2;
		foreach (Indexes line in cutLines) {
			itemController1 = itemCollection.GetItemController(line.i);
			itemController2 = itemCollection.GetItemController(line.j);

			if ((itemController1.owner == 1) && itemController1.IsShootingAt(itemController2.GetId())) {
				itemController1.StopShootingAt(itemController2.GetId());
				lines [line.i, line.j].SendMessage ("Deactivate");
			}
			else if ((itemController1.owner == 1) && itemController2.IsShootingAt(itemController1.GetId())) {
				itemController2.StopShootingAt(itemController1.GetId());
				lines [line.i, line.j].SendMessage ("Deactivate");
			}
		}
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/

	private void DrawLines() {
		int itemsCollectionSize = itemCollection.GetCollectionSize();
		lines = new GameObject[itemsCollectionSize,itemsCollectionSize];
		GameObject itemGameObjectI, itemGameObjectJ;

		for (int i = 0; i < itemsCollectionSize; i++) {
			for (int j = 0; j < itemsCollectionSize; j++) {
				if (i != j) {
					itemGameObjectI = itemCollection.GetItemObject(i);
					itemGameObjectJ = itemCollection.GetItemObject(j);
					lines[i,j] = Instantiate (this.line, transform.position, transform.rotation) as GameObject;
					lines[i,j].GetComponent<LineRenderer>().SetPosition(0, 
					    new Vector3(
							itemGameObjectI.transform.position.x,
							0,
							itemGameObjectI.transform.position.z
						)
					);
					lines[i,j].GetComponent<LineRenderer>().SetPosition(1,
						new Vector3(
							itemGameObjectJ.transform.position.x,
					   		0,
							itemGameObjectJ.transform.position.z
						)
					);
					lines[i,j].GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
					lines[i,j].SendMessage("Deactivate");
					lines[i,j].SendMessage("SetOwnerOut",j);
					lines[i,j].SendMessage("SetOwnerIn",i);
					lines[i,j].SendMessage("SetStartPoint",
						new Vector3(
							itemGameObjectI.transform.position.x,
							0,
							itemGameObjectI.transform.position.z)
					);
					lines[i,j].SendMessage("SetEndPoint",
						new Vector3(
							itemGameObjectJ.transform.position.x,
							0,
							itemGameObjectJ.transform.position.z)
					);
					lines[i,j].SendMessage("ComputeConstants");
					lines[j,i] = lines[i,j];
				}
				else {
					lines[i,j] = null;
				}
			}
		}

		this.dragDropManager.SendMessage ("SetLines", this.lines);
	}

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
