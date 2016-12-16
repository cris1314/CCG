using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

	public float defaultRunSpeed = 7.0f;
	public float speedUpRate = 5.0f;
	public Text elapseTimeGUIText;
	public Text distanceGUIText;
	private float runSpeed;
	private	int runDirection;
	private Vector3 startpoint;
	private Rigidbody rgbody;
	public float movementForce;
	string currentDirection = "Front";
	// Use this for initialization
	private Animator anim;
	//Jump
	//public float heightToJump = 8.0f;
	private bool isFalling = false;
	public float jumpforce;
	public static PlayerController player;

	void Start()
	{
		anim = this.GetComponent<Animator> ();
		player = this;
		startpoint = this.transform.position;
		this.runSpeed = this.defaultRunSpeed;
		rgbody = this.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update()
	{
		//---------------------------Screen
		this.processKeyInput();
		this.processTouchInput();
		//----------------------------Movement
		this.move();
		this.updateElapsedTimeLabel();
		this.speedUp();
		this.calcDistance ();

	}

	private void calcDistance(){
		float distance;
		distance = Vector3.Distance (startpoint, this.transform.position);
		float distanceMeters = (distance * (2.54f / 96)) * 10;
		distanceGUIText.text = "X: " + Input.acceleration.x.ToString () + " Y: " + Input.acceleration.y.ToString ();
	}
	// touch
	private Vector2 touchStartPos;

	private void processTouchInput()
	{
		if (Input.touchCount > 0) {

		Touch touch = Input.touches [0];
		TouchPhase phase = touch.phase;
		if (phase == TouchPhase.Began) {
			touchStartPos = touch.position;
		} else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled) {
				if(isFalling == false){
					if (touch.position.x < (touchStartPos.x - 75)) {
						runDirection -= 90;
					} else if (touch.position.x > (touchStartPos.x + 75)){
						runDirection += 90;

					}
				}

				if (touch.position.y > (touchStartPos.y + 85) && isFalling == false) {
						Jump ();
						isFalling = true;
						arrowKeyPressed = true;

				} else if (touch.position.y < (touchStartPos.y - 85) && isFalling == false) {
					StartCoroutine ("DownMovement");

					arrowKeyPressed = true;
				}
		}
		}

		if (Input.acceleration.x < 0) {
			switch(currentDirection){
			case "Front":
				rgbody.AddForce (Input.acceleration.x * 500, 0, 0);
				break;
			case "Left":
				rgbody.AddForce (0, 0, Input.acceleration.x * 500);
				break;
			case "Back":
				rgbody.AddForce (Input.acceleration.x * 500, 0, 0);
				break;
			case "Right":
				rgbody.AddForce (0, 0, Input.acceleration.x * 500);
				break;
			}
		} else if(Input.acceleration.x > 0){
			switch(currentDirection){
			case "Front":
				rgbody.AddForce (Input.acceleration.x * 500 , 0, 0);
				break;
			case "Left":
				rgbody.AddForce (0, 0, Input.acceleration.x * 500);
				break;
			case "Back":
				rgbody.AddForce (Input.acceleration.x * 500, 0, 0);
				break;
			case "Right":
				rgbody.AddForce (0, 0, Input.acceleration.x * 500);
				break;
			}

		}
	}

	//keyborad
	private float lastRotateTime;
	private bool  arrowKeyPressed;

	private void processKeyInput()
	{
		if (!arrowKeyPressed && Time.time - lastRotateTime > 0.1f) {
			if (Input.GetKey(KeyCode.LeftArrow)) {
				
					runDirection -= 90;  
					lastRotateTime = Time.time;
					arrowKeyPressed = true;
					Debug.Log (runDirection);


			} else if (Input.GetKey(KeyCode.RightArrow)) {
					
					runDirection += 90;
					lastRotateTime = Time.time;
					arrowKeyPressed = true;
					Debug.Log (runDirection);

			}
			if (Input.GetKey(KeyCode.UpArrow) && isFalling == false) { 
				Jump ();
				isFalling = true;
				arrowKeyPressed = true;

			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				StartCoroutine ("DownMovement");
				  
				arrowKeyPressed = true;

			}
			if (Input.GetKey(KeyCode.A)) {
				switch(currentDirection){
				case "Front":
					rgbody.AddForce (-movementForce * 100, 0, 0);
					break;
				case "Left":
					rgbody.AddForce (0, 0, movementForce * 100);
					break;
				case "Back":
					rgbody.AddForce (movementForce * 100, 0, 0);
					break;
				case "Right":
					rgbody.AddForce (0, 0, -movementForce * 100);
					break;
				}

			
			}
			if (Input.GetKey(KeyCode.D)) {
				switch(currentDirection){
				case "Front":
					rgbody.AddForce (movementForce * 100, 0, 0);
					break;
				case "Left":
					rgbody.AddForce (0, 0, -movementForce * 100);
					break;
				case "Back":
					rgbody.AddForce (-movementForce * 100, 0, 0);
					break;
				case "Right":
					rgbody.AddForce (0, 0, movementForce * 100);
					break;
				}

			}
		}

		if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) {
			arrowKeyPressed = false;
		}
	}

	private void move()
	{

		this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, runDirection, 0), 0.25f);

		Vector3 v = transform.forward * this.runSpeed;
		v.y = this.GetComponent<Rigidbody>().velocity.y;
		this.GetComponent<Rigidbody>().velocity = v;
	}

	private void speedUp()
	{
		//speed up 0.1 per 10sec.
		this.runSpeed = defaultRunSpeed + Time.time / 10.0f * this.speedUpRate;
	}

	void Jump(){
		rgbody.AddForce (0,jumpforce * 120,0);

	}
	IEnumerator DownMovement(){
		Debug.Log ("abajo");
		anim.SetBool ("Down",true);
		yield return new WaitForSeconds (1);
		anim.SetBool ("Down",false);
	}
	private void updateElapsedTimeLabel()
	{
		float now = Time.time;
		int sec = (int)now;
		float mil = (now - (float)sec) * 100.0f;
		this.elapseTimeGUIText.text = "Time: " + sec.ToString() + mil.ToString(":00");
			//string.Format("{0:00}:{1:00}", sec, mil);
	}
	void OnCollisionEnter(Collision collision) {
		switch(collision.gameObject.tag){
		case "Ground":
			isFalling = false;
			break;
		case "Obstacle":
			Death ();
			break;
		
		}



	}

	void OnTriggerEnter(Collider other) {
		switch (other.tag) {
		case "LeftDirection":
			currentDirection = "Left";
			Debug.Log ("adsadd");
			break;
		case "FrontDirection":
			currentDirection = "Front";
			break;
		case "BackDirection":
			currentDirection = "Back";
			break;
		case "RightDirection":
			currentDirection = "Right";
			break;
		}
	}
	public void Death(){
		Debug.Log ("Dead");
		anim.SetBool ("Dead", true);
		Invoke ("Reset",1.5f);
	}

	void Reset (){
		GameManager.gmanager.ResetScene ();
	}
	//private void checkFail() {
	//	if (transform.position.y < -10) {
	//		transform.position = new Vector3(0, 5, 0);
	////		this.runSpeed = this.defaultRunSpeed;

		////	GroundControl con = obj.GetComponent("GroundControl") as GroundControl;
			//con.resetGame();
	//	}
	}

