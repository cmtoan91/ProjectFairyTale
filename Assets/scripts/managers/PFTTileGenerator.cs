using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFTTileGenerator : MonoBehaviour
{
    #region props
    [SerializeField]
    int _tileCountX = 10;

    [SerializeField]
    int _tileCountZ = 10;

    [SerializeField]
    float _tileSize = 3f;

    [SerializeField]
    float _offset = 0.5f;

    [SerializeField]
    Material _matWhite;

    [SerializeField]
    Material _matBlack;

    #endregion

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        GenerateAllTiles(_tileCountX, _tileCountZ);
    }

    void GenerateAllTiles(int tileCountX, int tileCountZ)
    {
        for(int i = 0; i < tileCountX; i++)
        {
            for (int j = 0; j < tileCountZ; j++)
            {
                int sum = i + j;
                if (sum % 2 == 0)
                {
                    GenerateSingleTile(i, j, _matWhite);
                }
                else
                {
                    GenerateSingleTile(i, j, _matBlack);
                }
            }
        }
    }

    void GenerateSingleTile(int coorX, int coorZ, Material mat)
    {
        GameObject newTile = new GameObject(string.Format("Tile X: {0} Z: {1}", coorX, coorZ));

        //Set tile position
        newTile.transform.parent = transform;
        float size = _tileSize + _offset;
        float worldPosX = size * Mathf.Sqrt(3) * coorX - size * Mathf.Sqrt(3) * 0.5f * (coorZ % 2);
        float worldPosZ = 1.5f * size * coorZ;
        newTile.transform.localPosition = new Vector3(worldPosX, 0, worldPosZ);

        //Create tile Graphic
        Mesh mesh = new Mesh();
        MeshRenderer renderer = newTile.AddComponent<MeshRenderer>();
        newTile.AddComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[6];
        float halfWidth = _tileSize * Mathf.Sqrt(3) * 0.5f;
        float halfHeight = _tileSize;

        vertices[0] = new Vector3(0, 0, halfHeight);
        vertices[1] = new Vector3(halfWidth, 0, halfHeight * 0.5f);
        vertices[2] = new Vector3(halfWidth, 0, -halfHeight * 0.5f);
        vertices[3] = new Vector3(0, 0, -halfHeight);
        vertices[4] = new Vector3(-halfWidth, 0, -halfHeight * 0.5f);
        vertices[5] = new Vector3(-halfWidth, 0, halfHeight * 0.5f);

        int[] triangles = new int[] { 0, 1, 2, 0, 2, 5, 2, 3, 4, 2, 4, 5};

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        renderer.material = mat;

        //Add collider
        newTile.AddComponent<MeshCollider>().sharedMesh = mesh;
    }
}
