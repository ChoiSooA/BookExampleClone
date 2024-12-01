using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            if (GameManager.instance.currHP < 120)
            {
                GameManager.instance.currHP += 10;
                gameObject.SetActive(false);
                GameManager.instance.DisplayHealth();
            }
        }
    }
}
