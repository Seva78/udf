using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    int _counter;
    public Button soundButton;
    public Sprite soundOff;
    public Sprite soundOn;
    public GameObject projectile;
    public GameObject balrog;

    void Start()
    {
        if (PlayerPrefs.GetInt("sndOff", 0) == 1)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
    public void SwitchButton() {
        _counter++;
        if (_counter % 2 == 1)
        {
            soundButton.GetComponent<Image>().sprite = soundOff;
            projectile.GetComponent<AudioSource>().mute = true;
            balrog.GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetInt("sndOff", 1);
        }
        else {
            soundButton.GetComponent<Image>().sprite = soundOn;
            projectile.GetComponent<AudioSource>().mute = false;
            balrog.GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetInt("sndOff", 0);
        }
    }
}
