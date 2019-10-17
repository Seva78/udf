using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mine : MonoBehaviour
{
    [SerializeField] public GameObject CP;
    [SerializeField] public GameObject SP;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    [SerializeField] public GameObject Cube;
    public int CPPosLimit;
    public int SPPosLimit;
    public int SPConvLimit; // параметр, регулирующий минимальную степень сближения крайней точки с одной стороны и крайней точки с другой стороны в предыдущем позвонке (чтобы не было очень крутых изломов лабиринта)
    public float CollLength; //параметр для передачи в скрипт коллайдера - задаёт его длину
    private float speed;
    private int _CPPosLimitL;
    private int _CPPosLimitR;
    private int _SPPosLimitL;
    private int _SPPosLimitR;
    private int _xCP;
    private int _xSPL;
    private int _xSPR;
    private float _yCP;
    private float _ySPL;
    private float _last_ySPL;
    private float _ySPR;
    private float _last_ySPR;
    private int _yCPoffset;
    private Dictionary<int, GameObject> _vertebraDict;
    public Dictionary<int, Dictionary<int, GameObject>> _mineDict;
    private int _mineDictNumber;
    private int _vertebraToDelete;
    public int SidesSpawnTrigger; //пока не включен, фоновую текстуру не генерим

    void Start()
    {
        _vertebraDict = new Dictionary<int, GameObject>();
        _mineDict = new Dictionary<int, Dictionary<int, GameObject>>();
        _CPPosLimitL = MainCamera.pixelWidth / 2 - CPPosLimit;
        _CPPosLimitR = MainCamera.pixelWidth / 2 + CPPosLimit;
        _SPPosLimitL = MainCamera.pixelWidth / 2 - SPPosLimit;
        _SPPosLimitR = MainCamera.pixelWidth / 2 + SPPosLimit;
        _xCP = _CPPosLimitL + (_CPPosLimitR - _CPPosLimitL) * Random.Range(0,200)/200;
        _yCP = MainCamera.pixelHeight + 50 + 50 * Random.Range(0, 50)/50;
        _last_ySPL = MainCamera.pixelHeight * 2;
        _last_ySPR = _last_ySPL;
        GenerateVertebra(_xCP, _yCP);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        _yCP += speed;
        _last_ySPL += speed;
        _last_ySPR += speed;
        _yCPoffset = Random.Range(50, 100);
        _yCP -= _yCPoffset;
        if (_yCP > -1200) GenerateVertebra(_xCP, _yCP);
        else {
            SidesSpawnTrigger = 1;
            _yCP += _yCPoffset;
        }

        foreach (KeyValuePair<int, Dictionary<int, GameObject>>  vertebra in _mineDict)
        {
            if (vertebra.Value[0].transform.position.y > MainCamera.pixelHeight + 200)
            {
                _vertebraToDelete = vertebra.Key + 1;
            }
        }
        if (_vertebraToDelete != 0) {
            _mineDict.Remove(_vertebraToDelete - 1);
            _vertebraToDelete = 0;
        }
    }
    void GenerateVertebra(int _xCP, float _yCP)
    {
        _xCP += Random.Range(-100, 100);
        if (_xCP < _CPPosLimitL) _xCP = _CPPosLimitL;
        if (_xCP > _CPPosLimitR) _xCP = _CPPosLimitR;
        _xSPL = _xCP - 50 - Random.Range(0, 150);
        _xSPL = Mathf.Clamp(_xSPL, _SPPosLimitL, 255);
        if (_mineDictNumber > 0) while (Mathf.Abs(_xSPL - _mineDict[_mineDictNumber - 1][2].transform.position.x) < SPConvLimit) _xSPL -= 10; //при необходимости двигаем x-координату конца ребра от центра, чтобы избежать экстремальных изломов шахты
        _ySPL = _yCP + Random.Range(-25, 25);
        if (_last_ySPL == MainCamera.pixelHeight * 2) _last_ySPL = _ySPL; // выставляем первое реальное значение y-координаты конца ребра для запоминания
        if (_ySPL > _last_ySPL) _ySPL = _last_ySPL; // проверяем, не пересекаются ли рёбра, и если да, исправляем
        _last_ySPL = _ySPL; // запоминаем у-коррдинату конца ребра, чтобы при следующей генерации позвонка проверить, не пересекутся ли рёбра
        _xSPR = _xCP + 50 + Random.Range(0, 150);
        _xSPR = Mathf.Clamp(_xSPR, 256, _SPPosLimitR);
        if (_mineDictNumber > 0) while (Mathf.Abs(_xSPR - _mineDict[_mineDictNumber - 1][1].transform.position.x) < SPConvLimit) _xSPR += 10; //при необходимости двигаем x-координату конца ребра от центра, чтобы избежать экстремальных изломов шахты
        _ySPR = _yCP + Random.Range(-25, 25);
        if (_last_ySPR == MainCamera.pixelHeight * 2) _last_ySPR = _ySPR; // выставляем первое реальное значение y-координаты конца ребра для запоминания
        if (_ySPR > _last_ySPR) _ySPR = _last_ySPR; // проверяем, не пересекаются ли рёбра, и если да, исправляем
        _last_ySPR = _ySPR; // запоминаем у-коррдинату конца ребра, чтобы при следующей генерации позвонка проверить, не пересекутся ли рёбра
        var CPI = Instantiate(CP, new Vector3(_xCP, _yCP, 0), Quaternion.identity);
        CPI.transform.parent = transform;
        CPI.name = _mineDictNumber.ToString();
        _vertebraDict = new Dictionary<int, GameObject>();
        _vertebraDict.Add(0, CPI);
        var SPLI = Instantiate(SP, new Vector3(_xSPL, _ySPL, 0), Quaternion.identity);
        SPLI.transform.parent = CPI.transform;
        SPLI.name = _mineDictNumber.ToString() + "L";
        SPLI.tag = "SPL";
        if (_mineDictNumber > 0)
        {
            float CollLengthX = Mathf.Abs(_xSPL - _mineDict[_mineDictNumber - 1][1].transform.position.x);
            float CollLengthY = Mathf.Abs(_ySPL - _mineDict[_mineDictNumber - 1][1].transform.position.y);
            CollLength = Mathf.Sqrt(Mathf.Pow(CollLengthX, 2) + Mathf.Pow(CollLengthY, 2));
            Cube.GetComponent<Cube>().length = CollLength;
            Cube.GetComponent<Cube>().Point = SPLI;
            var MineCollL = Instantiate(Cube, new Vector3(_xSPL - (_xSPL - _mineDict[_mineDictNumber - 1][1].transform.position.x) / 2, _ySPL + CollLengthY / 2, 0), Quaternion.identity);
            MineCollL.transform.parent = SPLI.transform;
        }
        _vertebraDict.Add(1, SPLI);
        var SPRI = Instantiate(SP, new Vector3(_xSPR, _ySPR, 0), Quaternion.identity);
        SPRI.transform.parent = CPI.transform;
        SPRI.name = _mineDictNumber.ToString() + "R";
        SPRI.tag = "SPR";
        if (_mineDictNumber > 0)
        {
            float CollLengthX = Mathf.Abs(_xSPR - _mineDict[_mineDictNumber - 1][2].transform.position.x);
            float CollLengthY = Mathf.Abs(_ySPR - _mineDict[_mineDictNumber - 1][2].transform.position.y);
            CollLength = Mathf.Sqrt(Mathf.Pow(CollLengthX, 2) + Mathf.Pow(CollLengthY, 2));
            Cube.GetComponent<Cube>().length = CollLength;
            Cube.GetComponent<Cube>().Point = SPRI;
            var MineCollR = Instantiate(Cube, new Vector3(_xSPR - (_xSPR - _mineDict[_mineDictNumber - 1][2].transform.position.x) / 2, _ySPR + CollLengthY / 2, 0), Quaternion.identity);
            MineCollR.transform.parent = SPRI.transform;
        }
        _vertebraDict.Add(2, SPRI);
        _mineDict.Add(_mineDictNumber, _vertebraDict);
        _mineDictNumber++;
    }

    void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying) { 
            foreach (KeyValuePair<int, Dictionary<int, GameObject>> vertebra in _mineDict) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(vertebra.Value[1].transform.position,vertebra.Value[0].transform.position);
                Gizmos.DrawLine(vertebra.Value[2].transform.position, vertebra.Value[0].transform.position);
                if (_mineDict.ContainsKey(vertebra.Key - 1))
                {
                    Gizmos.DrawLine(_mineDict[vertebra.Key - 1][0].transform.position, vertebra.Value[0].transform.position);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(_mineDict[vertebra.Key - 1][2].transform.position, vertebra.Value[2].transform.position);
                    Gizmos.DrawLine(_mineDict[vertebra.Key - 1][1].transform.position, vertebra.Value[1].transform.position);
                }
            }
        }
    }
}