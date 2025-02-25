using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    int cherriesCollected = 0;

    [SerializeField]
    Text cherriesText;

    [SerializeField]
    AudioSource collectSoundEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cherry"))
        {
            cherriesCollected++;
            cherriesText.text = "Cherries: " + cherriesCollected;
            collectSoundEffect.Play();
            Destroy(other.gameObject);
        }
    }
}
