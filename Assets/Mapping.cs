using UnityEngine;
using System.Collections;

public class Mapping : MonoBehaviour {
    public enum obstacleType
    {
        Cube = PrimitiveType.Cube,
        Cylinder = PrimitiveType.Cylinder,
        Capsule = PrimitiveType.Capsule,
        Sphere = PrimitiveType.Sphere,
        Quad = PrimitiveType.Quad
    }
    public Camera cam;
    public obstacleType _obstacleType;
    public Texture _tex;
    Shader shader1;
	// Use this for initialization
	void Start () {
        shader1 = Shader.Find("Diffuse");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.mousePresent )
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit rayHit;                     
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out rayHit))
                    {

                        var obj = GameObject.CreatePrimitive((PrimitiveType) _obstacleType);
                        obj.transform.position = rayHit.point;
                        obj.transform.rotation = transform.rotation;
                        obj.AddComponent("Rigidbody");
                        obj.renderer.material.shader = shader1;
                        obj.renderer.material.mainTexture = _tex;
                    }
                }
            }
        }

        if (Input.touchCount > 0)
        {
            foreach (var t in Input.touches)
            {
                RaycastHit rayHit;
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out rayHit))
                {

                    var obj = GameObject.CreatePrimitive((PrimitiveType)_obstacleType);
                    obj.transform.position = rayHit.point;
                    obj.transform.rotation = transform.rotation;
                    obj.AddComponent("Rigidbody");
                    obj.renderer.material.shader = shader1;
                    obj.renderer.material.mainTexture = _tex;
                }
            }
        }
	}

    void OnGUI()
    {
        var objs = GameObject.FindObjectsOfType(typeof(GameObject));
        GUI.Label(new Rect(0, 0, 225, 50), "Total Objects: " + objs.Length);
    }

}
