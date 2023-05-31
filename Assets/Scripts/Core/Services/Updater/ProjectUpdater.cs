using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Updater
{
    public class ProjectUpdater: MonoBehaviour, IProjectUpdater
    {
        public static IProjectUpdater Instance;
        
        private readonly List<Coroutine> _activeInvokers = new List<Coroutine>();
        
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled;
        public event Action LateUpdateCalled;

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

        private void OnDestroy()
        {
            foreach (var element in _activeInvokers.Where(element => element != null))
                StopCoroutine(element);
            
            _activeInvokers.Clear();
        }
        
        Coroutine IProjectUpdater.StartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
        void IProjectUpdater.StopCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);

        public void Invoke(Action action, float time)
        {
            _activeInvokers.Add(StartCoroutine(InvokeCoroutine(action, time)));
        }

        private IEnumerator InvokeCoroutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);
            yield return new WaitUntil(() => !IsPaused);
            action?.Invoke();
            _activeInvokers.RemoveAll(element => element == null);
        }
    }
}