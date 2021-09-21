using System.Collections.Generic;
using Lift.Controllers;

namespace Lift.Models
{
    public enum State
    {
        Open,
        Stay,
        Move
    }
    
    
    public class Elevator
    {
        public int Id { get; set; }
        public State State { get; set; }
        public int CurrentFloor { get; set; }
        public List<int> ActiveButtons { get; set; }

        public int Distance { get; set; }
        public List<int> OrderList { get; set; }

        public Direction Direction { get; set; }

    }
}