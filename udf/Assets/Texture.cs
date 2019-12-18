using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] public GameObject TileSides;
    [SerializeField] public GameObject TileBackground;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    public float backgroundLagCoeff; //отставание текстуры бэкграунда от текстуры стенок шахты
    private float speed;
    private float _ySidesTile;
    private float _yBackgroundTile;
    private Dictionary<int, GameObject> _tileSidesDict;
    private Dictionary<int, GameObject> _tileBackgroundDict;
    private int _tileSidesDictNumber;
    private int _tileBackgroundDictNumber;
    private int _tileSidesToDelete;
    private int _tileBackgroundToDelete;
    void Start()
    {
        _tileSidesDict = new Dictionary<int, GameObject>();
        _tileBackgroundDict = new Dictionary<int, GameObject>();
        _ySidesTile = MainCamera.pixelHeight + 256;
        _yBackgroundTile = MainCamera.pixelHeight + 256;
        GenerateSidesTile(256, _ySidesTile);
        GenerateBackgroundTile(256, _yBackgroundTile);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _ySidesTile += speed;
        _yBackgroundTile += speed - speed / backgroundLagCoeff;
        _ySidesTile -= TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;
        _yBackgroundTile -= TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y;
        if (_ySidesTile > -TileSides.GetComponent<SpriteRenderer>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateSidesTile(256, _ySidesTile);
        else _ySidesTile += TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;
        if (_yBackgroundTile > -TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateBackgroundTile(256, _yBackgroundTile);
        else _yBackgroundTile += TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y;
        foreach (KeyValuePair<int, GameObject> tile in _tileSidesDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileSidesToDelete = tile.Key + 1;
            }
        }
        if (_tileSidesToDelete != 0)
        {
            _tileSidesDict.Remove(_tileSidesToDelete - 1);
            _tileSidesToDelete = 0;
        }
        foreach (KeyValuePair<int, GameObject> tile in _tileBackgroundDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileBackgroundToDelete = tile.Key + 1;
            }
        }
        if (_tileBackgroundToDelete != 0)
        {
            _tileBackgroundDict.Remove(_tileBackgroundToDelete - 1);
            _tileBackgroundToDelete = 0;
        }
    }
    void GenerateSidesTile(int _xTile, float _ySidesTile)
    {
        var TileSidesI = Instantiate(TileSides, new Vector3(_xTile, _ySidesTile, 0), Quaternion.identity);
        TileSidesI.transform.parent = transform;
        TileSidesI.name = "SideTexture" + _tileSidesDictNumber;
        //print(TileSidesI.name);
        _tileSidesDict.Add(_tileSidesDictNumber, TileSidesI);
        _tileSidesDictNumber++;
    }
    void GenerateBackgroundTile(int _xTile, float _yBackgroundTile)
    {
        var TileBackgroundI = Instantiate(TileBackground, new Vector3(_xTile, _yBackgroundTile, 0), Quaternion.identity);
        TileBackgroundI.transform.parent = transform;
        TileBackgroundI.name = "BackgroundTexture" + _tileBackgroundDictNumber;
        //print(TileBackgroundI.name);
        _tileBackgroundDict.Add(_tileBackgroundDictNumber, TileBackgroundI);
        _tileBackgroundDictNumber++;
    }
}