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
        speed = b.GetComponent<Barlog>().vertSpeed;
        _yMaskTile = TileMovement(_yMaskTile, TileMask, "Mask", _tileMaskList, 0);
        _ySidesTile = TileMovement(_ySidesTile, TileSides, "Sides", _tileSidesList, speed / sidesLagCoefficient);
        _yBackgroundTile = TileMovement(_yBackgroundTile, TileBackground, "Background", _tileBackgroundList, - speed / backgroundLagCoefficient);
    }

    float TileMovement(float tileY, GameObject tileSource, string n, List<GameObject> tileList, float speedCorrective)
    {
        GameObject tile = tileList[tileList.Count - 1];
        tileY += speed + speedCorrective;
        if (tile.GetComponent<SpriteMask>())
        {
            tileY -= tile.GetComponent<SpriteMask>().sprite.texture.height * tile.transform.localScale.y;
            if (tileY > -tile.GetComponent<SpriteMask>().sprite.texture.height && GetComponent<Mine>().textureSpawnTrigger == 1)
            {
                GenerateTile(tileY, tileSource, n, tileList);
            }
            else
            {
                tileY += tile.GetComponent<SpriteMask>().sprite.texture.height;
            }            
        }
        else
        {
            tileY -= tile.GetComponent<SpriteRenderer>().sprite.texture.height * tile.transform.localScale.y;
            if (tileY > -tile.GetComponent<SpriteRenderer>().sprite.texture.height * tileSource.transform.localScale.y && GetComponent<Mine>().textureSpawnTrigger == 1)
            {
                GenerateTile(tileY, tileSource, n, tileList);
            }
            else
            {
                tileY += tile.GetComponent<SpriteRenderer>().sprite.texture.height * tileSource.transform.localScale.y;
            }
        }
        return tileY;
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