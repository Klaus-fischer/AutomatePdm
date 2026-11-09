// <copyright file="DropIndicatorAdorner.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

internal sealed class DropIndicatorAdorner : Adorner
{
    private double indicatorY;

    private readonly Pen indicatorPen = new(
        Brushes.DodgerBlue,
        1.5);

    public DropIndicatorAdorner(UIElement adornedElement)
        : base(adornedElement)
    {
        this.IsHitTestVisible = false;

        this.indicatorPen.StartLineCap = PenLineCap.Round;
        this.indicatorPen.EndLineCap = PenLineCap.Round;
    }

    public void UpdatePosition(double y)
    {
        this.indicatorY = y;
        this.InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        double width = this.AdornedElement.RenderSize.Width;

        if (width <= 0)
        {
            return;
        }

        // Linie
        drawingContext.DrawLine(
            this.indicatorPen,
            new Point(6, this.indicatorY),
            new Point(width - 6, this.indicatorY));

        // Dreieck am linken Ende
        Point[] points =
        {
            new(7, this.indicatorY),
            new(0, this.indicatorY - 5),
            new(0, this.indicatorY + 5),
        };

        var geometry = new StreamGeometry();

        using (StreamGeometryContext context = geometry.Open())
        {
            context.BeginFigure(points[0], true, true);
            context.LineTo(points[1], true, false);
            context.LineTo(points[2], true, false);
        }

        geometry.Freeze();

        drawingContext.DrawGeometry(
            Brushes.DodgerBlue,
            null,
            geometry);
    }
}