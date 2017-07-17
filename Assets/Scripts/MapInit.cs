using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapInit : MonoBehaviour
{
    public List<GameObject> cubeList;
    public int length, width, height;

    // Use this for initialization
    void Start()
    {
        InitMap();
    }

    void InitMap()
    {
        if (cubeList != null)
            GameMap.InitMap(length, width, height, cubeList.Count);

        var map = GameMap.GetMap();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (-1 != map[i, j, k])
                    {
                        GameObject go = Instantiate<GameObject>((GameObject)cubeList[map[i, j, k]]) as GameObject;
                        go.transform.position = new Vector3(i * 0.25f, j * 0.25f, k * 0.25f);
                        go.transform.parent = this.transform;
                    }
                }
            }
        }
    }

}
