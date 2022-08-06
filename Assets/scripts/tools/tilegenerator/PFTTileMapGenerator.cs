using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapDesigner
{
    public class PFTTileMapGenerator : MonoBehaviour
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


        public int TileCountX => _tileCountX;
        public int TileCountZ => _tileCountZ;
        public float TileSize => _tileSize;
        public float OffSet => _offset;

        public Material MaterialWhite => _matWhite;
        Dictionary<Vector2, Transform> _tileMap = new Dictionary<Vector2, Transform>();
        #endregion

        public void GenerateAllTiles(int tileCountX, int tileCountZ, float size, float offset)
        {
            ClearAllTile();
            for (int i = 0; i < tileCountX; i++)
            {
                for (int j = 0; j < tileCountZ; j++)
                {
                    _tileMap[new Vector2(i, j)] = GenerateSingleTileSlot(i, j, size, offset, _matWhite);
                }
            }
        }

        public void ClearAllTile()
        {
            List<Transform> allchild = new List<Transform>();
            for(int i = 0; i< transform.childCount; i++)
            {
                allchild.Add(transform.GetChild(i));
            }

            foreach(Transform trans in allchild)
            {
                DestroyImmediate(trans.gameObject);
            }

            _tileMap = new Dictionary<Vector2, Transform>();
        }

        Transform GenerateSingleTileSlot(int coorX, int coorZ, float size, float offset, Material mat)
        {
            GameObject newTile = new GameObject(string.Format("Tile X: {0} Z: {1}", coorX, coorZ));

            //Set tile position
            SetTilePosition(newTile.transform, size, offset, coorX, coorZ);
            //Create tile Graphic
            Mesh mesh = new Mesh();
            MeshRenderer renderer = newTile.AddComponent<MeshRenderer>();
            newTile.AddComponent<MeshFilter>().mesh = mesh;

            Vector3[] vertices = new Vector3[6];
            float halfWidth = size * Mathf.Sqrt(3) * 0.5f;
            float halfHeight = size;

            vertices[0] = new Vector3(0, 0, halfHeight);
            vertices[1] = new Vector3(halfWidth, 0, halfHeight * 0.5f);
            vertices[2] = new Vector3(halfWidth, 0, -halfHeight * 0.5f);
            vertices[3] = new Vector3(0, 0, -halfHeight);
            vertices[4] = new Vector3(-halfWidth, 0, -halfHeight * 0.5f);
            vertices[5] = new Vector3(-halfWidth, 0, halfHeight * 0.5f);

            int[] triangles = new int[] { 0, 1, 2, 0, 2, 5, 2, 3, 4, 2, 4, 5 };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            renderer.material = mat;

            //Add collider
            //newTile.AddComponent<MeshCollider>();
            newTile.AddComponent<PFTTileSlot>().SetCorners(vertices);

            return newTile.transform;
        }

        Vector3 SetTilePosition(Transform tilePos, float tileSize, float offset, int coorX, int coorZ)
        {
            tilePos.parent = transform;
            float size = tileSize + offset;
            float worldPosX = size * Mathf.Sqrt(3) * coorX - size * Mathf.Sqrt(3) * 0.5f * (coorZ % 2);
            float worldPosZ = 1.5f * size * coorZ;
            tilePos.localPosition = new Vector3(worldPosX, 0, worldPosZ);

            return tilePos.localPosition;
        }
    }
}