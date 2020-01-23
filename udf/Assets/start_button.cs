using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class start_button : MonoBehaviour
{
    public GUI button;
    void Start()
    {
        if (PlayerPrefs.GetInt("Restart", 0) == 1) {
            PlayerPrefs.SetInt("Restart", 0);
            GetComponent<Button>().onClick.Invoke();
        }
    }
}