using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    int coins = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Coin Collectible"))
        {
            Destroy(other.gameObject);
            coins++;
            Debug.Log(coins);
        }
    }
}
