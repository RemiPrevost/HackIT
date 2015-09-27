using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	/********************************/
	/* STATIC FUNCTIONS & VARIABLES */
	/********************************/

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


	/********************************/
	/*          ATTRIBUTES          */
	/********************************/

	public GameObject itemPrefab;
	public GameObject line;
	public SelectionManager selectionManager;
	public DragDropManager dragDropManager;

	private GameObject[] items;
	private GameObject[,] lines;


	void Start() {
		items = GameObject.FindGameObjectsWithTag("Item");
		for (int i = 0; i < items.Length; i++) {
			items[i].SendMessage("SetId",i);
		}
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
		Vector3 fromPosition = itemFrom.gameObject.transform.position;
		Vector3 toPosition = itemTo.gameObject.transform.position;
		Vector3 relativePos = toPosition - fromPosition;
		itemFrom.StartShootingWave (Quaternion.LookRotation(relativePos));

		lines [itemTo.GetId (), itemFrom.GetId()].SendMessage ("Activate");
	}

	public void onShotItem(GameObject itemShot) {
		itemShot.SendMessage ("AlterNrjBy", 1);
	}

	public void onZeroNrj(ItemController item) {
		item.SetOwner (0);
	}

	public GameObject[,] GetLines() {
		print (lines);
		return this.lines;
	}

	/********************************/
	/*       PRIVATES METHODS       */
	/********************************/ 

	private void DrawLines() {
		lines = new GameObject[this.items.Length,this.items.Length];

		for (int i = 0; i < this.items.Length; i++) {
			for (int j = 0; j < this.items.Length; j++) {
				if (i < j) {
					lines[i,j] = Instantiate (this.line, transform.position, transform.rotation) as GameObject;
					lines[i,j].GetComponent<LineRenderer>().SetPosition(0, new Vector3(this.items[i].transform.position.x,0,this.items[i].transform.position.z));
					lines[i,j].GetComponent<LineRenderer>().SetPosition(1, new Vector3(this.items[j].transform.position.x,0,this.items[j].transform.position.z));
					lines[i,j].GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
					lines[i,j].SendMessage("Deactivate");
					lines[i,j].SendMessage("SetOwnerOut",j);
					lines[i,j].SendMessage("SetOwnerIn",i);
					lines[i,j].SendMessage("SetStartPoint",new Vector3(this.items[i].transform.position.x,0,this.items[i].transform.position.z));
					lines[i,j].SendMessage("SetEndPoint",new Vector3(this.items[j].transform.position.x,0,this.items[j].transform.position.z));
					lines[i,j].SendMessage("ComputeConstants");
					lines[j,i] = lines[i,j];
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
