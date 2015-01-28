using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace BehaviourTree
{
	#region BotBehaviour (base class)
	//Base class for all behaviour tree components
	public abstract class BotBehaviour
	{
		public abstract bool Execute(GameObject agent);
	}
	#endregion

	#region Composite
	//Composite nodes are composed of multiple behaviours
	public abstract class Composite : BotBehaviour
	{
		protected List<BotBehaviour> _children = new List<BotBehaviour>();

		public void AddChild(BotBehaviour behaviour)
		{
			_children.Add(behaviour);
		}
		public void AddChildren(params BotBehaviour[] behaviours)
		{
			foreach(BotBehaviour behaviour in behaviours)
			{
				_children.Add (behaviour);
			}
		}
	}
	#endregion

	#region Selector
	//Selectors execute left to right (or first to last) , if any child succeeds, it returns
	//success to the parent node, if they all fail, it returns failure to the parent node.
	public class Selector : Composite
	{
		//Just an idea using the params keyword, perhaps we can have overloaded constructors
		//on the actual defined node types to eliminate the need to call AddChild(ren)()
		public Selector(params BotBehaviour[] behaviours)
		{
			foreach(BotBehaviour behaviour in behaviours)
			{
				_children.Add (behaviour);
			}
		}

		public override bool Execute(GameObject agent)
		{
			foreach(BotBehaviour behaviour in _children)
			{
				if(behaviour.Execute (agent))
				{
					return true;
				}
			}
			return false;
		}
	}
	#endregion

	#region Sequence
	//Sequences are the inverse of a Selector, to succeed, all children must succeed
	//if any fail they all fail
	public class Sequence : Composite
	{
		public Sequence(params BotBehaviour[] behaviours)
		{
			foreach(BotBehaviour behaviour in behaviours)
			{
				_children.Add (behaviour);
			}
		}

		public override bool Execute(GameObject agent)
		{
			foreach(BotBehaviour behaviour in _children)
			{
				if(!behaviour.Execute (agent))
				{
					return false;
				}
			}
			return true;
		}
	}
	#endregion

	#region Seek
	public class Seek : BotBehaviour
	{
		private Transform _target;
		private float _speed = 5F;
		private float _arriveDistance = .9F;
		
		public Seek(Transform target)
		{
			_target = target;
		}
		
		public override bool Execute(GameObject agent)
		{
			Rigidbody rb = agent.GetComponent<Rigidbody>();
			
			if(Vector3.Distance (agent.transform.position, _target.position) <= _arriveDistance)
			{
				rb.velocity = Vector3.zero;
				return false;
			}

			Vector3 targetDirection = Vector3.Normalize (_target.position - agent.transform.position);
			Vector3 desiredVelocity = targetDirection * _speed;	

			agent.transform.LookAt (targetDirection);
			rb.AddForce (desiredVelocity - rb.velocity);
			
			return true;
		}
	}
	#endregion
	
	#region Flee
	public class Flee : BotBehaviour
	{
		private Transform _target;
		private float _speed = 5F;
		private float _panicDistance = 10F;

		
		public Flee(Transform target)
		{
			_target = target;
		}
		
		public override bool Execute(GameObject agent)
		{
			Rigidbody rb = agent.GetComponent<Rigidbody>();
			
			if(Vector3.Distance (agent.transform.position, _target.position) >= _panicDistance)
			{
				rb.velocity = Vector3.zero;
				return false;
			}
			
			Vector3 targetDirection = Vector3.Normalize (agent.transform.position - _target.position );
			Vector3 desiredVelocity = targetDirection * _speed;
		
			agent.transform.LookAt (targetDirection);
			rb.AddForce (desiredVelocity - rb.velocity);

			return true;
		}
	}
	#endregion

	#region AvoidObstacles

	public class AvoidObstacles : BotBehaviour
	{
		private float _rayLength = 10F;

		public override bool Execute(GameObject agent)
		{
			RaycastHit hit;
			if(Physics.Raycast (agent.transform.position, agent.transform.forward))
			return true;
		}
	}

	#endregion
}

