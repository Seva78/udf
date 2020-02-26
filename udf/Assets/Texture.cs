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
    private List<GameObject> _tileMaskList;
    private List<GameObject> _tileSidesList;
    private List<GameObject> _tileBackgroundList;
    private int _tileToDelete;

    void Start()
    {
        _tileMaskList = new List<GameObject>();
        _tileSidesList = new List<GameObject>();
        _tileBackgroundList = new List<GameObject>();
        _yMaskTile = MainCamera.pixelHeight + 256;
        _ySidesTile = MainCamera.pixelHeight + 256;
        _yBackgroundTile = MainCamera.pixelHeight + 256;
        _tileMaskList = GenerateTile(_yMaskTile, TileMask, "Mask", _tileMaskList);
        _tileSidesList = GenerateTile(_ySidesTile, TileSides, "Sides", _tileSidesList);
        _tileBackgroundList = GenerateTile(_yBackgroundTile, TileBackground, "Background", _tileBackgroundList);
    }
    void Update()
    {
        speed = b.GetComponent<B>().vertSpeed;
        _yMaskTile = TileMovement(_yMaskTile, TileMask, "Mask", _tileMaskList, 0);
        _ySidesTile = TileMovement(_ySidesTile, TileSides, "Sides", _tileSidesList, speed / sidesLagCoeff);
        _yBackgroundTile = TileMovement(_yBackgroundTile, TileBackground, "Background", _tileBackgroundList, - speed / backgroundLagCoeff);
    }

    float TileMovement(float _yTile, GameObject tileSource, string n, List<GameObject> _tileList, float speedCorrective)
    {
        GameObject tile = _tileList[_tileList.Count - 1];
        _yTile += speed + speedCorrective;
        if (tile.GetComponent<SpriteMask>())
        {
            _yTile -= tile.GetComponent<SpriteMask>().sprite.texture.height * tile.transform.localScale.y;
            if (_yTile > -tile.GetComponent<SpriteMask>().sprite.texture.height && GetComponent<Mine>().textureSpawnTrigger == 1)
            {
                GenerateTile(_yTile, tileSource, n, _tileList);
            }
            else
            {
                _yTile += tile.GetComponent<SpriteMask>().sprite.texture.height;
            }            
        }
        else
        {
            _yTile -= tile.GetComponent<SpriteRenderer>().sprite.texture.height * tile.transform.localScale.y;
            if (_yTile > -tile.GetComponent<SpriteRenderer>().sprite.texture.height * tileSource.transform.localScale.y && GetComponent<Mine>().textureSpawnTrigger == 1)
            {
                GenerateTile(_yTile, tileSource, n, _tileList);
            }
            else
            {
                _yTile += tile.GetComponent<SpriteRenderer>().sprite.texture.height * tileSource.transform.localScale.y;
            }
        }
        return _yTile;
    }
    List<GameObject> GenerateTile(float _yTile, GameObject tile, string n, List<GameObject> _tileList)
    {
        var TileI = Instantiate(tile, new Vector3(256, _yTile, 0), Quaternion.identity);
        TileI.transform.parent = transform;
        TileI.name = n;
        _tileList.Add(TileI);
        _tileListCut(_tileList);
        return _tileList;
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