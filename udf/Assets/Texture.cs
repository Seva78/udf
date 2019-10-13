using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    [SerializeField] public GameObject TileSides;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public GameObject b;
    private float speed;
    private float _yTile;
    private Dictionary<int, GameObject> _tileSidesDict;
    private int _tileSidesDictNumber;
    private int _tileToDelete;
    void Start()
    {
        _tileSidesDict = new Dictionary<int, GameObject>();
        _yTile = MainCamera.pixelHeight + 256;
        GenerateTile(256, _yTile);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _yTile += speed;
        _yTile -= TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;
        if (_yTile > -TileSides.GetComponent<SpriteRenderer>().sprite.texture.height && GameObject.Find("Controller").GetComponent<Mine>().SidesSpawnTrigger == 1) GenerateTile(256, _yTile);
        else _yTile += TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;

        foreach (KeyValuePair<int, GameObject> tile in _tileSidesDict)
        {
            if (tile.Value.transform.position.y > MainCamera.pixelHeight + 200)
            {
                _tileToDelete = tile.Key + 1;
            }
        }
        if (_tileToDelete != 0)
        {
            _tileSidesDict.Remove(_tileToDelete - 1);
            _tileToDelete = 0;
        }
    }
    void GenerateTile(int _xTile, float _yTile)
    {
        var TileSidesI = Instantiate(TileSides, new Vector3(_xTile, _yTile, 100), Quaternion.identity);
        TileSidesI.transform.parent = transform;
        TileSidesI.name = "SideTexture" + _tileSidesDictNumber;
        _tileSidesDict.Add(_tileSidesDictNumber, TileSidesI);
        _tileSidesDictNumber++;
    }
}