using System.Collections.Generic;
using System.Linq;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.AI
{
    public class AITasksManager : MonoSingleton<AITasksManager>
    {
        private List<AITask> allTasks;

        private List<List<AITask>> currentTasks;

        protected override void Awake()
        {
            base.Awake();
            allTasks = new List<AITask>();
            currentTasks = new List<List<AITask>>();
        }

        public void AddMeToAllTasks(AITask aiTask)
        {
            allTasks.Add(aiTask);

            for (int i = 0; i < allTasks.Count; i++)
            {
                if (allTasks[i] is AIReceiveTask)
                {
                    UpdateCurrentTasks(allTasks[i] as AIReceiveTask);
                }
            }
        }

        private void UpdateCurrentTasks(AIReceiveTask aiReceiveTask)
        {
            currentTasks.Clear();
            var chainTask = allTasks.Where(x =>
            {
                if (x is AISendTask)
                {
                    var aiSendTask = x as AISendTask;
                    return aiSendTask.GetMySendItemType() == aiReceiveTask.GetMyReceiveItemType();
                }
                return false;
            }).FirstOrDefault();

            var tasks = new List<AITask>();
            if (chainTask != null)
                tasks.Add(chainTask);
            tasks.Add(aiReceiveTask);
            currentTasks.Add(tasks);
        }

        public List<AITask> GetMeATaskChain()
        {
            if (currentTasks.Count == 0)
            {
                return null;
            }
            else
            {
                var chain = currentTasks[0];
                currentTasks.RemoveAt(0);
                return chain;
            }
        }

    }
}