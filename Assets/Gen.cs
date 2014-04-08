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

	void StartDoingStuff (GameObject obj) {
		if(obj == null)
			return;
		_stuff = charachter.GetComponent<GUIStuff>();
		_totalGenerations = _stuff.totalGenerations;
        totalJoints++;
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
            var t2 = obj.transform.position;
            t2.y -= 50;
            obj.transform.position = t2;
            body.AddComponent<FixedJoint>().connectedBody = obj.GetComponent<Rigidbody>();
            obj.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[totalJoints - 1], (float)GeneticComputations.bestFit.scale[totalJoints - 1], (float)GeneticComputations.bestFit.scale[totalJoints - 1]);
            var d = GeneticComputations.bestFit.position[totalJoints - 1];
            obj.transform.position = new Vector3(obj.transform.position.x * (float)d, obj.transform.position.y * (float)d, obj.transform.position.z * (float)d);
			obj.gameObject.name = "obj" + totalJoints;
        }
        else
        {
            body = obj;
            body.gameObject.name = "BODY";
            body.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[0], (float)GeneticComputations.bestFit.scale[0], (float)GeneticComputations.bestFit.scale[0]);       		
		}

		obj.rigidbody.freezeRotation = true;
		_currentObjects.Add(obj);
        obj.AddComponent("Joints");        
	}
	
	// Update is called once per frame
	void Update () {
        if(body)
            if (camera.GetComponent<SmoothFollow>().target == null)
            {
                camera.GetComponent<SmoothFollow>().target = body.transform;
            }

        if (body)
            foreach (var t in body.GetComponents<HingeJoint>())
            {
                t.breakTorque = _stuff.maxBreakForce;
                t.breakForce = _stuff.maxBreakForce;
            }

        foreach (var obj in _currentObjects)
        {
            if (obj.transform.position.y < -100)
                obj.renderer.enabled = false;
        }
	}

    void LateUpdate()
    {
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
                            t2.y += 50;
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
                        t2.y += 50;
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
    }

    void AddNewJoint(Vector3 point)
    {
        var temp = (GameObject)GameObject.CreatePrimitive(PrimitiveType
		                                                  .Cube);
        temp.renderer.material.SetColor("1", Color.red);
		temp.transform.position = point;
        temp.gameObject.AddComponent("Rigidbody");
        StartDoingStuff(temp);
    }
    
    public void Reset()
    {
        foreach (var obj in _currentObjects)
            GameObject.Destroy(obj);

        _currentObjects = new List<GameObject>();
        StartDoingStuff(null);
    }

    void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint Break " + breakForce);
        Reset();
    }
}
