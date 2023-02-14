using System.Collections.Generic;
using UnityEngine;

// Recommended to attach this script to the Main Camera instance.
// Will not render in play mode if not attached to Camera.
[ExecuteInEditMode]
public class LinesManager : MonoBehaviour
{
    private Color defaultColor = new Color(255, 255, 255, 255);
    public Color baseColor;
    public Material material;

    //public Transform origin;
    public List<KeyValuePair<Vector3, Vector3>> lines;

    private void Awake()
    {
        lines = new List<KeyValuePair<Vector3, Vector3>>();
    }

    //void OnPreRender()
    //{
    //    RenderLines(lines);
    //}

    void OnDrawGizmos()
    {
        RenderLines(lines);
    }

    void RenderLines(List<KeyValuePair<Vector3, Vector3>> lines)
    {
        if (!ValidateInput(lines))
        {
            return;
        }

        GL.Begin(GL.LINES);
        material.SetPass(0);
        for (int i = 0; i < lines.Count; i++)
        {
            GL.Color(baseColor);
            GL.Vertex(lines[i].Key);
            GL.Color(baseColor);
            GL.Vertex(lines[i].Value);
        }
        GL.End();
    }

    private bool ValidateInput(List<KeyValuePair<Vector3, Vector3>> lines)
    {
        return lines != null;
    }

    public void Reset()
    {
        lines.Clear();
        baseColor = defaultColor;
    }
}


