using UnityEngine;

public class TerrainFace
{
    private Mesh _mesh;
    private int _resolution;
    private Vector3 _localUp;
    private Vector3 _axisA;
    private Vector3 _axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;

        // _axisA is perpendicular to _localUp, and _axisB is perpendicular to both.
        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(_axisA, localUp);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int iterations = x + y * _resolution;
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA
                                                   + (percent.y - 0.5f) * 2 * _axisB;
                Vector3 pointsOnUnitSphere = pointOnUnitCube.normalized;
                vertices[iterations] = pointsOnUnitSphere;

                // Another approach from catlikecoding  - https://catlikecoding.com/unity/tutorials/cube-sphere/
//                float x2 = pointOnUnitCube.x * pointOnUnitCube.x;
//                float y2 = pointOnUnitCube.y * pointOnUnitCube.y;
//                float z2 = pointOnUnitCube.z * pointOnUnitCube.z;
//                Vector3 pointOnUnitSphere = new Vector3(pointOnUnitCube.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f), pointOnUnitCube.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f), pointOnUnitCube.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f));
//                vertices[iterations] = pointsOnUnitSphere;

                // avoiding the edges
                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    // two triangles, clockwise order
                    triangles[triIndex    ] = iterations;
                    triangles[triIndex + 1] = iterations + _resolution + 1;
                    triangles[triIndex + 2] = iterations + 1;

                    triangles[triIndex + 3] = iterations;
                    triangles[triIndex + 4] = iterations + _resolution;
                    triangles[triIndex + 5] = iterations + _resolution + 1;
                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

}
