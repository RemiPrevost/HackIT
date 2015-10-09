using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ErrorManager : MonoBehaviour {

	public static void DisplayErrorMessage(string message) {
		Debug.LogError (message);
	}

}