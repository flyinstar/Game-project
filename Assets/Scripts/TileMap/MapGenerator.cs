using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace TileMap
{
    [Serializable]
    public class ItemDate
    {
        public Tile tile;
        public int weight;
    }

    public class MapGenerator : MonoBehaviour
    {
        public Tilemap groundTilemap;//地面图层
        public Tilemap itemTilemap;//物品图层

        //地图宽高
        public int width;
        public int height;

        //地图种子
        public int seed;
        public bool useRandomSeed;

        //细致度
        public float lacunarity;

        //背景图层比例
        [Range(0f, 1f)]
        public float backgroundPobability;

        //可生成物品列表
        public List<ItemDate> itemDates;

        //用于填充图层的瓦片
        public TileBase backgroundTile; 
        public TileBase tile1;

        //地图生成数据
        public float[,] mapDate;

        public void GenerateMap()
        {
            //调整物品权重
            itemDates.Sort((date1, date2) => date1.weight.CompareTo(date2.weight));
            //生成地图数据
            GenerateMapDate();
            //根据数据生成地图
            GenerateTileMap();
        }

        public void GenerateMapDate()
        {
            //是否使用随机种子
            if (useRandomSeed)
            {
                seed = Time.time.GetHashCode();
            }
            UnityEngine.Random.InitState(seed);

            mapDate = new float[width, height];

            float randomOffset = UnityEngine.Random.Range(-10000,10000);

            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            //生成地图数据
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * lacunarity + randomOffset, y * lacunarity + randomOffset);
                    mapDate[x, y] = noiseValue;
                    if (noiseValue < minValue) minValue = noiseValue;
                    if (noiseValue > maxValue) maxValue = noiseValue;
                }
            }

            //将数据平滑至[0,1]
            for (int x = 0;x < width; x++)
            {
                for(int y = 0;y < height; y++)
                {
                    mapDate[x, y] = Mathf.InverseLerp(minValue, maxValue, mapDate[x, y]);
                }
            }
        }

        public void GenerateTileMap()
        {
            for(int x = 0; x < width; x++)
            {
                for( int y = 0; y < height; y++)
                {
                    TileBase tile = mapDate[x, y] > backgroundPobability ? backgroundTile : tile1;
                    groundTilemap.SetTile(new Vector3Int(x, y), tile);
                }
            }

            //计算总权重
            int weightTotal = 0;
            for(int i = 0; i < itemDates.Count; i++)
            {
                weightTotal += itemDates[i].weight;
            }

            for (int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++) 
                {
                    //物品应被生成在地面上
                    if(mapDate[x, y] > backgroundPobability)
                    {
                        //随机生成一个数，判断它所处的权重区间
                        float randValue = UnityEngine.Random.Range(1, weightTotal + 1);
                        float temp = 0;

                        for(int i=0;i<itemDates.Count;i++)
                        {
                            temp += itemDates[i].weight;//跳转到下一个权重区间
                            //如果落在当前物品权重区间
                            if(temp > randValue)
                            {
                                //且当前物品非空
                                if (itemDates[i].tile)
                                {
                                    //生成物品
                                    itemTilemap.SetTile(new Vector3Int(x, y), itemDates[i].tile);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void CleanTileMap()
        {
            groundTilemap.ClearAllTiles();
            itemTilemap.ClearAllTiles();
        }
    }
}