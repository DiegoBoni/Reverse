using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform _player;
    public float Speed;
    public bool chase = true;

    private void FixedUpdate()
    {
        if (_player != null && chase)
        {
            Vector3 playerPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
            Vector3 direction = (playerPos - transform.position).normalized;
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
        if (other.CompareTag("Door"))
            StartCoroutine(StopChase());
    }

    IEnumerator StopChase() 
    {
        chase = false;
        yield return new WaitForSeconds(1);
        chase = true;
    }
}