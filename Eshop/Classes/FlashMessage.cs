using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Classes
{
    public enum FlashMessageType
    {
        Info,
        Success,
        Warning,
        Danger
    }

    public class FlashMessage
    {
        public string Message { get; set; }
        public FlashMessageType Type { get; set; }

        public FlashMessage(string message, FlashMessageType type)
        {
            Message = message;
            Type = type;
        }
    }
}
