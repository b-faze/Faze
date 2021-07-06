﻿/*
ChessLib, a chess data structure library

MIT License

Copyright (c) 2017-2020 Rudy Alex Kohn

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Rudz.Chess.Enums
{
    using System.Runtime.CompilerServices;

    public enum MoveTypes : ushort
    {
        Normal = 0,
        Promotion = 1 << 14,
        Enpassant = 2 << 14,
        Castling = 3 << 14
    }

    public static class MoveTypesExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsInt(this MoveTypes @this) => (int)@this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlagFast(this MoveTypes value, MoveTypes flag) => (value & flag) != 0;
    }
}