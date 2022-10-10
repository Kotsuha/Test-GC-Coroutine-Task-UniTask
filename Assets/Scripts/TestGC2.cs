using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

public class TestGC2 : MonoBehaviour
{
    [SerializeField] private int _pauseFrame = int.MaxValue;
    [SerializeField] private TestGC3 _test3;

    [SerializeField]
    private Transform _tr;

    private int _n;
    private Vector3 _v;


    private enum RunTask
    {
        None,
        Do_UniTaskDelayFrame,
        Do_UniTaskDelayFrameLocal,
        DoTest3_StartCoroutine,
        DoTest3_AwaitCoroutine,
        DoTest3_AwaitTask,
        DoTest3_AwaitUniTask,
        DoTest3_UniTaskForget,
        DoTest3_UniTaskVoidForget,
    }

    private RunTask _runTask = RunTask.None;
    private RunTask _runTaskReal = RunTask.None;
    private int _runTaskRealFrame = -1;

    void Update()
    {
        if (_runTaskReal != RunTask.None && _runTaskRealFrame == Time.frameCount)
        {
            switch (_runTaskReal)
            {
                case RunTask.Do_UniTaskDelayFrame:
                    _Do_UniTaskDelayFrame();
                    break;
                case RunTask.Do_UniTaskDelayFrameLocal:
                    _Do_UniTaskDelayFrameLocal();
                    break;
                case RunTask.DoTest3_StartCoroutine:
                    _DoTest3_StartCoroutine();
                    break;
                case RunTask.DoTest3_AwaitCoroutine:
                    _DoTest3_AwaitCoroutine();
                    break;
                case RunTask.DoTest3_AwaitTask:
                    _DoTest3_AwaitTask();
                    break;
                case RunTask.DoTest3_AwaitUniTask:
                    _DoTest3_AwaitUniTask();
                    break;
                case RunTask.DoTest3_UniTaskForget:
                    _DoTest3_UniTaskForget();
                    break;
                case RunTask.DoTest3_UniTaskVoidForget:
                    _DoTest3_UniTaskVoidForget();
                    break;
            }
            _runTaskReal = RunTask.None;
            _runTaskRealFrame = -1;
        }
        if (_runTask != RunTask.None)
        {
            _runTaskReal = _runTask;
            _runTaskRealFrame = Time.frameCount + 2;
            _runTask = RunTask.None;
        }
    }

    // -------------------------------------------------------------------------
    // Triggered from UnityEngine.UI.Button

    public void Do_UniTaskDelayFrame()
    {
        _runTask = RunTask.Do_UniTaskDelayFrame;
    }

    public void Do_UniTaskDelayFrameLocal()
    {
        _runTask = RunTask.Do_UniTaskDelayFrameLocal;
    }

    public void DoTest3_StartCoroutine()
    {
        _runTask = RunTask.DoTest3_StartCoroutine;
    }

    public void DoTest3_AwaitCoroutine()
    {
        _runTask = RunTask.DoTest3_AwaitCoroutine;
    }

    public void DoTest3_AwaitTask()
    {
        _runTask = RunTask.DoTest3_AwaitTask;
    }

    public void DoTest3_AwaitUniTask()
    {
        _runTask = RunTask.DoTest3_AwaitUniTask;
    }

    public void DoTest3_UniTaskForget()
    {
        _runTask = RunTask.DoTest3_UniTaskForget;
    }

    // -------------------------------------------------------------------------
    // Triggered from Update

    private async UniTaskVoid _Do_UniTaskDelayFrame()
    {
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrame").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrame 1").Auto()) { }
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrame 2").Auto()) { }
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrame 3").Auto()) { }
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrame 4").Auto()) { }
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrame 5").Auto()) { }
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private async UniTask<int> UniTask_DelayFrame_10_()
    {
        await UniTask.DelayFrame(10);
        return 0;
    }

    private async UniTaskVoid _Do_UniTaskDelayFrameLocal()
    {
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrameLocal").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrameLocal 1").Auto()) { }
        await UniTask_DelayFrame_10_();
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrameLocal 2").Auto()) { }
        await UniTask_DelayFrame_10_();
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrameLocal 3").Auto()) { }
        await UniTask_DelayFrame_10_();
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrameLocal 4").Auto()) { }
        await UniTask_DelayFrame_10_();
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] UniTaskDelayFrameLocal 5").Auto()) { }
        await UniTask_DelayFrame_10_();
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private void _DoTest3_StartCoroutine()
    {
        using (new ProfilerMarker("[MyTest] DoTest3_StartCoroutine").Auto()) { }
        StartCoroutine(__DoTest3_Coroutine());
    }

    private IEnumerator __DoTest3_Coroutine()
    {
        _tr.transform.localPosition = Vector3.zero;
        using (new ProfilerMarker("[MyTest] DoTest3_StartCoroutine 1").Auto()) { }
        yield return _test3.DoCoroutine();
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] DoTest3_StartCoroutine 2").Auto()) { }
        yield return _test3.DoCoroutine();        
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] DoTest3_StartCoroutine 3").Auto()) { }
        yield return _test3.DoCoroutine();        
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] DoTest3_StartCoroutine 4").Auto()) { }
        yield return _test3.DoCoroutine();        
        _tr.transform.localPosition = Random.onUnitSphere;
        using (new ProfilerMarker("[MyTest] DoTest3_StartCoroutine 5").Auto()) { }
        yield return _test3.DoCoroutine();        
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private async UniTaskVoid _DoTest3_AwaitCoroutine()
    {
        using (new ProfilerMarker("[MyTest] DoTest3_AwaitCoroutine").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        await _test3.DoCoroutine();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoCoroutine();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoCoroutine();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoCoroutine();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoCoroutine();
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private async UniTaskVoid _DoTest3_AwaitTask()
    {
        using (new ProfilerMarker("[MyTest] DoTest3_AwaitTask").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        await _test3.DoTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private async UniTaskVoid _DoTest3_AwaitUniTask()
    {
        using (new ProfilerMarker("[MyTest] DoTest3_AwaitUniTask").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        await _test3.DoUniTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoUniTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoUniTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoUniTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        await _test3.DoUniTask();
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private async UniTaskVoid _DoTest3_UniTaskForget()
    {
        using (new ProfilerMarker("[MyTest] DoTest3_UniTaskForget").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        _test3.DoUniTask().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTask().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTask().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTask().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTask().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }

    private async UniTaskVoid _DoTest3_UniTaskVoidForget()
    {
        using (new ProfilerMarker("[MyTest] DoTest3_UniTaskVoidForget").Auto()) { }
        _tr.transform.localPosition = Vector3.zero;
        _test3.DoUniTaskVoid().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTaskVoid().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTaskVoid().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTaskVoid().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        _test3.DoUniTaskVoid().Forget();
        await UniTask.DelayFrame(10);
        _tr.transform.localPosition = Random.onUnitSphere;
        Debug.Break();
    }
}
