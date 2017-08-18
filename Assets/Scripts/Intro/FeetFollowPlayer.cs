﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetFollowPlayer : MonoBehaviour {

	private Transform camTransform;
	//[SerializeField]private float rotationToSpeed = 1;
	private Transform thisTransform;

	void Start () {

	
		thisTransform = GetComponent<Transform>();
		camTransform = Camera.main.transform;


	}


	void Update () {
		


		RaycastHit hit;


		thisTransform.rotation = Quaternion.AngleAxis (camTransform.eulerAngles.y, Vector3.up);

	
		if (Physics.Raycast (transform.position, -transform.up, out hit, 10f)) {


			transform.rotation = (Quaternion.FromToRotation (transform.up, hit.normal)) * transform.rotation;

		}
			

	//	thisTransform.rotation = Quaternion.AngleAxis (camTransform.eulerAngles.y, Vector3.up);

	//	AlignTransform (thisTransform);

		thisTransform.rotation *= Quaternion.Euler (Vector3.right* 90);
			
	

	}



	public static void AlignTransform(Transform transform)
	{
		Vector3 sample = SampleNormal(transform.position);

		Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, sample)) * sample;
		transform.rotation = Quaternion.LookRotation(proj, sample);
	}

	public static Vector3 SampleNormal(Vector3 position)
	{
		Terrain terrain = Terrain.activeTerrain;
		var terrainLocalPos = position - terrain.transform.position;
		var normalizedPos = new Vector2(
			Mathf.InverseLerp(0f, terrain.terrainData.size.x, terrainLocalPos.x),
			Mathf.InverseLerp(0f, terrain.terrainData.size.z, terrainLocalPos.z)
		);
		var terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);

		return terrainNormal;
	}
}

