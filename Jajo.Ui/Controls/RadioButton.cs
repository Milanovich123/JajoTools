using System.Windows;

namespace Jajo.Ui.Controls;

public class RadioButton : System.Windows.Controls.RadioButton
{
    static RadioButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(typeof(RadioButton)));
    }

    public static readonly DependencyProperty SvgStyleProperty = DependencyProperty.Register(
        name: nameof(SvgStyle),
        propertyType: typeof(Style),
        ownerType: typeof(RadioButton));

    public Style SvgStyle
    {
        get => (Style)GetValue(SvgStyleProperty);
        set => SetValue(SvgStyleProperty, value);
    }
}