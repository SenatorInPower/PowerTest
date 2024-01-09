using System;

public class AsyncProcessor
{
    public async Cysharp.Threading.Tasks.UniTask<string> ProcessDataAsync(string input)
    {
        await Cysharp.Threading.Tasks.UniTask.Delay(TimeSpan.FromSeconds(1));
        return $"Processed {input}";
    }
}