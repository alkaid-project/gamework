using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    public Tilemap tilemap;//引用的Tilemap，加入脚本后需要将对应tilemap拖进来
    private Dictionary<string, Tile> arrTiles; //地块种类
    private List<string> TilesName;
    string[] TileType;
    //大地图宽高
    public int levelW;
    public int levelH;
    // Start is called before the first frame update
    void Start()
    {
        arrTiles = new Dictionary<string, Tile>();
        TilesName = new List<string>();
        InitTile();
        InitMapTilesInfo();
        InitData();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitData()
    {
        for (int i = 0; i < levelH; i++)
        {//根据地面类型TileType初始化tilemap
            for (int j = 0; j < levelW; j++)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), arrTiles[TileType[i * levelW + j]]);
            }
        }
    }

    //地图信息录入
    void InitMapTilesInfo()
    {
        //初始化地图信息，即每个单位对应的地面类型
        TileType = new string[levelH * levelW];
        for (int i = 0; i < levelH; i++)
        {
            for (int j = 0; j < levelW; j++)
            {
                TileType[i * levelW + j] = TilesName[Random.Range(0, TilesName.Count)];
            }
        }
    }

    //地图信息读取
    void InitMapTilesByFile()
    {

    }

    //随机地图生成
    void InitMapTilesByRandom()
    {

    }

    //初始化地面瓦片
    void InitTile()
    {
        //[0-0]道路型地面
        AddTile("soil", "河流");
        //[1-1]障碍型地面
        AddTile("brick", "障碍");
        //[2-2]功能型地面
        AddTile("grass", "路面");

    }

    void AddTile(string labelName, string spritePath)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();//创建Tile，注意，要使用这种方式
        Sprite tmp = Resources.Load<Sprite>(spritePath);
        tile.sprite = tmp;
        arrTiles.Add(labelName, tile);
        TilesName.Add(labelName);

    }
}