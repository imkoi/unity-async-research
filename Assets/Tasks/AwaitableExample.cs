using DefaultNamespace;
using UnityEngine;

public class AwaitableExample : ExampleBase
{
    protected override void Execute()
    {
        for (var i = 0; i < Capacity; i++)
        {
            Delay().Forget();
        }
    }
    
    private async Awaitable Delay()
    {
        while (!IsDone && !CancellationToken.IsCancellationRequested)
        {
            await Awaitable.NextFrameAsync();
            await Awaitable.NextFrameAsync();
            await Awaitable.NextFrameAsync();
        }
    }
}
