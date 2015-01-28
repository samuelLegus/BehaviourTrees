using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;

public class DefensiveAI : MonoBehaviour 
{
	public Transform _Target;
	
	Selector _root;
	
	// Use this for initialization
	void Start ()
	{
	
		_root = new Selector( new Flee(_Target));
	}
	
	// Update is called once per frame
	void Update ()
	{
		_root.Execute (gameObject);
	}
}
