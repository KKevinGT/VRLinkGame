using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager
{

    struct point3
    {
        public int x, y, z;
    }

    struct queueItem
    {
        public int x, y, z, d, t, from;
    }
    static int[,,] _map = new int[GameMap.MaxLength + 4, GameMap.MaxWidth + 4, GameMap.MaxHeight + 4];
    //static int[,,] _vis = new int[GameMap.MaxLength + 4, GameMap.MaxWidth + 4, GameMap.MaxHeight + 4];

    static queueItem[] queue = new queueItem[GameMap.MaxLength * GameMap.MaxWidth * GameMap.MaxHeight * 6 * 3];
    static Dictionary<int, int> _vis=new Dictionary<int, int>();
    static int[] dx = new int[] { 1, -1, 0, 0, 0, 0 };
    static int[] dy = new int[] { 0, 0, 1, -1, 0, 0 };
    static int[] dz = new int[] { 0, 0, 0, 0, 1, -1 };

    const float EPS = 1e-8f;

    private static int getVisCode(queueItem now)
    {
        int ret = 0;
        ret += now.x;
        ret *= GameMap.MaxWidth + 4;
        ret += now.y;
        ret *= GameMap.MaxHeight + 4;
        ret += now.z;
        ret *= 6;
        ret += now.d;
        ret *= 3;
        ret += now.t;
        return ret;
    }

    public static bool GetPath(Vector3 start_point, Vector3 end_point, out Vector3 mid_point_1, out Vector3 mid_point_2)
    {
        var map = GameMap.GetMap();
        int width = GameMap.GetWidth();
        int length = GameMap.GetLength();
        int height = GameMap.GetWidth();

        for (int i = 0; i < width + 4; i++)
        {
            for (int j = 0; j < length + 4; j++)
            {
                for (int k = 0; k < height + 4; k++)
                {
                    _map[i, j, k] = -2;
                }
            }
        }

        for (int i = 1; i < width + 3; i++)
        {
            for (int j = 1; j < length + 3; j++)
            {
                for (int k = 1; k < height + 3; k++)
                {
                    _map[i, j, k] = -1;
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    _map[i + 2, j + 2, k + 2] = map[i, j, k];
                }
            }
        }

        point3 st, ed;
        st.x = (int)(start_point.x + EPS) + 2;
        st.y = (int)(start_point.y + EPS) + 2;
        st.z = (int)(start_point.z + EPS) + 2;

        ed.x = (int)(end_point.x + EPS) + 2;
        ed.y = (int)(end_point.y + EPS) + 2;
        ed.z = (int)(end_point.z + EPS) + 2;

        //init
        _vis.Clear();

        queueItem now, nxt;

        now.x = st.x;
        now.y = st.y;
        now.z = st.z;
        now.d = -1;
        now.t = 0;
        now.from = -1;

        int r = 0;
        int l = 0;

        for (int d = 0; d < 6; d++)
        {
            now.d = d;
            queue[r++] = now;
            _vis[getVisCode(now)] = 1;
        }

        while (l < r)
        {
            now = queue[l];
            l++;
            for (int d = 0; d < 6; d++)
            {
                nxt.t = now.t;

                if (d != now.d)
                    nxt.t = now.t + 1;

                if (nxt.t > 2) continue;

                nxt.x = now.x + dx[d];
                nxt.y = now.y + dy[d];
                nxt.z = now.z + dz[d];
                nxt.d = d;
                nxt.from = l - 1;

                if (-2 == _map[nxt.x, nxt.y, nxt.z])
                {
                    continue;
                }

                if (nxt.x == ed.x && nxt.y == ed.y && nxt.z == ed.z)
                {
                    point3 tmp1, tmp2;
                    tmp1 = tmp2 = st;
                    tmp1.x -= 2;
                    tmp1.y -= 2;
                    tmp1.z -= 2;
                    tmp2.x -= 2;
                    tmp2.y -= 2;
                    tmp2.z -= 2;
                    queueItem p = nxt;
                    while (p.from != -1)
                    {
#if TEST
                        Debug.Log("path " + (p.x - 2) + " " + (p.y - 2) + " " + (p.z - 2) + " " + p.d + " " + p.t + " " + p.from);
#endif
                        if (p.d != queue[p.from].d)
                        {
                            tmp2.x = queue[p.from].x - 2;
                            tmp2.y = queue[p.from].y - 2;
                            tmp2.z = queue[p.from].z - 2;
                            p = queue[p.from];
                            break;
                        }
                        p = queue[p.from];
                    }
                    while (-1 != p.from)
                    {
#if TEST
                        Debug.Log("path " + (p.x - 2) + " " + (p.y - 2) + " " + (p.z - 2) + " " + p.d + " " + p.t + " " + p.from);
#endif
                        if (p.d != queue[p.from].d)
                        {
                            tmp1.x = queue[p.from].x - 2;
                            tmp1.y = queue[p.from].y - 2;
                            tmp1.z = queue[p.from].z - 2;
                            p = queue[p.from];
                            break;
                        }
                        p = queue[p.from];
                    }
#if TEST
                    while (-1 != p.from)
                    {
                        Debug.Log("path " + (p.x - 2) + " " + (p.y - 2) + " " + (p.z - 2) + " " + p.d + " " + p.t + " " + p.from);
                        mid_point_1.x = (float)tmp1.x;
                        p = queue[p.from];
                    }
#endif
                    mid_point_1.x = (float)tmp1.x;
                    mid_point_1.y = (float)tmp1.y;
                    mid_point_1.z = (float)tmp1.z;
                    mid_point_2.x = (float)tmp2.x;
                    mid_point_2.y = (float)tmp2.y;
                    mid_point_2.z = (float)tmp2.z;
                    if (!GameMap.delBlock(st.x - 2, st.y - 2, st.z - 2))
                        Debug.Log("cannot delete " + (st.x - 2) + (st.y - 2) + (st.z - 2));
                    if (!GameMap.delBlock(ed.x - 2, ed.y - 2, ed.z - 2))
                        Debug.Log("cannot delete " + (ed.x - 2) + (ed.y - 2) + (ed.z - 2));
                    return true;
                }

                if (-1 != _map[nxt.x, nxt.y, nxt.z])
                {
                    continue;
                }
                if (!_vis.ContainsKey(getVisCode(nxt)))
                {
                    _vis[getVisCode(nxt)] = 1;
                    queue[r++] = nxt;
                }

                
            }
        }
        mid_point_1 = new Vector3();
        mid_point_2 = new Vector3();
        return false;
    }

    //判断是否游戏结束
    //return true : 游戏结束
    public static bool isEndGame()
    {
        var map = GameMap.GetMap();
        int width = GameMap.GetWidth();
        int length = GameMap.GetLength();
        int height = GameMap.GetHeight();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (-1 != map[i, j, k])
                        return false;
                }
            }
        }
        return true;
    }

    ////判断当前还有没有格子能消除
    ////
    ////未实现
    //public static bool isDeath()
    //{
    //    int color_nums = GameMap.getColorNums();
    //    int width = GameMap.GetWidth();
    //    int length = GameMap.GetLength();
    //    int height = GameMap.GetHeight();
    //    return false;
    //}

    ////获取提示
    ////
    ////未实现
    //public static bool getHit(out Vector3 mid_point_1, out Vector3 mid_point_2)
    //{
    //    mid_point_1 = new Vector3(1, 2, 3);
    //    mid_point_2 = new Vector3(2, 3, 4);
    //    if (true) return true;
    //}
}
