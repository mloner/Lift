using System.Collections.Generic;

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

        public void OpenDoor()
        {
            DoorState = DoorState.Opened;
        }
        
        public void CloseDoor()
        {
            DoorState = DoorState.Closed;
        }
        
        public void AddActiveButton(int number)
        {
            ActiveButtons.Add(number);
        }

        public void RemoveActiveButton(int number)
        {
            ActiveButtons.Remove(number);
        }
    }
}