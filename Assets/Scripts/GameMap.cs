using UnityEngine;
using System.Collections;
using System;
public class GameMap
{

    // Use this for initialization

    public const int MaxLength = 50;
    public const int MaxWidth = 50;
    public const int MaxHeight = 50;
    public const int MaxColorNums = 16;

    private static int _length = 0;
    private static int _width = 0;
    private static int _height = 0;
    private static int _color_nums = 0;

    private const int _random_nums = 200;

    private static int[] _random_list = new int[MaxWidth * MaxLength * MaxHeight];

    public static bool InitMap(int length, int width, int height, int color_nums)
    {
        if (width < 0 || width > MaxWidth) return false;
        if (length < 0 || length > MaxLength) return false;
        if (height < 0 || height > MaxHeight) return false;
        if (color_nums < 0 || color_nums > MaxColorNums) return false;
        int totBlock = width * length * height;

        if (1 == (totBlock & 1)) totBlock ^= 1;

        if (totBlock < color_nums * 2) return false;

        _width = width;
        _height = height;
        _length = length;
        _color_nums = color_nums;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    _map[i, j, k] = -1;
                }
            }
        }

        for (int i = 0; i < totBlock; i++)
        {
            _random_list[i] = (i >> 1) % color_nums;
        }
#if !TEST
        System.Random rand = new System.Random();

        for (int i = 0, l, r, t; i < _random_nums; i++)
        {
            l = rand.Next() % totBlock;
            r = rand.Next() % totBlock;
            t = _random_list[l];
            _random_list[l] = _random_list[r];
            _random_list[r] = t;
        }
#endif
        for (int i = 0, tot = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height && tot < totBlock; k++)
                {
#if TEST
                    _map[i, j, k] = 1;
#else
                    _map[i, j, k] = _random_list[tot++];
#endif
                }
            }
        }

        return true;
    }

    public static bool delBlock(int length, int width, int height)
    {
        if (width < 0 || width > _width) return false;
        if (length < 0 || length > _length) return false;
        if (height < 0 || height > _height) return false;

        if (-1 == _map[length, width, height]) return false;

        _map[length, width, height] = -1;

        return true;
    }

    public static bool canDelBlock(int length, int width, int height)
    {
        if (width < 0 || width > _width) return false;
        if (length < 0 || length > _length) return false;
        if (height < 0 || height > _height) return false;

        if (-1 == _map[width, length, height]) return false;

        return true;
    }

    private static int[,,] _map = new int[MaxWidth, MaxLength, MaxHeight];

    public static int[,,] GetMap()
    {
        return _map;
    }

    public static int GetLength()
    {
        return _length;
    }

    public static int GetWidth()
    {
        return _width;
    }

    public static int GetHeight()
    {
        return _height;
    }

    public static int getColorNums()
    {
        return _color_nums;
    }
}
