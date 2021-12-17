﻿using UnityEngine;

// This class corresponds to the 3rd person camera features.
public class ThirdPersonOrbitCam : MonoBehaviour 
{
	public Transform player;                                           // Player's reference.
	public Vector3 pivotOffset = new Vector3(0.0f, 1.0f,  0.0f);       // Offset to repoint the camera.
	public Vector3 camOffset   = new Vector3(0.4f, 0.5f, -2.0f);       // Offset to relocate the camera related to the player position.
	public float smooth = 10f;                                         // Speed of camera responsiveness.
	//public float horizontalAimingSpeed = 6f;                           // Horizontal turn speed.
	//public float verticalAimingSpeed = 6f;                             // Vertical turn speed.
	public float maxVerticalAngle = 30f;                               // Camera max clamp angle. 
	public float minVerticalAngle = -60f;                              // Camera min clamp angle.
	public string XAxis = "Analog X";                                  // The default horizontal axis input name.
	public string YAxis = "Analog Y";                                  // The default vertical axis input name.

	private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
	private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.
	private Transform cam;                                             // This transform.
	private Vector3 relCameraPos;                                      // Current camera position relative to the player.
	private float relCameraPosMag;                                     // Current camera distance to the player.
	private Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation.
	private Vector3 smoothCamOffset;                                   // Camera current offset on interpolation.
	private Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate.
	private Vector3 targetCamOffset;                                   // Camera offset target to interpolate.
	private float defaultFOV;                                          // Default camera Field of View.
	private float targetFOV;                                           // Target camera Field of View.
	private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle.
	private float deltaH = 0;                                          // Delta to horizontaly rotate camera when locking its orientation.      
	private Vector3 firstDirection;                                    // The direction to lock camera for the first time.
	private Vector3 directionToLock;                                   // The current direction to lock the camera.
	private float recoilAngle = 0f;                                    // The angle to vertically bounce the camera in a recoil movement.
	private Vector3 forwardHorizontalRef;                              // The forward reference on horizontal plane when clamping camera rotation.
	private float leftRelHorizontalAngle, rightRelHorizontalAngle;     // The left and right angles to limit rotation relative to the forward reference.
	GameController_Grappling gameController;
	// Get the camera horizontal angle.
	public float GetH { get { return angleH; } }

	void Awake()
	{
		// Reference to the camera transform.
		cam = transform;

		// Set camera default position.
		cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
		cam.rotation = Quaternion.identity;

		// Get camera position relative to the player, used for collision test.
		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;

		// Set up references and default values.
		smoothPivotOffset = pivotOffset;
		smoothCamOffset = camOffset;
		defaultFOV = cam.GetComponent<Camera>().fieldOfView;
		angleH = player.eulerAngles.y;

		ResetTargetOffsets ();
		ResetFOV ();
		ResetMaxVerticalAngle();
		gameController = FindObjectOfType<GameController_Grappling>();
	}

	void Update()
	{
		if (Game.playerIdDead)
			return;
		// Get mouse movement to orbit the camera.
		// Mouse:
		if (!Application.isMobilePlatform)
        {
			angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * gameController.rotate_Slider.value;
			angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * gameController.rotate_Slider.value;
		}
        else
        {
			// Joystick:
			angleH += Mathf.Clamp(GameController_Grappling.rotate_Joystick.Horizontal, -1, 1) * 60 * gameController.rotate_Slider.value * Time.deltaTime;
			angleV += Mathf.Clamp(GameController_Grappling.rotate_Joystick.Vertical, -1, 1) * 60 * gameController.rotate_Slider.value * Time.deltaTime;

		}

		// Set vertical movement limit.
		angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

		// Set vertical camera bounce.
		angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, 10f*Time.deltaTime);

		// Handle camera orientation lock.
		if (firstDirection != Vector3.zero)
		{
			angleH -= deltaH;
			UpdateLockAngle();
			angleH += deltaH;
		}

		// Handle camera horizontal rotation limits if set.
		if(forwardHorizontalRef != default(Vector3))
		{
			ClampHorizontal();
		}

		// Set camera orientation.
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		cam.rotation = aimRotation;

		// Set FOV.
		cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp (cam.GetComponent<Camera>().fieldOfView, targetFOV,  Time.deltaTime);

		// Test for collision with the environment based on current camera position.
		Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
		Vector3 noCollisionOffset = targetCamOffset;
		for(float zOffset = targetCamOffset.z; zOffset <= 0; zOffset += 0.5f)
		{
			noCollisionOffset.z = zOffset;
			if (DoubleViewingPosCheck (baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0) 
			{
				break;
			} 
		}

		// Repostition the camera.
		smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
		smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);

		cam.position =  player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

		// Amortize Camera vertical bounce.
		if (recoilAngle > 0)
			recoilAngle -= 5 * Time.deltaTime;
		else if(recoilAngle < 0)
			recoilAngle += 5 * Time.deltaTime;
	}

	// Set/Unset horizontal rotation limit angles relative to custom direction.
	public void ToggleClampHorizontal(float LeftAngle = 0, float RightAngle = 0, Vector3 fwd = default(Vector3))
	{
		forwardHorizontalRef = fwd;
		leftRelHorizontalAngle = LeftAngle;
		rightRelHorizontalAngle = RightAngle;
	}

	// Limit camera horizontal rotation.
	private void ClampHorizontal()
	{
		// Get angle between reference and current forward direction.
		Vector3 cam2dFwd = this.transform.forward;
		cam2dFwd.y = 0;
		float angleBetween = Vector3.Angle(cam2dFwd, forwardHorizontalRef);
		float sign = Mathf.Sign(Vector3.Cross(cam2dFwd, forwardHorizontalRef).y);
		angleBetween = angleBetween * sign;

		// Get current input movement to compensate after limit angle is reached.
		float acc = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * gameController.rotate_Slider.value;
		acc += Mathf.Clamp(Input.GetAxis("Analog X"), -1, 1) * 60 * gameController.rotate_Slider.value * Time.deltaTime;

		// Limit left angle.
		if (sign < 0 && angleBetween < leftRelHorizontalAngle)
		{
			if (acc > 0)
				angleH -= acc;
		}
		// Limit right angle.
		else if (angleBetween > rightRelHorizontalAngle)
		{
			if (acc < 0)
				angleH -= acc;
		}
	}

	// Bounce the camera vertically.
	public void BounceVertical(float degrees)
	{
		recoilAngle = degrees;
	}

	// Handle current camera facing when locking on a specific dynamic orientation.
	private void UpdateLockAngle()
	{
		directionToLock.y = 0f;
		float centerLockAngle = Vector3.Angle(firstDirection, directionToLock);
		Vector3 cross = Vector3.Cross(firstDirection, directionToLock);
		if (cross.y < 0) centerLockAngle = -centerLockAngle;
		deltaH = centerLockAngle;
	}

	// Lock camera orientation to follow a specific direction. Usually used in short movements.
	// Example uses: (player turning cover corner, skirting convex wall, vehicle turning)
	public void LockOnDirection(Vector3 direction)
	{
		if (firstDirection == Vector3.zero)
		{
			firstDirection = direction;
			firstDirection.y = 0f;
		}
		directionToLock = Vector3.Lerp(directionToLock, direction, 0.15f * smooth * Time.deltaTime);
	}

	// Unlock camera orientation to free mode.
	public void UnlockOnDirection()
	{
		deltaH = 0;
		firstDirection = directionToLock = Vector3.zero;
	}

	// Set camera offsets to custom values.
	public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCamOffset = newCamOffset;
	}

	// Reset camera offsets to default values.
	public void ResetTargetOffsets()
	{
		targetPivotOffset = pivotOffset;
		targetCamOffset = camOffset;
	}

	// Reset the camera vertical offset.
	public void ResetYCamOffset()
	{
		targetCamOffset.y = camOffset.y;
	}

	// Set camera vertical offset.
	public void SetYCamOffset(float y)
	{
		targetCamOffset.y = y;
	}

	// Set camera horizontal offset.
	public void SetXCamOffset(float x)
	{
		targetCamOffset.x = x;
	}

	// Set custom Field of View.
	public void SetFOV(float customFOV)
	{
		this.targetFOV = customFOV;
	}

	// Reset Field of View to default value.
	public void ResetFOV()
	{
		this.targetFOV = defaultFOV;
	}

	// Set max vertical camera rotation angle.
	public void SetMaxVerticalAngle(float angle)
	{
		this.targetMaxVerticalAngle = angle;
	}

	// Reset max vertical camera rotation angle to default value.
	public void ResetMaxVerticalAngle()
	{
		this.targetMaxVerticalAngle = maxVerticalAngle;
	}

	// Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
	bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
	{
		float playerFocusHeight = player.GetComponent<CapsuleCollider> ().height *0.5f;
		return ViewingPosCheck (checkPos, playerFocusHeight) && ReverseViewingPosCheck (checkPos, playerFocusHeight, offset);
	}

	// Check for collision from camera to player.
	bool ViewingPosCheck (Vector3 checkPos, float deltaPlayerHeight)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, player.position+(Vector3.up* deltaPlayerHeight) - checkPos, out hit, relCameraPosMag))
		{
			// ... if it is not the player...
			if(hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
			{
				// This position isn't appropriate.
				return false;
			}
		}
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		return true;
	}

	// Check for collision from player to camera.
	bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
	{
		RaycastHit hit;

		if(Physics.Raycast(player.position+(Vector3.up* deltaPlayerHeight), checkPos - player.position, out hit, maxDistance))
		{
			if(hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
			{
				return false;
			}
		}
		return true;
	}

	// Get camera magnitude.
	public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
	{
		return Mathf.Abs ((finalPivotOffset - smoothPivotOffset).magnitude);
	}
}
