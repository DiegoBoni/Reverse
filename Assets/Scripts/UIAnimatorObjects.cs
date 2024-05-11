using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimatorObjects : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("For horizontal displacements, we use the X axis of the Vector, and for vertical displacements, we use the Y axis. Positive sign is used to animate downwards and to the left, while negative sign animates upwards and to the right.")]
    [SerializeField] private Vector3 _deltaPosition;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _easeType;
    [SerializeField] private bool _activateAnimationOutOnButtonPress = false;

    [Tooltip("The screen dimensions used to set up and test the original animation")]
    protected Vector2 _baseScreenDimensions;

    private Vector3 _defaultPosition;
    private Button[] _buttons;

    private void Awake()
    {
        _defaultPosition = transform.position;
        _baseScreenDimensions = new Vector2((float)UnityEngine.Screen.width, (float)UnityEngine.Screen.height);
    } 

    private void OnEnable() 
    {
        Animate(AnimationType.In);

        if(_activateAnimationOutOnButtonPress)
        {
            AddButtonCallbacks();
        }
    }

    private void AddButtonCallbacks()
    {
        _buttons = GetComponentsInChildren<Button>(true);
        foreach(Button button in _buttons)
        {
            button.onClick.AddListener(() => Animate(AnimationType.Out));
        }
    }

    public void Animate(AnimationType animationType)
    {
        transform.position = FixVector3ToScreenDimensions(_defaultPosition);
        float direction = 1f;

        if(animationType == AnimationType.In)
        {
            transform.position += FixVector3ToScreenDimensions(_deltaPosition);
            direction = -1f;
        }

        transform.DOMove(FixVector3ToScreenDimensions(_deltaPosition) * direction, _duration).SetRelative(true).SetEase(_easeType);
    }

    private void RemoveButtonCallbacks()
    {
        foreach(Button button in _buttons)
        {
            button.onClick.RemoveListener(() => Animate(AnimationType.Out));
        }
    }

    private void OnDisable()
    {
        transform.position = FixVector3ToScreenDimensions(_defaultPosition);
        
        if(_activateAnimationOutOnButtonPress)
        {
            RemoveButtonCallbacks();
        }
    }

    protected virtual Vector3 FixVector3ToScreenDimensions(Vector3 deltaPosition)
    { 
        Vector3 deltaPosHelper = new Vector3
        (
            (deltaPosition.x * (float)UnityEngine.Screen.width) / _baseScreenDimensions.x,
            (deltaPosition.y * (float)UnityEngine.Screen.height) / _baseScreenDimensions.y,
            deltaPosition.z
        );

        return deltaPosHelper;
    }
}

public enum AnimationType
{
    In,
    Out
}