﻿using Faze.Abstractions.Core;
using Faze.Abstractions.Rendering;
using Faze.Rendering.TreeRenderers;
using Microsoft.Diagnostics.Tracing.Parsers;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Faze.Rendering.Benchmarks.SquareTreeRendererBenchmarks.Renderers
{
    public class SkiaSquareTreeRenderer : IPaintedTreeRenderer
    {
        private readonly SquareTreeRendererOptions options;
        private readonly int imageSize;
        private readonly SKSurface surface;

        public SkiaSquareTreeRenderer(SquareTreeRendererOptions options)
        {
            this.options = options;
            this.imageSize = options.ImageSize;
            var imageInfo = new SKImageInfo(imageSize, imageSize);
            this.surface = SKSurface.Create(imageInfo);
        }

        public SKSurface Surface => surface;

        public Tree<T> GetVisible<T>(Tree<T> tree)
        {
            return tree;
        }

        public void Draw(Tree<Color> tree)
        {
            DrawHelper(surface.Canvas, tree, SKRect.Create(0, 0, imageSize, imageSize), 0, options.MaxDepth);
        }

        public void WriteToStream(Stream stream, IProgressTracker progress = null)
        {
            using SKImage image = surface.Snapshot();
            using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);

            data.AsStream().CopyTo(stream);
        }

        private void DrawHelper(SKCanvas canvas, Tree<Color> node, SKRect rect, int depth, int? maxDepth = null)
        {
            if (node == null)
                return;

            if (maxDepth.HasValue && depth > maxDepth.Value)
                return;

            canvas.DrawRect(rect, new SKPaint() { Color = new SKColor((uint)node.Value.ToArgb()) });

            if (node.Children == null)
                return;

            var borderOffset = options.BorderProportion;
            var borderSize = rect.Width * borderOffset;
            if (borderSize < 1) borderSize = 0; // Don't render a border that is sub-pixel size
            var innerRectSize = rect.Width - 2 * borderSize;
            var innerRect = SKRect.Create(rect.Left + borderSize, rect.Top + borderSize, rect.Left + innerRectSize, rect.Top + innerRectSize);
            var childSize = innerRectSize / options.Size;

            if (childSize > 1 && childSize < innerRectSize)
            {
                var childIndex = 0;
                foreach (var child in node.Children)
                {
                    var (x, y, _) = Utilities.Flatten(new[] { childIndex }, options.Size);
                    var childRect = SKRect.Create(innerRect.Left + innerRectSize * x, innerRect.Top + innerRectSize * y, childSize, childSize);
                    DrawHelper(canvas, child, childRect, depth + 1, maxDepth);

                    childIndex++;
                }
            }
        }

        public void Dispose()
        {
            this.surface?.Dispose();
        }
    }
}
