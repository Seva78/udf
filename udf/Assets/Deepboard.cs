using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Deepboard : MonoBehaviour
{
    GameObject Barlog;
    public GameObject Gollum;
    public GameObject Wormtongue;
    public GameObject Beorn;
    public GameObject BertTheTroll;
    public GameObject Smaug;
    public GameObject Saruman;
    public GameObject NazgulKing;
    public GameObject Sauron;
    int GollumDe

    // Start is called before the first frame update
    void Start()
    {
        Barlog = transform.Find("Barlog").gameObject;

        Barlog.transform.Find("position").GetComponent<TextMeshProUGUI>().text = "!!!";
        Barlog.transform.position = new Vector3(Barlog.transform.position.x, Barlog.transform.position.y+20);

    }
}
