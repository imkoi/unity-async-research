using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ValueTaskEntryPoint : MonoBehaviour
{
    private void Start()
    {
        var valueTaskGenerated = new ValueTaskGenerated();

        Forget(valueTaskGenerated.Method250(CancellationToken.None));
    }

    private async void Forget(ValueTask<int> task)
    {
        await task;
    }
}
