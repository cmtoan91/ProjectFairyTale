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
        newTile.transform.localPosition = new Vector3(_tileSize * coorX, 0, _tileSize * coorZ);

        //Create tile Graphic
        Mesh mesh = new Mesh();
        MeshRenderer renderer = newTile.AddComponent<MeshRenderer>();
        newTile.AddComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 0, _tileSize);
        vertices[2] = new Vector3(_tileSize, 0, _tileSize);
        vertices[3] = new Vector3(_tileSize, 0, 0);

        int[] triangles = new int[] { 0, 1, 2, 2, 3, 0 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        renderer.material = mat;

        //Add collider
        newTile.AddComponent<MeshCollider>();
    }
}
