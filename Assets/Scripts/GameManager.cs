using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private GameObject _players;
    public float PlayerSpeed;
    public float EnemySpeed;
    public static GameManager Instance;

    public static Action OnGameStart;
    public static Action OnGameEnd;
    
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
    }

    private void Init()
    {
        _players.SetActive(false);
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();

        _players.SetActive(true);
        SetChildrenActive(_players, true);

        ResetPlayerPositions();

        UserStats.CurrentScore = 0;
        ScreensManager.Instance.UpdateScore(UserStats.CurrentScore);
    }

    private void SetChildrenActive(GameObject parent, bool active)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    public void OnEndGameHandler()
    {
        _players.GetComponentInChildren<EnemyController>().Speed = 0;
    }

    public void ResetPlayerPositions()
    {
        for (int i = 0; i < _players.transform.childCount; i++)
        {
            _players.transform.GetChild(i).position = _playerInitialPositions[i];
        }
    }

    private void SaveInitialPositions()
    {
        _playerInitialPositions = new Vector3[_players.transform.childCount];
        for (int i = 0; i < _players.transform.childCount; i++)
        {
            _playerInitialPositions[i] = _players.transform.GetChild(i).position;
        }
    }
}
