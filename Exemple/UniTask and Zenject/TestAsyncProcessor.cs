using PowerTest;
using System;
using System.Threading.Tasks;

#if Zenject
public class TestAsyncProcessor : Zenject.ZenjectUnitTestFixture
#else
public class TestAsyncProcessor
#endif
{

    private AsyncProcessor processor;
    [PowerTestSetup]
    public void SetupProcess()
    {
        Setup();
    }

#if Zenject
    public override void Setup()
    {
        base.Setup();
        // Привязки для теста
        Container.Bind<AsyncProcessor>().AsSingle();
        Container.Inject(this);
    }
    [Zenject.Inject]
    public void Construct(AsyncProcessor asyncProcessor)
    {
        processor = asyncProcessor;
        UnityEngine.Debug.Log(processor);
    }

#else

  public void Setup()
    {
        // Создание и инициализация AsyncProcessor
        processor = new AsyncProcessor();
        // Здесь можно выполнить любую дополнительную настройку, если это необходимо
    }

#endif

    [PowerTest]
    public async Cysharp.Threading.Tasks.UniTask TestAsyncMethod_ReturnsProcessedData()
    {

        string testData = "test";
        string expected = "Processed test";

        string result = await processor.ProcessDataAsync(testData);
        UnityEngine.Debug.Log(result);

        Asserts.AreEqual(expected, result, "Async method should return processed result");


    }

    [PowerTest]
    public async Cysharp.Threading.Tasks.UniTask TestAsyncMethod_WithEmptyString()
    {

        string testData = "";
        string expected = "Processed ";


        string result = await processor.ProcessDataAsync(testData);
        UnityEngine.Debug.Log(result);

        Asserts.AreEqual(expected, result, "Async method should handle empty string input");

    }
#if UniTask
    [PowerTest]
    public async Cysharp.Threading.Tasks.UniTask TestAsyncMethod_DelayedCheck()
    {
      
        string testData = "some data";
        string expected = "Expected result after processing";

        
        string result = await processor.ProcessDataAsync(testData);

       
        await Cysharp.Threading.Tasks.UniTask.Delay(TimeSpan.FromSeconds(5));

      
        UnityEngine.Debug.Log(result);
        Asserts.AreNotEqual(expected, result, "Async method should return correct result after a delay");
    }
#else
    [PowerTest]
    public async Task TestAsyncMethod_DelayedCheck()
    {
       
        string testData = "some data";
        string expected = "Expected result after processing";

    
        string result = await processor.ProcessDataAsync(testData);

      
        await Task.Delay(TimeSpan.FromSeconds(5));

      
        UnityEngine.Debug.Log(result);
        Asserts.AreNotEqual(expected, result, "Async method should return correct result after a delay");
    }

#endif
}
