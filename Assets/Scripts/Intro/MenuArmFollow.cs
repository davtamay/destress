﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArmFollow : MonoBehaviour {

	// Use this for initialization
	private Transform thisTransform;
	private Animator thisAnimator;

	private Transform camTransform;
	private GameObject stressMenu;

	private Vector3 offset;

	void Awake(){
		stressMenu = GameObject.FindWithTag ("StressMenu");
		thisTransform = GetComponent<Transform> ();
		thisAnimator = GetComponentInChildren<Animator> ();

		camTransform = Camera.main.transform;
		offset = thisTransform.position - Camera.main.transform.position;

		//offset.z *= -1;
	}
	void Start(){
	
		thisTransform.GetChild(0).gameObject.SetActive(false);
	
	}
	


	Quaternion rotation;

	bool isInitialClick = false;

	//this is checked off by button look click (BackToGame Button)
	bool isClosedClick = false;

	public void BoolClosed(){
		thisAnimator.SetBool ("Close", true);
		isClosedClick = true;
	}


	Vector3 oldViewingAngle;
	Vector3 curViewingAngle;

	bool isLerping = false;
	void LateUpdate () {
		if (Input.GetMouseButton (0)) {

			thisTransform.GetChild(0).gameObject.SetActive(true);
			oldViewingAngle = camTransform.forward;
			isInitialClick = true;

		}

		if (isInitialClick) {
		
			if (thisAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Idle")) {
				stressMenu.SetActive (true);
				isInitialClick = false;
			
			
			}
		
		} else if (isClosedClick) {

				

			if (thisAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Start")) {


				thisAnimator.SetBool ("Close", false);
				stressMenu.SetActive (false);
				//thisAnimator.Rebind ();
				thisTransform.GetChild(0).gameObject.SetActive(false);

			}

		
		}

		if (GameController.Instance.IsMenuActive) {

			curViewingAngle = camTransform.forward;

			if (Vector3.Angle (oldViewingAngle, curViewingAngle) > 80 || isLerping) {

				isLerping = true;

				rotation = Quaternion.Euler (0, camTransform.eulerAngles.y, 0);

				thisTransform.position = Vector3.Lerp (thisTransform.position, camTransform.position - (rotation * (offset * -1)), Time.deltaTime * 3f);

				thisTransform.LookAt (2 * thisTransform.position - camTransform.position, camTransform.up);
			


					if (Vector3.Distance(thisTransform.position, camTransform.position - (rotation * (offset * -1))) < 0.05f){
					isLerping = false;
					oldViewingAngle = curViewingAngle;
					}
			}
			//Debug.Log (Vector3.Distance(thisTransform.position, camTransform.position - (rotation * (offset * -1))));
		}
			
		
	}
}