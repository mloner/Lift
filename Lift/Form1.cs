using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lift
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var flp = new FlowLayoutPanel()
            {
                Size = new Size()
                {
                    Height = this.Height,
                    Width = this.Width
                },
                FlowDirection = FlowDirection.BottomUp,
                Dock = DockStyle.Bottom
            };
            
            this.Controls.Add(flp);
            
            
            int floorsCount = 6;
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
                    upButton.Name = $"btnUpFloor{currentFloor}";
                    upButton.AutoSize = true;
                    gbFp.Controls.Add(upButton);
                }
                //down
                if (currentFloor != 0)
                {
                    var downButton = new Button();
                    downButton.Text = "Down";
                    downButton.Name = $"btnDownFloor{currentFloor}";
                    downButton.AutoSize = true;
                    gbFp.Controls.Add(downButton);
                }
                
                gb.Controls.Add(gbFp);
                
                flp.Controls.Add(gb);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}