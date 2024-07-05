using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame.Scripts.IdleGame.BehaviourTree.Actions
{
    [Name("Seek")]
    [Category("Movement/Pathfinding")]
    public class MoveToTarget : ActionTask<NavMeshAgent>
    {
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 4;
        public BBParameter<float> keepDistance = 0.1f;
        public bool waitActionFinish = true;

        private Vector3? lastRequest;

        protected override string info
        {
            get { return "Seek " + target; }
        }

        protected override void OnExecute()
        {
            if (target.value == null) { EndAction(false); return; }
            agent.speed = speed.value;
            if (Vector3.Distance(agent.transform.position, target.value.transform.position) <= keepDistance.value)
            {
                EndAction(true);
                return;
            }
        }

        protected override void OnUpdate()
        {
            if (target.value == null) { EndAction(false); return; }
            agent.speed = speed.value;
            var pos = target.value.transform.position;
            if (lastRequest != pos)
            {
                agent.destination = pos;
            }

            lastRequest = pos;

            if (Vector3.Distance(agent.transform.position, target.value.transform.position) <= keepDistance.value)
            {
                EndAction(true);
            }

            if (!waitActionFinish)
            {
                EndAction(false);
            }
        }

        protected override void OnPause() { OnStop(); }
        protected override void OnStop()
        {
            //if (agent.gameObject.activeSelf)
            //{
            //    agent.CalculatePath();
            //}
            lastRequest = null;
        }

        public override void OnDrawGizmosSelected()
        {
            if (target.value != null)
            {
                Gizmos.DrawWireSphere(target.value.transform.position, keepDistance.value);
            }
        }
    }
}
