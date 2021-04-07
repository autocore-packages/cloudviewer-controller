using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLines : MonoBehaviour
{
    //public v
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Material mat;
    private void OnRenderObject()
    {
        mat.SetPass(0);
        GL.Color(Color.black);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Vertex3(0, 0.1f, 0);
        GL.Vertex3(1, 0.1f, 0);
        GL.Vertex3(0.1f, 0, 0);
        GL.Vertex3(1f, 1, 0); 
        GL.End();
        GL.PopMatrix();

    }
}
