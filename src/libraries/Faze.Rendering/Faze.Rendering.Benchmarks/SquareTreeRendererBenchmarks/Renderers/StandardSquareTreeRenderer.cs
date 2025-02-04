﻿using Faze.Abstractions.Core;
using Faze.Abstractions.Rendering;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Faze.Rendering.TreeRenderers
{
    public class StandardSquareTreeRenderer : IPaintedTreeRenderer, IDisposable
    {
        private readonly SquareTreeRendererOptions options;
        private readonly Bitmap bitmap;

        public StandardSquareTreeRenderer(SquareTreeRendererOptions options) 
        {
            this.options = options;
            var imageSize = options.ImageSize;
            this.bitmap = new Bitmap(imageSize, imageSize);
        }

        public Tree<T> GetVisible<T>(Tree<T> tree)
        {
            throw new NotImplementedException();
        }

        public void Draw(Tree<Color> tree)
        {
            DrawHelper(bitmap, tree, 0, options.MaxDepth);
        }

        private void DrawHelper(Bitmap img, Tree<Color> node, int depth, int? maxDepth = null)
        {
            if (node == null)
                return;

            if (maxDepth.HasValue && depth > maxDepth.Value)
                return;

            var color = node.Value;
            if (!color.IsEmpty)
            {
                foreach (var (pX, pY) in Utilities.GetPixels(0, 0, img.Width, img.Height))
                {
                    img.SetPixel(pX, pY, color);
                }
            }

            if (node.Children == null)
                return;

            var borderOffset = (int)(img.Width * options.BorderProportion);
            var innerSize = img.Width - borderOffset * 2;
            var childSize = innerSize / options.Size;
            if (childSize > 1 && childSize < innerSize) 
            {
                var childIndex = 0;
                foreach (var child in node.Children)
                {
                    using (var childImg = new Bitmap(childSize, childSize))
                    {
                        DrawHelper(childImg, child, depth + 1, maxDepth);

                        var (x, y, width, height) = Utilities.Flatten(new[] { childIndex }, options.Size, innerSize, innerSize);
                        foreach (var (pX, pY) in Utilities.GetPixels(childImg))
                        {
                            img.SetPixel(x + borderOffset + pX, y + borderOffset + pY, childImg.GetPixel(pX, pY));
                        }
                    }

                    childIndex++;
                }
            }
        }

        public void Dispose()
        {
            this.bitmap?.Dispose();
        }

        public void WriteToStream(Stream stream, IProgressTracker progress = null)
        {
            bitmap.Save(stream, ImageFormat.Png);
        }
    }
}
