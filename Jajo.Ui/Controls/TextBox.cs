// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Jajo.Ui.Common;
using Jajo.Ui.Controls.Interfaces;

// TODO: Fix clear button

namespace Jajo.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBox"/> with additional parameters like <see cref="PlaceholderText"/>.
/// </summary>
public class TextBox : System.Windows.Controls.TextBox, IAppearanceControl
{

    /// <summary>
	/// Property for <see cref="PlaceholderText"/>.
	/// </summary>
	public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText),
		typeof(string), typeof(TextBox), new PropertyMetadata(string.Empty));

	/// <summary>
	/// Property for <see cref="PlaceholderEnabled"/>.
	/// </summary>
	public static readonly DependencyProperty PlaceholderEnabledProperty = DependencyProperty.Register(
		nameof(PlaceholderEnabled),
		typeof(bool), typeof(TextBox), new PropertyMetadata(true));

	/// <summary>
	/// Property for <see cref="ClearButtonEnabled"/>.
	/// </summary>
	public static readonly DependencyProperty ClearButtonEnabledProperty = DependencyProperty.Register(nameof(ClearButtonEnabled),
		typeof(bool), typeof(TextBox), new PropertyMetadata(true));

	/// <summary>
	/// Property for <see cref="ShowClearButton"/>.
	/// </summary>
	public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(nameof(ShowClearButton),
		typeof(bool), typeof(TextBox), new PropertyMetadata(false));

	/// <summary>
	/// Property for <see cref="TemplateButtonCommand"/>.
	/// </summary>
	public static readonly DependencyProperty TemplateButtonCommandProperty =
		DependencyProperty.Register(nameof(TemplateButtonCommand),
			typeof(IRelayCommand), typeof(TextBox), new PropertyMetadata(null));

	/// <summary>
	/// Gets or sets numbers pattern.
	/// </summary>
	public string PlaceholderText
	{
		get => (string)GetValue(PlaceholderTextProperty);
		set => SetValue(PlaceholderTextProperty, value);
	}

	/// <summary>
	/// Gets or sets a value determining whether to display the placeholder.
	/// </summary>
	public bool PlaceholderEnabled
	{
		get => (bool)GetValue(PlaceholderEnabledProperty);
		set => SetValue(PlaceholderEnabledProperty, value);
	}

	/// <summary>
	/// Gets or sets a value determining whether to enable the clear button.
	/// </summary>
	public bool ClearButtonEnabled
	{
		get => (bool)GetValue(ClearButtonEnabledProperty);
		set => SetValue(ClearButtonEnabledProperty, value);
	}

	/// <summary>
	/// Gets or sets a value determining whether to show the clear button when <see cref="TextBox"/> is focused.
	/// </summary>
	public bool ShowClearButton
	{
		get => (bool)GetValue(ShowClearButtonProperty);
		protected set => SetValue(ShowClearButtonProperty, value);
	}

	/// <summary>
	/// Command triggered after clicking the button.
	/// </summary>
	public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

	/// <summary>
	/// Creates a new instance and assigns default events.
	/// </summary>
	public TextBox()
	{
		SetValue(TemplateButtonCommandProperty, new RelayCommand(o => OnTemplateButtonClick(this, o)));
	}

	/// <inheritdoc />
	protected override void OnTextChanged(TextChangedEventArgs e)
	{
		base.OnTextChanged(e);

		if (PlaceholderEnabled && Text.Length > 0)
			PlaceholderEnabled = false;

		if (!PlaceholderEnabled && Text.Length < 1)
			PlaceholderEnabled = true;

		RevealClearButton();
	}

	/// <inheritdoc />
	protected override void OnGotFocus(RoutedEventArgs e)
	{
		base.OnGotFocus(e);

		RevealClearButton();
	}

	/// <inheritdoc />
	protected override void OnLostFocus(RoutedEventArgs e)
	{
		base.OnLostFocus(e);

		HideClearButton();
	}

	/// <summary>
	/// Triggered by clicking a button in the control template.
	/// </summary>
	/// <param name="sender">Sender of the click event.</param>
	/// <param name="parameter">Additional parameters.</param>
	protected virtual void OnTemplateButtonClick(object sender, object parameter)
	{
		if (parameter == null)
			return;

		var param = parameter as string ?? string.Empty;

		switch (param)
		{
			case "clear":
				if (Text.Length > 0)
					Text = string.Empty;

				break;
		}
	}

	private void RevealClearButton()
	{
		if (ClearButtonEnabled && IsKeyboardFocusWithin)
			ShowClearButton = Text.Length > 0;
	}

	private void HideClearButton()
	{
		if (ClearButtonEnabled && !IsKeyboardFocusWithin && ShowClearButton)
			ShowClearButton = false;
	}

	/// <summary>
	/// Property for <see cref="Appearance"/>.
	/// </summary>
	public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
		typeof(ControlAppearance), typeof(TextBox),
		new PropertyMetadata(ControlAppearance.JajoPrimary));


	/// <inheritdoc />
	[Bindable(true), Category("Appearance")]
	public ControlAppearance Appearance
	{
		get => (ControlAppearance)GetValue(AppearanceProperty);
		set => SetValue(AppearanceProperty, value);
	}
}
