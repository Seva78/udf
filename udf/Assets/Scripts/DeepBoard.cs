using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class DeepBoard : MonoBehaviour
{
    public GameObject barlog;
    public GameObject gollum;
    public GameObject wormtongue;
    public GameObject beorn;
    public GameObject bertTheTroll;
    public GameObject smaug;
    public GameObject saruman;
    public GameObject nazgulKing;
    public GameObject sauron;
    public GameObject controller;
    private Dictionary<int, GameObject> _deepBoardOrder;
    private Vector3 _tablePosition;

    void Start()
    {
        _tablePosition = new Vector3(barlog.transform.localPosition.x, barlog.transform.localPosition.y);
        _deepBoardOrder = new Dictionary<int, GameObject>();
        _deepBoardOrder.Add(9, barlog);
        _deepBoardOrder.Add(8, barlog);
        string barlogDepthString = Mathf.Round(controller.transform.position.y / 20).ToString();
        _deepBoardOrder[8].transform.Find("depth").GetComponent<TextMeshProUGUI>().text = barlogDepthString;
        _deepBoardOrder.Add(7, gollum);
        _deepBoardOrder.Add(6, wormtongue);
        _deepBoardOrder.Add(5, beorn);
        _deepBoardOrder.Add(4, bertTheTroll);
        _deepBoardOrder.Add(3, smaug);
        _deepBoardOrder.Add(2, saruman);
        _deepBoardOrder.Add(1, nazgulKing);
        _deepBoardOrder.Add(0, sauron);
        int y = 0;
        for (int i = _deepBoardOrder.Keys.Max() - 1; i >= _deepBoardOrder.Keys.Min(); i--) {
            if (_deepBoardOrder[i] == barlog && i > 0)
            {
                int barlogDepthInt =
                    Convert.ToInt16(_deepBoardOrder[i].transform.Find("depth").GetComponent<TextMeshProUGUI>().text);
                int concurrentDepth =
                    Convert.ToInt16(_deepBoardOrder[i-1].transform.Find("depth").GetComponent<TextMeshProUGUI>().text);
                if (barlogDepthInt > concurrentDepth) {
                    _deepBoardOrder[9] = _deepBoardOrder[i - 1];
                    _deepBoardOrder[i - 1] = _deepBoardOrder[i];
                    _deepBoardOrder[i] = _deepBoardOrder[9];
                }
            }
            _deepBoardOrder[i].transform.Find("position").GetComponent<TextMeshProUGUI>().text = Convert.ToString(i + 1);
            _deepBoardOrder[i].transform.localPosition = new Vector3(_tablePosition.x, _tablePosition.y - y);
            y += 25;
        }
    }
}
