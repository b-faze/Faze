﻿using Faze.Abstractions.Core;
using Faze.Abstractions.GameMoves;
using Faze.Abstractions.GameResults;
using Faze.Abstractions.GameStates;
using Faze.Abstractions.Rendering;
using Faze.Core.Extensions;
using Faze.Core.Pipelines;
using Faze.Engine.ResultTrees;
using Faze.Engine.Simulators;
using Faze.Examples.Games.OX;
using Faze.Core.TreeLinq;
using Faze.Core.Adapters;

namespace Faze.Examples.Gallery.Visualisations.OX.DataGenerators
{
    public class OXSimulatedDataPipeline
    {
        private readonly IFileTreeDataProvider<WinLoseDrawResultAggregate> treeDataProvider;

        public OXSimulatedDataPipeline(IFileTreeDataProvider<WinLoseDrawResultAggregate> treeDataProvider)
        {
            this.treeDataProvider = treeDataProvider;
        }

        public IPipeline Create(string dataId, int simulations, int depth)
        {
            var resultsMapper = new WinLoseDrawResultsTreeMapper(new GameSimulator(), simulations);

            var pipeline = ReversePipelineBuilder.Create()
                .SaveTree(dataId, treeDataProvider)
                .Map(resultsMapper)
                .GameTree(new SquareTreeAdapter(3))
                .Build(() => OXState.Initial);

            return pipeline;
        }
    }
}
