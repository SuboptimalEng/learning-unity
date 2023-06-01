using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Invoke("CompleteLevel", 2f);
        }
    }

    void CompleteLevel()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            buildIndex = 0;
        }
        SceneManager.LoadScene(buildIndex);
    }
}
