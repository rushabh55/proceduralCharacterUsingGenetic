﻿using UnityEngine;
using System.Collections;
using GA;

public class GUIStuff : MonoBehaviour {
    public GUIStyle _style;
    public Rect BoxLocation;
    public Rect _startButton;
    public Rect genCount;
    public Rect _genRect;
    public Rect breakForce;
    public Rect _breakRect;
    public Rect _resetRect;
    public int totalGenerations = 0;
    public int maxBreakForce = 0;
    public Rect _dump;

	// Use this for initialization
    void Start()
    {
        Time.timeScale = 0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    private Vector2 scrollPosition;
    void OnGUI()
    {
        GUI.Box(BoxLocation, "GA Charachter");
        if (GUI.Button(_startButton, "Start/Stop Simulation"))
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;
        }

        if (GUI.Button(_resetRect, "Reset"))
        {

        }

        totalGenerations = System.Convert.ToInt32(GUI.TextArea(genCount, totalGenerations.ToString()));
        maxBreakForce = System.Convert.ToInt32(GUI.TextArea(breakForce, maxBreakForce.ToString()));

        GUI.Label(_genRect, "Total generations");
        GUI.Label(_breakRect, "Joint break force");

        var t =  GeneticComputations.var_dump() ;
        if(t == null || t == "")
            GUI.Label(_dump, "" );
        

        var objs = GameObject.FindObjectsOfType(typeof(GameObject));
        GUI.Label(new Rect(0, 0, 225, 50), "Total Objects: " + objs.Length);
    }
}