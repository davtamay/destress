﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LookSelect : MonoBehaviour {

	//need to be provided a GO for it to work in VR
	public static bool isTimerAFactor = true;

	public GameObject selectedCard;
	public GameObject notNullOnSelected;
	public Camera cam;
	public GameObject firstCard;
	public GameObject secondCard;
	public bool isFirstCard;
	public bool isSecondCard;

	public int setCompleted;
	public ParticleSystem particles;
	public int spawnedCards;
	private CardSpawner cardSpawner;
	public Text coinText;
	private LookSelect lookSelect;


	[SerializeField] float timeToBeatFirstLevel;
	[SerializeField] float timeToBeatSecondLevel;
	//Beating second levels accomplishes mission, 3rd level is to see highscore?
	[SerializeField] float timeToBeatThirdLevel;





	void Start (){


		selectedCard = notNullOnSelected;

		cardSpawner = GetComponent<CardSpawner> ();
		cam = Camera.main;

		spawnedCards = cardSpawner.GetSpawned;
		//spawnedCards = CardSpawner.GetSpawned;

		GameController.Instance.TimeToAdd (ref isTimerDone, timeToBeatFirstLevel);
	}
	bool isTimerDone = false;
	void Update(){

	

		if (isTimerDone) {

			DataManager.Instance.CheckHighScore (SceneController.Instance.GetCurrentSceneName(), PlayerManager.Instance.points);
			GameController.Instance.isGameOver = true;
			//GameController.Instance.Paused = true;
			this.enabled = false;

		}

	
		if (isTimerAFactor) {
		

			GameController.Instance.TimeToAdd (ref isTimerDone);


		}
	




	

	Ray ray = new Ray (cam.transform.position, cam.transform.rotation * Vector3.forward);


	

	RaycastHit hit;

	if (Physics.Raycast (ray, out hit, 120)) {
		
			if (hit.transform.gameObject.CompareTag ("Card")) {

				//Debug.Log (selectedCard.transform.gameObject.name);

				if (hit.transform.gameObject.GetInstanceID () == selectedCard.GetInstanceID () || isSecondCard)
					return;
			



				
			


				selectedCard = hit.transform.gameObject;

			
			
				StartCoroutine ("CardSelect");
			}

		
	}else{

			/*if (!isFirstCard || !isSecondCard) {

				StopCoroutine ("CardSelect");

			} */
	}
   }
	IEnumerator CardSelect(){

		if (!isFirstCard && !isSecondCard) {
			
			isFirstCard = true;
			firstCard = selectedCard;

			StartCoroutine (RotateCard (firstCard));
		
			yield break;
		
		}else if (isFirstCard && !isSecondCard){
			
			isFirstCard = false;
			isSecondCard = true;

			secondCard = selectedCard;

			StartCoroutine (TestCards (firstCard, secondCard));
			StartCoroutine (RotateCard (secondCard));



		

		}

	
	}

	IEnumerator RotateCard (GameObject selected){

		Vector3 rot = new Vector3 (0,180,0);
		bool isRotating = true;
		float timeToRotate = 0.0f;

		Vector3 selectedCurRot = selected.transform.localEulerAngles;

		while (isRotating) {

			yield return null;

			timeToRotate += Time.deltaTime;

			//selected.transform.rotation = Quaternion.LE (selected.transform.rotation, selected.transform.rotation * Quaternion.AngleAxis (180, Vector3.up), Time.deltaTime * 2f);
				//Quaternion.AngleAxis (180, Vector3.up); //Quaternion.Lerp (selected.transform.rotation, selected.transform.rotation * Quaternion.AngleAxis (180, Vector3.up), Time.deltaTime * 2f);
				//Quaternion.RotateTowards (selected.transform.rotation, selected.transform.rotation * Quaternion.AngleAxis(90, Vector3.up), 5f);
				// Quaternion.LookRotation (-selected.transform.forward, Vector3.up);
				;//Quaternion.RotateTowards (selected.transform.rotation, Quaternion.AngleAxis(180, Vector3.up), 5f);
			selected.transform.Rotate (rot * Time.deltaTime);


			//error cards are not turning all the way
			if (timeToRotate > 1f){

		//	if (timeToRotate > 5f){
					isRotating = false;
					yield break;
			
			}
		}
	
	}
	IEnumerator TestCards(GameObject a, GameObject b){
	
		yield return new WaitForSeconds (1.4f);
		if (string.Equals(a.name,b.name, System.StringComparison.CurrentCultureIgnoreCase))
		{
			a.GetComponent<MeshCollider> ().enabled = false;
			b.GetComponent<MeshCollider> ().enabled = false;
			
			PlayerManager.Instance.points = 1;

			Debug.Log ("we have a match!");

			spawnedCards -= 2;

			PlayerManager.Instance.points = 1;

		//	particles.Play();
		//	yield return new WaitForSeconds (3);
		//	particles.Stop();


			if (spawnedCards == 0) {

				GameController.Instance.StopTimer ();
				StartCoroutine (GameController.Instance.NewWave ());

				particles.Play ();
				yield return new WaitForSeconds (3);
				particles.Stop();
			
				if (cardSpawner.GetWave == 0) {

					cardSpawner.ChangeWave (Difficulty.medium);
					GameController.Instance.TimeToAdd (ref isTimerDone, timeToBeatSecondLevel);
					spawnedCards = cardSpawner.GetSpawned;


				} else if (cardSpawner.GetWave == 1) {


					cardSpawner.ChangeWave (Difficulty.hard);
					GameController.Instance.TimeToAdd (ref isTimerDone, timeToBeatThirdLevel);
					spawnedCards = cardSpawner.GetSpawned;

				}else if (cardSpawner.GetWave == 2) {
					GameController.Instance.isGameOver = true;
				}

				GameController.Instance.ResumeTimer ();
			}

		}else {
			

			yield return StartCoroutine (RotateCard (firstCard));
			//85 is the original rotation of the card may have to change this by keeping and reflecting original value
			firstCard.transform.localEulerAngles = new Vector3 (85, 0,0); //Vector3.zero;
			firstCard = null;

		
	
		//	yield return *error: turn back faster
			yield return StartCoroutine (RotateCard (secondCard));

			secondCard.transform.localEulerAngles = new Vector3 (85, 0,0);//Vector3.zero;
			secondCard = null;

		
			selectedCard = notNullOnSelected;



		}
		isSecondCard = false;
	}


}
