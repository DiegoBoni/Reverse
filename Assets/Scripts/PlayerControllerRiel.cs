using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _eachMove;

    [SerializeField] private float _left;
    [SerializeField] private float _middle;
    [SerializeField] private float _right;
    [SerializeField] private List<float> _positions = new List<float>();

    [SerializeField] private int _currentPosition = 1;

    private void Start() 
    {
        SetPositions();
        Move(0);
    }

    private void Update()
    {   
        if(Input.GetKeyUp(KeyCode.A))
        {
            Move(-1);
        }

        if(Input.GetKeyUp(KeyCode.D))
        {
            Move(+1);
        }
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
}