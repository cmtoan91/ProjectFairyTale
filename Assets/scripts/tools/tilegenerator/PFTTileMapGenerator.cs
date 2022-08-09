using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MapDesigner
{
    public class PFTTileMapGenerator : MonoBehaviour
    {
        #region props
        [SerializeField]
        int _tileCountX = 10;

        [SerializeField]
        int _tileCountZ = 10;

        
        [Header("Adjustable after generate")]
        [SerializeField]
        float _tileSize = 3f;

        [SerializeField]
        float _prefabTileSize = 0.5f;

        [SerializeField]
        float _prefabTileHeight = 1;

        [SerializeField]
        float _offset = 0.5f;

        [SerializeField]
        Material _matWhite;

        [SerializeField]
        string _tileMapName = "New Tile Map";

        [SerializeField]
        PFTTileMap _currentTileMap;

        public int TileCountX => _tileCountX;
        public int TileCountZ => _tileCountZ;
        public float TileSize => _tileSize;
        public float OffSet => _offset;

        public Material MaterialWhite => _matWhite;

        Dictionary<TileTerrainType, GameObject> _tilePrefabData = new Dictionary<TileTerrainType, GameObject>();
        public Dictionary<TileTerrainType, GameObject> TilePrefabData => _tilePrefabData;
        #endregion

        public void GenerateAllTiles(int tileCountX, int tileCountZ, float size, float offset)
        {
            ClearAllTile();
            for (int i = 0; i < tileCountX; i++)
            {
                for (int j = 0; j < tileCountZ; j++)
                {
                    PFTTileSlot slot = GenerateSingleTileSlot(i, j, size, offset, _matWhite);
                    if(_currentTileMap != null)
                    {
                        _currentTileMap.AddTileSlotToMap(slot);
                    }
                }
            }
            _currentTileMap.LoadTileMapData(TilePrefabData);
        }

        public void ClearAllTile()
        {
            if(_currentTileMap == null)
            {
                Debug.Log("Tile Map not selected");
                return;
            }
            List<Transform> allchild = new List<Transform>();

            for(int i = 0; i < _currentTileMap.transform.childCount; i++)
            {
                allchild.Add(_currentTileMap.transform.GetChild(i));
            }

            foreach(Transform trans in allchild)
            {
                DestroyImmediate(trans.gameObject);
            }

            _currentTileMap.ClearTileMap();
        }

        public void RefreshData()
        {
            _tilePrefabData = new Dictionary<TileTerrainType, GameObject>();
            SO_TilePrefabHolder[] tileinfos = Resources.LoadAll<SO_TilePrefabHolder>("SOs/SO_Tiles");
            if(tileinfos.Length > 0)
            {
                foreach(SO_TilePrefabHolder tile in tileinfos)
                {
                    _tilePrefabData[tile.TerrainType] = tile.TilePrefab;
                    Debug.Log(tile.TerrainType + " loaded");
                }
            }

            if (_currentTileMap != null)
                _currentTileMap.LoadTileMapData(_tilePrefabData);
        }

        public GameObject GetTilePrefab(TileTerrainType type)
        {
            GameObject prefab;
            if(_tilePrefabData.TryGetValue(type, out prefab))
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Khong tim thay Tile. An nut Refresh Data hoac them Tile vao folder");
                return null;
            }
        }

        public void SaveTileMap()
        {
            bool iSuccess;
            string localPath = "Assets/resources/prefabs/map/" + _tileMapName + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            Object prefab = PrefabUtility.SaveAsPrefabAsset(_currentTileMap.gameObject, localPath, out iSuccess);
            if(iSuccess)
            {
                Debug.Log("Succesfully save file to " + localPath);
            }
            else
            {
                Debug.Log("Save fail");
            }
        }

        PFTTileSlot GenerateSingleTileSlot(int coorX, int coorZ, float size, float offset, Material mat)
        {
            GameObject newTile = new GameObject(string.Format("Tile X: {0} Z: {1}", coorX, coorZ));

            //Set tile position
            SetTilePosition(newTile.transform, size, offset, coorX, coorZ);
            //Create tile Graphic
            Mesh mesh = new Mesh();
            MeshRenderer renderer = newTile.AddComponent<MeshRenderer>();
            newTile.AddComponent<MeshFilter>().mesh = mesh;

            Vector3[] vertices = CalculateVertices(size);

            int[] triangles = new int[] { 0, 1, 2, 0, 2, 5, 2, 3, 4, 2, 4, 5 };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            renderer.material = mat;

            //Add collider
            //newTile.AddComponent<MeshCollider>();
            PFTTileSlot tileSlot = newTile.AddComponent<PFTTileSlot>();
            tileSlot.SetCornerOffsets(vertices);
            tileSlot.SetCorners();
            tileSlot.SetMap(_currentTileMap);
            tileSlot.SetSize(size, _prefabTileSize, _prefabTileHeight);
            tileSlot.SetCoordinate(coorX, coorZ);
            return tileSlot;
        }

        Vector3[] CalculateVertices(float size)
        {
            Vector3[] vertices = new Vector3[6];
            float halfWidth = size * Mathf.Sqrt(3) * 0.5f;
            float halfHeight = size;

            vertices[0] = new Vector3(0, 0, halfHeight);
            vertices[1] = new Vector3(halfWidth, 0, halfHeight * 0.5f);
            vertices[2] = new Vector3(halfWidth, 0, -halfHeight * 0.5f);
            vertices[3] = new Vector3(0, 0, -halfHeight);
            vertices[4] = new Vector3(-halfWidth, 0, -halfHeight * 0.5f);
            vertices[5] = new Vector3(-halfWidth, 0, halfHeight * 0.5f);

            return vertices;
        }

        Vector3 SetTilePosition(Transform tilePos, float tileSize, float offset, int coorX, int coorZ)
        {
            if(_currentTileMap == null)
            {
                GameObject tileMapObject = new GameObject(_tileMapName);
                tileMapObject.transform.position = Vector3.zero;
                _currentTileMap = tileMapObject.AddComponent<PFTTileMap>();
            }

            tilePos.parent = _currentTileMap.transform;
            float size = tileSize + offset;
            float worldPosX = size * Mathf.Sqrt(3) * coorX - size * Mathf.Sqrt(3) * 0.5f * (coorZ % 2);
            float worldPosZ = 1.5f * size * coorZ;
            tilePos.localPosition = new Vector3(worldPosX, 0, worldPosZ);

            return tilePos.localPosition;
        }

        private void OnValidate()
        {
            if (_currentTileMap == null)
                return;

            if (_currentTileMap.AllSlot == null)
                return;

            foreach(PFTTileSlot slot in _currentTileMap.AllSlot)
            {
                SetTilePosition(slot.transform, _tileSize, _offset, slot.SlotCoordinate.x, slot.SlotCoordinate.y);
                slot.SetSize(_tileSize, _prefabTileSize, _prefabTileHeight);
                Mesh mesh = slot.GetComponent<MeshFilter>().sharedMesh;
                Vector3[] verts = CalculateVertices(_tileSize);
                mesh.vertices = verts;
                slot.SetCornerOffsets(verts);
                slot.SetCorners();
            }
        }
    }
}