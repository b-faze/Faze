﻿using Faze.Abstractions.GameMoves;
using Faze.Abstractions.GameResults;
using Faze.Abstractions.GameStates;
using Faze.Abstractions.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Faze.Examples.Games.GridGames
{
    public class OX3DState : IGameState<GridMove, WinLoseDrawResult?>
    {
        private const int dimention = 3;
        public int Dimention => 3;
        public int TotalPlayers => 2;
        public PlayerIndex CurrentPlayerIndex => p1Turn ? PlayerIndex.P1 : PlayerIndex.P2;

        private List<List<int[]>> winningStates;
        private bool p1Turn = true;
        private bool?[,,] pMoves;
        private List<GridMove> availableMoves;

        public static OX3DState Initial()
        {
            var pMoves = new bool?[dimention, dimention, dimention];
            var availableMoves = Enumerable.Range(0, 9).SelectMany(x => Enumerable.Repeat(x, dimention)).Select(x => new GridMove(x)).ToList();
            var winningStates = GetWinningStates(dimention).ToList();
            return new OX3DState(pMoves, availableMoves, p1Turn: true, winningStates);
        }

        private OX3DState(bool?[,,] pMoves, List<GridMove> availableMoves, bool p1Turn, List<List<int[]>> winningStates)
        {
            this.pMoves = (bool?[,,])pMoves.Clone();
            this.p1Turn = p1Turn;
            this.availableMoves = availableMoves.ToList();
            this.winningStates = winningStates;
        }

        public IEnumerable<GridMove> GetAvailableMoves()
        {
            if (GetResult() != null)
                return new GridMove[0];

            return availableMoves.Distinct().ToList();
        }

        public IGameState<GridMove, WinLoseDrawResult?> Move(GridMove move)
        {
            var moveX = move % Dimention;
            var moveY = move / Dimention;
            var i = 0;
            while (pMoves[moveX, moveY, i].HasValue)
                i++;

            var newPMoves = CloneBoard(pMoves, Dimention);
            var newAvailableMoves = availableMoves.ToList();

            newPMoves[moveX, moveY, i] = p1Turn;
            newAvailableMoves.Remove(move);

            return new OX3DState(newPMoves, newAvailableMoves, !p1Turn, winningStates);
        }

        public WinLoseDrawResult? GetResult()
        {
            foreach (var state in winningStates)
            {
                if (state.All(s => pMoves[s[0], s[1], s[2]] == true)) return WinLoseDrawResult.Win;
                if (state.All(s => pMoves[s[0], s[1], s[2]] == false)) return WinLoseDrawResult.Lose;
            }

            return availableMoves.Count == 0 ? WinLoseDrawResult.Draw : (WinLoseDrawResult?)null;
        }

        public static IEnumerable<List<int[]>> GetWinningStates(int dimention)
        {
            var directions = GetDirections(dimention);
            var states = directions.SelectMany(GetStates);
            return states.Select(l => l.ToList());
        }

        public static IEnumerable<List<int[]>> GetStates(int[] direction)
        {
            var freedom = direction.Count(x => x == 0);
            var dimention = direction.Length;
            if (freedom == 0)
            {
                var res = new List<int[]>();
                for (var i = 0; i < dimention; i++)
                {
                    res.Add(direction.Select((d, di) => TransformDirection(direction, di, dimention, new int[0], i)).ToArray());
                }

                yield return res;
                yield break;
            }

            var fs = GetTransformedFixed(direction);
            foreach (var f in fs)
            {
                var res = new List<int[]>();
                for (var i = 0; i < dimention; i++)
                {
                    res.Add(direction.Select((d, di) => TransformDirection(direction, di, dimention, f, i)).ToArray());
                }
                yield return res;
            }
        }

        private static int TransformDirection(int[] direction, int di, int dimention, int[] f, int i)
        {
            switch (direction[di])
            {
                case -1:
                    return dimention - 1 - i;

                case 0:
                    return f[direction.Take(di).Count(x => x == 0)];

                case 1:
                    return 0 + i;
            }

            throw new Exception("Something went wrong");
        }

        public static IEnumerable<int[]> GetTransformedFixed(int[] direction)
        {
            var freedom = direction.Count(x => x == 0);
            var dimention = direction.Length;
            return FixedTransformations(freedom, dimention);
        }

        private static IEnumerable<int[]> FixedTransformations(int freedom, int dimention)
        {
            if (freedom == 0) return new int[0][];

            var range = Enumerable.Range(0, dimention).ToArray();
            if (freedom == 1) return range.Select(r => new[] { r });

            var fs = FixedTransformations(freedom - 1, dimention).ToArray();
            return range.SelectMany(r => fs.Select(f => f.Concat(new[] { r }).ToArray()));
        }

        public static IEnumerable<int[]> GetDirections(int dimention)
        {
            return GetDirectionsHelper(dimention).TakeWhile(x => x.Any(i => i != 0));
        }

        private static IEnumerable<int[]> GetDirectionsHelper(int dimention)
        {
            if (dimention == 1)
            {
                return Enumerable.Range(-1, 3).Select(x => new[] { x });
            }

            var r = GetDirectionsHelper(dimention - 1);
            return Enumerable.Range(-1, 3).SelectMany(x => r.Select(r2 => r2.Concat(new[] { x }).ToArray()));
        }

        private static bool?[,,] CloneBoard(bool?[,,] arr, int dimension)
        {
            var newArr = new bool?[dimension, dimension, dimension];
            for (var i = 0; i < dimension; i++)
            {
                for (var j = 0; j < dimension; j++)
                {
                    for (var k = 0; k < dimension; k++)
                    {
                        newArr[i, j, k] = arr[i, j, k];
                    }
                }
            }

            return newArr;
        }
    }

}
