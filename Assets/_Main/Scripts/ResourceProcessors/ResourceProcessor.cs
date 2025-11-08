using System;
using Scripts.ResourceProcessors;
using Scripts.Timer;
using UnityEngine;

namespace Scripts.ResourceProcessors
{
    [RequireComponent(typeof(SimpleTimer))]
    public class ResourceProcessor : MonoBehaviour
    {
        [SerializeField] private ResourceProcessorVisual resourceProcessorVisual;
        [SerializeField] private float processTime;
        private SimpleTimer timer;
        private bool isProcessorEnabled = false;

        public event Action<float> OnProcessBeginEvent;
        public event Action OnProcessEndEvent, OnProcessStoppedEvent;

        public bool IsProcessorEnabled => isProcessorEnabled;

        private void Awake()
        {
            timer = GetComponent<SimpleTimer>();
        }

        private void OnEnable()
        {
            timer.OnTimerEndedEvent += HandleTimerEnd;
        }

        private void OnDisable()
        {
            timer.OnTimerEndedEvent -= HandleTimerEnd;
        }

        public void HandleTimerEnd()
        {
            resourceProcessorVisual.StopVisualizingSpawning();
            OnProcessEndEvent?.Invoke();
        }

        public void StopProcessing()
        {
            timer.StopTimer();
            isProcessorEnabled = false;
            resourceProcessorVisual.StopVisualizingSpawning();
            OnProcessStoppedEvent?.Invoke();
        }

        public void StartProcessing()
        {
            timer.StartTimer(processTime);
            isProcessorEnabled = true;
            resourceProcessorVisual.StartVisualizingSpawning(processTime);
            OnProcessBeginEvent?.Invoke(processTime);
        }
    }
}