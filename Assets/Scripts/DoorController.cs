using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float InitialPosX;
    [SerializeField] private float AnimationTime = 1;
    [SerializeField] private Transform DoorLeft;
    [SerializeField] private Transform DoorRight;

    [SerializeField] private bool Destroyable = true;
    [SerializeField] private float ExplotionForce = 1;
    [SerializeField] private float ExplotionRadius = 1;
    [SerializeField] private Transform ExplotionPivot;
    [SerializeField] private Transform DoorsPivot;
    [SerializeField] private GameObject DoorLeftPrefab;
    [SerializeField] private GameObject DoorRightPrefab;
    [SerializeField] private GameObject DoorLeftBroken;
    [SerializeField] private GameObject DoorRightBroken;
    [SerializeField] private bool DoorsBrokens = true;

    private void Start()
    {      
        GameManager.OnGameStart += SetDoorSettings;
    }

    public void SetDoorSettings() 
    {
        DoorLeft.gameObject.SetActive(true);
        DoorRight.gameObject.SetActive(true);
        DoorLeft.localPosition = new Vector3(InitialPosX, 0, 0);
        DoorRight.localPosition = new Vector3(-InitialPosX, 0, 0);

        if (DoorsBrokens) 
        {
            Destroy(DoorRightBroken);
            Destroy(DoorLeftBroken);
            DoorLeftBroken = Instantiate(DoorLeftPrefab, DoorsPivot);
            DoorRightBroken = Instantiate(DoorRightPrefab, DoorsPivot);
        }      

        GetComponent<Collider>().enabled = true;
        DoorsBrokens = false;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player") 
        {
            DoorLeft.DOLocalMove(Vector3.zero, AnimationTime);
            DoorRight.DOLocalMove(Vector3.zero, AnimationTime);
        }
        if (coll.gameObject.tag == "Enemy" && Destroyable) 
        {
            DoorLeft.gameObject.SetActive(false);
            DoorRight.gameObject.SetActive(false);
            DoorLeftBroken.SetActive(true);
            DoorRightBroken.SetActive(true);

            Rigidbody[] rbL = DoorLeftBroken.GetComponentsInChildren<Rigidbody>();
            Rigidbody[] rbR = DoorRightBroken.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbL)            
                rb.AddExplosionForce(ExplotionForce, ExplotionPivot.position, ExplotionRadius, 3.0F);
            foreach (Rigidbody rb in rbR)
                rb.AddExplosionForce(ExplotionForce, ExplotionPivot.position, ExplotionRadius, 3.0F);

            GetComponent<Collider>().enabled = false;
            DoorsBrokens = true;
        }
    }
}
