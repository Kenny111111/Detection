using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    // Unity calls this method automatically when it enables this component
    private void OnEnable()
    {
        // Add DoRenderLines as a delegate of the RenderPipelineManager.beginCameraRendering event
        RenderPipelineManager.beginCameraRendering += DoRenderLines;
    }

    // Unity calls this method automatically when it disables this component
    private void OnDisable()
    {
        // Remove DoRenderLines as a delegate of the RenderPipelineManager.beginCameraRendering event
        RenderPipelineManager.beginCameraRendering -= DoRenderLines;
    }

    void DoRenderLines(ScriptableRenderContext context, Camera camera)
    {
        RenderLines(lines);
    }


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


