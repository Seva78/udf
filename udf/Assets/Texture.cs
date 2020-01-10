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
    public float backgroundLagCoeff; //отставание текстуры бэкграунда от маски
    public float sidesLagCoeff; //отставание текстуры стенок от маски
    private float speed;
    private float _yMaskTile;
    private float _ySidesTile;
    private float _yBackgroundTile;
    private Dictionary<int, GameObject> _tileMaskDict;
    private Dictionary<int, GameObject> _tileSidesDict;
    private Dictionary<int, GameObject> _tileBackgroundDict;
    private int _tileMaskDictNumber;
    private int _tileSidesDictNumber;
    private int _tileBackgroundDictNumber;
    private int _tileMaskToDelete;
    private int _tileSidesToDelete;
    private int _tileBackgroundToDelete;
    void Start()
    {
        _tileMaskDict = new Dictionary<int, GameObject>();
        _tileSidesDict = new Dictionary<int, GameObject>();
        _tileBackgroundDict = new Dictionary<int, GameObject>();
        _yMaskTile = MainCamera.pixelHeight + 256;
        _ySidesTile = MainCamera.pixelHeight + 256;
        _yBackgroundTile = MainCamera.pixelHeight + 256;
        GenerateMaskTile(256, _yMaskTile);
        GenerateSidesTile(256, _ySidesTile);
        GenerateBackgroundTile(256, _yBackgroundTile);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _yMaskTile += speed;
        _ySidesTile += speed + speed / sidesLagCoeff;
        _yBackgroundTile += speed - speed / backgroundLagCoeff;
        _yMaskTile -= TileMask.GetComponent<SpriteMask>().sprite.texture.height;
        _ySidesTile -= TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;
        _yBackgroundTile -= TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y;
        if (_yMaskTile > -TileMask.GetComponent<SpriteMask>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateMaskTile(256, _yMaskTile);
        else _yMaskTile += TileMask.GetComponent<SpriteMask>().sprite.texture.height;
        if (_ySidesTile > -TileSides.GetComponent<SpriteRenderer>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateSidesTile(256, _ySidesTile);
        else _ySidesTile += TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;
        if (_yBackgroundTile > -TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateBackgroundTile(256, _yBackgroundTile);
        else _yBackgroundTile += TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y;
        foreach (KeyValuePair<int, GameObject> tile in _tileMaskDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileMaskToDelete = tile.Key + 1;
            }
        }
        if (_tileMaskToDelete != 0)
        {
            _tileMaskDict.Remove(_tileMaskToDelete - 1);
            _tileMaskToDelete = 0;
        }
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
    void GenerateMaskTile(int _xTile, float _yMaskTile)
    {
        var TileMaskI = Instantiate(TileMask, new Vector3(_xTile, _yMaskTile, 0), Quaternion.identity);
        TileMaskI.transform.parent = transform;
        TileMaskI.name = "Mask" + _tileMaskDictNumber;
        _tileMaskDict.Add(_tileMaskDictNumber, TileMaskI);
        _tileMaskDictNumber++;
    }
    void GenerateSidesTile(int _xTile, float _ySidesTile)
    {
        var TileSidesI = Instantiate(TileSides, new Vector3(_xTile, _ySidesTile, 0), Quaternion.identity);
        TileSidesI.transform.parent = transform;
        TileSidesI.name = "Side" + _tileSidesDictNumber;
        _tileSidesDict.Add(_tileSidesDictNumber, TileSidesI);
        _tileSidesDictNumber++;
    }
    void GenerateBackgroundTile(int _xTile, float _yBackgroundTile)
    {
        var TileBackgroundI = Instantiate(TileBackground, new Vector3(_xTile, _yBackgroundTile, 0), Quaternion.identity);
        TileBackgroundI.transform.parent = transform;
        TileBackgroundI.name = "Background" + _tileBackgroundDictNumber;
        _tileBackgroundDict.Add(_tileBackgroundDictNumber, TileBackgroundI);
        _tileBackgroundDictNumber++;
    }
}