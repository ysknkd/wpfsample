using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messenger
{
    // @see https://code.msdn.microsoft.com/windowsdesktop/MVVM-ViewModelView-d1c0714d

    public class Message
    {
        public object Body { get; private set; }
        public object Response { get; set; }

        public Message(object body)
        {
            Body = body;
        }
    }
}
