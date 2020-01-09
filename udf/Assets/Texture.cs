using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] public GameObject TileSides;
    [SerializeField] public GameObject TileSidesTexture;
    [SerializeField] public GameObject TileBackground;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    public float backgroundLagCoeff; //отставание текстуры бэкграунда от маски
    public float sidesLagCoeff; //отставание текстуры стенок от маски
    private float speed;
    private float _ySidesTile;
    private float _ySidesTextureTile;
    private float _yBackgroundTile;
    private Dictionary<int, GameObject> _tileSidesDict;
    private Dictionary<int, GameObject> _tileSidesTextureDict;
    private Dictionary<int, GameObject> _tileBackgroundDict;
    private int _tileSidesDictNumber;
    private int _tileSidesTextureDictNumber;
    private int _tileBackgroundDictNumber;
    private int _tileSidesToDelete;
    private int _tileSidesTextureToDelete;
    private int _tileBackgroundToDelete;
    void Start()
    {
        _tileSidesDict = new Dictionary<int, GameObject>();
        _tileSidesTextureDict = new Dictionary<int, GameObject>();
        _tileBackgroundDict = new Dictionary<int, GameObject>();
        _ySidesTile = MainCamera.pixelHeight + 256;
        _ySidesTextureTile = MainCamera.pixelHeight + 256;
        _yBackgroundTile = MainCamera.pixelHeight + 256;
        GenerateSidesTile(256, _ySidesTile);
        GenerateSidesTextureTile(256, _ySidesTile);
        GenerateBackgroundTile(256, _yBackgroundTile);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _ySidesTile += speed;
        _ySidesTextureTile += speed + speed / sidesLagCoeff;
        _yBackgroundTile += speed - speed / backgroundLagCoeff;
        _ySidesTile -= TileSides.GetComponent<SpriteMask>().sprite.texture.height;
        _ySidesTextureTile -= TileSidesTexture.GetComponent<SpriteRenderer>().sprite.texture.height;
        _yBackgroundTile -= TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y;
        if (_ySidesTile > -TileSides.GetComponent<SpriteMask>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateSidesTile(256, _ySidesTile);
        else _ySidesTile += TileSides.GetComponent<SpriteMask>().sprite.texture.height;
        if (_ySidesTextureTile > -TileSidesTexture.GetComponent<SpriteRenderer>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateSidesTextureTile(256, _ySidesTextureTile);
        else _ySidesTextureTile += TileSidesTexture.GetComponent<SpriteRenderer>().sprite.texture.height;
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
        foreach (KeyValuePair<int, GameObject> tile in _tileSidesTextureDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileSidesTextureToDelete = tile.Key + 1;
            }
        }
        if (_tileSidesTextureToDelete != 0)
        {
            _tileSidesTextureDict.Remove(_tileSidesTextureToDelete - 1);
            _tileSidesTextureToDelete = 0;
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
        TileSidesI.name = "Mask" + _tileSidesDictNumber;
        _tileSidesDict.Add(_tileSidesDictNumber, TileSidesI);
        _tileSidesDictNumber++;
    }
    void GenerateSidesTextureTile(int _xTile, float _ySidesTextureTile)
    {
        var TileSidesTextureI = Instantiate(TileSidesTexture, new Vector3(_xTile, _ySidesTextureTile, 0), Quaternion.identity);
        TileSidesTextureI.transform.parent = transform;
        TileSidesTextureI.name = "SideTexture" + _tileSidesTextureDictNumber;
        _tileSidesTextureDict.Add(_tileSidesTextureDictNumber, TileSidesTextureI);
        _tileSidesTextureDictNumber++;
    }
    void GenerateBackgroundTile(int _xTile, float _yBackgroundTile)
    {
        var TileBackgroundI = Instantiate(TileBackground, new Vector3(_xTile, _yBackgroundTile, 0), Quaternion.identity);
        TileBackgroundI.transform.parent = transform;
        TileBackgroundI.name = "BackgroundTexture" + _tileBackgroundDictNumber;
        _tileBackgroundDict.Add(_tileBackgroundDictNumber, TileBackgroundI);
        _tileBackgroundDictNumber++;
    }
}