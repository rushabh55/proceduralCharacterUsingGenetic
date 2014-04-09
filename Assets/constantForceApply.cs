using UnityEngine;
using System.Collections;

public class constantForceApply : MonoBehaviour {
    public GameObject[] _obj = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void applyForce(float z)
    {
        foreach (var t in _obj)
        {
            var w = t.GetComponent<ParticleEmitter>();
            if(w != null)
            {
                var vel = w.localVelocity;
                vel.z = z;
                w.localVelocity = vel;
            }
        }
    }
}
