using System.Collections.Generic;
using Lift.Models;

namespace Lift.Controllers
{
    public class LiftsController
    {
        public int FloorCount { get; set; }
        public int LiftCount { get; set; }
        public List<Elevator> Elevators { get; set; }
        public List<PressedButton> PressedButtons { get; set; }
        public LiftsController(int liftCount, int floorCount)
        {
            LiftCount = liftCount;
            FloorCount = floorCount;
            Elevators = new List<Elevator>();
            for (int i = 0; i < LiftCount; i++)
            {
                Elevators.Add(new Elevator()
                {
                    ActiveButtons = new List<int>(),
                    CurrentFloor = 0,
                    DoorState = DoorState.Closed
                });
            }

            PressedButtons = new List<PressedButton>();
        }
        public void AddButtonLift(int liftNum, int floorNum)
        {
            if (!Elevators[liftNum].ActiveButtons.Contains(floorNum))
            {
                Elevators[liftNum].ActiveButtons.Add(floorNum);
            }
        }
        public void RemoveButtonLift(int liftNum, int floorNum)
        {
            if (Elevators[liftNum].ActiveButtons.Contains(floorNum))
            {
                Elevators[liftNum].ActiveButtons.Remove(floorNum);
            }
        }
        
    }
}