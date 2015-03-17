using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


	//This component is only enabled to my player. IE the player belonging to the local client

	public float speed = 10f;
	public float jumpSpeed = 6f;

	//Booking variables
	Vector3 direction = Vector3.zero; //Forward/back/left/right
	float verticalVelocity = 0;

	CharacterController cc;
	Animator anim;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//WASD forward/back/left/right movement is stored in "direction"
		direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if (direction.magnitude > 1f) {
			direction = direction.normalized;
		}

		//Set our animator "Speed" parameter. This will move us from "idle" to "run" animation
		anim.SetFloat("Speed", direction.magnitude); // Use Input.GetAxis("Vertical")  to get +1 for forward and -1 for backwards

		//Handle Jumping
		//If we're on the ground and the player wants to jump, set verticalVelocity to a positive number.
		//If you want double-jumping, you'll want some extra code here instead of just checking "cc.isGrounded"
		if (cc.isGrounded && Input.GetButtonDown("Jump")) {
			verticalVelocity = jumpSpeed;
		}

		AdjustAimAngle();
	}

	void AdjustAimAngle() {
		Camera myCamera = this.GetComponentInChildren<Camera>();

		if(myCamera == null) {
			Debug.LogError ("Why doesn't my character have a camera? this is an FPS!");
			return;
		}

		float AimAngle = 0;

		if(myCamera.transform.rotation.eulerAngles.x <= 90f) {
			//We are looking down
			AimAngle = -myCamera.transform.rotation.eulerAngles.x;
		}
		else {
			AimAngle = 360 - myCamera.transform.rotation.eulerAngles.x;
		}

		anim.SetFloat("AimAngle", AimAngle);
	}

	//FixedUpdate is called once per physics loop
	//Do all movement and other physics stuff here
	void FixedUpdate() {

		// "direction" is the desired movement direction, based on our players input
		Vector3 dist = direction * speed * Time.deltaTime;

		if (cc.isGrounded && verticalVelocity < 0) {

			//We are currently on the ground and vertical velocity is not positive (i.e. we are not starting a jump)

			//Ensure that we aren't playing the jumping animation
			anim.SetBool("Jumping", false);

			//Set our vertical velocity to *almost* zero. This ensures that:
			// a) we don't start falling at warp speed if we fall off of a cliff
			// b) cc.isGrounded returns true every frame (by still being slightly negative, as opposed to zero)
			verticalVelocity = Physics.gravity.y * Time.deltaTime;
		}

		else {
			//We are either not grounded, or we have a positive verticalVelocity (i.e. we ARE starting a jump)

			//To make sure we don't go into the jump animation while walking down a slope, make sure that verticalVelocity is above some 
			//arbitrary threshold before triggering the animation. 75% of "jumpSpeed" seems like a good number, but could be a standalone public variable too.

			//Another option would be to do a raycast down and start the jump/fall animation whenever we were more than ___ distance above the ground.
			if (Mathf.Abs(verticalVelocity) > jumpSpeed*0.75f) {
				anim.SetBool("Jumping", true);
			}

			//Apply gravity
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
		}

		//Add our verticalVelocity to our actual movement for this frame
		dist.y = verticalVelocity * Time.deltaTime;

		//Apply the movement to our character controller (which handles collisions for us)
		cc.Move(dist);
	}
}
