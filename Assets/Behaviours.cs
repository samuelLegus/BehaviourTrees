using UnityEngine;
using System.Collections;

namespace BehaviourTree
{
	namespace Behaviours
	{
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
				
				Vector3 desiredVelocity = Vector3.Normalize (_target.position - agent.transform.position) * _speed;
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
				
				Vector3 desiredVelocity = Vector3.Normalize (agent.transform.position - _target.position ) * _speed;
				rb.AddForce (desiredVelocity - rb.velocity);
				
				return true;
			}
		}
		#endregion
	}
}
