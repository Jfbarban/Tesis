using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis.Models.NextCloudClient
{
    public class NexCloudFileResult
    {
        public string Name { get; private set; }
        public string Url { get; private set; }
        public long Size { get; private set; }
        public NexCloudState State { get; private set; }
        public string DUrl { get; private set; }

        public NexCloudFileResult(string name, string url, long size, NexCloudState state, string durl)
        {
            Name = name;
            Url = url;
            Size = size;
            State = state;
            DUrl = durl;
        }
    }
}
