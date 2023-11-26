using Cysharp.Threading.Tasks;
using DefaultNamespace;

public class UniTaskExample : ExampleBase
{
    protected override void Execute()
    {
        for (var i = 0; i < Capacity; i++)
        {
            TaskExtensions.Forget(Delay());
        }
    }

    private async UniTask Delay()
    {
        while (!IsDone && !CancellationToken.IsCancellationRequested)
        {
            await UniTask.Yield();
        }
    }
}
