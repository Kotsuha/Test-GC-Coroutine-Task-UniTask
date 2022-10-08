using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Profiling;

/*******************************************************************************

GC Alloc                                               | Coroutine | Task                     | UniTask
------------------------------------------------------ | --------- | ------------------------ | -------
In Editor (CodeOptimization: Debug)                    | 64 B      | 288 B                    | 264 B
In Editor (CodeOptimization: Release)                  | 64 B      | 296 B (160 B each frame) | 248 B
In WebGL Development Build (CodeOptimization: Debug)   | 36 B      | 152 B                    | 200 B
In WebGL Development Build (CodeOptimization: Release) | 36 B      | 152 B (80 B each frame)  | 200 B
In WebGL Release Build                                 |           |                          |        

> Allocation on Profiler
> https://github.com/Cysharp/UniTask#allocation-on-profiler
> In UnityEditor the profiler shows allocation of compiler generated AsyncStateMachine but it only occurs in debug(development) build. C# Compiler generates AsyncStateMachine as class on Debug build and as struct on Release build.
> Unity supports Code Optimization option starting in 2020.1 (right, footer).
> You can change C# compiler optimization to release to remove AsyncStateMachine allocation in development builds. This optimization option can also be set via Compilation.CompilationPipeline-codeOptimization, and Compilation.CodeOptimization.

*******************************************************************************/


public class TestGC_Coroutine_Task_UniTask : MonoBehaviour
{
    [SerializeField] private bool _testNoAlloc = false;
    [SerializeField] private bool _testCoroutine = false;
    [SerializeField] private bool _testTask = false;
    [SerializeField] private bool _testUniTask = false;
    [SerializeField] private int _delayFrameCount = 3;
    [SerializeField] private bool _break = false;

    public bool TestNoAlloc
    {
        set => _testNoAlloc = value;
    }

    public bool TestCoroutine
    {
        set => _testCoroutine = value;
    }

    public bool TestTask
    {
        set => _testTask = value;
    }

    public bool TestUniTask
    {
        set => _testUniTask = value;
    }

    public void SetDelayFrameCount(float value)
    {
        _delayFrameCount = (int)value;
    }

    // Update is called once per frame
    void Update()
    {
        if (_testNoAlloc)
        {
            _testNoAlloc = false;
            using (new ProfilerMarker("[My Test] NoAlloc").Auto())
            {
                DoNoAlloc();
            }
        }
        if (_testCoroutine)
        {
            _testCoroutine = false;
            using (new ProfilerMarker("[My Test] Coroutine").Auto())
            {
                DoCoroutine();
            }
        }
        if (_testTask)
        {
            _testTask = false;
            using (new ProfilerMarker("[My Test] Task").Auto())
            {
                DoTask();
            }
        }
        if (_testUniTask)
        {
            _testUniTask = false;
            using (new ProfilerMarker("[My Test] UniTask").Auto())
            {
                DoUniTask();
            }
        }
        if (_break)
        {
            _break = false;
            Debug.Break();
        }
    }

    private void DoNoAlloc()
    {
        _DoNoGC(_delayFrameCount);
    }

    private void _DoNoGC(int n)
    {
        int i = 0;
        while (n > 0)
        {
            n--;
            i++;
        }
        _break = true;
    }

    private void DoCoroutine()
    {
        StartCoroutine(_DoCoroutine(_delayFrameCount));
    }

    private IEnumerator _DoCoroutine(int delayFrameCount)
    {
        while (delayFrameCount > 0)
        {
            delayFrameCount--;
            yield return null;
        }
        _break = true;
    }

    private void DoTask()
    {
        _ = _DoTask(_delayFrameCount);
    }

    private async Task _DoTask(int delayFrameCount)
    {
        while (delayFrameCount > 0)
        {
            delayFrameCount--;
            await Task.Yield();
        }
        _break = true;
    }

    private void DoUniTask()
    {
        _ = _DoUniTask(_delayFrameCount);
    }

    private async UniTask _DoUniTask(int delayFrameCount)
    {
        await UniTask.DelayFrame(delayFrameCount);
        _break = true;
    }
}
