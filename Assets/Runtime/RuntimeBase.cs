using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DefaultNamespace
{
    public abstract class RuntimeBase : MonoBehaviour
    {
        [SerializeField] private int _capacity;
        [SerializeField] private int _acitveSeconds;

        protected Stopwatch Stopwatch { get; private set; }
        protected int Capacity => _capacity;//
        protected int ActiveMilliseconds => _acitveSeconds * 1000;
        protected CancellationToken CancellationToken => destroyCancellationToken;
        protected bool IsDone => Stopwatch.ElapsedTicks / 10000f >= ActiveMilliseconds;//

        protected abstract void Execute();

        private void Awake()
        {
            Stopwatch = Stopwatch.StartNew();
            Execute();
            
            Debug.Log($"{GetType().Name} STARTUP: {Stopwatch.ElapsedTicks / 10000f}");
        }
    }
}