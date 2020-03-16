using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    int _counter;
    public Button musicButton;
    public Sprite musicOff;
    public Sprite musicOn;
    public GameObject controller;

    
    void Start()
    {
        if (PlayerPrefs.GetInt("musicOff", 0) == 1)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }

    public void SwitchButton()
    {
        _counter++;

        if (_counter % 2 == 1)
        {
            musicButton.GetComponent<Image>().sprite = musicOff;
            controller.GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetInt("musicOff", 1);
        }
        else {
            musicButton.GetComponent<Image>().sprite = musicOn;
            controller.GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetInt("musicOff", 0);
        }
    }
}
