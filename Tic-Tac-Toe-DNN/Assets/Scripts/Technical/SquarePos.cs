using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Technical
{
    public struct SquarePos
    {
        public int x;
        public int y;

        public SquarePos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static SquarePos operator -(SquarePos move)
        {
            return new SquarePos(-move.x, -move.y);
        }

        public static SquarePos operator -(SquarePos moveA, SquarePos moveB)
        {
            return new SquarePos(moveA.x - moveB.x, moveA.y - moveB.y);
        }

        public static SquarePos operator +(SquarePos moveA, SquarePos moveB)
        {
            return new SquarePos(moveA.x + moveB.x, moveA.y + moveB.y);
        }

        public static implicit operator SquarePos(Vector2 vector2)
        {
            return new SquarePos((int)vector2.x, (int)vector2.y);
        }
    }
}
