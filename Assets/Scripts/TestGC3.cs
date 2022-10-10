using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestGC3 : MonoBehaviour
{
    public int number;

    private void DoNoAlloc()
    {
        int i = 0;
    }

    public IEnumerator DoCoroutine()
    {
        number = 10;
        while (number-- > 0)
        {
            yield return null;
        }
    }

    private System.Runtime.CompilerServices.YieldAwaitable Task_Yield = Task.Yield();

    public async Task DoTask()
    {
        number = 10;
        while (number-- > 0)
        {
            await Task_Yield;
        }
    }

    public async UniTask DoUniTask()
    {
        number = 10;
        while (number-- > 0)
        {
            await UniTask.DelayFrame(1);
        }
    }

    public async UniTaskVoid DoUniTaskVoid()
    {
        number = 10;
        while (number-- > 0)
        {
            await UniTask.DelayFrame(1);
        }
    }
}
