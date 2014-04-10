using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using GA;



public class Gen : MonoBehaviour {
    GUIStuff _stuff = new GUIStuff();

    System.Random rand = new System.Random();

	public GameObject camera = null;  
    
    private static List<GameObject> _currentObjects = new List<GameObject>();

    public GameObject model;

	public GameObject charachter = null;

    private int totalJoints = 0;

    public int _totalGenerations = 0;

    GeneticComputations c = null;

    private GameObject body = null;

    public GameObject _engine;

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
    void Start()
    {
        shader1 = Shader.Find("Diffuse");
    }

    void Awake()
    {
        if (!body)
            body = GameObject.Find("BODY");
        totalJoints = GameObject.FindGameObjectsWithTag("body").Length;
    }

    void StartDoingStuff(Vector3 point)
    {
        _stuff = charachter.GetComponent<GUIStuff>();
        _totalGenerations = _stuff.totalGenerations;
        totalJoints++;
        GameObject obj = null;
        if (body != null)
            obj = (GameObject)GameObject.Instantiate(_engine);
        else
            obj = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Cube);

        obj.transform.position = point;
        obj.gameObject.AddComponent("Rigidbody");
        obj.gameObject.tag = "body";


        try
        {
            c = new GeneticComputations(_totalGenerations, totalJoints);

        }
        catch (System.Exception e_)
        {
            Debug.Log(e_.Message);
        }

        if (_currentObjects.Count >= 1 && body != null)
        {
            body.AddComponent<HingeJoint>().connectedBody = obj.GetComponent<Rigidbody>();
            obj.gameObject.name = "Engine " + totalJoints;
            obj.rigidbody.velocity = Vector3.zero;
            ReassignAll();
        }
        else
        {
            body = obj;
            body.rigidbody.velocity = Vector3.zero;
            body.gameObject.name = "BODY";
        }

        obj.rigidbody.freezeRotation = true;
        _currentObjects.Add(obj);
        obj.AddComponent("Joints");

        if (body)
            cam.transform.LookAt(body.transform);
        if (body)
            if (camera.GetComponent<SmoothFollow>().target == null)
            {
                camera.GetComponent<SmoothFollow>().target = body.transform;
            }
    }

    private void ReassignAll()
    {
        double mass = 0;

        for (int i = 0; i < GeneticComputations.bestFit.mass.Count; i++)
        {
            mass += (float)GeneticComputations.bestFit.mass[i];
            try
            {
                _currentObjects[i].GetComponent<ConstantForce>().force = new Vector3(0, 0, 18 * (float)GeneticComputations.bestFit.force[i]);
            }
            catch (System.Exception) { }
        }
        Debug.Log("MASS = " + mass);
        body.rigidbody.mass = (float)mass * 10;
    }
	
	// Update is called once per frame
	void Update () {
        if (body)
            foreach (var t in body.GetComponents<FixedJoint>())
            {
                t.breakForce = _stuff.maxBreakForce;
            }

        foreach (var obj in _currentObjects)
        {
            if (obj.transform.position.y < -200)
                Reset();
        }
	}

    void LateUpdate()
    {
        if (_stuff.contains)
            return;
		GameObject body = this.body ? this.body : this.gameObject;
        if (Input.mousePresent)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit rayHit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out rayHit))
                    {

                        if (Vector3.Distance(body.transform.position, rayHit.point) < 4 || _currentObjects.Count < 1)
                        {
                            var t2 = rayHit.point;
                            t2.y += 0.5f;
                            AddNewJoint(t2);
                        }
                        else
                        {
                            AddObstacle(rayHit);
                        }
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
                    if (Vector3.Distance(body.transform.position, rayHit.point) < 4)
                    {
                        var t2 = rayHit.point;
                        t2.y += 0.5f;
                        AddNewJoint(t2);
                    }
                    else
                    {
                        AddObstacle(rayHit);
                    }
                }
            }
        }
    }

    private void AddObstacle(RaycastHit rayHit)
    {
        var obj = GameObject.CreatePrimitive((PrimitiveType)_obstacleType);
        obj.transform.position = rayHit.point;
        var t2 = obj.transform.position;
        t2.y += .5f;
        obj.transform.position = t2;
        obj.transform.rotation = transform.rotation;
        obj.AddComponent("Rigidbody");
        obj.rigidbody.mass = 100;
        obj.renderer.material.shader = shader1;
        obj.renderer.material.mainTexture = _tex;
        obj.gameObject.tag = "Destroy";
    }

    void AddNewJoint(Vector3 point)
    {
        StartDoingStuff(point);
    }
    
    public void Reset()
    {
        foreach (var obj in _currentObjects)
            GameObject.Destroy(obj);

        foreach (var obj in GameObject.FindGameObjectsWithTag("Destroy"))
            GameObject.Destroy(obj);

        foreach (var obj in GameObject.FindGameObjectsWithTag("body"))
            GameObject.Destroy(obj);

        _currentObjects = new List<GameObject>();
		body = null;
        Start();
    }

    void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint Break " + breakForce);
        Reset();
    }
}
