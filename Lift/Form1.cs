using System;
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
            
            int floorCount = 7;
            int liftCount = 3;
            
            _liftsController = new LiftsController(liftCount, floorCount);
            
            InitInterface(floorCount, liftCount);

            System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
            myTimer.Interval = 1000;

            myTimer.Tick += HandleTicks;
            myTimer.Enabled = true;

            //_liftsController.SplitListByPeak(new List<int>() {5, 6, 2, 7});

            // var threadParameters = new System.Threading.ThreadStart(HandleTicks);
            // var thread2 = new System.Threading.Thread(threadParameters);
            // thread2.Start();
        }
        
        
        private void HandleTicks(object objectInfo, EventArgs e)
        {
            //MessageBox.Show("sdf");
            //Thread.Sleep(1000);
            RedrawSystemState();
            _liftsController.MainCycle();
            
            
            _liftsController.isCrisha(new List<int>(){6,3}, 2, 5);
            /*var cb = Controls.Find("CurFloor1_1", true).First() as RadioButton;
            if(_liftsController.Elevators[2].State ==  State.Open)
                _liftsController.Elevators[2].State = State.Stay;
            else
            {
                _liftsController.Elevators[2].State = State.Open;
            }*/
            
            
        }

        private void RedrawSystemState()
        {
            #region Floor buttons

            //погасить фонарь кнокпи этажей лифтов
            for (int i = 0; i < _liftsController.LiftCount; i++)
            {
                for (int j = 0; j < _liftsController.FloorCount; j++)
                {
                    var rb = Controls.Find($"CurFloor{i}_{j}", true).First() as RadioButton;
                    rb.Invoke((MethodInvoker)delegate
                    {
                        rb.Checked = false;
                       // rb.BackColor = new Color();
                    });
                }
            }

            //зажечь фонарь там, где лифт сечас
            var count = 0;
            foreach (var lift in _liftsController.Elevators)
            {
                var radioButton = Controls.Find($"CurFloor{count}_{lift.CurrentFloor}", true).First() as RadioButton;
                count++;
                radioButton.Checked = true;
              //  radioButton.BackColor = Color.MediumOrchid;
            }

            #endregion

            #region Open door lights

            count = 0;
            //погасить все фонари открытия двери
            for (int i = 0; i < _liftsController.LiftCount; i++)
            {
                var rb = Controls.Find($"light{i}", true).First() as RadioButton;
                rb.Checked = false;
            }
        
            //зажечь фонарь открытия двери, если дверь в лифте открыта
            foreach (var lift in _liftsController.Elevators)
            {
                var radioButton = Controls.Find($"light{count}", true).First() as RadioButton;
                count++;
                if (lift.State == State.Open)
                {
                    radioButton.Checked = true;
                }
            }

            #endregion

            #region Lift buttons

            int liftCounter = 0;
            //если кнопка горит и лифт туда приехал, оффнуть кнопку
            foreach (var lift in _liftsController.Elevators)
            {
                if (lift.OrderList.Contains(lift.CurrentFloor))
                {
                    var btn = Controls.Find($"btnLift{liftCounter}_{lift.CurrentFloor}", true).First() as Button;
                    btn.BackColor = new Color();
                    btn.Enabled = true;
                }

                liftCounter++;
            }
            
            #endregion

            #region Floor buttons

            foreach (var lift in _liftsController.Elevators)
            {
                
            }

            #endregion
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
                FlowDirection = FlowDirection.TopDown,
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
               // rb.Enabled = false;
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
               // Enabled = false,
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
                var liftNum = nums[0];
                var buttonNum = nums[1];
                
                var opSuccess = _liftsController.HandlePressedButtonInLift(liftNum, buttonNum);
                if (opSuccess)
                {
                    _liftsController.TurnOnButtonInLift(liftNum, buttonNum);
                    btn.BackColor = Color.LightCoral;
                    btn.Enabled = false;
                }
            }
            //UP button in a floor 
            else if (btnName.StartsWith("btnFloorUp"))
            {
                var num = Convert.ToInt32(btnName.Substring("btnFloorUp".Length));
                
                var opSuccess = _liftsController.HandlePressedButtonOnFloor(num, Direction.Up);
                if (opSuccess)
                {
                    _liftsController.PressedButtons.Add(new PressedButton()
                    {
                        Direction = Direction.Up,
                        FloorNum = num
                    });
                    btn.BackColor = Color.Aqua;
                    btn.Enabled = false;
                }
                
            }
            //DOWN button in a floor
            else if (btnName.StartsWith("btnFloorDown"))
            {
                var num = Convert.ToInt32(btnName.Substring("btnFloorDown".Length));
                var opSuccess = _liftsController.HandlePressedButtonOnFloor(num, Direction.Down);
                if (opSuccess)
                {
                    _liftsController.PressedButtons.Add(new PressedButton()
                    {
                        Direction = Direction.Down,
                        FloorNum = num
                    });
                    btn.BackColor = Color.Yellow;
                    btn.Enabled = false;
                }
            }

            //MessageBox.Show(btn.Name);
        }

    }
}