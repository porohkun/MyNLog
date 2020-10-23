using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyNLog
{
    public class TextBoxEnterKeyUpdateBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
                AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            base.OnDetaching();
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == Key.Enter)
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
