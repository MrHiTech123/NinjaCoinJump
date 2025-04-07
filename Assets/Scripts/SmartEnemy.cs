using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmartEnemy : MonoBehaviour
{
	public Transform target;
	private NavMeshAgent agent;
    // Start is called before the first frame update
	
	private static void initializeAgent(NavMeshAgent agent) {
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		agent.enabled = true;
	}
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
		agent = GetComponent<NavMeshAgent>();
		
		initializeAgent(agent);
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
}
