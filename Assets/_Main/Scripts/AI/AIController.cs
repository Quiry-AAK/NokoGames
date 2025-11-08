using System.Collections.Generic;
using Scripts.Tail;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private BackManager backManager;
        [SerializeField] private Animator aiAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float turnSpeed;
        private List<AITask> myTaskChain;

        private AITask currentTask;
        private int currentTaskIndex;

        public BackManager BackManager => backManager;

        private void Awake()
        {
            currentTaskIndex = -1;
            navMeshAgent.updateRotation = false;
        }

        private void Update()
        {
            if (myTaskChain == null)
            {
                LookForAJob();
            }

            else
            {
                if (currentTask == null)
                {
                    GoToNextTask();
                }

                else
                {
                    ProcessTask();
                }
            }

            UpdateRotation();
        }

        private void LookForAJob()
        {
            var taskChain = AITasksManager.Instance.GetMeATaskChain();
            if (taskChain != null)
            {
                myTaskChain = taskChain;
                foreach (var chain in myTaskChain)
                {
                    chain.OverrideAssignedAI(this);
                }
            }
        }

        private void GoToNextTask()
        {
            currentTaskIndex++;
            currentTaskIndex %= myTaskChain.Count;

            if (myTaskChain[currentTaskIndex].ShouldStart())
            {
                currentTask = myTaskChain[currentTaskIndex];
            }

            else
            {
                DoNothing();
            }
        }

        private void ProcessTask()
        {
            aiAnimator.SetBool("Running", true);
            navMeshAgent.SetDestination(currentTask.TargetPos);

            if (currentTask.ShouldFinish())
            {
                GoToNextTask();
            }
        }

        private void DoNothing()
        {
            currentTask = null;
            aiAnimator.SetBool("Running", false);
        }

        private void UpdateRotation()
        {
            if (!navMeshAgent.hasPath) return;

            var direction = (navMeshAgent.steeringTarget - transform.position).normalized;

            var targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}