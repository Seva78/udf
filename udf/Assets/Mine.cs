using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Mine : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject b;
    public GameObject vertebraSource;
    public int centralPointPosLimit;
    public int sidePointPosLimit;
    public int sidePointsMinDist; 
    // параметр, регулирующий минимальную степень сближения крайней точки с одной стороны и крайней точки с другой стороны в предыдущем позвонке (чтобы не было очень крутых изломов лабиринта)
    public float speed;
    private int _centralPointPosLimitL;
    private int _centralPointPosLimitR;
    private int _sidePointsPosLimitL;
    [FormerlySerializedAs("_xCP")] public int centralPointX;
    [FormerlySerializedAs("_xSPL")] public int sidePointLeftX;
    [FormerlySerializedAs("_xSPR")] public int sidePointRightX;
    [FormerlySerializedAs("_yCP")] public float centralPointY;
    [FormerlySerializedAs("_ySPL")] public float sidePointLeftY;
    private float _last_ySPL;
    public float _ySPR;
    private float _last_ySPR;
    private int _yCPoffset;
    public List<GameObject> _mineList;
    private int _vertebraToDeleteL;
    public int TextureSpawnTrigger; //пока не включен, фоновую текстуру не генерим
    public GameObject Depth_UI;

    void Start()
    {
        _mineList = new List<GameObject>();
        _centralPointPosLimitL = mainCamera.pixelWidth / 2 - centralPointPosLimit;
        _centralPointPosLimitR = mainCamera.pixelWidth / 2 + centralPointPosLimit;
        centralPointX = _centralPointPosLimitL + (_centralPointPosLimitR - _centralPointPosLimitL) * Random.Range(0,200)/200;
        centralPointY = mainCamera.pixelHeight + 50 + 50 * Random.Range(0, 50)/50;
        _last_ySPL = mainCamera.pixelHeight * 2;
        _last_ySPR = _last_ySPL;
        GenerateVertebra(centralPointX, centralPointY);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        centralPointY += speed;
        _last_ySPL += speed;
        _last_ySPR += speed;
        _yCPoffset = Random.Range(50, 100);
        centralPointY -= _yCPoffset;
        if (centralPointY > -1200) GenerateVertebra(centralPointX, centralPointY);
        else {
            TextureSpawnTrigger = 1;
            centralPointY += _yCPoffset;
        }

        foreach (GameObject vertebra in _mineList)
        {
            if (vertebra.transform.position.y > mainCamera.pixelHeight + 200)
            {
                _vertebraToDeleteL = _mineList.IndexOf(vertebra) + 1;
            }
        }
        if (_vertebraToDeleteL != 0) {
            _mineList.RemoveAt(_vertebraToDeleteL - 1);
            _vertebraToDeleteL = 0;
        }
        if (b.GetComponent<B>().startButtonPressed == 1) Depth_UI.GetComponent<TextMeshProUGUI>().text = Mathf.Round(transform.position.y / 20).ToString() + " m";
    }
    void GenerateVertebra(int _xCP, float _yCP)
    {
        _xCP += Random.Range(-100, 100);
        if (_xCP < _centralPointPosLimitL) _xCP = _centralPointPosLimitL;
        if (_xCP > _centralPointPosLimitR) _xCP = _centralPointPosLimitR;
        sidePointLeftX = _xCP - 50 - Random.Range(0, 150);
        sidePointLeftX = Mathf.Clamp(sidePointLeftX, mainCamera.pixelWidth / 2 - sidePointPosLimit, 255);
        if (_mineList.Count > 0) while (Mathf.Abs(sidePointLeftX - _mineList[_mineList.Count - 1].GetComponent<Vertebra>().rightPoint.transform.position.x) < sidePointsMinDist) sidePointLeftX -= 10; //при необходимости двигаем x-координату конца ребра от центра, чтобы избежать экстремальных изломов шахты
        sidePointLeftY = _yCP + Random.Range(-25, 25);
        if (_last_ySPL == mainCamera.pixelHeight * 2) _last_ySPL = sidePointLeftY; // выставляем первое реальное значение y-координаты конца ребра для запоминания
        if (sidePointLeftY > _last_ySPL) sidePointLeftY = _last_ySPL; // проверяем, не пересекаются ли рёбра, и если да, исправляем
        _last_ySPL = sidePointLeftY; // запоминаем у-коррдинату конца ребра, чтобы при следующей генерации позвонка проверить, не пересекутся ли рёбра
        sidePointRightX = _xCP + 50 + Random.Range(0, 150);
        sidePointRightX = Mathf.Clamp(sidePointRightX, 256, mainCamera.pixelWidth / 2 + sidePointPosLimit);
        if (_mineList.Count > 0) while (Mathf.Abs(sidePointRightX - _mineList[_mineList.Count - 1].GetComponent<Vertebra>().leftPoint.transform.position.x) < sidePointsMinDist) sidePointRightX += 10; //при необходимости двигаем x-координату конца ребра от центра, чтобы избежать экстремальных изломов шахты
        _ySPR = _yCP + Random.Range(-25, 25);
        if (_last_ySPR == mainCamera.pixelHeight * 2) _last_ySPR = _ySPR; // выставляем первое реальное значение y-координаты конца ребра для запоминания
        if (_ySPR > _last_ySPR) _ySPR = _last_ySPR; // проверяем, не пересекаются ли рёбра, и если да, исправляем
        _last_ySPR = _ySPR; // запоминаем у-коррдинату конца ребра, чтобы при следующей генерации позвонка проверить, не пересекутся ли рёбра
        var vertebra = Instantiate(vertebraSource, new Vector3(_xCP, _yCP, 0), Quaternion.identity);
        vertebra.name = "vertebra" + _mineList.Count.ToString();
        _mineList.Add(vertebra);
    }

    //void OnDrawGizmos()
    //{
    //    if (EditorApplication.isPlaying)
    //    {
    //        foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDict)
    //        {
    //            Gizmos.color = Color.blue;
    //            Gizmos.DrawLine(vertebra.Value[1].transform.position, vertebra.Value[0].transform.position);
    //            Gizmos.DrawLine(vertebra.Value[2].transform.position, vertebra.Value[0].transform.position);
    //            if (_mineDict.ContainsKey(vertebra.Key - 1))
    //            {
    //                Gizmos.DrawLine(_mineDict[vertebra.Key - 1][0].transform.position, vertebra.Value[0].transform.position);
    //                Gizmos.color = Color.red;
    //                Gizmos.DrawLine(_mineDict[vertebra.Key - 1][2].transform.position, vertebra.Value[2].transform.position);
    //                Gizmos.DrawLine(_mineDict[vertebra.Key - 1][1].transform.position, vertebra.Value[1].transform.position);
    //            }
    //        }
    //    }
    //}
}