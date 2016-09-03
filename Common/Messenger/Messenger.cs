using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messenger
{
    public class Messenger
    {
        public event EventHandler<MessageEventArgs> Raised;

        public void Raise(Message message, Action<Message> callback)
        {
            Raised?.Invoke(this, new MessageEventArgs(message, callback));
        }
    }
}
