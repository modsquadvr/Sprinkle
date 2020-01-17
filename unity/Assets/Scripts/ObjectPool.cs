using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int size = 10;
    [Tooltip("Limitless size")]
    public int maxSize = 0;
    public GameObject prefab;
    public GameObject[] pool;
    public int idHead;
    public int idTail;

    const float goldenRatio = 1.61803398875f;

    public void Awake()
    {
        if (size < 2)
            size = 2;

        BuildPool();
    }

    /// <summary>
    /// Returns the next available object in the pool 
    /// </summary>
    /// <returns></returns>
    public GameObject NextGameObject()
    {
        for (int i = (idTail + 1) % size; ; i = (i + 1) % pool.Length)
        {
            if (pool[i].activeSelf == false)
            {
                idTail = i;
                break;
            }
            else if (i == idTail)
            {
                if (maxSize == 0 || size < maxSize)
                {
                    idTail = size;
                    size = Mathf.FloorToInt(pool.Length * goldenRatio);

                    if (maxSize > 0 && size > maxSize)
                        size = maxSize;
                    else if (size < 0)
                        size = int.MaxValue;

                    BuildPool();
                    break;
                }
                else
                {
                    Debug.LogWarning("No available gameobjects from pool " + name + " max size reached at " + size);
                    return null;
                }
            }
        }

        return pool[idTail];
    }

    public void BuildPool()
    {
        GameObject[] p = new GameObject[size];
        for (int k = 0; k < p.Length; k++)
        {
            if (k >= pool.Length)
            {
                p[k] = Instantiate(prefab);
                p[k].transform.SetParent(transform, false);
                p[k].gameObject.SetActive(false);
            }
            else
                p[k] = pool[k];
        }
        pool = p;
    }

    public void PoolRange()
    {
        if (pool.Length == 0)
            return;

        for (int i = idHead; ; i = (i + 1) % pool.Length)
        {
            pool[i].gameObject.SetActive(false);
            pool[i].transform.SetParent(transform);

            if (i == idTail)
                break;
        }
        idHead = idTail;
    }

    /// <summary>
    /// Pools a range of objects, if start is after end, counting overflows to 0 and continues to the end.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void PoolRange(int start, int end)
    {
        if (pool.Length == 0)
            return;

        for (int i = start; ; i = (i + 1) % pool.Length)
        {
            pool[i].gameObject.SetActive(false);

            if (i == end)
                break;
        }
    }

}
