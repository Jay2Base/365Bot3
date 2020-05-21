using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _365Bot3
{
    public class MessageRecord : IStoreItem
{
        public string MessageFrom { get; set; }
        public string MessageTime { get; set; }
        public string MesageText { get; set; }
        public string MessageTo { get; set; }
        public string ETag { get; set; }

    }
}
