using System.Threading.Tasks;
using DefaultNamespace;

public class TaskRuntime : RuntimeBase
{
    protected override void Execute()
    {
        for (var i = 0; i < Capacity; i++)
        {
            Delay().Forget();
        }
    }

    private async Task Delay()
    {
        while (!IsDone && !CancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
        }
    }
}
