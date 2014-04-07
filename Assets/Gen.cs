using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using GA;



public class Gen : MonoBehaviour {
    System.Random rand = new System.Random();
	public GameObject camera = null;
    GameObject 		
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
        try
        {
			GeneticComputations c = null;
			try
			{
			c = new GeneticComputations();
			}
			catch(System.Exception) {}
            c.var_dump();
        }
        catch (System.Exception e_)
        {
            Debug.Log(e_.Message);
        }
		body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.transform.position = new Vector3((float)GeneticComputations.bestFit.position[0], (float)GeneticComputations.bestFit.position[0], (float)GeneticComputations.bestFit.position[0]);
        body.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[0], (float)GeneticComputations.bestFit.scale[0], (float)GeneticComputations.bestFit.scale[0]);
		body.gameObject.AddComponent("Rigidbody");

        arm = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        arm.transform.position = new Vector3((float)GeneticComputations.bestFit.position[1], (float)GeneticComputations.bestFit.position[1], (float)GeneticComputations.bestFit.position[1]);
        arm.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[1], (float)GeneticComputations.bestFit.scale[1], (float)GeneticComputations.bestFit.scale[1]);
		arm.gameObject.AddComponent("Rigidbody");

        arm1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        arm1.transform.position = new Vector3((float)GeneticComputations.bestFit.position[2], (float)GeneticComputations.bestFit.position[2], (float)GeneticComputations.bestFit.position[2]);
        arm1.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[2], (float)GeneticComputations.bestFit.scale[2], (float)GeneticComputations.bestFit.scale[2]);
		arm1.gameObject.AddComponent("Rigidbody");

        leg = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        leg.transform.position = new Vector3((float)GeneticComputations.bestFit.position[3], (float)GeneticComputations.bestFit.position[3], (float)GeneticComputations.bestFit.position[3]);
        leg.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[3], (float)GeneticComputations.bestFit.scale[3], (float)GeneticComputations.bestFit.scale[3]);
		leg.gameObject.AddComponent("Rigidbody");


        leg1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        leg1.transform.position = new Vector3((float)GeneticComputations.bestFit.position[4], (float)GeneticComputations.bestFit.position[4], (float)GeneticComputations.bestFit.position[4]);
        leg1.transform.localScale = new Vector3((float)GeneticComputations.bestFit.scale[4], (float)GeneticComputations.bestFit.scale[4], (float)GeneticComputations.bestFit.scale[4]);
		leg1.gameObject.AddComponent("Rigidbody");	
		
		body.AddComponent<HingeJoint>().connectedBody = leg.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = leg1.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = arm.GetComponent<Rigidbody>();
        body.AddComponent<HingeJoint>().connectedBody = arm1.GetComponent<Rigidbody>();   

		body.rigidbody.freezeRotation = true; 
		body.gameObject.name = "body";
		arm.gameObject.name 
			= "arm";
		arm1.gameObject.name = "arm1";
		leg.gameObject.name = "arm1";
		leg1.gameObject.name = "leg1";
        _currentObjects.Add(body);
        _currentObjects.Add(arm);
        _currentObjects.Add(arm1);
        _currentObjects.Add(leg);
        _currentObjects.Add(leg1);

        foreach (var obj in _currentObjects)
        {
            obj.AddComponent("Joints");
        }
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
        breakForce = System.Convert.ToInt32(GUI.TextArea(new Rect(Screen.width - 100, 20, 100, 20), breakForce.ToString()));
    }

    public void Reset()
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
