using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterUtils{
	public class Rain : MonoBehaviour {
        [SerializeField] int RainNum = 100;
        [SerializeField] float Radius = 5f;
        [SerializeField] float RandomRadius = 0.5f;
        [SerializeField] Vector3 FromDir = Vector3.up * 10f;

        Mesh rainMesh;
        // Use this for initialization
        void Start()
        {
            Vector3[] vertces = new Vector3[RainNum * 2];
            for (int i = 0; i < RainNum; ++i)
            {
                Vector2 sPos = Random.insideUnitCircle * Radius;
                Vector2 rPos = Random.insideUnitCircle * RandomRadius;
                Vector3 wPos = new Vector3(sPos.x, 0f, sPos.y);
                vertces[i * 2 + 0] = wPos;
                vertces[i * 2 + 1] = wPos + FromDir + new Vector3(rPos.x, 0f, rPos.y);
            }
            rainMesh = CreateLine(vertces, LineMeshType.Lines, Color.white);
            GetComponent<MeshFilter>().mesh = rainMesh;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public enum LineMeshType
        {
            LineStrip,
            Ring,
            Lines
        };
        public static Mesh CreateLine(Vector3[] _vertices, LineMeshType _lineMeshType, Color _color)
        {
            bool isRing = (_lineMeshType == LineMeshType.Ring);
            MeshTopology topology = (_lineMeshType == LineMeshType.Lines) ? MeshTopology.Lines : MeshTopology.LineStrip;
            int vertNum = _vertices.Length;
            int[] incides = new int[isRing ? vertNum + 1 : vertNum];
            Vector2[] uv = new Vector2[vertNum];
            Color[] colors = new Color[vertNum];
            Vector3[] normals = new Vector3[vertNum];

            for (int ii = 0; ii < _vertices.Length; ++ii)
            {
                Vector3 normal = new Vector3(0.0f, 1.0f, 0.0f);
                if (ii < (_vertices.Length - 1))
                {
                    normal = _vertices[ii + 1] - _vertices[ii];
                }
                incides[ii] = ii;
                uv[ii] = new Vector2(_vertices[ii].x + 0.5f, _vertices[ii].y + 0.5f);
                colors[ii] = _color;
                normals[ii] = normal;
            }
            if (isRing)
            {
                incides[vertNum] = 0;
            }
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices;
            mesh.uv = uv;
            mesh.colors = colors;
            mesh.normals = normals;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.SetIndices(incides, topology, 0);
            return mesh;
        }
    }
}
