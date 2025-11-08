using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Timer
{
    public class SimpleTimer : MonoBehaviour
    {
        private float timerChecker, timerDuration, timerStartTime;
        private bool timerActive = false;
        public event Action OnTimerEndedEvent;
        public event Action OnTimerStartedEvent;
        public event Action<float> OnTimerUpdatingEvent;
        public event Action OnTimerStoppedEvent;
        public event Action OnTimerContinueEvent;

        public float TimerDuration => timerDuration;

        public void StartTimer(float time)
        {
            timerChecker = time;
            timerActive = true;
            timerDuration = 0;

            timerStartTime = time;
            OnTimerStartedEvent?.Invoke();
        }

        private void Update()
        {
            if (!timerActive) return;
            timerChecker -= Time.deltaTime;
            timerDuration += Time.deltaTime;
            OnTimerUpdatingEvent?.Invoke(timerChecker);
            if (timerChecker <= 0f)
            {
                timerActive = false;
                OnTimerEndedEvent?.Invoke();
            }
        }

        public void StopTimer()
        {
            timerActive = false;
            OnTimerStoppedEvent?.Invoke();
        }

        public void ContinueTimer()
        {
            timerActive = true;
            OnTimerContinueEvent?.Invoke();
        }

        public void ForwardTimer(float forwardAmount)
        {
            var newTimerCheckerAmount = timerChecker - forwardAmount;
            timerChecker = newTimerCheckerAmount < 0f ? 0f : newTimerCheckerAmount;

            var newTimerDurationAmount = timerDuration + forwardAmount;
            timerDuration = newTimerDurationAmount > timerStartTime ? timerStartTime : newTimerDurationAmount;
            OnTimerUpdatingEvent?.Invoke(timerChecker);
        }

        public float GetTimerRate()
        {
            return Mathf.Clamp01(timerDuration / timerStartTime);
        }
    }
}