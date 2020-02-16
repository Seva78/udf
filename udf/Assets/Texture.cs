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
        GenerateTile(_yMaskTile, TileMask, "Mask", _tileMaskList);
        GenerateTile(_ySidesTile, TileSides, "Sides", _tileSidesList);
        GenerateTile(_yBackgroundTile, TileBackground, "Background", _tileBackgroundList);
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
        if (_yMaskTile > -TileMask.GetComponent<SpriteMask>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateTile(_yMaskTile, TileMask, "Mask", _tileMaskList);
        else _yMaskTile += TileMask.GetComponent<SpriteMask>().sprite.texture.height;
        if (_ySidesTile > -TileSides.GetComponent<SpriteRenderer>().sprite.texture.height && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateTile(_ySidesTile, TileSides, "Sides", _tileSidesList);
        else _ySidesTile += TileSides.GetComponent<SpriteRenderer>().sprite.texture.height;
        if (_yBackgroundTile > -TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y && gameObject.GetComponent<Mine>().TextureSpawnTrigger == 1) GenerateTile(_yBackgroundTile, TileBackground, "Background", _tileBackgroundList);
        else _yBackgroundTile += TileBackground.GetComponent<SpriteRenderer>().sprite.texture.height * TileBackground.transform.localScale.y;
        _tileListCut(_tileMaskList);
        _tileListCut(_tileSidesList);
        _tileListCut(_tileBackgroundList);
    }
    void GenerateTile(float _yTile, GameObject obj, string n, List<GameObject> _tileList)
    {
        var TileI = Instantiate(obj, new Vector3(256, _yTile, 0), Quaternion.identity);
        TileI.transform.parent = transform;
        TileI.name = n;
        _tileList.Add(TileI);
    }
    void _tileListCut(List<GameObject> _tileList)
    {
        foreach (GameObject tile in _tileList)
        {
            if (tile.transform.position.y > MainCamera.pixelHeight + 200)
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