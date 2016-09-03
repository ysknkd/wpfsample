using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messenger
{
    public class MessageEventArgs: EventArgs
    {
        public Message Message { get; private set; }
        public Action<Message> Callback { get; private set; }

        public MessageEventArgs(Message message, Action<Message> callback)
        {
            Message = message;
            Callback = callback;
        }
    }
}
