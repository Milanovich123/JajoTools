// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using Jajo.Ui.Common;
using Jajo.Ui.Controls.Interfaces;
using Jajo.Ui.MVVM.Contracts;

namespace Jajo.Ui.MVVM.Services;

/// <summary>
///     A service that provides methods related to displaying the <see cref="ISnackbarControl" />.
/// </summary>
public class SnackbarService : ISnackbarService
{
    private ISnackbarControl? _snackbar;

    /// <inheritdoc />
    public bool IsShown
    {
        get
        {
            if (_snackbar == null)
                throw new InvalidOperationException(
                    $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

            return _snackbar.IsShown;
        }
    }

    /// <inheritdoc />
    public int Timeout
    {
        get => _snackbar?.Timeout ?? 0;
        set
        {
            if (_snackbar == null)
                throw new InvalidOperationException(
                    $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

            _snackbar.Timeout = value;
        }
    }

    /// <inheritdoc />
    public void SetSnackbarControl(ISnackbarControl snackbar)
    {
        _snackbar = snackbar;
    }

    /// <inheritdoc />
    public ISnackbarControl GetSnackbarControl()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar;
    }

    /// <inheritdoc />
    public bool Show()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show();
    }

    /// <inheritdoc />
    public bool Show(string title)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title);
    }

    ///// <inheritdoc />
    //public bool Show(string title, string message)
    //{
    //    if (_snackbar is null)
    //        throw new InvalidOperationException(
    //            $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

    //    return _snackbar.Show(title, message);
    //}
    /// <inheritdoc />
    public bool Show(string message, ControlAppearance appearance)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(message, appearance);
    }

    /// <inheritdoc />
    public bool Show(string title, string message)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title, message);
    }

    /// <inheritdoc />
    public bool Show(string title, string message, ControlAppearance appearance)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title, message, appearance);
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync();
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title);
    }

    ///// <inheritdoc />
    //public async Task<bool> ShowAsync(string title, string message)
    //{
    //    if (_snackbar is null)
    //        throw new InvalidOperationException(
    //            $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

    //    return await _snackbar.ShowAsync(title, message);
    //}
    /// <inheritdoc />
    public async Task<bool> ShowAsync(string message, ControlAppearance appearance)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(message, appearance);
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title, message);
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message, ControlAppearance appearance)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title, message, appearance);
    }

    /// <inheritdoc />
    public bool Hide()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Hide();
    }


    /// <inheritdoc />
    public async Task<bool> HideAsync()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.HideAsync();
    }
}