using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu = null;
    [SerializeField] private GameObject _gameplay = null;
    [SerializeField] private GameObject _gameOver = null;
    [SerializeField] private GameObject _pause = null;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _mainMenu.SetActive(true);
        _gameplay.SetActive(false);
        _gameOver.SetActive(false);
        _pause.SetActive(false);
    }

    void Update()
    {
        
    }
}
