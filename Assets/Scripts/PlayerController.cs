using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public	float 	speed;
	public	Text	countText;
	public	Text	winText;

	private Rigidbody	rb;
	private	int	count;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		count = 0;
		winText.text = "";
		SetCountText (null);
	}
		
	// Apply as Movement
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	// using OnTrigger enter, not on Colider enter
	void OnTriggerEnter(Collider other) 
	{
		Debug.Log ("OnTriggerEnter") ;

		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText (other);
		}

		if (other.gameObject.name.StartsWith ("NorthWall"))
		{
			// Kick Off the Cleanup Script
			Debug.Log ("Kicked off Cleanup Script...") ;
			CleanUp ();
			Debug.Log ("After Cleanup Script...") ;

		}
	}


	void SetCountText (Collider other) {
		countText.text = "Count: " + count.ToString ();

		if (count == 12) {
			winText.text = "You win!";
		}
		else {
			// PRINT the position of the PickUp that was hit.
			if (other != null)
			{
				Debug.Log (">" + other.name + "=" + other.transform.position.ToString ());
			}
		}
	}


	void CleanUp () {


		// Player Ball hits the NorthWall...We Clean up the Balls
		// move Player to closest Active 'Pick Up'
		// Get a list of the Active 'Pick Up
		float step;
		Vector3 myPos;
		Vector3 pickUpPos;
		GameObject[] myPickUps = null;

		// Initialize vector with the coords of the ("Pick Up") tagged objects
		if (myPickUps == null) {  myPickUps = GameObject.FindGameObjectsWithTag("Pick Up"); }

		// HOOK TO A KEYSTROKE START/PLAY :: User this code to collide with each of the pickUps
		foreach (GameObject pickUp in myPickUps)
		{
			// Move Player from curPos - to - Active pickUp.Pos (in list)
			if ( pickUp.activeSelf ) 
			{
				// find the first [closest] pickUp position (refinement of this, first pass just hit them all)
				myPos = transform.position;
				pickUpPos = pickUp.transform.position;
				step = 30 * speed * Time.deltaTime;
				Debug.Log ("Cleanup: Player =" + transform.position + ".  Pickup =" + pickUp.transform.position + "<  speed =" + 
					speed + "<   deltatime=" + Time.deltaTime + "<");

				transform.position = Vector3.MoveTowards(transform.position, pickUp.transform.position, 10000);
				// transform.position = Vector3.MoveTowards(pickUp.transform.position, transform.position, step);

				/* move player toward closest pickUp position @ x speed
				// 
				// player repeat til done
				rb.AddForce (pickUp.transform.position * speed);
				public Transform target;
				public float speed;
				void Update() {
					float step = speed * Time.deltaTime;
					transform.position = Vector3.MoveTowards(transform.position, pickUp.transform.position, step);
				}
				*/
			}
		}
	}
		
}
