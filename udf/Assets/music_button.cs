using UnityEngine;
using UnityEngine.UI;

public class music_button : MonoBehaviour
{
    public int counter = 0;
    public Button music;
    public Sprite music_off;
    public Sprite music_on;
    public GameObject controller;

    void Start()
    {
        if (PlayerPrefs.GetInt("Music_off", 0) == 1)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }

    public void changePic()
    {
        counter++;
        if (counter % 2 == 1)
        {
            music.GetComponent<Image>().sprite = music_off;
            controller.GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetInt("Music_off", 1);
        }
        else {
            music.GetComponent<Image>().sprite = music_on;
            controller.GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetInt("Music_off", 0);
        }
    }
}
