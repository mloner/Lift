﻿namespace Lift.Controllers
{
    public enum Direction
    {
        Up,
        Down
    }
    public class PressedButton
    {
        public Direction Direction { get; set; }
        public int FloorNum { get; set; }
    }
}