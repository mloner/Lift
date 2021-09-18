using System.Collections.Generic;
using Lift.Controllers;

namespace Lift.Models
{
    public enum DoorState
    {
        Opened,
        Closed
    }
    
    
    public class Elevator
    {
        public DoorState DoorState { get; set; }
        public int CurrentFloor { get; set; }
        public List<int> ActiveButtons { get; set; }

        public Direction Direction { get; set; }

    }
}