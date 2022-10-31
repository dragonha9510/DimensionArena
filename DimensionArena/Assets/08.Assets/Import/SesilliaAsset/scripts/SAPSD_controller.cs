using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPSD_controller : MonoBehaviour {

	public float gravity;
	public float speedJump;
	int jumpCount = 0;

	Vector3 moveDirection = Vector3.zero;

	CharacterController _SAPSDcontroller;
	Animator _SAPSDanimator;
	private MotionController _UIController;         //Character Animation UI Connection

	private SkinnedMeshRenderer _SAPSDRenderer_Face;        //Character Skin Mesh Renderer for Face
	private SkinnedMeshRenderer _SAPSDRenderer_Brow;        //Character Skin Mesh Renderer for Blow

	private Transform _t_bone_Eye_L;
	private Transform _t_bone_Eye_R;

	private GameObject _eyeSore_GEO;
	private GameObject _eyeSore_close_GEO;
	private GameObject _sweat_GEO;

	// Use this for initialization
	void Start () {

		//mption
		_SAPSDcontroller = GetComponent<CharacterController> ();
		_SAPSDanimator = GetComponent<Animator> ();
		_UIController = GameObject.Find("UI_Controller").GetComponent<MotionController>();

		_t_bone_Eye_L = GameObject.Find ("eye_L").GetComponent<Transform> ();
		_t_bone_Eye_R = GameObject.Find ("eye_R").GetComponent<Transform> ();

		_eyeSore_GEO = GameObject.Find ("eyeSore_GEO");
		_eyeSore_close_GEO = GameObject.Find ("eyeSore_close_GEO");
		_sweat_GEO = GameObject.Find ("sweat_GEO");

		//facial
		Transform[] SAPSDChildren = GetComponentsInChildren<Transform>();

		foreach (Transform t in SAPSDChildren)
		{
			if (t.name == "face_GEO")
				_SAPSDRenderer_Face = t.gameObject.GetComponent<SkinnedMeshRenderer>();
			if (t.name == "brow_GEO")
				_SAPSDRenderer_Brow = t.gameObject.GetComponent<SkinnedMeshRenderer>();
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (_UIController._Animation == "Idle" ||_UIController._Animation ==  null) {
			Idle ();
		} else if (_UIController._Animation == "Run") {
			Run ();
		} else if (_UIController._Animation == "Walk") {
			Walk ();
		} else if (_UIController._Animation == "Sit") {
			Sit ();
		} else if (_UIController._Animation == "Debuff") {
			Debuff ();
		} else if (_UIController._Animation == "Charge") {
			Charge ();
		} else if (_UIController._Animation == "BeFloored") {
			BeFloored ();
		} else if (_UIController._Animation == "Climb") {
			Climb ();
		} else if (_UIController._Animation == "FlyingA") {
			FlyingA ();
		} else if (_UIController._Animation == "FlyingB") {
			FlyingB ();
		} else if (_UIController._Animation == "Kick_A") {
			Kick_A ();
		} else if (_UIController._Animation == "Kick_B") {
			Kick_B ();
		} else if (_UIController._Animation == "Kick_C") {
			Kick_C ();
		} else if (_UIController._Animation == "Combo1") {
			Combo1 ();
		} else if (_UIController._Animation == "Combo2") {
			Combo2 ();
		} else if (_UIController._Animation == "Guard") {
			Guard ();
		} else if (_UIController._Animation == "Damaged") {
			Damaged ();
		} else if (_UIController._Animation == "Damaged_down") {
			Damaged_down ();
		} else if (_UIController._Animation == "Climb_End") {
			Climb_End ();
		} else if (_UIController._Animation == "Jump") {
			Jump ();
		} else if (_UIController._Animation == "W_Jump") {
			W_Jump ();
		} else if (_UIController._Animation == "Victory") {
			Victory ();
		} else if (_UIController._Animation == "Defeat") {
			Defeat ();
		}

		Vector3 globalDirection;
		moveDirection.y -= gravity * Time.deltaTime;
		globalDirection = transform.TransformDirection (moveDirection);
		_SAPSDcontroller.Move (globalDirection * Time.deltaTime);

		SetFacial ();

	}

	/// <summary>
	/// Animation Setter (Loopable Animation) 
	/// </summary>
	/// 
	/// 
		
	void Idle(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Idle" , true);
	}

	void Run(){
		Idle ();
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Run" , true);
	}

	void Walk(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Walk" , true);
	}

	void Sit(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Sit" , true);
	}

	void Debuff(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Debuff" , true);
	}

	void Charge(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Charge" , true);
	}

	void BeFloored(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("BeFloored" , true);
	}

	void Climb(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Climb" , true);
	}

	void FlyingA(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("FlyingA" , true);
	}

	void FlyingB(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("FlyingB" , true);
	}

	/// <summary>
	/// Animation Setter (Non Loopable Animation) 
	/// </summary>
	/// 
	/// 

	void Kick_A(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Kick_A" , true);
		_UIController._Animation = "Idle";


	}

	void Kick_B(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Kick_B" , true);
		_UIController._Animation = "Idle";

	}

	void Kick_C(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Kick_C" , true);
		_UIController._Animation = "Idle";

	}

	void Combo1(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Combo1", true);

		if (_SAPSDanimator.GetCurrentAnimatorClipInfo (0) [0].clip.name.ToString () == "kick_B") {
			_SAPSDanimator.SetBool ("Combo1", false);
			_UIController._Animation = "Idle";
		}
	}

	void Combo2(){

		InitializeFlags ();
		_SAPSDanimator.SetBool ("Combo2", true);

		if (_SAPSDanimator.GetCurrentAnimatorClipInfo (0) [0].clip.name.ToString () == "kick_C") {
			_SAPSDanimator.SetBool ("Combo2", false);
			_UIController._Animation = "Idle";
		}

	}
		
	void Guard(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Guard" , true);
		_UIController._Animation = "Idle";

	}

	void Damaged(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Damaged" , true);
		_UIController._Animation = "Idle";

	}

	void Damaged_down(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("DamagedDown" , true);
		_UIController._Animation = "Idle";

	}

	void Climb_End(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("ClimbEnd", true);

		if (_SAPSDanimator.GetCurrentAnimatorClipInfo (0) [0].clip.name.ToString () == "climb_end") {
			_SAPSDanimator.SetBool ("ClimbEnd", false);
			_UIController._Animation = "Idle";
		}


	}

	void Jump(){
		
		if (_SAPSDcontroller.isGrounded) {
			moveDirection.y = speedJump;
			_SAPSDanimator.SetBool ("Jump_Up", true);
		}


		if (_SAPSDcontroller.velocity.y > 0.0) {

		} else {
			_SAPSDanimator.SetBool ("Jump_Down", true);
		}

	}

	void W_Jump(){
		if (_SAPSDcontroller.isGrounded && jumpCount!=1) {
			moveDirection.y = speedJump;
			_SAPSDanimator.SetBool ("WJump_Up", true);
			jumpCount = 1;
		}

		if (_SAPSDcontroller.velocity.y < 0.0) {

			if (jumpCount < 2) {

				_SAPSDanimator.SetBool ("WJump_Turn", true);
				moveDirection.y = speedJump + moveDirection.y;
				jumpCount++;
			} else {
				_SAPSDanimator.SetBool ("WJump_Down", true);
			}
		}
	}

	void Victory(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Victory" , true);
		_UIController._Animation = "Idle";

	}

	void Defeat(){
		InitializeFlags ();
		_SAPSDanimator.SetBool ("Defeat" , true);
		_UIController._Animation = "Idle";

	}

		
	void InitializeFlags(){
		_SAPSDanimator.SetBool ("Idle" , false);
		_SAPSDanimator.SetBool ("Run" , false);
		_SAPSDanimator.SetBool ("Walk" , false);
		_SAPSDanimator.SetBool ("Sit" , false);
		_SAPSDanimator.SetBool ("Debuff" , false);
		_SAPSDanimator.SetBool ("Charge" , false);
		_SAPSDanimator.SetBool ("BeFloored" , false);
		_SAPSDanimator.SetBool ("Climb" , false);
		_SAPSDanimator.SetBool ("FlyingA" , false);
		_SAPSDanimator.SetBool ("FlyingB" , false);

		_SAPSDanimator.SetBool ("Kick_A" , false);
		_SAPSDanimator.SetBool ("Kick_B" , false);
		_SAPSDanimator.SetBool ("Kick_C" , false);
		_SAPSDanimator.SetBool ("Guard" , false);
		_SAPSDanimator.SetBool ("Combo1" , false);
		_SAPSDanimator.SetBool ("Combo2" , false);
		_SAPSDanimator.SetBool ("Damaged" , false);
		_SAPSDanimator.SetBool ("DamagedDown" , false);
		_SAPSDanimator.SetBool ("ClimbEnd" , false);
		_SAPSDanimator.SetBool ("Jump_Up" , false);
		_SAPSDanimator.SetBool ("Jump_Down" , false);
		_SAPSDanimator.SetBool ("WJump_Up" , false);
		_SAPSDanimator.SetBool ("Victory" , false);
		_SAPSDanimator.SetBool ("Defeat" , false);
	}

	void SetFacial(){

		if (_UIController._Facial == "Smile") {

			_SAPSDRenderer_Face.SetBlendShapeWeight(0,0);//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(1,0);//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(2,0);//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(3,0);//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(4,100);//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(5,100);//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(6,100);//eyes_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(7,100);//eyes_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(8,0);//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(9,0);//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(10,0);//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(11,100);//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(12,0);//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(13,0);//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(14,0);//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(15,0);//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight(16,0);//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight(17,0);//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight(18,0);//open
			_SAPSDRenderer_Face.SetBlendShapeWeight(19,0);//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight(20,100);//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight(21,0);//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight(22,0);//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight(0,0);//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(1,0);//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(2,0);//_Facial_brow_sad_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(3,0);//_Facial_brow_sad_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(4,100);//_Facial_brow_raise_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(5,100);//_Facial_brow_raise_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(6,100);//_Facial_brow_down_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(7,100);//_Facial_brow_down_L


		} else if (_UIController._Facial == "Anger"){
			_SAPSDRenderer_Face.SetBlendShapeWeight (0, 100);	//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight (1, 100);	//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight (2, 0);	//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight (3, 0);	//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight (4, 0);	//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight (5, 0);	//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight (6, 0);	//eye_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight (7, 0);	//eye_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight (8, 0);	//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight (9, 0);	//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight (10, 0);	//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight (11, 100);	//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight (12, 50);	//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight (13, 50);	//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight (14, 0);	//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight (15, 100);	//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight (16, 0);	//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight (17, 0);	//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight (18, 0);	//open
			_SAPSDRenderer_Face.SetBlendShapeWeight (19, 0);	//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight (20, 0);	//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight (21, 0);	//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight (22, 0);	//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight (0, 100);	//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight (1, 100);	//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight (2, 0);	//_Facial_brow_sad__R
			_SAPSDRenderer_Brow.SetBlendShapeWeight (3, 0);	//_Facial_brow_sad__L
			_SAPSDRenderer_Brow.SetBlendShapeWeight (4, 0);	//_Facial_brow_raise__R
			_SAPSDRenderer_Brow.SetBlendShapeWeight (5, 0);	//_Facial_brow_raise__L
			_SAPSDRenderer_Brow.SetBlendShapeWeight (6, 0);	//_Facial_brow_down__R
			_SAPSDRenderer_Brow.SetBlendShapeWeight (7, 0);	//_Facial_brow_down__L

		} else if (_UIController._Facial == "Sad"){

			_SAPSDRenderer_Face.SetBlendShapeWeight(0,0);//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(1,0);//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(2,100);//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(3,100);//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(4,0);//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(5,0);//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(6,30);	//eye_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(7,30);	//eye_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(8,0);//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(9,0);//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(10,0);//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(11,0);//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(12,0);//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(13,0);//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(14,0);//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(15,0);//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight(16,0);//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight(17,0);//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight(18,0);//open
			_SAPSDRenderer_Face.SetBlendShapeWeight(19,0);//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight(20,0);//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight(21,0);//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight(22,100);//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight(0,0);//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(1,0);//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(2,100);//_Facial_brow_sad_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(3,100);//_Facial_brow_sad_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(4,0);//_Facial_brow_raise_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(5,0);//_Facial_brow_raise_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(6,0);//_Facial_brow_down_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(7,0);//_Facial_brow_down_L

		} else if (_UIController._Facial == "Coward"){

			_SAPSDRenderer_Face.SetBlendShapeWeight(0,0);//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(1,0);//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(2,0);//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(3,0);//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(4,0);//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(5,0);//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(6,0);//eyes_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(7,0);//eyes_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(8,70);//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(9,70);//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(10,50);//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(11,0);//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(12,0);//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(13,0);//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(14,0);//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(15,0);//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight(16,100);//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight(17,0);//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight(18,0);//open
			_SAPSDRenderer_Face.SetBlendShapeWeight(19,0);//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight(20,0);//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight(21,0);//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight(22,0);//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight(0,0);//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(1,0);//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(2,100);//_Facial_brow_sad_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(3,100);//_Facial_brow_sad_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(4,50);//_Facial_brow_raise_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(5,50);//_Facial_brow_raise_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(6,15);//_Facial_brow_down_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(7,15);//_Facial_brow_down_L

		} else if (_UIController._Facial == "Serious"){

			_SAPSDRenderer_Face.SetBlendShapeWeight(0,50);//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(1,50);//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(2,0);//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(3,0);//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(4,0);//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(5,0);//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(6,25);//eyes_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(7,25);//eyes_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(8,0);//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(9,0);//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(10,0);//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(11,0);//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(12,0);//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(13,0);//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(14,0);//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(15,0);//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight(16,0);//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight(17,0);//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight(18,15);//open
			_SAPSDRenderer_Face.SetBlendShapeWeight(19,0);//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight(20,0);//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight(21,0);//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight(22,0);//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight(0,50);//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(1,50);//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(2,0);//_Facial_brow_sad_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(3,0);//_Facial_brow_sad_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(4,0);//_Facial_brow_raise_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(5,0);//_Facial_brow_raise_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(6,25);//_Facial_brow_down_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(7,25);//_Facial_brow_down_L


		} else if (_UIController._Facial == "Damage"){

			_SAPSDRenderer_Face.SetBlendShapeWeight(0,0);//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(1,0);//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(2,0);//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(3,0);//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(4,0);//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(5,0);//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(6,95);//eyes_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(7,95);//eyes_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(8,0);//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(9,0);//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(10,50);//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(11,30);//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(12,0);//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(13,0);//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(14,0);//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(15,0);//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight(16,10);//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight(17,0);//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight(18,0);//open
			_SAPSDRenderer_Face.SetBlendShapeWeight(19,0);//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight(20,0);//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight(21,0);//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight(22,0);//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight(0,0);//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(1,0);//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(2,100);//_Facial_brow_sad_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(3,100);//_Facial_brow_sad_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(4,0);//_Facial_brow_raise_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(5,0);//_Facial_brow_raise_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(6,50);//_Facial_brow_down_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(7,50);//_Facial_brow_down_L

		} else {
			_SAPSDRenderer_Face.SetBlendShapeWeight(0,0);	//eye_anger_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(1,0);	//eye_anger_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(2,0);	//eye_sad_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(3,0);	//eye_sad_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(4,0);	//eyes_happy_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(5,0);	//eyes_happy_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(6,0);	//eyes_close_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(7,0);	//eyes_close_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(8,0);	//eye_wide_R
			_SAPSDRenderer_Face.SetBlendShapeWeight(9,0);	//eye_wide_L
			_SAPSDRenderer_Face.SetBlendShapeWeight(10,0);	//AHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(11,0);	//EHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(12,0);	//EEHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(13,0);	//UHHHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(14,0);	//OHH
			_SAPSDRenderer_Face.SetBlendShapeWeight(15,0);	//triLips
			_SAPSDRenderer_Face.SetBlendShapeWeight(16,0);	//mouthSP01
			_SAPSDRenderer_Face.SetBlendShapeWeight(17,0);	//wide
			_SAPSDRenderer_Face.SetBlendShapeWeight(18,0);	//open
			_SAPSDRenderer_Face.SetBlendShapeWeight(19,0);	//smile
			_SAPSDRenderer_Face.SetBlendShapeWeight(20,0);	//bigSmile
			_SAPSDRenderer_Face.SetBlendShapeWeight(21,0);	//sad
			_SAPSDRenderer_Face.SetBlendShapeWeight(22,0);	//openSad

			_SAPSDRenderer_Brow.SetBlendShapeWeight(0,0);	//_Facial_brow_angry_R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(1,0);	//_Facial_brow_angry_L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(2,0);	//_Facial_brow_sad__R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(3,0);	//_Facial_brow_sad__L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(4,0);	//_Facial_brow_raise__R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(5,0);	//_Facial_brow_raise__L
			_SAPSDRenderer_Brow.SetBlendShapeWeight(6,0);	//_Facial_brow_down__R
			_SAPSDRenderer_Brow.SetBlendShapeWeight(7,0);	//_Facial_brow_down__L
		}

	}

	void LateUpdate(){

		if (_SAPSDcontroller.isGrounded) {
			if (_SAPSDanimator.GetBool ("Jump_Down") == true || _SAPSDanimator.GetBool ("WJump_Down") == true) {
				_SAPSDanimator.SetBool ("Jump_Up", false);
				_SAPSDanimator.SetBool ("Jump_Down", false);
				_SAPSDanimator.SetBool ("WJump_Up", false);
				_SAPSDanimator.SetBool ("WJump_Turn", false);
				_SAPSDanimator.SetBool ("WJump_Down", false);
				jumpCount = 0;
				_UIController._Animation = "Idle";
			}

		}

		if (_UIController._Facial == "Smile") {
			_t_bone_Eye_L.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);
			_eyeSore_GEO.SetActive (false);
			_eyeSore_close_GEO.SetActive (false);
			_sweat_GEO.SetActive (false);
		} else if (_UIController._Facial == "Anger") {
			_t_bone_Eye_L.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);
			_eyeSore_GEO.SetActive (false);
			_eyeSore_close_GEO.SetActive (false);
			_sweat_GEO.SetActive (false);
		} else if (_UIController._Facial == "Sad") {
			_t_bone_Eye_L.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_eyeSore_GEO.SetActive (false);
			_eyeSore_close_GEO.SetActive (false);
			_sweat_GEO.SetActive (false);
		} else if (_UIController._Facial == "Coward") {
			_t_bone_Eye_L.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);
			_eyeSore_GEO.SetActive (true);
			_eyeSore_close_GEO.SetActive (false);
			_sweat_GEO.SetActive (true);
		} else if (_UIController._Facial == "Serious") {
			_t_bone_Eye_L.transform.localScale = new Vector3 (0.7f, 0.8f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (0.7f, 0.8f, 1.0f);
			_eyeSore_GEO.SetActive (false);
			_eyeSore_close_GEO.SetActive (false);
			_sweat_GEO.SetActive (false);
		} else if (_UIController._Facial == "Damage") {
			_t_bone_Eye_L.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_eyeSore_GEO.SetActive (false);
			_eyeSore_close_GEO.SetActive (true);
			_sweat_GEO.SetActive (false);
		} else {
			_t_bone_Eye_L.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_t_bone_Eye_R.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			_eyeSore_GEO.SetActive (false);
			_eyeSore_close_GEO.SetActive (false);
			_sweat_GEO.SetActive (false);
		}
	}
}
