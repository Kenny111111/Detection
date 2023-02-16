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

    private void Start()
    {
        lines = new List<KeyValuePair<Vector3, Vector3>>();
        //Vector3 start = new Vector3(0, 0, 0);
        //Vector3 end = new Vector3(5, 5, 5);
        //lines.Add(new KeyValuePair<Vector3, Vector3>(start, end));
    }

    // Unity calls this method automatically when it enables this component
    private void OnEnable()
    {
        // When I uncomment(only) this line, DoRenderLines is called, but nothing is drawn in scene or game
        //RenderPipelineManager.beginCameraRendering += DoRenderLines;

        // VERY HOPEFUL
        RenderPipelineManager.endCameraRendering += DoRenderLines;

        // When I uncomment(only) this line, DoRenderLines is called, but nothing is drawn in scene or game
        //RenderPipelineManager.beginFrameRendering += DoRenderLines;

        // When I uncomment(only) this line, bug + cant see in headset, but can see in game view
        //RenderPipelineManager.endFrameRendering += DoRenderLines;
    }

    // Unity calls this method automatically when it disables this component
    private void OnDisable()
    {
        //RenderPipelineManager.beginCameraRendering -= DoRenderLines;

        // VERY HOPEFUL
        RenderPipelineManager.endCameraRendering -= DoRenderLines;

        //RenderPipelineManager.beginFrameRendering -= DoRenderLines;

        // When I uncomment(only) this line, bug + cant see in headset, but can see in game view
        //RenderPipelineManager.endFrameRendering -= DoRenderLines;
    }

    void DoRenderLines(ScriptableRenderContext context, Camera camera)
    {
        RenderLines(lines);
    }

    // When I uncomment (only) this function, it draws multiple instances of the same line in local space on all objects
    // in both scene and game (regardless of if my VR headset is connected)
    //void OnRenderObject()
    //{
    //    RenderLines(lines);
    //}

    // When I uncomment (only) this function,
    // (if my VR headset is not connected) it draws properly in scene and game
    // (if my VR headset is connected) it draws properly in scene and doesnt draw anything in game
    //void OnDrawGizmos()
    //{
    //    RenderLines(lines);
    //}


    // When I uncomment (only) this function, nothing draws (in scene and in game)
    // this function is never hit when I breakpoint.
    //private void OnPostRender()
    //{
    //    RenderLines(lines);
    //}

    // When I uncomment (only) this function, nothing draws (in scene and in game)
    // this function is never hit when I breakpoint.
    //private void OnPreRender()
    //{
    //    RenderLines(lines);
    //}

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


