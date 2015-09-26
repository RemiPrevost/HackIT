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

	private GameObject[] items = new GameObject[4];
	public SelectionManager selectionManager;

	void Start() {
		items[0] = Instantiate (itemPrefab, new Vector3(9.63F,0.0F,-10.41F), transform.rotation) as GameObject;
		items[1] = Instantiate (itemPrefab, new Vector3(-12.76F,0.0F,-10.41F), transform.rotation) as GameObject;
		items[2] = Instantiate (itemPrefab, new Vector3(0F,0.0F,12F), transform.rotation) as GameObject;
		items[3] = Instantiate (itemPrefab, new Vector3(0F,0F,0F), transform.rotation) as GameObject;
	}

	public void onItemSelect(ItemController itemController) {
		selectionManager.newSelection (itemController);
	}

	public void onItemDeselect(ItemController item) {
		selectionManager.deselect (item);
	}

	public void selectionValidated(ItemController itemFrom, ItemController itemTo) {
		Vector3 fromPosition = itemFrom.gameObject.transform.position;
		Vector3 toPosition = itemTo.gameObject.transform.position;
		Vector3 relativePos = toPosition - fromPosition;
		itemFrom.Shoot (Quaternion.LookRotation(relativePos));
	}
}
