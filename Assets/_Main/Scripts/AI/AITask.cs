using System;
using Scripts.ObjectPools;
using UnityEngine;

namespace Scripts.AI
{
    public abstract class AITask : MonoBehaviour
    {
        protected IAITaskCauser aiTaskCauser;
        private GameObject taskCauser;
        private Vector3 targetPos;

        protected AIController assignedAI;

        public Vector3 TargetPos => targetPos;

        protected virtual void Awake()
        {
            aiTaskCauser = GetComponent<IAITaskCauser>();
            taskCauser = gameObject;
            targetPos = aiTaskCauser.ItemStackArea.transform.position;

            assignedAI = null;
        }

        protected virtual void Start()
        {
            AITasksManager.Instance.AddMeToAllTasks(this);
        }

        public void OverrideAssignedAI(AIController newAssignedAI)
        {
            assignedAI = newAssignedAI;
        }

        public abstract bool ShouldStart();
        public abstract bool ShouldFinish();

    }
}