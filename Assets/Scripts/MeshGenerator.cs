using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
    Mesh mesh;
    MeshCollider meshCollider;

    [SerializeField] int xSize = 20;
    [SerializeField] int zSize = 20;
    [SerializeField] float scale = 0.1f;
    [SerializeField] [Range(0, 0.3f)] float perlinScale = 0.3f;
    [SerializeField] [Range(0, 0.3f)] float perlinScale2 = 1f;
    [SerializeField] [Range(0, 0.3f)] float perlinScale3 = 1f;
    [SerializeField] float firstLevelAmp = 3f;
    [SerializeField] float secondLevelAmp = 3f;
    [SerializeField] float thirdLevelAmp = 3f;

    [SerializeField] Gradient gradient;


    Vector3[] verts;
    int[] triangles;
    Color[] colors;

    float minTerrainHeight, maxTerrainHeight;

    private void Start() {

    }

    private void Update() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        CreateShape();
        UpdateMesh();
        meshCollider.sharedMesh = mesh;
    }

    void CreateShape() {
        verts = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float level1 = Mathf.PerlinNoise(x * perlinScale, z * perlinScale) * firstLevelAmp;
                float level2 = Mathf.PerlinNoise(x * perlinScale2, z * perlinScale2) * secondLevelAmp;
                float level3 = Mathf.PerlinNoise(x * perlinScale3, z * perlinScale3) * thirdLevelAmp;
                float y = level1 + level2 + level3;
                verts[i] = new Vector3(x * scale, y * scale, z * scale);
                if (y > maxTerrainHeight) {
                    maxTerrainHeight = y;
                }
                if (y < minTerrainHeight) {
                    minTerrainHeight = y;
                }
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++) {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        colors = new Color[verts.Length];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, verts[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}
