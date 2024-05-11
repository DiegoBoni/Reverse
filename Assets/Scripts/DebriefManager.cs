using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DebriefManager : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private void Start()
    {
        if(_mainMenuButton != null)
        {
            _mainMenuButton.onClick.AddListener(ScreensManager.Instance.OnMainMenuHandler);
        }
    }

    public void SetDebrief(bool isWon)
    {
        _scoreText.text = $"Score: {UserStats.CurrentScore.ToString()}";

        if(isWon)
        {
            _descriptionText.text = "<color=green>Level Completed!</color>";
        }
        else
        {
            _descriptionText.text = "<color=red>Game Over</color>";
        }
    }
}
