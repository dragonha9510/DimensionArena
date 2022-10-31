using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPSD_GameController : MonoBehaviour {

	CharacterController _SAPSDcontroller;
	Animator _SAPSDanimator;

	Vector3 moveDirection;

	public float gravity;
	public float speedZ;
	public float speedJump;

	// Use this for initialization
	void Start () {

		_SAPSDcontroller = GetComponent<CharacterController> ();
		_SAPSDanimator = this.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

		if (_SAPSDcontroller.isGrounded == true) {

			_SAPSDanimator.SetBool ("Jump_Down", false);

			if (Input.GetAxis ("Vertical") > 0.0f) {

				moveDirection.z = Input.GetAxis ("Vertical") * speedZ;

				_SAPSDanimator.SetBool ("Run", true);
			} else {
				moveDirection.z = 0;
				_SAPSDanimator.SetBool ("Run", false);
			}

			if (Input.GetButtonDown ("Jump")) {
				moveDirection.y = speedJump;
				_SAPSDanimator.SetBool ("Run", false);
				_SAPSDanimator.SetBool ("Jump_Up", true);

			} 

			transform.Rotate (0, Input.GetAxis ("Horizontal") * 3, 0);

		} else {


			if (_SAPSDcontroller.velocity.y < -2.0f) {
				_SAPSDanimator.SetBool ("Jump_Up", false);
				_SAPSDanimator.SetBool ("Jump_Down", true);
			}
		}

		moveDirection.y -= gravity * Time.deltaTime;

		Vector3 globalDirection = transform.TransformDirection (moveDirection);
		_SAPSDcontroller.Move (globalDirection * Time.deltaTime);

		if (_SAPSDcontroller.isGrounded == true) {

			if (Input.GetAxis ("Vertical") > 0.0f) {
				_SAPSDanimator.SetBool ("Run", true);
			} else {
				_SAPSDanimator.SetBool ("Run", false);
			}
		}

		if (_SAPSDcontroller.isGrounded == true) {
			moveDirection.y = 0;
		}


		if (Input.GetKeyDown(KeyCode.Z)) {
			_SAPSDanimator.SetTrigger("Kick_A");
		}

		if (Input.GetKeyDown(KeyCode.X)) {

			_SAPSDanimator.SetTrigger("Kick_B");
		}

		if (Input.GetKeyDown(KeyCode.C)) {
			_SAPSDanimator.SetTrigger("Kick_C");
		}

		
	}




		
}
