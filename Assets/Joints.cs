using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using GA;


    class Joints : MonoBehaviour 
	{
        public Gen _gen;
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
