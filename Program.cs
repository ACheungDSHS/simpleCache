using Microsoft.VisualBasic;
using System;
using System.Net;
using System.Runtime.ConstrainedExecution;

namespace simpleCache
{
    class DownloadCache
    {
        private DownloadCacheItem[] cache;
        private readonly int length;
        public DownloadCache(int items)
        {
            this.cache = new DownloadCacheItem[items];
            this.length = items;
        }

        public string getCache(int i)
        {
            if (i > 0 && i < length)
                return this.cache[i].getData();
            /* What are you trying to pull? */
            return null;
        }

        public int setCache(int i, String data)
        {
            if (this.cache[i] != null || i < 0 || i >= this.length)
                return -1;
            this.cache[i] = new DownloadCacheItem(data);
            return i;
        }

        public int nextFree()
        {
            int i = 0;
            while (this.cache[i] != null)
            {
                i++;
                if (i == this.length)
                {
                    /* Cache is full! */
                    return -1;
                }
            }
            return i;
        }
    }
    class DownloadCacheItem
    {
        private readonly String location;
        private string data;
        public DownloadCacheItem(String location)
        {
            this.location = location;
        }

        public string getData()
        {
            if (this.data != null)
            {
                /* This is called a cache hit- we have the data already! */
                return this.data;
            }

            /* This is called a cache miss- we don't have the data so need to fetch it */

            using (WebClient client = new WebClient())
            {
                this.data = client.DownloadString(this.location);
            }

            return this.data;
        }
    }
    class Program
    {
        static void timestamp(string msg)
        {
            Console.WriteLine(DateTime.Now + " " + msg);
        }

        static void Main(string[] args)
        {
            DownloadCache removed = new DownloadCache(10);
            int current;

            timestamp("About to make the cache");
            current = removed.setCache(removed.nextFree(), "https://people.freebsd.org/~crees/removed_ports/index.xml");
            timestamp("Cache made, now let's try to access it and get the first thirty characters...");
            Console.WriteLine(removed.getCache(current).Substring(0, 30));
            timestamp("Let's try again...");
            Console.WriteLine(removed.getCache(current).Substring(0, 30));
            timestamp("Woah, that was quicker!");

        }
    }
}
