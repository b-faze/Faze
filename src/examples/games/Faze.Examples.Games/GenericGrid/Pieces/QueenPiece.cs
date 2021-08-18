﻿using Faze.Abstractions.GameMoves;
using Faze.Abstractions.GameResults;
using Faze.Abstractions.GameStates;
using Faze.Abstractions.Players;
using Faze.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Faze.Examples.Games.GridGames.Pieces
{
    public class QueenPiece : IPiece
    {
        public IEnumerable<GridMove> GetPieceMoves(int posIndex, int dimension)
        {
            var x = posIndex % dimension;
            var y = posIndex / dimension;

            var horizontal = Enumerable.Range(0, dimension).Select(i => new GridMove(i, y, dimension));
            var vertical = Enumerable.Range(0, dimension).Select(i => new GridMove(x, i, dimension));
            var diagonals = SquareGridUtilities.GetDiagonals((x, y), dimension).Select(p => new GridMove(p.x, p.y, dimension));

            return horizontal.Concat(vertical).Concat(diagonals);
        }


    }
}