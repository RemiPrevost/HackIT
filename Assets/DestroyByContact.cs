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
		if (count == 0)
		{
			count++;
			return;
		}
		gameController.onShotItem (other.gameObject);
		Destroy(gameObject);
	}
}
