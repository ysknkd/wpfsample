using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Triggers
{
    // @see https://code.msdn.microsoft.com/windowsdesktop/MVVM-ViewModelView-d1c0714d
    using System.Windows.Interactivity;

    public class MessageTrigger: EventTrigger
    {
        /// <summary>
        /// Messenger の Raised イベントを受信する Trigger
        /// </summary>
        /// <returns></returns>
        protected override string GetEventName()
        {
            return "Raised";
        }
    }
}
