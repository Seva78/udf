using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] public GameObject TileSides;
    [SerializeField] public GameObject TileMine;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    private float speed;
    private float _yTile;
    private Dictionary<int, GameObject> _tileMineDict;
    //private Dictionary<int, GameObject> _tileSidesDict;
    private int _tileMineDictNumber;
    //private int _tileSidesDictNumber;
    private int _tileToDelete;
    void Start()
    {
        _tileMineDict = new Dictionary<int, GameObject>();
        //_tileSidesDict = new Dictionary<int, GameObject>();
        _yTile = MainCamera.pixelHeight + 50;
        GenerateTile(256, _yTile);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _yTile += speed;
        _yTile -= TileMine.GetComponent<SpriteRenderer>().sprite.texture.height;
        if (_yTile > -TileMine.GetComponent<SpriteRenderer>().sprite.texture.height) GenerateTile(256, _yTile);
        else _yTile += TileMine.GetComponent<SpriteRenderer>().sprite.texture.height;

        foreach (KeyValuePair<int, GameObject> tile in _tileMineDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileToDelete = tile.Key + 1;
            }
        }
        if (_tileToDelete != 0)
        {
            _tileMineDict.Remove(_tileToDelete - 1);
            _tileToDelete = 0;
        }
    

        //foreach (KeyValuePair<int, GameObject> tile in _tileSidesDict)
        //{
        //    if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
        //    {
        //        _tileToDelete = tile.Key + 1;
        //    }
        //}
        //if (_tileToDelete != 0)
        //{
        //    _tileSidesDict.Remove(_tileToDelete - 1);
        //    _tileToDelete = 0;
        //}
    }
    void GenerateTile(int _xTile, float _yTile)
    {
        var TileMineI = Instantiate(TileMine, new Vector3(_xTile, _yTile, 5), Quaternion.identity);
        //var TileSidesI = Instantiate(TileSides, new Vector3(_xTile, _yTile, 100), Quaternion.identity);
        TileMineI.transform.parent = transform;
        TileMineI.name = "MineBG" + _tileMineDictNumber;
        //TileSidesI.transform.parent = transform;
        _tileMineDict.Add(_tileMineDictNumber, TileMineI);
        _tileMineDictNumber++;
        //_tileSidesDict.Add(_tileSidesDictNumber, TileSidesI);
        //_tileSidesDictNumber++;
    }
}