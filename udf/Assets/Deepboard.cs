using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class Deepboard : MonoBehaviour
{
    public GameObject Barlog;
    public GameObject Gollum;
    public GameObject Wormtongue;
    public GameObject Beorn;
    public GameObject BertTheTroll;
    public GameObject Smaug;
    public GameObject Saruman;
    public GameObject NazgulKing;
    public GameObject Sauron;
    public GameObject Controller;
    private Dictionary<int, GameObject> deepboard_order;
    private Vector3 table_position;

    void Start()
    {
        table_position = new Vector3(Barlog.transform.localPosition.x, Barlog.transform.localPosition.y);
        deepboard_order = new Dictionary<int, GameObject>();
        deepboard_order.Add(9, Barlog);
        deepboard_order.Add(8, Barlog);
        deepboard_order[8].transform.Find("depth").GetComponent<TextMeshProUGUI>().text = Mathf.Round(Controller.transform.position.y / 20).ToString();
        deepboard_order.Add(7, Gollum);
        deepboard_order.Add(6, Wormtongue);
        deepboard_order.Add(5, Beorn);
        deepboard_order.Add(4, BertTheTroll);
        deepboard_order.Add(3, Smaug);
        deepboard_order.Add(2, Saruman);
        deepboard_order.Add(1, NazgulKing);
        deepboard_order.Add(0, Sauron);
        int y = 0;
        for (int i = deepboard_order.Keys.Max() - 1; i >= deepboard_order.Keys.Min(); i--) {
            if (deepboard_order[i] == Barlog && i > 0) {
                if (Convert.ToInt32(deepboard_order[i].transform.Find("depth").GetComponent<TextMeshProUGUI>().text) > Convert.ToInt32(deepboard_order[i - 1].transform.Find("depth").GetComponent<TextMeshProUGUI>().text)) {
                    deepboard_order[9] = deepboard_order[i - 1];
                    deepboard_order[i - 1] = deepboard_order[i];
                    deepboard_order[i] = deepboard_order[9];
                }
            }
            deepboard_order[i].transform.Find("position").GetComponent<TextMeshProUGUI>().text = Convert.ToString(i + 1);
            deepboard_order[i].transform.localPosition = new Vector3(table_position.x, table_position.y - y);
            y += 25;
        }

        for (int i = deepboard_order.Keys.Max() - 1; i >= deepboard_order.Keys.Min(); i--)
        {

        }
    }
}
