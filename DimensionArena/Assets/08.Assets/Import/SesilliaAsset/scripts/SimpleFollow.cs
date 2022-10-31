using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour {

	Vector3 _targetPos;

	public GameObject target;
	public GameObject CameraPos;
	public float followSpeed;

	void Update(){

		Vector3 newPos = new Vector3 (
			                 CameraPos.transform.position.x,
			                 0.3f,
			                 CameraPos.transform.position.z
		                 );

		transform.position = Vector3.Lerp (
			transform.position,
			newPos,
			followSpeed * Time.deltaTime
		);

		transform.LookAt (target.transform.position);
	}

}
