using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class CharacterControls : MonoBehaviour 
{	
	[Header("Settings Character")]
	[SerializeField] private Animator _character;
	[SerializeField] private GameObject _cam;
	[SerializeField] private string _idleAnimation;
	[SerializeField] private string _runAnimation;
	[SerializeField] private string _jumpAnimation;
	[SerializeField] private string _slideAnimation;
	public Vector3 CheckPoint;
	[Space]

	[Header("Settings Controllers")]
	[SerializeField] private float _speed; 
	[SerializeField] private float _airVelocity; 
	[SerializeField] private float _gravity; 
	[SerializeField] private float _maxVelocityChange; 
	[SerializeField] private float _jumpHeight; 
	[SerializeField] private float _maxFallSpeed; 
	[SerializeField] private float _rotateSpeed; 

	private Rigidbody _rb;
	private Vector3 _pushDir;
	private Vector3 _moveDir;

	private float _distToGround;
	private bool _canMove = true; 
	private bool _isStuned = false;
	private bool _wasStuned = false; 
	private float _pushForce;
	
	private void Awake () 
	{
		_rb = GetComponent<Rigidbody>();
		_rb.freezeRotation = true;
		_rb.useGravity = false;

		CheckPoint = transform.position;
	}
	
	private void Start ()
	{
		_distToGround = GetComponent<Collider>().bounds.extents.y;
		Cursor.visible = true;
	}

	private void Update()
	{
		float h;
		float v;

		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");

		Vector3 v2 = v * _cam.transform.forward; 
		Vector3 h2 = h * _cam.transform.right; 
		_moveDir = (v2 + h2).normalized; 

		if(h == 0 && v == 0)
		{
			_character.SetTrigger(_idleAnimation);
		}
		else
		{
			_character.SetTrigger(_runAnimation);
		}
	}

	private void FixedUpdate() 
	{
		if (_canMove)
		{
			if (_moveDir.x != 0 || _moveDir.z != 0)
			{
				Vector3 targetDir = _moveDir; 
				targetDir.y = 0;
				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion tr = Quaternion.LookRotation(targetDir); 
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * _rotateSpeed); 
				transform.rotation = targetRotation;
			}

			if (IsGrounded())
			{
				Vector3 targetVelocity = _moveDir;
				targetVelocity *= _speed;

				Vector3 velocity = _rb.velocity;
				if (targetVelocity.magnitude < velocity.magnitude) 
				{
					targetVelocity = velocity;
					_rb.velocity /= 1.1f;
				}

				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
				velocityChange.y = 0;

				if (Mathf.Abs(_rb.velocity.magnitude) < _speed * 1.0f)
				{
					_rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}
						
				if (IsGrounded() && Input.GetButton("Jump"))
				{
					_rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);

					// Character.SetTrigger(_jumpAnimation);

					if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
					{
						_character.SetTrigger(_slideAnimation);
					}
				}
			}
			else
			{

				Vector3 targetVelocity = new Vector3(_moveDir.x * _airVelocity, _rb.velocity.y, _moveDir.z * _airVelocity);
				Vector3 velocity = _rb.velocity;
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
				_rb.AddForce(velocityChange, ForceMode.VelocityChange);

				if (velocity.y < - _maxFallSpeed)
				{
					_rb.velocity = new Vector3(velocity.x, - _maxFallSpeed, velocity.z);
				}
			}
		}
		else
		{
			_rb.velocity = _pushDir * _pushForce;
		}

		_rb.AddForce(new Vector3(0, -_gravity * GetComponent<Rigidbody>().mass, 0));
	}
	
	private bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
	}

	public void IniciarRun()
	{
		_character.SetTrigger(_idleAnimation);
		Cursor.visible = false;
	}

	public void Jump()
	{
		if (IsGrounded())
		{
			Vector3 velocity = _rb.velocity;
			_rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
		}
	}

	float CalculateJumpVerticalSpeed () 
	{
		return Mathf.Sqrt(2 * _jumpHeight * _gravity);
	}

	public void HitPlayer(Vector3 velocityF, float time)
	{
		_rb.velocity = velocityF;

		_pushForce = velocityF.magnitude;
		_pushDir = Vector3.Normalize(velocityF);
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	public void LoadCheckPoint()
	{
		transform.position = CheckPoint;
	}

	private IEnumerator Decrease(float value, float duration)
	{
		if (_isStuned)
			_wasStuned = true;
		_isStuned = true;
		_canMove = false;

		float delta = 0;
		delta = value / duration;

		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;

			_pushForce = _pushForce - Time.deltaTime * delta;
			_pushForce = _pushForce < 0 ? 0 : _pushForce;

			_rb.AddForce(new Vector3(0, -_gravity * GetComponent<Rigidbody>().mass, 0));
		}

		if (_wasStuned)
		{
			_wasStuned = false;
		}
		else
		{
			_isStuned = false;
			_canMove = true;
		}
	}
}
