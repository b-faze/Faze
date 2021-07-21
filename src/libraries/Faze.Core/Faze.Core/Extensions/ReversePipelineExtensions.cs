﻿using Faze.Abstractions.Core;
using Faze.Abstractions.GameStates;
using Faze.Abstractions.Rendering;
using Faze.Core.Extensions;
using Faze.Core.TreeLinq;
using System;
using System.Drawing;

namespace Faze.Core.Extensions
{
    public static class ReversePipelineExtensions
    {
        public static IReversePipelineBuilder<IPaintedTreeRenderer> File(this IReversePipelineBuilder builder, string filename) 
        {
            return builder.Require<IPaintedTreeRenderer>(renderer => renderer.SaveToFile(filename));
        }

        public static IReversePipelineBuilder<Tree<Color>> Render(this IReversePipelineBuilder<IPaintedTreeRenderer> builder, IPaintedTreeRenderer renderer)
        {
            return builder.Require<Tree<Color>>(tree =>
            {
                renderer.Draw(tree);
                return renderer;
            });
        }

        public static IReversePipelineBuilder<Tree<T>> Paint<T>(this IReversePipelineBuilder<Tree<Color>> builder, ITreePainter painter)
        {
            return builder.Require<Tree<T>>(tree =>
            {
                return painter.Paint(tree);
            });
        }

        public static IReversePipelineBuilder<Tree<T>> Paint<T>(this IReversePipelineBuilder<Tree<Color>> builder, ITreePainter<T> painter)
        {
            return builder.Require<Tree<T>>(tree =>
            {
                return painter.Paint(tree);
            });
        }

        public static IReversePipelineBuilder<Tree<double>> Paint(this IReversePipelineBuilder<Tree<Color>> builder, IColorInterpolator colorInterpolator)
        {
            return builder.Require<Tree<double>>(tree =>
            {
                return tree.MapValue(colorInterpolator);
            });
        }

        public static IReversePipelineBuilder<Tree<TIn>> Map<TOut, TIn>(this IReversePipelineBuilder<Tree<TOut>> builder, Func<Tree<TIn>, Tree<TOut>> fn)
        {
            return builder.Require(fn);
        }

        public static IReversePipelineBuilder<Tree<TIn>> Map<TOut, TIn>(this IReversePipelineBuilder<Tree<TOut>> builder, ITreeMapper<TIn, TOut> mapper)
        {
            return builder.Require<Tree<TIn>>((tree, progress) =>
            {
                return mapper.Map(tree, progress);
            });
        }

        public static IReversePipelineBuilder<Tree<T>> LimitDepth<T>(this IReversePipelineBuilder<Tree<T>> builder, int maxDepth)
        {
            return builder.Require<Tree<T>>(tree =>
            {
                return tree.LimitDepth(maxDepth);
            });
        }

        public static IReversePipelineBuilder<IGameState<TMove, TResult>> GameTree<TMove, TResult>(this IReversePipelineBuilder<Tree<IGameState<TMove, TResult>>> builder, IGameStateTreeAdapter<TMove> adapter = null)
        {
            return builder.Require<IGameState<TMove, TResult>>(state =>
            {
                return adapter != null 
                    ? state.ToStateTree(adapter)
                    : state.ToStateTree();
            });
        }        
        
        public static IPipeline LoadTree<TId, T>(this IReversePipelineBuilder<Tree<T>> builder, TId id, ITreeDataProvider<TId, T> treeDataProvider)
        {
            return builder.Build(() =>
            {
                return TreeFileExtensions.Load(id, treeDataProvider);
            });
        }

        public static IReversePipelineBuilder<Tree<T>> SaveTree<TId, T>(this IReversePipelineBuilder builder, TId id, ITreeDataStore<TId,T> treeDataStore)
        {
            return builder.Require<Tree<T>>(tree =>
            {
                TreeFileExtensions.Save(tree, id, treeDataStore);
            });
        }


    }
}
