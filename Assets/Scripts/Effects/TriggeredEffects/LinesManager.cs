using System.Collections.Generic;
using UnityEngine;

public class Line
{
    public Vector3 startPoint;
    public Vector3 endPoint;

    public Line(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;
    }
}

// Usage example; FindObjectOfType<LinesManager>();
public class LinesManager : MonoBehaviour
{
    public static LinesManager manager;
    public Material material;
    public List<KeyValuePair<Vector3, Vector3>> lines;
    private Color defaultColor = new Color(255, 255, 255, 255);
    public Color color;
    //public Camera camera;

    private void Awake()
    {
        // Ensure only one manager exists
        if (manager == null)
        {
            manager = this;
        }
        else Destroy(gameObject);

        color = defaultColor;
        lines = new List<KeyValuePair<Vector3, Vector3>>();
    }

    public void Reset()
    {
        if (lines != null) lines.Clear();
        color = defaultColor;
    }
    void OnRenderObject()
    {
        RenderLines();
    }

    private void RenderLines()
    {
        //Matrix4x4 mat = new Matrix4x4();
        //mat.SetTRS(camera.transform.position, camera.transform.rotation, camera.transform.localScale);
        //GL.PushMatrix();
        //GL.MultMatrix(mat);
        GL.Begin(GL.LINES);
        material.SetPass(0);
        RenderLineList(lines, color);
        GL.End();
        //GL.PopMatrix();
    }

    private void RenderLineList(List<KeyValuePair<Vector3, Vector3>> lineList, Color color)
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            GL.Color(color);
            GL.Vertex(lineList[i].Key);
            GL.Color(color);
            GL.Vertex(lineList[i].Value);
        }
    }

}