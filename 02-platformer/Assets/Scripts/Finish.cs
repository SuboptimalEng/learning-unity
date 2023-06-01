using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    AudioSource finishSoundEffect;

    bool soundEffectPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        finishSoundEffect = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !soundEffectPlayed)
        {
            finishSoundEffect.Play();
            soundEffectPlayed = true;
        }
    }
}
