// <copyright file="DragPreviewAdorner.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

internal sealed class DragPreviewAdorner : Adorner
{
    private readonly VisualBrush visualBrush;
    private Point position;

    public DragPreviewAdorner(UIElement adornedElement, FrameworkElement previewElement)
        : base(adornedElement)
    {
        this.visualBrush = new VisualBrush(previewElement)
        {
            Stretch = Stretch.None,
            Opacity = 0.85,
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
        };

        this.IsHitTestVisible = false;
        this.position = new Point(0, 0);
    }

    public void UpdatePosition(Point point)
    {
        this.position = point;
        this.InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        FrameworkElement? preview = this.visualBrush.Visual as FrameworkElement;

        if (preview is null)
        {
            return;
        }

        double width = preview.ActualWidth;
        double height = preview.ActualHeight;

        if (width <= 0 || height <= 0)
        {
            return;
        }

        Rect rect = new(
            this.position.X + 12,
            this.position.Y + 12,
            width,
            height);

        drawingContext.DrawRectangle(this.visualBrush, null, rect);
    }
}
