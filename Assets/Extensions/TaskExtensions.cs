using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public static class TaskExtensions
    {
        public static async void Forget(this Awaitable awaitable)
        {
            await awaitable;
        }
        
        public static async void Forget(this UniTask awaitable)
        {
            await awaitable;
        }
        
        public static async void Forget(this Task awaitable)
        {
            await awaitable;
        }
        
        public static async void Forget(this ValueTask valueTask)
        {
            await valueTask;
        }
    }
}