using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour
{
    public int id;
    public bool isChoose = false;
    public GameObject choosedObject;
    private LineRenderer link;
    // Use this for initialization
    void Awake()
    {
        link = this.GetComponentInChildren<LineRenderer>();
    }
    private void Start()
    {
        link.SetPosition(0, this.transform.position);
        link.SetPosition(1, this.transform.position);
        link.SetPosition(2, this.transform.position);
        link.SetPosition(3, this.transform.position);
        isChoose = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (choosedObject != null)
        {
            choosedObject.SetActive(isChoose);
        }
        if (isChoose)
        {
            link.SetVertexCount(4);
        }else
        {
            link.SetVertexCount(0);
        }
    }
    public bool SetPathPoint(Vector3 start, Vector3 end)
    {
        Vector3 mid1, mid2;

        if (GameManager.GetPath(4 * start, 4 * end, out mid1, out mid2))
        {
            link.SetPosition(0, start);
            link.SetPosition(1, mid1 * 0.25f);
            link.SetPosition(2, mid2 * 0.25f);
            link.SetPosition(3, end);
            return true;
        }
        return false; 
    }

}
