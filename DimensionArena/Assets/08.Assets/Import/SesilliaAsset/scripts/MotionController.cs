using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionController : MonoBehaviour {

	internal string _Animation = null;
	internal string _Facial = "Default";
	internal string _EyesChangeType = null;
	internal string _GeneralChangeType = null;
	internal float _FacialValue = 0.0f;
	internal bool _FacialValueBool = false;

//	private Slider _SliderCloseEyeL;
//	private Slider _SliderCloseEyeR;
//
//	void Start () {
//		_SliderCloseEyeL = GameObject.Find ("Slider_CloseEyeL").GetComponent<Slider> ();
//		_SliderCloseEyeR = GameObject.Find ("Slider_CloseEyeR").GetComponent<Slider> ();
//
//	}
//		
	/// <summary>
	/// Set Loopable Animation flag
	/// </summary>

	public void SetIdle(){
		_Animation = "Idle";
	}

	public void SetRun(){
		_Animation = "Run";
	}

	public void SetWalk(){
		_Animation = "Walk";
	}

	public void SetSit(){
		_Animation = "Sit";
	}

	public void SetDebuff(){
		_Animation = "Debuff";
	}
		
	public void SetCharge(){
		_Animation = "Charge";
	}

	public void SetBeFloored(){
		_Animation = "BeFloored";
	}

	public void SetClimb(){
		_Animation = "Climb";
	}

	public void SetFlyingA(){
		_Animation = "FlyingA";
	}

	public void SetFlyingB(){
		_Animation = "FlyingB";
	}

	/// <summary>
	/// Set NonLoopable Animation flag
	/// </summary>
	/// 
	/// 

	public void SetKick_A(){
		_Animation = "Kick_A";
	}

	public void SetKick_B(){
		_Animation = "Kick_B";
	}

	public void SetKick_C(){
		_Animation = "Kick_C";
	}

	public void SetCombo1(){
		_Animation = "Combo1";
	}

	public void SetCombo2(){
		_Animation = "Combo2";
	}

	public void SetGuard(){
		_Animation = "Guard";
	}

	public void SetDamaged(){
		_Animation = "Damaged";
	}

	public void SetDamaged_down(){
		_Animation = "Damaged_down";
	}

	public void SetClimbEnd(){
		_Animation = "Climb_End";
	}

	public void SetJump(){
		_Animation = "Jump";
	}

	public void SetWJump(){
		_Animation = "W_Jump";
	}

	public void SetVictory(){
		_Animation = "Victory";
	}

	public void SetDefeat(){
		_Animation = "Defeat";
	}

	public void SetFacialSmile(){
		_Facial = "Smile";

	}

	public void SetFacialAnger(){
		_Facial = "Anger";

	}

	public void SetFacialSad(){
		_Facial = "Sad";

	}

	public void SetFacialCoward(){
		_Facial = "Coward";

	}

	public void SetFacialSerious(){
		_Facial = "Serious";

	}

	public void SetFacialDamage(){
		_Facial = "Damage";

	}

	public void SetFacialDefault(){
		_Facial = "Default";

	}		
//	public void SetFacial_Eye_L_Closed()
//	{
//		_GeneralChangeType = "eyes";
//		_EyesChangeType = "closedL";
//		_FacialValue = _SliderCloseEyeL.value * 100;
//	}
//
//	public void SetFacial_Eye_R_Closed()
//	{
//		_GeneralChangeType = "eyes";
//		_EyesChangeType = "closedR";
//		_FacialValue = _SliderCloseEyeR.value * 100;
//	}

}
