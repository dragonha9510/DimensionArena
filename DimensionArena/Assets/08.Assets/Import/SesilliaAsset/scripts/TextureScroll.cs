using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour {
	public float offsetSpeedX;
	public float offsetSpeedY;

	private Renderer _renderer;


	// Use this for initialization
	void Start () {

		_renderer = GetComponent<Renderer> ();
		_renderer.materials[0].SetTextureOffset ("_MainTex", Vector2.zero);
	}
	
	// Update is called once per frame
	void Update () {

		var x = Mathf.Repeat (Time.time * offsetSpeedX, 1);
		var y = Mathf.Repeat (Time.time * offsetSpeedY, 1);

		var offset = new Vector2 (x, y);

		GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex", offset);
		
	}
}
