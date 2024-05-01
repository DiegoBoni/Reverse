using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform _player;
    public float Speed;

    private void FixedUpdate()
    {
        if (_player != null)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            transform.position += direction * Speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            ScreensManager.Instance.OnShowDebriefHandler(false);
        }
    }
}