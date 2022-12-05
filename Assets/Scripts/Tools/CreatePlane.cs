using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class CreatePlane : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreatePlaneMesh();
    }

    Mesh CreatePlaneMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
             new Vector3(-5f, 0, -5f),
             new Vector3(-5f, 0, 5f),
             new Vector3(5f, 0, -5f),
             new Vector3(5f, 0, 5f),
        };

        Vector2[] uv = new Vector2[]
        {
             new Vector2(1, 1),
             new Vector2(1, 0),
             new Vector2(0, 1),
             new Vector2(0, 0),
        };

        int[] triangles = new int[]
        {
             0, 1, 2,
             2, 1, 3,
        };

        mesh.name = "Hex";

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.normals = new Vector3[] {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back
        };

#if UNITY_EDITOR
        Debug.Log("Vertices: " + mesh.vertices.Length + " Normals: " + mesh.normals.Length);

        AssetDatabase.CreateAsset(mesh, "Assets/square.asset");
#endif

        return mesh;
    }
}