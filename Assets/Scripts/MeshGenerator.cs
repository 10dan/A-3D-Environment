using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
    public Mesh mesh;
    MeshCollider meshCollider;
    int xSize = 100;
    int zSize = 100;

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
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        CreateShape();
        UpdateMesh();
        meshCollider.sharedMesh = mesh;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            AddLand();
        }
    }

    private void AddLand() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        if (meshCollider.Raycast(ray, out hitPoint, 400f)) {
            Vector3 pos = ray.GetPoint(hitPoint.distance);
            int x = Mathf.RoundToInt(pos.x);
            int z = Mathf.RoundToInt(pos.z);
            verts[x + z *     (zSize + 1)] += Vector3.up * 2;
            CalcTriangles();
        }
    }

    void CreateShape() {
        verts = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float level1 = Mathf.PerlinNoise(x * perlinScale, z * perlinScale) * firstLevelAmp;
                float level2 = Mathf.PerlinNoise(x * perlinScale2, z * perlinScale2) * secondLevelAmp;
                float level3 = Mathf.PerlinNoise(x * perlinScale3, z * perlinScale3) * thirdLevelAmp;
                float y = level1 + level2 + level3;
                verts[i] = new Vector3(x, y, z);
                if (y > maxTerrainHeight) {
                    maxTerrainHeight = y;
                }
                if (y < minTerrainHeight) {
                    minTerrainHeight = y;
                }
                i++;
            }
        }

        CalcTriangles();
    }

    private void CalcTriangles() {
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
        UpdateMesh();
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}
