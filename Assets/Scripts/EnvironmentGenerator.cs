using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject decor1;
    public GameObject[] decor2;
    public GameObject[] decor3;

    private float decor1Size;
    public List<float> cactusSizes;

    public float x1, x2;
    public float y;

    private RaycastHit hit;

    public void GetDecor2Sizes()
    {
        decor1Size = decor1.transform.localScale.y * decor1.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y / 2;
        for (int i = 0; i < decor2.Length; i++)
        {
            cactusSizes.Add(decor2[i].transform.localScale.y * decor2[i].GetComponent<MeshFilter>().sharedMesh.bounds.extents.y / 2);
        }
    }

    private void GenerateDecor1()
    {
        Vector3 pos = new Vector3(Random.Range(x1, x2), y, Random.Range(x1, x2));
        if (Physics.Raycast(pos, Vector3.down, out hit, 200))
        {
            if (hit.transform.name == "Terrain")
            {
                pos = hit.point;
                pos.y -= decor1Size;
                var go = Instantiate(decor1, pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                go.tag = "decor1";
                go.isStatic = true;
            }
        }
    }

    private void GenerateDecor2()
    {
        Vector3 pos = new Vector3(Random.Range(x1, x2), y, Random.Range(x1, x2));
        if (Physics.Raycast(pos, Vector3.down, out hit, 200))
        {
            if (hit.transform.name == "Terrain")
            {
                int index = Random.Range(0, decor2.Length );
                Debug.Log(index);
                pos = hit.point;
                pos.y += cactusSizes[index];
                var go = Instantiate(decor2[index], pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                go.tag = "Cactus";
                go.isStatic = true;
            }
        }
    }

    private void GenerateDecor3()
    {
        Vector3 pos = new Vector3(Random.Range(x1, x2), y, Random.Range(x1, x2));
        if (Physics.Raycast(pos, Vector3.down, out hit, 200))
        {
            if (hit.transform.name == "Terrain")
            {
                var go = Instantiate(decor3[Random.Range(0, decor3.Length )], hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                go.tag = "Rock";
                go.isStatic = true;
            }
        }
    }

    public void GenerateDecor1s(int count)
    {
        for (int i = 0; i < count; i++) GenerateDecor1();
    }

    public void GenerateDecor2s(int count)
    {
        for (int i = 0; i < count; i++) GenerateDecor2();
    }

    public void GenerateDecor3s(int count)
    {
        for (int i = 0; i < count; i++) GenerateDecor3();
    }
}
