using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    public GameObject mask;
    public GameObject sides;
    public GameObject background;
    public Camera mainCamera;
    public GameObject barlog;
    public float backgroundLagCoefficient; //отставание текстуры бэкграунда от маски
    public float sidesLagCoefficient; //отставание текстуры стенок от маски
    private float _speed;
    private float _maskGenerateCheckPoint;
    private float _sidesGenerateCheckPoint;
    private float _backgroundGenerateCheckPoint;
    private List<GameObject> _tileMaskList;
    private List<GameObject> _tileSidesList;
    private List<GameObject> _tileBackgroundList;
    private int _tileToDelete;

    void Start()
    {

        _tileMaskList = new List<GameObject>();
        _tileSidesList = new List<GameObject>();
        _tileBackgroundList = new List<GameObject>();
        _maskGenerateCheckPoint = mainCamera.pixelHeight + 256;
        _sidesGenerateCheckPoint = mainCamera.pixelHeight + 256;
        _backgroundGenerateCheckPoint = mainCamera.pixelHeight + 256;
        _tileMaskList = GenerateTile(_maskGenerateCheckPoint, mask, "Mask", _tileMaskList);
        _tileSidesList = GenerateTile(_sidesGenerateCheckPoint, sides, "Sides", _tileSidesList);
        _tileBackgroundList = GenerateTile(_backgroundGenerateCheckPoint, background, "Background", _tileBackgroundList);
    }
    void Update()
    {
        _speed = barlog.GetComponent<Barlog>().vertSpeed;
        _maskGenerateCheckPoint = TileMovement(_maskGenerateCheckPoint, mask, "Mask", 
            _tileMaskList, 0);
        _sidesGenerateCheckPoint = TileMovement(_sidesGenerateCheckPoint, sides, "Sides", 
            _tileSidesList, _speed / sidesLagCoefficient);
        _backgroundGenerateCheckPoint = TileMovement(_backgroundGenerateCheckPoint, background, "Background",
            _tileBackgroundList, - _speed / backgroundLagCoefficient);
    }

    float TileMovement(float tileGenerateCheckPoint, GameObject tileSource, string n, List<GameObject> tileList, float speedCorrective)
    {
        GameObject tile = tileList[tileList.Count - 1];
        tileGenerateCheckPoint += _speed + speedCorrective;
        if (tile.GetComponent<SpriteMask>())
        {
            var tileHeight = tile.GetComponent<SpriteMask>().sprite.texture.height;
            tileGenerateCheckPoint -= tileHeight * tile.transform.localScale.y;
            if (tileGenerateCheckPoint > -tileHeight && GetComponent<Mine>().textureSpawnTrigger == 1)
            {
                GenerateTile(tileGenerateCheckPoint, tileSource, n, tileList);
            }
            else
            {
                tileGenerateCheckPoint += tileHeight;
            }            
        }
        else
        {
            var tileHeight = tile.GetComponent<SpriteRenderer>().sprite.texture.height;
            tileGenerateCheckPoint -= tileHeight * tile.transform.localScale.y;
            if (tileGenerateCheckPoint > -tileHeight * tileSource.transform.localScale.y && GetComponent<Mine>().textureSpawnTrigger == 1)
            {
                GenerateTile(tileGenerateCheckPoint, tileSource, n, tileList);
            }
            else
            {
                tileGenerateCheckPoint += tileHeight * tileSource.transform.localScale.y;
            }
        }
        return tileGenerateCheckPoint;
    }
    List<GameObject> GenerateTile(float tileGenerateCheckPoint, GameObject tile, string n, List<GameObject> tileList)
    {
        var tileObj = Instantiate(tile, new Vector3(256, tileGenerateCheckPoint, 0), Quaternion.identity);
        tileObj.transform.parent = transform;
        tileObj.name = n;
        tileList.Add(tileObj);
        _tileListCut(tileList);
        return tileList;
    }
    void _tileListCut(List<GameObject> tileList)
    {
        foreach (GameObject tile in tileList)
        {
            if (tile.transform.position.y > mainCamera.pixelHeight + 400)
            {
                _tileToDelete = tileList.IndexOf(tile) + 1;
            }
        }
        if (_tileToDelete != 0) {
            tileList.RemoveAt(_tileToDelete - 1);
            _tileToDelete = 0;
        }
    }
}