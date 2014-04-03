using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {
        var chromosome = new float[18, 18];
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
		
		chromosome[add_count,chromo_count]=x;
		chromosome[add_count,++chromo_count]=y;
		chromosome[add_count,++chromo_count]=z;
		
		
		body = GameObject.CreatePrimitive(PrimitiveType.Cube);
		body.transform.position = new Vector3(16.11f, 0.7F, 5f);
		body.transform.localScale = new Vector3(1,1,1);
		body.gameObject.AddComponent("Rigidbody");
		
		x=(float)rand.NextDouble();
		y=(float)rand.NextDouble();
		z=(float)rand.NextDouble();
		
		chromosome[add_count,chromo_count]=x;
		chromosome[add_count,++chromo_count]=y;
		chromosome[add_count,++chromo_count]=z;
		
		arm = GameObject.CreatePrimitive(PrimitiveType.Cube);
		arm.transform.position = new Vector3(15.6f, 0.7F, 5f);
		arm.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		arm.gameObject.AddComponent("Rigidbody");
		
		arm1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		arm1.transform.position = new Vector3(16.6f, 0.7F, 5f);
		arm1.transform.localScale = new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		arm1.gameObject.AddComponent("Rigidbody");
		
		x=(float)rand.NextDouble();
		y=(float)rand.NextDouble();
		z=(float)rand.NextDouble();
		
		chromosome[add_count,chromo_count]=x;
		chromosome[add_count,++chromo_count]=y;
		chromosome[add_count,++chromo_count]=z;
		
		leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
		leg.transform.position = new Vector3(16.6f, 0.0F, 5f);
		leg.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		leg.gameObject.AddComponent("Rigidbody");
		
		
		leg1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		leg1.transform.position = new Vector3(15.6f, 0.0F, 5f);
		leg1.transform.localScale =  new Vector3(1 * (float)x,1 * (float) y,1 * (float)z);
		leg1.gameObject.AddComponent("Rigidbody");	
		
		head.AddComponent<FixedJoint>().connectedBody = body.GetComponent<Rigidbody>();
		body.AddComponent<FixedJoint>().connectedBody = leg.GetComponent<Rigidbody>();
		body.AddComponent<FixedJoint>().connectedBody = leg1.GetComponent<Rigidbody>();
		body.AddComponent<FixedJoint>().connectedBody = arm.GetComponent<Rigidbody>();
		body.AddComponent<FixedJoint>().connectedBody = arm1.GetComponent<Rigidbody>();

		head.rigidbody.freezeRotation = true;
		body.rigidbody.freezeRotation = true;

		body.AddComponent("CharacterMotor");
		//head.rigidbody.useGravity = false;
		//body.rigidbody.useGravity = false;
		//arm.rigidbody.useGravity = false;
		//arm1.rigidbody.useGravity = false;
		//leg.rigidbody.useGravity = false;
		//leg1.rigidbody.useGravity = false;

		body.gameObject.name = "body";
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (chromo_count);
		camera.transform.LookAt(body.transform.position);
	}
}
