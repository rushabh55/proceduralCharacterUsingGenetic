using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using GA;


    class Joints : MonoBehaviour 
	{
        public Gen _gen;
        void Update()
        {
            this.rigidbody.mass -= 0.001f;
            if (this.rigidbody.mass < 0.01)
            {
                if(this.constantForce != null)
                    this.constantForce.force = Vector3.zero;
                GameObject.Destroy(this);
                Debug.Log("Destroyed");
                this.rigidbody.mass = 1;
                //_gen.Reset();
            }
        }

        void OnJointBreak(float breakForce)
        {
            if (_gen == null)
            {
                _gen = GameObject.Find("Charachter").GetComponent<Gen>();
            }
            Debug.Log("Joint Break " + breakForce);
            _gen.Reset();
        }
	}
