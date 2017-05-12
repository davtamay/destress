﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleInteraction : InteractionBehavior {


	[SerializeField]private Transform positionToMoveTo;
	[SerializeField]private Transform positionToGetOut;

	[SerializeField]private float velocity = 0.7f;//private float speedOfBike;
	[SerializeField]private float rotSpeed = 5f;
	[SerializeField] private float minMoveAngleFromUp = 89.0f;
	[SerializeField] private float maxMoveAngleFromUp = 180.0f;

	private bool isOnBike;



	private Vector3 moveDirection;
	private Coroutine Drive;



	public void MoveToPosition(){

	
		if (!isOnBike) {
			ReachedProximity ();
			player.position = positionToMoveTo.position;
			player.GetComponent<CollectorLookWalk> ().enabled = false;
			Drive = StartCoroutine (DriveBike ());
			isOnBike = true;
		} else {
		
			player.position = positionToGetOut.position;
			player.GetComponent<CollectorLookWalk> ().enabled = true;
			StopCoroutine (Drive);
			isOnBike = false;
			
		}

		
	}

	IEnumerator DriveBike(){
	
		//thisTransform.SetParent (player, false);
		//player.SetParent(thisTransform,true);
		while (true) {
			player.position = positionToMoveTo.position;
			yield return new WaitForEndOfFrame();
			player.position = positionToMoveTo.position;
			moveDirection = thisTransform.TransformDirection (Vector3.forward);//thisTransform.forward.normalized;
			player.position = Vector3.Lerp (player.position, positionToMoveTo.position, Time.deltaTime * 3);
			//player.position = positionToMoveTo.position;

			thisTransform.localRotation = Quaternion.RotateTowards(thisTransform.rotation, Quaternion.Euler (new Vector3(0, Camera.main.transform.eulerAngles.y,0)), Time.deltaTime * 20 * rotSpeed); //Quaternion.AngleAxis (plare.position, Vector3.up);//Quaternion.RotateTowards(thisTransform.rotation, Quaternion.LookRotation(player.position), Time.deltaTime * rotSpeed);
		

			if (minMoveAngleFromUp < CameraAngleFromUp() && CameraAngleFromUp() < maxMoveAngleFromUp) {

				moveDirection.x = 0;
				moveDirection.z = 0;

			} 

		//	moveDirection.x *= velocity ;
		//	moveDirection.z *= velocity ;
			//moveSpeed based On Tilt
			moveDirection *= (90 - CameraAngleFromUp ()) * Time.deltaTime * velocity;
			moveDirection.y = thisTransform.position.y;

			thisTransform.localPosition += moveDirection;

		//	thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, Quaternion.LookRotation(Camera.main.transform.position), Time.deltaTime *20);
			yield return null;

		}
	
	}

	private float CameraAngleFromUp(){
		return Vector3.Angle (Vector3.up, Camera.main.transform.rotation * Vector3.forward);}
	

}