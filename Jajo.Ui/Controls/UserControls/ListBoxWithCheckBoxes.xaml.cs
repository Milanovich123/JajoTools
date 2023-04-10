﻿using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Jajo.Ui.Controls.UserControls;

public partial class ListBoxWithCheckBoxes
{
    public ListBoxWithCheckBoxes()
    {
        InitializeComponent();
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable),
            typeof(ListBoxWithCheckBoxes));

    public object ChBoxIsChecked
    {
        get => (object)GetValue(ChBoxIsCheckedProperty);
        set => SetValue(ChBoxIsCheckedProperty, value);
    }

    public static readonly DependencyProperty ChBoxIsCheckedProperty =
        DependencyProperty.Register(nameof(ChBoxIsChecked), typeof(object),
            typeof(ListBoxWithCheckBoxes));

    public object ChBoxText
    {
        get => (object)GetValue(ChBoxTextProperty);
        set => SetValue(ChBoxTextProperty, value);
    }

    public static readonly DependencyProperty ChBoxTextProperty =
        DependencyProperty.Register(nameof(ChBoxText), typeof(object),
            typeof(ListBoxWithCheckBoxes));
}