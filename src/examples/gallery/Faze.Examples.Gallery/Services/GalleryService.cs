﻿using Faze.Abstractions.Rendering;
using System;
using System.IO;

namespace Faze.Examples.Gallery.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly GalleryServiceConfig config;

        public GalleryService(GalleryServiceConfig config)
        {
            this.config = config;
        }

        public void Save(IPaintedTreeRenderer renderer, GalleryItemMetadata data)
        {
            var filePath = GetImageFilename(data);

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var fs = File.OpenWrite(filePath))
            {
                renderer.Save(fs);
            }
        }

        public string GetImageFilename(GalleryItemMetadata data)
        {
            var relativePath = Path.Combine(data.Albums);
            var filename = Path.Combine(config.ImageBasePath, relativePath, data.FileName);

            var directory = Path.GetDirectoryName(Path.GetFullPath(filename));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return filename;
        }

        public string GetDataFilename(string id)
        {
            var filename = Path.Combine(config.DataBasePath, id);

            var directory = Path.GetDirectoryName(Path.GetFullPath(filename));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return filename;
        }
    }
}
