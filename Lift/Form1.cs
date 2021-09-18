﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lift.Controllers;
using Lift.Models;

namespace Lift
{
    public partial class Form1 : Form
    {
        private LiftsController _liftsController;
        public Form1()
        {
            InitializeComponent();
            
            int floorCount = 5;
            int liftCount = 3;
            
            _liftsController = new LiftsController(liftCount, floorCount);
            
            InitInterface(floorCount, liftCount);

            
            var threadParameters = new System.Threading.ThreadStart(HandleTicks);
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
        }

        public void HandleTicks()
        {
            while (true)
            {
                Thread.Sleep(1500);

                RedrawSystemState();
                //_liftsController.MainCycle();
                
                
                /*_liftsController.MoveLift(0, Direction.Up);
                var cb = Controls.Find("CurFloor1_1", true).First() as RadioButton;
                if(_liftsController.Elevators[2].State ==  State.Open)
                    _liftsController.Elevators[2].State = State.Stay;
                else
                {
                    _liftsController.Elevators[2].State = State.Open;
                }*/
            }
            
        }

        private void RedrawSystemState()
        {
            //Дрочим одно
            for (int i = 0; i < _liftsController.LiftCount; i++)
            {
                for (int j = 0; j < _liftsController.FloorCount; j++)
                {
                    var rb = Controls.Find($"CurFloor{i}_{j}", true).First() as RadioButton;
                    rb.Checked = false;
                }
            }

            var count = 0;
            foreach (var lift in _liftsController.Elevators)
            {
                var radioButton = Controls.Find($"CurFloor{count}_{lift.CurrentFloor}", true).First() as RadioButton;
                count++;
                radioButton.Checked = true;
            }

            count = 0;
            //Дрочим другое
            for (int i = 0; i < _liftsController.LiftCount; i++)
            {
                var rb = Controls.Find($"light{i}", true).First() as RadioButton;
                rb.Checked = false;
            }
            
            foreach (var lift in _liftsController.Elevators)
            {
                var radioButton = Controls.Find($"light{count}", true).First() as RadioButton;
                count++;
                if (lift.State == State.Open)
                {
                    radioButton.Checked = true;
                }
            }
        }
        

        private void InitInterface(int floorCount, int liftCount)
        {
            var mainLayout = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            
            this.Controls.Add(mainLayout);
            CreateBuilding(mainLayout, floorCount);
            CreateLifts(mainLayout, liftCount, floorCount);
        }

        private void CreateBuilding(FlowLayoutPanel layout, int floorsCount)
        {
            var flp = new FlowLayoutPanel()
            {
                Size = new Size()
                {
                    Height = floorsCount * 100 + 200,
                    Width = 250 + 10
                },
                FlowDirection = FlowDirection.BottomUp,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray
            };
            layout.Controls.Add(flp);
            
            for (int currentFloor = 0; currentFloor < floorsCount; currentFloor++)
            {
                GroupBox gb = new GroupBox();
                
                gb.Size = new Size(250, 100);
                gb.Name = $"gbFloor{currentFloor}";
                gb.Text = $"Floor {currentFloor + 1}";

                var gbFp = new FlowLayoutPanel()
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown
                };
                
                //add buttons
                //up
                if (currentFloor != floorsCount - 1)
                {
                    var upButton = new Button();
                    upButton.Text = "Up";
                    upButton.Name = $"btnFloorUp{currentFloor}";
                    upButton.AutoSize = true;
                    gbFp.Controls.Add(upButton);
                    upButton.Click += new EventHandler(this.btn_Clicked);
                }
                //down
                if (currentFloor != 0)
                {
                    var downButton = new Button();
                    downButton.Text = "Down";
                    downButton.Name = $"btnFloorDown{currentFloor}";
                    downButton.AutoSize = true;
                    gbFp.Controls.Add(downButton);
                    downButton.Click += new EventHandler(this.btn_Clicked);
                }
                
                gb.Controls.Add(gbFp);
                
                flp.Controls.Add(gb);
            }
        }

        private void CreateLifts(FlowLayoutPanel layout, int liftCount, int floorsCount)
        {
            for (int i = 0; i < liftCount; i++)
            {
                var liftLayout = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.BottomUp,
                    
                    Dock = DockStyle.Bottom,
                    Size = new Size()
                    {
                        Height = floorsCount * 100 + 200,
                        Width = 100
                    }
                };
                
                CreateLift(liftLayout, floorsCount, i);
                
                layout.Controls.Add(liftLayout);
            }
            
        }
        
        private void CreateLift(FlowLayoutPanel layout, int floorsCount, int liftNum)
        {
            //lift buttons
            var liftLayout = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown
            };
            
            for (int i = 0; i < floorsCount; i++)
            {
                var rbFlp = new FlowLayoutPanel()
                {
                    Size = new Size()
                    {
                        Height = 100,
                        Width = 50
                    },
                    //BackColor = Color.Black
                };
                
                var rb = new RadioButton();
                rb.Name = $"CurFloor{liftNum}_{i}";
                rb.Enabled = false;
                rbFlp.Controls.Add(rb);
                layout.Controls.Add(rbFlp);

                // кнрпки внутри лифта
                var btn = new Button()
                {
                    Name = $"btnLift{liftNum}_{i}",
                    Text = $"{i+1}",
                    Size = new Size()
                    {
                        Height = 25,
                        Width = 25
                    },
                };
                btn.Click += new System.EventHandler(this.btn_Clicked);
                
                liftLayout.Controls.Add(btn);
            }
            
            layout.Controls.Add(liftLayout);

            var rbOpenDoorLight = new RadioButton()
            {
                Enabled = false,
                Name = $"light{liftNum}"
            };
            layout.Controls.Add(rbOpenDoorLight);

        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            var btn = (Button)sender;

            var btnName = btn.Name;
            
            //button in a lift
            if (btnName.StartsWith("btnLift"))
            {
                var nums = btnName.Substring("btnLift".Length).Split('_').Select(x => Convert.ToInt32(x)).ToList();
                _liftsController.AddButtonLift(nums[0], nums[1]);
                btn.BackColor = Color.Gray;
                if (_liftsController.Elevators[nums[0]].CurrentFloor > nums[1])
                {
                    _liftsController.CommandList.Add(new PressedButton()
                    {
                        Direction = Direction.Down,
                        FloorNum = nums[1]
                    });
                }
                else if(_liftsController.Elevators[nums[0]].CurrentFloor < nums[1])
                {
                    _liftsController.CommandList.Add(new PressedButton()
                    {
                        Direction = Direction.Up,
                        FloorNum = nums[1]
                    });
                }
            }
            //UP button in a floor 
            else if (btnName.StartsWith("btnFloorUp"))
            {
                var num = Convert.ToInt32(btnName.Substring("btnFloorUp".Length));
                _liftsController.PressedButtons.Add(new PressedButton()
                {
                    Direction = Direction.Up,
                    FloorNum = num
                });
                btn.BackColor = Color.Yellow;
            }
            //DOWN button in a floor
            else if (btnName.StartsWith("btnFloorDown"))
            {
                var num = Convert.ToInt32(btnName.Substring("btnFloorDown".Length));
                _liftsController.PressedButtons.Add(new PressedButton()
                {
                    Direction = Direction.Down,
                    FloorNum = num
                });
                btn.BackColor = Color.Yellow;
            }

            //MessageBox.Show(btn.Name);
        }

    }
}