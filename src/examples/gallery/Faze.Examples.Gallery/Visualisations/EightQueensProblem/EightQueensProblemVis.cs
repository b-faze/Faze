﻿using Faze.Abstractions.Core;
using Faze.Abstractions.GameMoves;
using Faze.Abstractions.GameResults;
using Faze.Abstractions.GameStates;
using Faze.Abstractions.Rendering;
using Faze.Core.Pipelines;
using Faze.Examples.Gallery.Interfaces;
using Faze.Examples.GridGames;
using Faze.Examples.GridGames.Pieces;
using Faze.Rendering.TreeRenderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faze.Examples.Gallery.Services.Aggregates;
using Faze.Examples.Gallery.Visualisations.EightQueensProblem.DataGenerators;

namespace Faze.Examples.Gallery.Visualisations.EightQueensProblem
{
    public class EightQueensProblemVis : IImageGenerator
    {
        private readonly IGalleryService galleryService;
        private readonly ITreeDataProvider<EightQueensProblemSolutionAggregate> treeDataProvider;

        public EightQueensProblemVis(IGalleryService galleryService, ITreeDataProvider<EightQueensProblemSolutionAggregate> treeDataProvider) 
        {
            this.galleryService = galleryService;
            this.treeDataProvider = treeDataProvider;
        }

        public ImageGeneratorMetaData GetMetaData()
        {
            return new ImageGeneratorMetaData(new[] { Albums.EightQueensProblem });
        }

        public Task Generate(IProgressBar progress)
        {
            var maxDepth = 3;
            progress.SetMaxTicks(maxDepth * 4);
            progress.SetMessage(Albums.EightQueensProblem);

            for (var i = 1; i <= maxDepth; i++)
            {
                RunVariation1(progress, i);
                progress.Tick();

                RunVariation2(progress, i);
                progress.Tick();

                RunVariation3(progress, i);
                progress.Tick();

                RunVariation4(progress, i);
                progress.Tick();
            }

            return Task.CompletedTask;
        }

        private Task RunVariation1(IProgressBar progress, int maxDepth)
        {
            var id = $"8 Queens Problem Solutions depth {maxDepth}.png";


            var metaData = new GalleryItemMetadata
            {
                FileName = id,
                Album = Albums.EightQueensProblem,
            };

            if (File.Exists(galleryService.GetImageFilename(metaData)))
                return Task.CompletedTask;

            var rendererConfig = new SquareTreeRendererOptions(8, 600)
            {
                MaxDepth = maxDepth,
                //BorderProportions = 0.1f
            };

            var painterConfig = new EightQueensProblemPainterConfig
            {
                BlackParentMoves = false,
                BlackUnavailableMoves = true
            };

            var pipeline = Create(metaData, rendererConfig, painterConfig);
            pipeline.Run();

            return Task.CompletedTask;
        }

        private Task RunVariation2(IProgressBar progress, int maxDepth)
        {
            var id = $"Var 2 8QP Solutions depth {maxDepth}.png";


            var metaData = new GalleryItemMetadata
            {
                FileName = id,
                Album = Albums.EightQueensProblem,
            };

            if (File.Exists(galleryService.GetImageFilename(metaData)))
                return Task.CompletedTask;

            var rendererConfig = new SquareTreeRendererOptions(8, 600)
            {
                MaxDepth = maxDepth,
                //BorderProportions = 0.1f
            };

            var painterConfig = new EightQueensProblemPainterConfig
            {
                BlackParentMoves = true,
                BlackUnavailableMoves = true
            };

            var pipeline = Create(metaData, rendererConfig, painterConfig);
            pipeline.Run();

            return Task.CompletedTask;
        }

        private Task RunVariation3(IProgressBar progress, int maxDepth)
        {
            var id = $"Var 3 8QP Solutions depth {maxDepth}.png";


            var metaData = new GalleryItemMetadata
            {
                FileName = id,
                Album = Albums.EightQueensProblem,
            };

            if (File.Exists(galleryService.GetImageFilename(metaData)))
                return Task.CompletedTask;

            var rendererConfig = new SquareTreeRendererOptions(8, 600)
            {
                MaxDepth = maxDepth,
                //BorderProportions = 0.1f
            };

            var painterConfig = new EightQueensProblemPainterConfig
            {
                BlackParentMoves = false,
                BlackUnavailableMoves = false
            };

            var pipeline = Create(metaData, rendererConfig, painterConfig);
            pipeline.Run();

            return Task.CompletedTask;
        }

        private Task RunVariation4(IProgressBar progress, int maxDepth)
        {
            var id = $"Var 4 8QP Solutions depth {maxDepth}.png";


            var metaData = new GalleryItemMetadata
            {
                FileName = id,
                Album = Albums.EightQueensProblem,
            };

            if (File.Exists(galleryService.GetImageFilename(metaData)))
                return Task.CompletedTask;

            var rendererConfig = new SquareTreeRendererOptions(8, 600)
            {
                MaxDepth = maxDepth,
                BorderProportions = 0.1f
            };

            var painterConfig = new EightQueensProblemPainterConfig
            {
                BlackParentMoves = false,
                BlackUnavailableMoves = false
            };

            var pipeline = Create(metaData, rendererConfig, painterConfig);
            pipeline.Run();

            return Task.CompletedTask;
        }

        public IPipeline Create(GalleryItemMetadata galleryMetaData, SquareTreeRendererOptions rendererConfig, EightQueensProblemPainterConfig painterConfig)
        {
            return ReversePipelineBuilder.Create()
                .GallerySave(galleryService, galleryMetaData)
                .Render(new SquareTreeRenderer(rendererConfig))
                .Paint(new EightQueensProblemPainter(painterConfig))
                .LoadTree(EightQueensProblemExhaustiveDataPipeline.Id, treeDataProvider);
        }
    }
}
