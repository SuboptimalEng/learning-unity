using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    int coins = 0;

    [SerializeField]
    Text coinsText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Coin Collectible"))
        {
            Destroy(other.gameObject);
            coins++;
            coinsText.text = "Coins: " + coins.ToString();
        }
    }
}
