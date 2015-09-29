using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public int count = 0;

	private GameController gameController;

	void Start() {
		gameController = GameController.getGameController ();
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Shot") {
			return;
		}
		if (count == 0)
		{
			count++;
			return;
		}
		gameController.onShotItem (other.gameObject.GetComponent<ItemController>(),
		                           gameObject.GetComponent<ShotController>().fromOwner);
		Destroy(gameObject);
	}
}
