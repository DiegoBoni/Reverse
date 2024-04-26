using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillController : MonoBehaviour
{
    [SerializeField] private int _points;

    private AudioSource _audioSource;

    private void Start()
    {
        GameManager.OnGameStart += OnInstanciatePillHandler;
        _audioSource = transform.parent.GetComponent<AudioSource>();
    }

    private void OnInstanciatePillHandler()
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UserStats.CurrentScore += _points;
            ScreensManager.Instance.UpdateScore(UserStats.CurrentScore);
            _audioSource.Play();
            gameObject.SetActive(false);
        }
    }
}