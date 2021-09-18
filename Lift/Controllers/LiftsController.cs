using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
                    State = State.Stay,
                    OrderList = new List<int>(),
                    Direction = Direction.Down
                });
            }

            PressedButtons = new List<PressedButton>();
        }
        
        //нужно чтобы знать, какие кнопки нажаты в лифте
        public void AddButtonLift(int liftNum, int floorNum)
        {
            if (!Elevators[liftNum].ActiveButtons.Contains(floorNum))
            {
                Elevators[liftNum].ActiveButtons.Add(floorNum);
            }
        }

        public void HandleButtonInLift(int liftNum, int floorNum)
        {
            var lift = this.Elevators[liftNum];

            var state = lift.State;
            var dir = lift.Direction;

            // нельзя ехать на свой этаж
            if (floorNum == lift.CurrentFloor)
            {
                return;
            }

            lift.OrderList = AddRequestToLiftQueue(lift.OrderList, lift.CurrentFloor, lift.Direction, floorNum);
        }

        public List<int> AddRequestToLiftQueue(List<int> orderList, int currentFloor, Direction direction, int reqFloor)
        {
            var tmpList = new List<int>(){reqFloor};
            if (orderList.Count == 0)
            {
                orderList.AddRange(tmpList);
                return orderList;
            }
            else if(orderList.Count == 1)
            {
                if (direction == Direction.Up)
                {
                    if (reqFloor > currentFloor && reqFloor < orderList[0])
                    {
                        //добавляем перед
                        tmpList.AddRange(orderList);
                        return tmpList;
                    }
                    else
                    {
                        //в конец
                        orderList.AddRange(tmpList);
                        return orderList;
                    }
                }
                //dir Down
                else
                {
                    if (reqFloor < currentFloor && reqFloor > orderList[0])
                    {
                        //добавляем перед
                        tmpList.AddRange(orderList);
                        return tmpList;
                    }
                    else
                    {
                        //в конец
                        orderList.AddRange(tmpList);
                        return orderList;
                    }
                }
            }

            return new List<int>();
        }

        public Direction GetRequestDirection(int currentFloorNum, int requiredFloorNum)
        {
            if (currentFloorNum > requiredFloorNum)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.Up;
            }
        }

        public void RemoveButtonLift(int liftNum, int floorNum)
        {
            if (Elevators[liftNum].ActiveButtons.Contains(floorNum))
            {
                Elevators[liftNum].ActiveButtons.Remove(floorNum);
            }
        }
        public void MoveLift(int liftNum, Direction direction)
        {
            if (Elevators[liftNum].CurrentFloor + 1 <= FloorCount)
            {
                Elevators[liftNum].CurrentFloor += (direction == Direction.Up ? 1 : -1);
            }
            else
            {
                //Console.WriteLine("Floors count is less then expected");
                //MessageBox.Show("Floors count is less then expected");
            }
        }

        public void MainCycle()
        {
            //идем по лифтам и смотрим задачи
            foreach (var lift in Elevators)
            {
                if (lift.State == State.Open)
                {
                    lift.State = State.Stay;
                    continue;
                }
                
                if (lift.OrderList.Count != 0)
                {
                    //мы приехали
                    if (lift.CurrentFloor == lift.OrderList.First())
                    {
                        lift.State = State.Open;
                        //снять задачу
                        lift.OrderList.Remove(lift.OrderList.First());
                        continue;
                    }
                    else if (lift.CurrentFloor < lift.OrderList.First())
                    {
                        lift.Direction = Direction.Up;
                        lift.CurrentFloor++;
                        continue;
                    }
                    else
                    {
                        lift.Direction = Direction.Down;
                        lift.CurrentFloor--;
                        continue;
                    }
                }
            }
        }
    }
}