using UnityEngine;
using UnityEngine.UI;

public class snd_button : MonoBehaviour
{
    public int counter = 0;
    public Button snd;
    public Sprite snd_off;
    public Sprite snd_on;
    public GameObject s;
    public GameObject b;

    void Start()
    {
        if (PlayerPrefs.GetInt("Snd_off", 0) == 1)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
    public void changePic() {
        counter++;
        if (counter % 2 == 1)
        {
            snd.GetComponent<Image>().sprite = snd_off;
            s.GetComponent<AudioSource>().mute = true;
            b.GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetInt("Snd_off", 1);
        }
        else {
            snd.GetComponent<Image>().sprite = snd_on;
            s.GetComponent<AudioSource>().mute = false;
            b.GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetInt("Snd_off", 0);
        }
    }
}
