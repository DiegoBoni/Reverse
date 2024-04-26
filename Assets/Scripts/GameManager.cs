using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static Action OnGameStart;
    public static Action OnGameEnd;

    public static GameManager Instance;

    [SerializeField] private GameObject _player;

    private Vector3[] _playerInitialPositions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
        SaveInitialPositions();
    }

    private void Init()
    {
        _player.SetActive(false);
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();

        _player.SetActive(true);
        ResetPlayerPositions();

        UserStats.CurrentScore = 0;
        ScreensManager.Instance.UpdateScore(UserStats.CurrentScore);
    }

    public void ResetPlayerPositions()
    {
        for (int i = 0; i < _player.transform.childCount; i++)
        {
            _player.transform.GetChild(i).position = _playerInitialPositions[i];
        }
    }

    private void SaveInitialPositions()
    {
        _playerInitialPositions = new Vector3[_player.transform.childCount];
        for (int i = 0; i < _player.transform.childCount; i++)
        {
            _playerInitialPositions[i] = _player.transform.GetChild(i).position;
        }
    }
}
