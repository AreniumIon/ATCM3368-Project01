using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject winText = null;
    [SerializeField] AudioClip winSound = null;

    public void Win()
    {
        AudioHelper.PlayClip2D(winSound, .5f);
        winText.SetActive(true);
        Time.timeScale = 0f;
    }
}
