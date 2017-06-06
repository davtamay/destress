﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveAnimations : MonoBehaviour {

	public Animator thisAnimator;

	private int animRandHash = Animator.StringToHash("Random");

	public bool isRandomOn = true;

	public Vector3 initialPos; 
	[SerializeField] private Vector3 curWayPoint;

	[SerializeField] private float moveForwardSpeed = 3f;

	[SerializeField] private float disUntilWayPointChange;
	public float distaceToSearch = 10;

	[SerializeField] private int perFrameChanceOfRandom = 1000;

	public bool isFirstTime;

	bool isTurning = false;
	Vector3 oldWayPoint;
	Quaternion rotationToLookTo;

	void Awake(){

		thisAnimator = GetComponent<Animator> ();
		initialPos = transform.position;

		//animRandHash = thisAnimator.StringToHash("Random");
	}

	public void Start(){

		//onUpdateIEnum = OnUpdate();
			
	//	onUpdateCoroutine = 
		StartCoroutine (OnUpdate());
	}

	IEnumerator OnUpdate(){

		while (true) {
			yield return null;


			if (isRandomOn) {
				
				Vector3 dir;

				if (thisAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Walk")) {
			
			
					if (!isFirstTime) {

						curWayPoint = initialPos + Random.insideUnitSphere * distaceToSearch;
						curWayPoint.y = initialPos.y; 

						isFirstTime = true;
					}

					if (Vector3.Distance (transform.position, curWayPoint) < disUntilWayPointChange) {

				
						curWayPoint = initialPos + Random.insideUnitSphere * distaceToSearch;
						curWayPoint.y = initialPos.y; 

					

						yield return StartCoroutine (Turn (curWayPoint));
			

					}
	


					dir = (curWayPoint - transform.position).normalized;

					Vector3 movePosition = dir * Time.deltaTime * moveForwardSpeed;
					movePosition.y = 0;

					transform.position += movePosition;

					transform.LookAt (curWayPoint, Vector3.up);

					thisAnimator.SetInteger (animRandHash, Random.Range (0, perFrameChanceOfRandom));
			
				}
			}
		}
		
		}

	public IEnumerator Turn(Vector3 toRotation){

		Quaternion targetRotation = Quaternion.LookRotation (toRotation - transform.position);
		float timer = 0;

		while (true) {
			timer += Time.deltaTime;
		
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation,3f);
		
			if (Quaternion.Dot (transform.rotation, targetRotation) >= 0.95 || timer > 7) 
				yield break;

			yield return null;
	
		}
	}

	
	
}

