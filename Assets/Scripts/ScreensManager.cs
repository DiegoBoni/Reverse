using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScreensManager : MonoBehaviour
{
    public static ScreensManager Instance;

    [Header("Screens")]
    [SerializeField] private GameObject _mainMenu = null;
    [SerializeField] private GameObject _countdown = null;
    [SerializeField] private GameObject _gameplay = null;
    [SerializeField] private GameObject _gameOver = null;
    [SerializeField] private GameObject _pause = null;
    [Space]

    [Header("Buttons")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _playButton;
    [Space]

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [Space]

    [Header("Musics")]
    [SerializeField] private AudioClip _musicMainMenu;
    [SerializeField] private AudioClip _musicGamePlay;
    // [SerializeField] private AudioClip _musicGameOver;
    [Space]

    [SerializeField] private AudioSource _musicSource;

    private bool _isPause = false;

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
        InitCallbacks();
    }

    private void Init()
    {
        OnMainMenuHandler();
    }

    private void InitCallbacks()
    {
        if(_mainMenuButton != null)
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuHandler);
        }

        if(_resumeButton != null)
        {
            _resumeButton.onClick.AddListener(OnPauseHandler);
        }

        if(_playButton != null)
        {
            _playButton.onClick.AddListener(OnStartGameHandler);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseHandler();
        }
    }

    private void OnStartGameHandler()
    {
        _mainMenu.SetActive(false);
        _countdown.SetActive(true);
        _gameOver.SetActive(false);
        _pause.SetActive(false);

        GameManager.Instance.StartGame();
        UpdateMusic(_musicGamePlay);
    }

    public void OnStartRunning()
    {
        _gameplay.SetActive(true);
        _countdown.SetActive(false);
    }

    private void OnPauseHandler()
    {
        if(!_mainMenu.activeSelf)
        {
            _isPause = !_isPause;
            Time.timeScale = _isPause ? 1 : 0;

            _mainMenu.SetActive(false);
            _countdown.SetActive(false);
            _gameplay.SetActive(_isPause);
            _gameOver.SetActive(false);
            _pause.SetActive(!_isPause);
        }
    }

    private void OnMainMenuHandler()
    {
        UpdateMusic(_musicMainMenu);
        Time.timeScale = 1;

        _mainMenu.SetActive(true);
        _countdown.SetActive(false);
        _gameplay.SetActive(false);
        _gameOver.SetActive(false);
        _pause.SetActive(false);
    }

     public void UpdateScore(int currentScore)
     {
        _scoreText.text = currentScore.ToString();

        RectTransform textRectTransform = _scoreText.GetComponent<RectTransform>();

        float originalScale = textRectTransform.localScale.x;
        textRectTransform.DOScale(originalScale * 1.1f, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() => textRectTransform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBounce));
     }

     public void UpdateMusic(AudioClip music)
     {
        _musicSource.clip = music;
        _musicSource.Play();
     }
}
