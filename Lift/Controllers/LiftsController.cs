using System.Collections.Generic;
using Lift.Models;

namespace Lift.Controllers
{
    public class LiftsController
    {
        public int FloorCount { get; set; }
        public int LiftCount { get; set; }
        public List<Elevator> Elevators { get; set; }

        public LiftsController()
        {
            Elevators = new List<Elevator>();
            for (int i = 0; i < LiftCount; i++)
            {
                Elevators.Add(new Elevator());
            }
            
        }
    }
}