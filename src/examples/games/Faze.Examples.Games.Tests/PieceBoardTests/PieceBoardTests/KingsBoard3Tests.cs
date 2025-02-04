﻿using Faze.Abstractions.GameMoves;
using Faze.Abstractions.GameResults;
using Faze.Abstractions.GameStates;
using Faze.Examples.Games.GridGames.Pieces;
using Faze.Utilities.Testing;
using System;
using System.Linq;
using Xunit;

namespace Faze.Examples.Games.GridGames.Tests
{
    public class KingsBoard3Tests
    {
        private const int Dimension = 3;
        private GameStateTestingService<GridMove, SingleScoreResult?> gameStateTestingService;

        public KingsBoard3Tests()
        {
            var gameStateTestingServiceConfig = new GameStateTestingServiceConfig<GridMove, SingleScoreResult?>(StateFactory);
            this.gameStateTestingService = new GameStateTestingService<GridMove, SingleScoreResult?>(gameStateTestingServiceConfig);
        }

        private IGameState<GridMove, SingleScoreResult?> StateFactory()
        {
            var config = new PiecesBoardStateConfig(Dimension, new KingPiece());
            return new PiecesBoardState(config);
        }


        [Fact]
        public void CorrectInitialMoves()
        {
            var expected = Enumerable.Range(0, Dimension * Dimension).Select(i => new GridMove(i));

            gameStateTestingService.TestInitialAvailableMoves(expected);
        }

        [Fact]
        public void Game1()
        {
            var moves = new GridMove[] { 4 };
            var expectedResult = new SingleScoreResult(1);

            gameStateTestingService.TestMovesForResult(moves, expectedResult);
        }

        [Fact]
        public void Game2()
        {
            var moves = new GridMove[] { 0, 2, 6, 8 };
            var expectedResult = new SingleScoreResult(4);

            gameStateTestingService.TestMovesForResult(moves, expectedResult);
        }

        [Fact]
        public void Game2Fail()
        {
            var moves = new GridMove[] { 0, 2, 6, 7 };
            var expectedResult = new SingleScoreResult(-1);

            gameStateTestingService.TestMovesForResult(moves, expectedResult);
        }

        [Fact]
        public void Game3()
        {
            var moves = new GridMove[] { 1, 7 };
            var expectedResult = new SingleScoreResult(2);

            gameStateTestingService.TestMovesForResult(moves, expectedResult);
        }

        [Fact]
        public void Game3Fail()
        {
            var moves = new GridMove[] { 1, 5 };
            var expectedResult = new SingleScoreResult(-1);

            gameStateTestingService.TestMovesForResult(moves, expectedResult);
        }
    }
}
