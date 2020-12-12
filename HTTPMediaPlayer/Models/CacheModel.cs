using System;
using System.Runtime.Caching;

namespace HTTPMediaPlayer.Models
{
  public class InMemoryCache : ICacheService
  {
    /// <summary>
    /// Timeout in seconds
    /// </summary>
    private int timeout;
    public InMemoryCache(int timeOut)
    {
      timeout = timeOut;
    }

    public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
    {
      T item = MemoryCache.Default.Get(cacheKey) as T;
      if (item == null)
      {
        item = getItemCallback();
        MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddSeconds(timeout));
      }
      return item;
    }
  }

  interface ICacheService
  {
    T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
  }
}