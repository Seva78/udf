using UnityEngine;
using UnityEngine.UI;

public class snd_button : MonoBehaviour
{
    public int counter = 0;
    public Button snd;
    public Sprite snd_off;
    public Sprite snd_on;
    public void changePic() {
        counter++;
        if (counter % 2 == 1) {
            snd.GetComponent<Image>().sprite = snd_off;
        } else snd.GetComponent<Image>().sprite = snd_on;
    }
}
