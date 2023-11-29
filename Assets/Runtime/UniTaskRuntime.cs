using Cysharp.Threading.Tasks;
using DefaultNamespace;

public class UniTaskRuntime : RuntimeBase
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
            await UniTask.Yield();
            await UniTask.Yield();
        }
    }
}
