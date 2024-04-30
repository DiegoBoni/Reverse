using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Animations")]
	[SerializeField] private Animator _character;
    [SerializeField] private string _idleAnimation;
	[SerializeField] private string _runAnimation;
	[SerializeField] private string _jumpAnimation;

    [Header("Player Movement")]
    [SerializeField] private float _eachMove;
    [SerializeField] private float _jumpForce;
    public float Speed;

    private float _left;
    private float _middle;
    private float _right;
    private List<float> _positions = new List<float>();
    private int _currentPosition = 1;
    private Rigidbody _rb; 
    private bool _isGrounded = false;

    private void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        SetPositions();
        Move(0);
    }

    private void Update()
    {   
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Move(+1);
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            Move(-1);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Run(Speed);
    }

    private void Run(float speed)
    {
        Vector3 movement = new Vector3(0.0f, 0.0f, speed);
        _rb.MovePosition(transform.position + movement * Time.deltaTime);

        _character.SetTrigger(speed > 0 ? _runAnimation : _idleAnimation);
    }

    private void SetPositions()
    {
        _middle = transform.position.x;
        _left = transform.position.x - _eachMove;
        _right = transform.position.x + _eachMove;

        CreateList();
    }

    private void CreateList()
    {
        _positions.Add(_left);
        _positions.Add(_middle);
        _positions.Add(_right);
    }

    private void Move(int deltaPosition)
    {
        _currentPosition += deltaPosition;
        _currentPosition = Mathf.Clamp(_currentPosition, 0, _positions.Count - 1);
        transform.position = new Vector3(_positions[_currentPosition], transform.position.y, transform.position.z);
    }

    private void Jump()
    {
        if(_isGrounded)
        {
            Vector3 velocity = _rb.velocity;
            _rb.velocity = new Vector3(velocity.x, _jumpForce, velocity.z);

            _character.SetTrigger(_jumpAnimation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isGrounded = false;
        }
    }
}