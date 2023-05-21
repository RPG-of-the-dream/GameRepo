using System;
using System.Collections;
using UnityEngine;

namespace Core.Services.Updater
{
    public class ProjectUpdater: MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled;
        public event Action LateUpdateCalled;

        Coroutine IProjectUpdater.StartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
        void IProjectUpdater.StopCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);

        private bool _isPaused;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (_isPaused == value)
                    return;
                
                Time.timeScale = value ? 0 : 1;
                _isPaused = value;
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public Coroutine GetCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void KillCoroutine(Coroutine coroutine)
        {
            if(coroutine is not null)
                StopCoroutine(coroutine);
        }

        private void Update()
        {
            if(IsPaused)
                return;
            
            UpdateCalled?.Invoke();
        }
        

        private void FixedUpdate()
        {
            if(IsPaused)
                return;

            FixedUpdateCalled?.Invoke();
        }

        private void LateUpdate()
        {
            if(IsPaused)
                return;

            LateUpdateCalled?.Invoke();
        }
    }
}