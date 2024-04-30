using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CountdownScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private PlayerController _playerController;

    private void OnEnable()
    {
        StartCoroutine(StartCountdown());
        _playerController.Speed = 0;
    }

    private IEnumerator StartCountdown()
    {
        int countdownTime = 3;

        while (countdownTime > 0)
        {
            _countdownText.text = countdownTime.ToString();
            _countdownText.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);

            yield return new WaitForSeconds(1);

            countdownTime--;
        }

        _countdownText.text = "GO!";
        _countdownText.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);

        yield return new WaitForSeconds(1);

        _playerController.Speed = GameManager.Instance.PlayerSpeed;

        ScreensManager.Instance.OnStartRunning();
    }

    private void OnDisable()
    {
        StopCoroutine(StartCountdown());
    }
}