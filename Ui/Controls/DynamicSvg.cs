using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Jajo.Ui.Controls;

public class DynamicSvg : Control
{
    static DynamicSvg()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DynamicSvg), new FrameworkPropertyMetadata(typeof(DynamicSvg)));
    }

    public new static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
        name: nameof(Foreground),
        propertyType: typeof(SolidColorBrush),
        ownerType: typeof(DynamicSvg),
        typeMetadata: new PropertyMetadata(new BrushConverter().ConvertFromString("#00AD6F")));

    public new SolidColorBrush Foreground
    {
        get => (SolidColorBrush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }
        
    public new static readonly DependencyProperty ForegroundSecondaryProperty = DependencyProperty.Register(
        name: nameof(ForegroundSecondary),
        propertyType: typeof(SolidColorBrush),
        ownerType: typeof(DynamicSvg),
        typeMetadata: new PropertyMetadata(new BrushConverter().ConvertFromString("#FFFFFFFF")));

    public new SolidColorBrush ForegroundSecondary
    {
        get => (SolidColorBrush)GetValue(ForegroundSecondaryProperty);
        set => SetValue(ForegroundSecondaryProperty, value);
    }
}