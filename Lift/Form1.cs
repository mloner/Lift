﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lift.Controllers;

namespace Lift
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int floorCount = 4;
            int liftCount = 3;
            
            InitInterface(floorCount, liftCount);

            var controller = new LiftsController()
            {
                FloorCount = floorCount,
                LiftCount = liftCount
            };


        }

        public void InitInterface(int floorCount, int liftCount)
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

        public void CreateBuilding(FlowLayoutPanel layout, int floorsCount)
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

        public void CreateLifts(FlowLayoutPanel layout, int liftCount, int floorsCount)
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
        
        public void CreateLift(FlowLayoutPanel layout, int floorsCount, int liftNum)
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
                Enabled = false
            };
            layout.Controls.Add(rbOpenDoorLight);

        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;

            MessageBox.Show(btn.Name);
        }

    }
}