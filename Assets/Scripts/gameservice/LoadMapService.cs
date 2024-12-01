using System;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using Object = UnityEngine.Object;

namespace gameservice
{
    public class LoadMapService : MonoBehaviour
    {
        [SerializeField] private GameObject brickPrefab; // Prefab gạch
        [SerializeField] private GameObject wallPrefab; // Prefab tường
        [SerializeField] private GameObject bridgePrefab; // Prefab cầu
        [SerializeField] private GameObject bridgePrefab2; // cầu ngang
        [SerializeField] private GameObject gameoverPrefab;
        [SerializeField] private Transform map;
        private string path = Path.Combine(Application.dataPath, "Data/level1.txt");
        private string line;
        
        [SerializeField] private float y= 2.0f;
        private float RotationX = -90f;
        private float RotationZ = -90f;

        private void Start()
        {
            ReadFile(); 
        }

        private void ReadFile()
        {
            if (map == null)
            {
                return;
            }
            foreach (Transform child in map)
            {
                DestroyImmediate(child.gameObject);
            }
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int row = 0;
                    while ((line = sr.ReadLine())!= null)
                    {
                        Debug.Log(line);
                        string[] tokens = line.Split(' ');

                        for (int col = 0; col < tokens.Length; col++)
                        {
                            if (int.TryParse(tokens[col], out int value))
                            {
                                Vector3 position = new Vector3(col, 0, -row);
                                InstantiatePrefab(value, position);
                            }
                        }
                        row++;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Lỗi khi đọc file: " + e.Message);
            }

        }

        private void InstantiatePrefab(int value, Vector3 position)
        {
            GameObject prefabInstantiate = null;
            switch (value)
            {
                case 1:
                    prefabInstantiate = brickPrefab;
                    break;
                case 2:
                    prefabInstantiate = wallPrefab;
                    break;
                case 3:
                    prefabInstantiate = bridgePrefab;
                    break;
                case 4:
                    prefabInstantiate = bridgePrefab2;
                    break;
                case 5:
                    prefabInstantiate = gameoverPrefab;
                    break;
                case 0:
                    return;

            }

            if (prefabInstantiate != null)
            {
                GameObject instantiatedObject = Instantiate(prefabInstantiate, position, Quaternion.identity);
                instantiatedObject.transform.parent = map;
                if (value == 3)
                {
                    instantiatedObject.transform.rotation = Quaternion.Euler(RotationX, 0, 0);
                    instantiatedObject.transform.position = new Vector3(position.x, position.y + y, position.z);
                }
                if (value == 4)
                {
                    instantiatedObject.transform.rotation = Quaternion.Euler(RotationX, 0, RotationZ);
                    instantiatedObject.transform.position = new Vector3(position.x, position.y + y, position.z);
                }
            }
        }
    }
}
