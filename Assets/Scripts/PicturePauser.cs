using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicturePauser : MonoBehaviour
{
    private void Start()
    {
        Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Resume();
            DelayHelper.DelayAction(this, Pause, 0.1f);
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
    }

    void Resume()
    {
        Time.timeScale = 1f;
    }
}
