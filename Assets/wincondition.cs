using UnityEngine;
using System.Collections;

public class wincondition : MonoBehaviour {
    public Gen resetScript;
    public GUIStuff _stuff;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) 
    {
        if (string.Equals(collision.gameObject.name, "BODY"))
        {
            _stuff._dumpData = "YOU WON, YOUR CHARACTER IS GOOD ENOUGH!";
			resetScript.Reset();
        }
    }
}
