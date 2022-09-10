using System.Runtime.Caching;


Console.WriteLine("Hello, World!");
MemoryCache _memCache = MemoryCache.Default;

var sleepTimeSpan = new TimeSpan(1000);
Dictionary<string, object> _cache = new Dictionary<string, object>();

for(int i = 0; i<5000000;i++)
{
    var timeStamp = GetCacheSafeTimeStamp();
    _cache.Add(timeStamp, new object());
}

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