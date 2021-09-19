using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
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
            //одна задача
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
            //много задач
            else
            {
                // /\
                var isCrishab = isCrisha(orderList, reqFloor, currentFloor);
                if (isCrisha(orderList) || isCrishab)
                {
                    var lists = new List<List<int>>();
                    lists.Add(new List<int>());//добавили лист0
                    lists.Add(new List<int>());//доьбавили лист1
                    Console.WriteLine("До добавления в крышу " + string.Join(",", orderList.Select(x => x + 1).ToArray()));
                    if (isCrishab)
                    {
                        lists[0] = orderList;
                        lists[1] = new List<int>();
                    }
                    else
                    {
                        lists = SplitListByPeak(orderList);
                    }
                    Console.WriteLine("Лист 0: " + string.Join(",", lists[0].Select(x => x + 1).ToArray()));
                    Console.WriteLine("Лист 1: " + string.Join(",", lists[1].Select(x => x + 1).ToArray()));
                    if (reqFloor < currentFloor)
                    {
                        lists[1].Add(reqFloor);
                        
                        lists[1].Sort();
                        lists[1].Reverse();
                        //Console.WriteLine("Лист 1: " + string.Join(",", lists[1].Select(x => x + 1).ToArray()));
                    }
                    else
                    {
                        lists[0].Add(reqFloor);
                        lists[0].Sort();
                        //Console.WriteLine("Лист 0: " + string.Join(",", lists[0].Select(x => x + 1).ToArray()));
                    }
                    
                    lists[0].AddRange(lists[1]);
                    Console.WriteLine("Крыша " + string.Join(",", lists[0].Select(x => x + 1).ToArray()));
                    return lists[0];
                }
                // /
                else if (NextGreaterThenPrevious(orderList) && (direction == Direction.Up || direction == Direction.None))
                {
                    //MessageBox.Show(string.Join(",", orderList.ToArray()));
                    orderList.Add(reqFloor);
                    orderList.Sort();
                    Console.WriteLine("Палка вверх " + string.Join(",", orderList.ToArray()));
                    return orderList;
                }
                // \
                else if (NextLessThenPrevious(orderList) && (direction == Direction.Down || direction == Direction.None))
                {
                    //MessageBox.Show(string.Join(",", orderList.ToArray()));
                    orderList.Add(reqFloor);
                    orderList.Sort();
                    orderList.Reverse();
                    Console.WriteLine("Палка вниз " + string.Join(",", orderList.ToArray()));
                    return orderList;
                }
                
            }

            return new List<int>();
        }

        public List<List<int>> SplitListByPeak(List<int> list)
        {
            var result = new List<List<int>>();
            var lst1 = new List<int>();
            var lst2 = new List<int>();
            result.Add(lst1);
            result.Add(lst2);

            int i = -1;
            do
            {
                i++;
                lst1.Add(list[i]);
            } while (list[i] < list[i + 1]);
            
            
            for (int j = i + 1; j < list.Count; j++)
            {
                lst2.Add(list[j]);
            }
            
            return result;
        }

        public bool NextGreaterThenPrevious(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] < list[i + 1])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        
        public bool NextLessThenPrevious(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] > list[i + 1])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool isCrisha(List<int> list, int? newFloor = null, int? currentFloor = null)
        {
            //крыша с currentflow and newflor
            if (newFloor != null && currentFloor != null)
            {
                bool found = true;
                foreach (var x in list)
                {
                    if (currentFloor > x)
                    {
                        found = false;
                        break;
                    }
                }

                if (newFloor < currentFloor && found)
                {
                    return true;
                }

                return false;
            }
            else
            {
                var ind = list.IndexOf(list.Max());
                if (ind != 0 && ind != list.Count - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

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
                if (lift.OrderList.Count == 0)
                {
                    lift.Direction = Direction.None;
                }
                
                if (lift.OrderList.Count != 0)
                {
                    //MessageBox.Show(lift.OrderList.Count.ToString());
                    //мы приехали
                    if (lift.CurrentFloor == lift.OrderList.First())
                    {
                        lift.State = State.Open;
                        
                        //снять задачу
                        Console.WriteLine("Этаж удален "+ (lift.OrderList.First()+1));
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