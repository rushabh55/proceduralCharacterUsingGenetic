using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using GA;

namespace GA
{
    class Chromosome
    {
        public int[] w = new int[6];
        public float[] distance = new float[6];
    }
}

public class Gen : MonoBehaviour {
    System.Random rand = new System.Random();
	public GameObject camera;
    GameObject 
		head,
        body,
        arm,
        arm1,
        leg, 
        leg1;

    int add_count, chromo_count = 0;

    public GUIStyle _guiStyle;   

    List<Chromosome> population = new List<Chromosome>();

    public List<GameObject> _currentObjects = new List<GameObject>();

    int breakForce = 100;

	// Use this for initialization
	void Start () {
        Chromosome chromosome = new Chromosome();
		float x=(float)rand.NextDouble();
		float y=(float)rand.NextDouble();
		float z=(float)rand.NextDouble();
		
		head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		head.transform.position = new Vector3(16.16f,1.2f,5f);
		head.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		head.gameObject.AddComponent("Rigidbody");
		
		x=(float)rand.NextDouble();
		y=(float)rand.NextDouble();
		z=(float)rand.NextDouble();
		
		body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //body.transform.position = new Vector3(16.11f, 0.7F, 5f);
        //body.transform.localScale = new Vector3(1,1,1);
		body.gameObject.AddComponent("Rigidbody");
		
		x=(float)rand.NextDouble();
		y=(float)rand.NextDouble();
		z=(float)rand.NextDouble();

        arm = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //arm.transform.position = new Vector3(15.6f, 0.7F, 5f);
        //arm.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		arm.gameObject.AddComponent("Rigidbody");

        arm1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //arm1.transform.position = new Vector3(16.6f, 0.7F, 5f);
        //arm1.transform.localScale = new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		arm1.gameObject.AddComponent("Rigidbody");
		
		x=(float)rand.NextDouble();
		y=(float)rand.NextDouble();
		z=(float)rand.NextDouble();

        leg = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //leg.transform.position = new Vector3(16.6f, 0.0F, 5f);
        //leg.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		leg.gameObject.AddComponent("Rigidbody");


        leg1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //leg1.transform.position = new Vector3(15.6f, 0.0F, 5f);
        //leg1.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		leg1.gameObject.AddComponent("Rigidbody");	
		
		head.AddComponent<HingeJoint>().connectedBody = body.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = leg.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = leg1.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = arm.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = arm1.GetComponent<Rigidbody>();

   

		head.rigidbody.freezeRotation = true;
		body.rigidbody.freezeRotation = true; 
		body.gameObject.name = "body";

        _currentObjects.Add(body);
        _currentObjects.Add(head);
        _currentObjects.Add(arm);
        _currentObjects.Add(arm1);
        _currentObjects.Add(leg);
        _currentObjects.Add(leg1);
	}
	
	// Update is called once per frame
	void Update () {
		camera.transform.LookAt(body.transform.position);
        if (camera.GetComponent<SmoothFollow>().target == null)
        {
            camera.GetComponent<SmoothFollow>().target = body.transform;
        }

        foreach (var t in body.GetComponents<HingeJoint>())
        {
            t.breakTorque = breakForce;
            t.breakForce = breakForce;
        }
	}

    void OnGUI()
    {
        
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Reset", _guiStyle))
        {
            Reset();          
        }
        GUI.Label(new Rect(Screen.width - 100, 0, 100, 20), "The MAX Break Force");
        breakForce = System.Convert.ToInt32(GUI.TextArea(new Rect(Screen.width - 100, 20, 100, 50), breakForce.ToString()));
    }

    void Reset()
    {
        foreach (var obj in _currentObjects)
            GameObject.Destroy(obj);

        _currentObjects = new List<GameObject>();
        Start();
    }

    void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint Break " + breakForce);
        Reset();
    }
}
