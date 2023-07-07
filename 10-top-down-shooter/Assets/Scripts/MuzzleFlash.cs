using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject flashHolder;

    public float flashTime;

    void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        flashHolder.SetActive(true);
        Invoke("Deactivate", flashTime);
    }

    void Deactivate()
    {
        flashHolder.SetActive(false);
    }
}
