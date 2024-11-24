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
        public Tilemap groundTilemap;//����ͼ��
        public Tilemap itemTilemap;//��Ʒͼ��

        //��ͼ���
        public int width;
        public int height;

        //��ͼ����
        public int seed;
        public bool useRandomSeed;

        //ϸ�¶�
        public float lacunarity;

        //����ͼ�����
        [Range(0f, 1f)]
        public float backgroundPobability;

        //��������Ʒ�б�
        public List<ItemDate> itemDates;

        //�������ͼ�����Ƭ
        public TileBase backgroundTile; 
        public TileBase tile1;

        //��ͼ��������
        public float[,] mapDate;

        public void GenerateMap()
        {
            //������ƷȨ��
            itemDates.Sort((date1, date2) => date1.weight.CompareTo(date2.weight));
            //���ɵ�ͼ����
            GenerateMapDate();
            //�����������ɵ�ͼ
            GenerateTileMap();
        }

        public void GenerateMapDate()
        {
            //�Ƿ�ʹ���������
            if (useRandomSeed)
            {
                seed = Time.time.GetHashCode();
            }
            UnityEngine.Random.InitState(seed);

            mapDate = new float[width, height];

            float randomOffset = UnityEngine.Random.Range(-10000,10000);

            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            //���ɵ�ͼ����
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

            //������ƽ����[0,1]
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

            //������Ȩ��
            int weightTotal = 0;
            for(int i = 0; i < itemDates.Count; i++)
            {
                weightTotal += itemDates[i].weight;
            }

            for (int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++) 
                {
                    //��ƷӦ�������ڵ�����
                    if(mapDate[x, y] > backgroundPobability)
                    {
                        //�������һ�������ж���������Ȩ������
                        float randValue = UnityEngine.Random.Range(1, weightTotal + 1);
                        float temp = 0;

                        for(int i=0;i<itemDates.Count;i++)
                        {
                            temp += itemDates[i].weight;//��ת����һ��Ȩ������
                            //������ڵ�ǰ��ƷȨ������
                            if(temp > randValue)
                            {
                                //�ҵ�ǰ��Ʒ�ǿ�
                                if (itemDates[i].tile)
                                {
                                    //������Ʒ
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