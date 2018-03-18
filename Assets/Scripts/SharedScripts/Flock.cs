using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is the modification of the implementation of the Boids behaviors from http://www.unifycommunity.com/wiki/index.php?title=Flocking
/// </summary>

public class Flock : MonoBehaviour 
{
	internal FlockController controller;

	private Rigidbody thisRigidBody;

	void Awake(){
	
		thisRigidBody = GetComponent<Rigidbody> ();

	}

    void Update()
    {

	
        if (controller)
        {
            Vector3 relativePos = steer() * Time.deltaTime;

			if (float.IsNaN (relativePos.x))
				return;
			
			if(relativePos != Vector3.zero)
				thisRigidBody.velocity = relativePos;


			

            // enforce minimum and maximum speeds for the boids
			float speed = thisRigidBody.velocity.magnitude;
            if (speed > controller.maxVelocity)
            {
				thisRigidBody.velocity = thisRigidBody.velocity.normalized * controller.maxVelocity;
            }
            else if (speed < controller.minVelocity)
            {
				thisRigidBody.velocity = thisRigidBody.velocity.normalized * controller.minVelocity;
            }
        }
    }

    //Calculate flock steering Vector based on the Craig Reynold's algorithm (Cohesion, Alignment, Follow leader and Seperation)
	private Vector3 steer()
	{
		Vector3 center = controller.flockCenter - transform.localPosition;			// cohesion
		Vector3 velocity = controller.flockVelocity - thisRigidBody.velocity; 			// alignment
		Vector3 follow = controller.target.localPosition - transform.localPosition; // follow leader
		Vector3 separation = Vector3.zero; 											// separation

        foreach (Flock flock in controller.flockList)
		{
            if (flock != this) 
            {
                Vector3 relativePos = transform.localPosition - flock.transform.localPosition;
				separation += relativePos / (relativePos.sqrMagnitude);		

				//flock.transform.LookAt (, Vector3.up);
			}
		}

        // randomize
		Vector3 randomize = new Vector3( (Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
		
		randomize.Normalize();
						
		return (controller.centerWeight*center + 
				controller.velocityWeight*velocity + 
				controller.separationWeight*separation + 
				controller.followWeight*follow + 
				controller.randomizeWeight*randomize);
	}	
}