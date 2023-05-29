using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenLogicManagerScript : MonoBehaviour
{
    [ContextMenu("Sample Scene")]
    public void changeToSampleScene()
    {
        // Debug.Log("Hello world again");
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
