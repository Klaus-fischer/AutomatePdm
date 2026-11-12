// <copyright file="GridLengthAnimation.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.Animation;

using System;
using System.Windows;
using System.Windows.Media.Animation;

/// <summary>
/// Animiert einen <see cref="GridLength"/>-Wert zwischen zwei Werten.
/// </summary>
public class GridLengthAnimation : AnimationTimeline
{
    /// <summary>
    /// Identifiziert die Abhängigkeitseigenschaft <see cref="From"/>.
    /// </summary>
    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register(
            nameof(From),
            typeof(GridLength),
            typeof(GridLengthAnimation));

    /// <summary>
    /// Identifiziert die Abhängigkeitseigenschaft <see cref="To"/>.
    /// </summary>
    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register(
            nameof(To),
            typeof(GridLength),
            typeof(GridLengthAnimation));

    /// <summary>
    /// Identifiziert die Abhängigkeitseigenschaft <see cref="EasingFunction"/>.
    /// </summary>
    public static readonly DependencyProperty EasingFunctionProperty =
        DependencyProperty.Register(
            nameof(EasingFunction),
            typeof(IEasingFunction),
            typeof(GridLengthAnimation));

    /// <summary>
    /// Gets the type of the property from this animation.
    /// </summary>
    public override Type TargetPropertyType =>
        typeof(GridLength);

    /// <summary>
    /// Gets or sets ruft den Startwert der Animation ab oder legt diesen fest.
    /// </summary>
    public GridLength From
    {
        get => (GridLength)this.GetValue(FromProperty);
        set => this.SetValue(FromProperty, value);
    }

    /// <summary>
    /// Gets or sets ruft den Zielwert der Animation ab oder legt diesen fest.
    /// </summary>
    public GridLength To
    {
        get => (GridLength)this.GetValue(ToProperty);
        set => this.SetValue(ToProperty, value);
    }

    /// <summary>
    /// Gets or sets ruft die optionale Beschleunigungsfunktion der Animation ab oder legt diese fest.
    /// </summary>
    public IEasingFunction? EasingFunction
    {
        get => (IEasingFunction?)this.GetValue(EasingFunctionProperty);
        set => this.SetValue(EasingFunctionProperty, value);
    }

    /// <summary>
    /// Berechnet den aktuellen <see cref="GridLength"/>-Wert der Animation.
    /// </summary>
    /// <param name="defaultOriginValue">
    /// Der Standardursprungswert der animierten Eigenschaft.
    /// </param>
    /// <param name="defaultDestinationValue">
    /// Der Standardzielwert der animierten Eigenschaft.
    /// </param>
    /// <param name="animationClock">
    /// Die Animationsuhr, die den aktuellen Fortschritt bereitstellt.
    /// </param>
    /// <returns>
    /// Der interpolierte <see cref="GridLength"/>-Wert.
    /// </returns>
    public override object GetCurrentValue(
        object defaultOriginValue,
        object defaultDestinationValue,
        AnimationClock animationClock)
    {
        double progress = animationClock.CurrentProgress ?? 0;

        progress = this.EasingFunction?.Ease(progress) ?? progress;

        double fromVal = this.From.Value;
        double toVal = this.To.Value;

        if (fromVal > toVal)
        {
            return new GridLength(((1 - progress) * (fromVal - toVal)) + toVal, this.To.GridUnitType);
        }

        return new GridLength((progress * (toVal - fromVal)) + fromVal, this.To.GridUnitType);
    }

    /// <summary>
    /// Erstellt eine neue Instanz dieser Animation.
    /// </summary>
    /// <returns>Eine neue Instanz von <see cref="GridLengthAnimation"/>.</returns>
    protected override Freezable CreateInstanceCore()
    {
        return new GridLengthAnimation();
    }
}