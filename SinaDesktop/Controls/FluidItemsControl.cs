using System.Windows;
using System.Windows.Controls;


namespace SinaDesktop.Controls
{
    public class FluidItemsControl : ItemsControl
    {
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(FluidItemsControl), new PropertyMetadata(null));
        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new FluidItemsControlItem { Style = ItemContainerStyle };
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is FluidItemsControlItem;
        }
    }
}
