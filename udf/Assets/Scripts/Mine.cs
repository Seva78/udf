using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

[SuppressMessage("ReSharper", "CommentTypo")]
public class Mine : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject barlog;
    public GameObject vertebraSource;
    public int centralPointPosLimit;
    public int sidePointPosLimit;
    // Параметр, регулирующий минимальную степень сближения крайней точки с одной стороны
    // и крайней точки с другой стороны в предыдущем позвонке (чтобы не было очень крутых изломов лабиринта).
    public int sidePointsMinDist; 
    public float speed;
    public int centralPointX;
    public float centralPointY;
    public int sidePointLeftX;
    public float sidePointLeftY;
    public int sidePointRightX;
    public float sidePointRightY;
    public List<GameObject> mineList;
    //пока не включен, фоновую текстуру не генерим
    public int textureSpawnTrigger; 
    public GameObject depthUi;
    private int _centralPointPosLimitL;
    private int _centralPointPosLimitR;
    private float _prevSidePointLeftY;
    private float _prevSidePointRightY;
    private int _vertebraToDelete;
    void Start()
    {
        mineList = new List<GameObject>();
        _centralPointPosLimitL = mainCamera.pixelWidth / 2 - centralPointPosLimit;
        _centralPointPosLimitR = mainCamera.pixelWidth / 2 + centralPointPosLimit;
        centralPointX = _centralPointPosLimitL + (_centralPointPosLimitR - _centralPointPosLimitL) * Random.Range(0,200)/200;
        centralPointY = mainCamera.pixelHeight + 50 + 50 * Random.Range(0, 50)/50;
        _prevSidePointLeftY = mainCamera.pixelHeight * 2;
        _prevSidePointRightY = _prevSidePointLeftY;
        GenerateVertebra(centralPointX, centralPointY);
    }
    void Update()
    {
        speed = barlog.GetComponent<Barlog>().VertSpeed*Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed);
        centralPointY += speed;
        _prevSidePointLeftY += speed;
        _prevSidePointRightY += speed;
        var centralPointYOffset = Random.Range(50, 100);
        centralPointY -= centralPointYOffset;
        if (centralPointY > -1200) GenerateVertebra(centralPointX, centralPointY);
        else {
            textureSpawnTrigger = 1;
            centralPointY += centralPointYOffset;
        }
        foreach (GameObject vertebra in mineList)
        {
            if (vertebra.transform.position.y > mainCamera.pixelHeight + 200)
            {
                _vertebraToDelete = mineList.IndexOf(vertebra) + 1;
            }
        }
        if (_vertebraToDelete != 0) {
            mineList.RemoveAt(_vertebraToDelete - 1);
            _vertebraToDelete = 0;
        }
        if (barlog.GetComponent<Barlog>().startButtonPressed == 1)
        {
            depthUi.GetComponent<TextMeshProUGUI>().text = Mathf.Round(transform.position.y / 20) + " m";
        }
    }
    void GenerateVertebra(int x, float y)
    {
        x += Random.Range(-100, 100);
        if (x < _centralPointPosLimitL) x = _centralPointPosLimitL;
        if (x > _centralPointPosLimitR) x = _centralPointPosLimitR;
        sidePointLeftX = x - 50 - Random.Range(0, 150);
        sidePointLeftX = Mathf.Clamp(sidePointLeftX, mainCamera.pixelWidth / 2 - sidePointPosLimit, 255);
        if (mineList.Count > 0) 
            while (Mathf.Abs(sidePointLeftX - mineList[mineList.Count - 1].GetComponent<Vertebra>().RightX) < sidePointsMinDist) 
                //при необходимости двигаем x-координату конца ребра от центра, чтобы избежать экстремальных изломов шахты
                sidePointLeftX -= 10; 
        sidePointLeftY = y + Random.Range(-25, 25);
        if (_prevSidePointLeftY == mainCamera.pixelHeight * 2) 
            // выставляем первое реальное значение y-координаты конца левого ребра для запоминания
            _prevSidePointLeftY = sidePointLeftY; 
        if (sidePointLeftY > _prevSidePointLeftY) 
            // проверяем, не пересекаются ли рёбра, и если да, исправляем
            sidePointLeftY = _prevSidePointLeftY;
        // запоминаем у-координату конца ребра, чтобы при следующей генерации позвонка проверить, не пересекутся ли рёбра
        _prevSidePointLeftY = sidePointLeftY; 
        sidePointRightX = x + 50 + Random.Range(0, 150);
        sidePointRightX = Mathf.Clamp(sidePointRightX, 256, mainCamera.pixelWidth / 2 + sidePointPosLimit);
        if (mineList.Count > 0) 
            while (Mathf.Abs(sidePointRightX - mineList[mineList.Count - 1].GetComponent<Vertebra>().LeftX) < sidePointsMinDist) 
                //при необходимости двигаем x-координату конца ребра от центра, чтобы избежать экстремальных изломов шахты
                sidePointRightX += 10; 
        sidePointRightY = y + Random.Range(-25, 25);
        if (_prevSidePointRightY == mainCamera.pixelHeight * 2) 
            // выставляем первое реальное значение y-координаты конца правого ребра для запоминания
            _prevSidePointRightY = sidePointRightY; 
        if (sidePointRightY > _prevSidePointRightY) 
            // проверяем, не пересекаются ли рёбра, и если да, исправляем
            sidePointRightY = _prevSidePointRightY;
        // запоминаем у-координату конца ребра, чтобы при следующей генерации позвонка проверить, не пересекутся ли рёбра
        _prevSidePointRightY = sidePointRightY; 
        var vertebra = Instantiate(vertebraSource, new Vector3(x, y, 0), Quaternion.identity);
        vertebra.name = "vertebra" + mineList.Count.ToString();
        mineList.Add(vertebra);
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