using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] public GameObject Tile;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    private float speed;
    private float _yTile;
    private Dictionary<int, GameObject> _tileDict;
    private int _tileDictNumber;
    private int _tileToDelete;
    void Start()
    {
        _tileDict = new Dictionary<int, GameObject>();
        _yTile = MainCamera.pixelHeight + 50;
        GenerateTile(256, _yTile);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _yTile += speed;
        _yTile -= Tile.GetComponent<SpriteRenderer>().sprite.texture.height / 1.9f;
        if (_yTile > -Tile.GetComponent<SpriteRenderer>().sprite.texture.height) GenerateTile(256, _yTile);
        else _yTile += Tile.GetComponent<SpriteRenderer>().sprite.texture.height / 1.9f;

        foreach (KeyValuePair<int, GameObject> tile in _tileDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileToDelete = tile.Key + 1;
            }
        }
        if (_tileToDelete != 0)
        {
            _tileDict.Remove(_tileToDelete - 1);
            _tileToDelete = 0;
        }
    }
    void GenerateTile(int _xTile, float _yTile)
    {
        var TileI = Instantiate(Tile, new Vector3(_xTile, _yTile, 100), Quaternion.identity);
        TileI.transform.parent = transform;
        _tileDict.Add(_tileDictNumber, TileI);
        _tileDictNumber++;
    }
}