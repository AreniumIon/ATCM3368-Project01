using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject text = null;

    public void Win()
    {
        text.SetActive(true);
        Time.timeScale = 0f;
    }
}
