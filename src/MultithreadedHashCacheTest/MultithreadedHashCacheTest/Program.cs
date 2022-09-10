using System.Runtime.Caching;


Console.WriteLine("Hello, World!");
MemoryCache _memCache = MemoryCache.Default;

var sleepTimeSpan = new TimeSpan(1000);
Dictionary<string, object> _cache = new Dictionary<string, object>();

var cacheTestTask = Task.Factory.StartNew(() =>
{

    var miniCache = new Dictionary<string, object>();

    for (int i = 0; i < 5000000; i++)
    {
        var timeStamp = GetCacheSafeTimeStamp();
        miniCache.Add(timeStamp, new object());
    }
    return miniCache;
}).ContinueWith((minicache) =>
{
    var dict = minicache.Result;
    var limit = dict.Count;
    foreach(var timestamp in dict)
        _cache.Add(timestamp.Key, new object());
});
await cacheTestTask;

Console.WriteLine(_cache.Keys.Count);


string GetCacheSafeTimeStamp()
{
    var dt = DateTime.Now;
    var ts = dt.ToFileTimeUtc().ToString();
    while (_memCache.Contains(ts))
    {
        Thread.Sleep(sleepTimeSpan);
        ts = dt.Add(sleepTimeSpan).ToFileTimeUtc().ToString();
    }
    _memCache.Add(ts, ts, DateTime.UtcNow.AddMilliseconds(220));
    return ts;
}