using MultithreadedHashCacheTest;
using System.Collections.Concurrent;
using System.Runtime.Caching;


Console.WriteLine("Hello, World!");
MemoryCache _memCache = MemoryCache.Default;

var sleepTimeSpan = new TimeSpan(200);
var safeDictionary = new SafeDictionary();

var biggerSafeDictionary = new SafeDictionary(disablePurge: true);

await TryFiveMillion();


Console.WriteLine(biggerSafeDictionary.Count);

Thread.Sleep(new TimeSpan(0, 0, 30));

await TryFiveMillion();

Console.WriteLine(biggerSafeDictionary.Count);

async Task TryFiveMillion()
{
    var tasks = new Task[]{
 Task.Factory.StartNew(() =>
    {

        var miniCache = new Dictionary<string, string>();

        for (int i = 0; i < 1000000; i++)
        {
            var timeStamp = GetCacheSafeTimeStamp();
            miniCache.Add(timeStamp, timeStamp);
        }
        return miniCache;
    }, TaskCreationOptions.AttachedToParent).ContinueWith((minicache) =>
    {
        var dict = minicache.Result;
        var limit = dict.Count;

        foreach (var timestamp in dict)
            biggerSafeDictionary.TryAdd(long.Parse(timestamp.Key), timestamp.Value);
    }),
 Task.Factory.StartNew(() =>
    {

        var miniCache = new Dictionary<string, string>();

        for (int i = 0; i < 1000000; i++)
        {
            var timeStamp = GetCacheSafeTimeStamp();
            miniCache.Add(timeStamp, timeStamp);
        }
        return miniCache;
    }, TaskCreationOptions.AttachedToParent).ContinueWith((minicache) =>
    {
        var dict = minicache.Result;
        var limit = dict.Count;

        foreach (var timestamp in dict)
            biggerSafeDictionary.TryAdd(long.Parse(timestamp.Key), timestamp.Value);
    }),
 Task.Factory.StartNew(() =>
    {

        var miniCache = new Dictionary<string, string>();

        for (int i = 0; i < 1000000; i++)
        {
            var timeStamp = GetCacheSafeTimeStamp();
            miniCache.Add(timeStamp, timeStamp);
        }
        return miniCache;
    }, TaskCreationOptions.AttachedToParent).ContinueWith((minicache) =>
    {
        var dict = minicache.Result;
        var limit = dict.Count;

        foreach (var timestamp in dict)
            biggerSafeDictionary.TryAdd(long.Parse(timestamp.Key), timestamp.Value);
    }),
 Task.Factory.StartNew(() =>
    {

        var miniCache = new Dictionary<string, string>();

        for (int i = 0; i < 1000000; i++)
        {
            var timeStamp = GetCacheSafeTimeStamp();
            miniCache.Add(timeStamp, timeStamp);
        }
        return miniCache;
    }, TaskCreationOptions.AttachedToParent).ContinueWith((minicache) =>
    {
        var dict = minicache.Result;
        var limit = dict.Count;

        foreach (var timestamp in dict)
            biggerSafeDictionary.TryAdd(long.Parse(timestamp.Key), timestamp.Value);
    }),
 Task.Factory.StartNew(() =>
    {

        var miniCache = new Dictionary<string, string>();

        for (int i = 0; i < 1000000; i++)
        {
            var timeStamp = GetCacheSafeTimeStamp();
            miniCache.Add(timeStamp, timeStamp);
        }
        return miniCache;
    }, TaskCreationOptions.AttachedToParent).ContinueWith((minicache) =>
    {
        var dict = minicache.Result;
        var limit = dict.Count;

        foreach (var timestamp in dict)
            biggerSafeDictionary.TryAdd(long.Parse(timestamp.Key), timestamp.Value);
    })
    };
    await Task.WhenAll(tasks);
}



string GetCacheSafeTimeStamp()
{
    var dt = DateTime.Now;
    var ts = dt.ToFileTimeUtc();
    while (safeDictionary.Contains(new KeyValuePair<long, string>(ts, ts.ToString())))
    {

        ts = DateTime.Now.ToFileTimeUtc();
    }
    if(safeDictionary.TryAdd(ts, ts.ToString()))
        return ts.ToString();
    
    return GetCacheSafeTimeStamp();
}