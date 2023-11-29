using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UniTaskEntryPoint : MonoBehaviour
{
    private void Start()
    {
        var uniTaskGenerated = new UniTaskGenerated();

        uniTaskGenerated.Method500(CancellationToken.None).Forget();
    }
}
