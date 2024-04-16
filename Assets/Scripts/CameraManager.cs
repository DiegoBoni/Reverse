using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
	[Header("Camera Settings")]
	[SerializeField] private Transform target; 
	[SerializeField] private float followSpeed = 3; 
	[SerializeField] private float mouseSpeed = 2; 
	//[SerializeField] private float controllerSpeed = 5; 
	[SerializeField] private float cameraDist = 3; 
	[SerializeField] private float lookAngle;
	[SerializeField] private float tiltAngle; 
	[SerializeField] private float minAngle = -35; 
	[SerializeField] private float maxAngle = 35; 
	[SerializeField] private bool IsTouch;

	[HideInInspector] public Transform pivot; 
	[HideInInspector] public Transform camTrans; 

	private float turnSmoothing = .1f; 
	
	private float smoothX;
	private float smoothY;
	private float smoothXvelocity;
	private float smoothYvelocity;

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		camTrans = Camera.main.transform;
		pivot = camTrans.parent;
	}

	private void FollowTarget(float d)
	{ 
		float speed = d * followSpeed; 
		Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed); 
		transform.position = targetPosition; 
	}

	private void HandleRotations(float d, float v, float h, float targetSpeed)
	{ 
		if (turnSmoothing > 0)
		{
			smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing); 
			smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
		}
		else
		{
			smoothX = h;
			smoothY = v;
		}

		tiltAngle -= smoothY * targetSpeed; 
		tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle); 
		pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0); 

		lookAngle += smoothX * targetSpeed; 
		transform.rotation = Quaternion.Euler(0, lookAngle, 0); 

	}

	private void FixedUpdate()
	{
		float h;
		float v;

	    if(IsTouch)
		{
			h = Input.touches[0].deltaPosition.x * 0.05f; 
			v = Input.touches[0].deltaPosition.y * 0.05f; 

		}
		else
		{
			h = Input.GetAxis("Mouse X");
			v = Input.GetAxis("Mouse Y");
		}
		
		//float c_h = Input.GetAxis("RightAxis X");
		//float c_v = Input.GetAxis("RightAxis Y");

		float targetSpeed = mouseSpeed;

		/*if (c_h != 0 || c_v != 0)
		{ //Overwrites if i use joystick
			h = c_h;
			v = -c_v;
			targetSpeed = controllerSpeed; 
		}*/

		FollowTarget(Time.deltaTime); 
		HandleRotations(Time.deltaTime, v, h, targetSpeed); 
	}

	private void LateUpdate()
	{
		
		float dist = cameraDist + 1.0f; 
		Ray ray = new Ray(camTrans.parent.position, camTrans.position - camTrans.parent.position);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, dist))
		{
			if (hit.transform.tag == "Wall")
			{
				// store the distance;
				dist = hit.distance - 0.25f;
			}
		}
	
		if (dist > cameraDist) dist = cameraDist;
		camTrans.localPosition = new Vector3(0, 0, -dist);
	}
}
