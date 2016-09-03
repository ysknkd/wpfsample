using System;
using System.Text;
using System.Threading.Tasks;

namespace Common.TriggerActions
{
    using Messenger;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Collections.Generic;
    using System.Linq;

    public class RequireFocusAction : TriggerAction<DependencyObject>
    {
        // @see http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        private IEnumerable<T> FindElements<T>(DependencyObject d) where T : DependencyObject
        {
            if (d != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i += 1)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(d, i);

                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindElements<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        // @see https://code.msdn.microsoft.com/windowsdesktop/MVVM-ViewModelView-d1c0714d
        protected override void Invoke(object parameter)
        {
            MessageEventArgs e = parameter as MessageEventArgs;
            if (e == null)
            {
                // MessageEventArgs 以外の場合は処理しない
                return;
            }

            IEnumerable<Control> controls = FindElements<Control>(AssociatedObject);

            try
            {
                // @see http://stackoverflow.com/questions/2736236/how-to-use-linq-to-find-the-minimum
                Control firstControl = controls
                    .Where(control => (Validation.GetErrors(control))?.Count > 0)
                    .Aggregate((ca, cb) => ca.TabIndex < cb.TabIndex ? ca : cb);

                firstControl.Focus();
            }
            catch
            {
                // Do Nothing
            }
        }
    }
}
