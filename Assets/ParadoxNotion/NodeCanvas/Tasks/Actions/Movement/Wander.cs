using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions{

	[Category("Movement")]
	[Description("Makes the agent wander randomly within the navigation map")]
	public class Wander : ActionTask<NavMeshAgent> {

		public BBParameter<float> Speed = 4;
		public BBParameter<float> StoppingDistance = 0.1f;
		public BBParameter<float> MinWanderDistance = 5;
		public BBParameter<float> MaxWanderDistance = 20;
        public bool Repeat = true;

		protected override void OnExecute(){
			agent.speed = Speed.value;
			agent.stoppingDistance = StoppingDistance.value;
			DoWander();
		}
		protected override void OnUpdate(){
			if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance){
				if (Repeat){
					DoWander();
				} else {
					EndAction();
				}
			}
		}

		void DoWander(){
			MinWanderDistance.value = Mathf.Min(MinWanderDistance.value, MaxWanderDistance.value);
			var wanderPos = (Random.insideUnitSphere * MaxWanderDistance.value) + agent.transform.position;
			while ( (wanderPos - agent.transform.position).sqrMagnitude < MinWanderDistance.value )
				wanderPos = (Random.insideUnitSphere * MaxWanderDistance.value) + agent.transform.position;

			agent.SetDestination(wanderPos);
		}

		protected override void OnPause(){ OnStop(); }
		protected override void OnStop(){
			if (agent.gameObject.activeSelf)
				agent.ResetPath();
		}
	}
}