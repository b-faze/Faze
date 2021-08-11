﻿using Faze.Abstractions.Core;
using Faze.Abstractions.Rendering;
using Faze.Rendering.Video.Extensions;

namespace Faze.Examples.Gallery
{
    public static class ReversePipelineExtensions
    {
        public static IReversePipelineBuilder<IPaintedTreeRenderer> GallerySave(this IReversePipelineBuilder builder, IGalleryService galleryService, GalleryItemMetadata data)
        {
            return builder.Require<IPaintedTreeRenderer>(renderer => galleryService.Save(renderer, data));
        }

        public static IReversePipelineBuilder<IStreamer> GalleryVideo(this IReversePipelineBuilder builder, IGalleryService galleryService, GalleryItemMetadata data)
        {
            var filename = galleryService.GetImageFilename(data);
            return builder
                .Video(filename, new VideoFFMPEGSettings(24));
        }
    }
}
