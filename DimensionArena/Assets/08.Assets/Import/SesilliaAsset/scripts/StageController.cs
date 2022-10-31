using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

	private Vector3 initialPosition;
	public float moveDistance;
	private MotionController _UIController;         //Character Animation UI Connection


	// Use this for initialization
	void Start () {
		initialPosition = new Vector3(0.0f,-0.5f,0.0f);
		_UIController = GameObject.Find("UI_Controller").GetComponent<MotionController>();

	}
	
	// Update is called once per frame
	void Update () {

		if (_UIController._Animation == "Climb" || _UIController._Animation == "Climb_End"){

			if (_UIController._Animation == "Climb" && transform.position.y < 0.08f) {
				transform.position = new Vector3 (
					transform.position.x,
					transform.position.y + (moveDistance * Time.deltaTime),
					transform.position.z);
			}

			if (_UIController._Animation == "Climb_End" && transform.position.y < 0.12f) {
				transform.position = new Vector3 (
					transform.position.x,
					transform.position.y + (moveDistance * Time.deltaTime),
					transform.position.z);
			}


		} else {
			transform.position = initialPosition;
		}
	}
}
