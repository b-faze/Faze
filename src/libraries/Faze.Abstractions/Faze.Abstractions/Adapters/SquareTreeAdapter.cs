﻿using Faze.Abstractions.Core;
using Faze.Abstractions.GameMoves;
using Faze.Abstractions.GameStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Faze.Abstractions.Adapters
{
    public class SquareTreeAdapter : IGameStateTreeAdapter<GridMove>
    {
        private readonly int size;

        public SquareTreeAdapter(int size)
        {
            this.size = size;
        }

        public IEnumerable<IGameState<GridMove, TResult>> GetChildren<TResult>(IGameState<GridMove, TResult> state)
        {
            var result = new IGameState<GridMove, TResult>[size * size];

            foreach (var move in state.GetAvailableMoves())
            {
                result[move] = state.Move(move);
            }

            return result;
        }
    }
}
