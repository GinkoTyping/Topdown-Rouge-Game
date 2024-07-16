using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Utilities
{
    public class Timer
    {
        public event Action OnTimerDone;

        public bool isActive;
        public float restTime;

        private float startTime;
        private float duration;
        private float targetTime;

        public Timer(float duration)
        {
            this.duration = duration;
            isActive = false;
        }

        public void StartTimer() {
            startTime = Time.time;
            restTime = duration;
            targetTime = startTime + duration;
            isActive = true;
        }

        public void StopTimer()
        {
            isActive = false;
        }

        public void Tick()
        {
            if (!isActive)
            {
                return;
            }
            
            if (Time.time >= targetTime) {
                OnTimerDone?.Invoke();
                StopTimer();
            } else
            {
                restTime = targetTime - Time.time;
            }
        }
    }
}
