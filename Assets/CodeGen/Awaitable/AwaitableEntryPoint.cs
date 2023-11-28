using System.Threading;
using UnityEngine;

public class AwaitableEntryPoint : MonoBehaviour
{
    private void Start()
    {
        var awaitableGenerated = new AwaitableGenerated();

        Forget(awaitableGenerated.Method250(CancellationToken.None));
    }
    
    private async void Forget(Awaitable<int> task)
    {
        await task;
    }
}
