using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] public GameObject TileMask;
    [SerializeField] public GameObject TileSides;
    [SerializeField] public GameObject TileBackground;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    public float backgroundLagCoefficient; //отставание текстуры бэкграунда от маски
    public float sidesLagCoefficient; //отставание текстуры стенок от маски
    private float speed;
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
        _maskGenerateCheckPoint = MainCamera.pixelHeight + 256;
        _sidesGenerateCheckPoint = MainCamera.pixelHeight + 256;
        _backgroundGenerateCheckPoint = MainCamera.pixelHeight + 256;
        _tileMaskList = GenerateTile(_maskGenerateCheckPoint, TileMask, "Mask", _tileMaskList);
        _tileSidesList = GenerateTile(_sidesGenerateCheckPoint, TileSides, "Sides", _tileSidesList);
        _tileBackgroundList = GenerateTile(_backgroundGenerateCheckPoint, TileBackground, "Background", _tileBackgroundList);
    }
    void Update()
    {
        speed = b.GetComponent<Barlog>().vertSpeed;
        _maskGenerateCheckPoint = TileMovement(_maskGenerateCheckPoint, TileMask, "Mask", 
            _tileMaskList, 0);
        _sidesGenerateCheckPoint = TileMovement(_sidesGenerateCheckPoint, TileSides, "Sides", 
            _tileSidesList, speed / sidesLagCoefficient);
        _backgroundGenerateCheckPoint = TileMovement(_backgroundGenerateCheckPoint, TileBackground, "Background",
            _tileBackgroundList, - speed / backgroundLagCoefficient);
    }

    float TileMovement(float tileGenerateCheckPoint, GameObject tileSource, string n, List<GameObject> tileList, float speedCorrective)
    {
        GameObject tile = tileList[tileList.Count - 1];
        tileGenerateCheckPoint += speed + speedCorrective;
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
        var TileI = Instantiate(tile, new Vector3(256, tileGenerateCheckPoint, 0), Quaternion.identity);
        TileI.transform.parent = transform;
        TileI.name = n;
        tileList.Add(TileI);
        _tileListCut(tileList);
        return tileList;
    }
    void _tileListCut(List<GameObject> _tileList)
    {
        foreach (GameObject tile in _tileList)
        {
            if (tile.transform.position.y > MainCamera.pixelHeight + 400)
            {
                _tileToDelete = _tileList.IndexOf(tile) + 1;
            }
        }
        if (_tileToDelete != 0) {
            _tileList.RemoveAt(_tileToDelete - 1);
            _tileToDelete = 0;
        }
    }
}