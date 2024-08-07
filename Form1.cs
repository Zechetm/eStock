using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eStock.Models;
using eStock.Core;
using System.Windows.Forms.DataVisualization.Charting;


namespace eStock
{
    public partial class Form1 : Form
    {
        private string api_key = "";
        private string local_search = "";
        private List<Company> portfolio = new List<Company>();

        public Form1()
        {
            InitializeComponent();

            chart1.Titles.Clear();
        }

        //add company
        private async void button1_Click(object sender, EventArgs e)
        {
            if(this.api_key == "")
            {
                return;
            }
            string ta = Microsoft.VisualBasic.Interaction.InputBox("Enter stock name of company: ");
            List<float> prices= await StockInt.Interpret(ta, this.api_key);
            prices.Reverse();
            if (prices.Count == 0)
            {
                MessageBox.Show("Error: Cannot find json file");
                return;
            }
            listBox1.Items.Add(ta);
            Company xyz = new Company(ta, prices);
            this.portfolio.Add(xyz);
        }
        
        //remove company
        private void button2_Click(object sender, EventArgs e)
        {
            string tr = Microsoft.VisualBasic.Interaction.InputBox("Enter stock name of company: ");
            listBox1.Items.Remove(tr);
        }


        //load from this machine
        private void button5_Click(object sender, EventArgs e)
        {
            if(this.api_key == "")
            {
                return;
            }
            DialogResult dr = MessageBox.Show("Use default search path?", "Local", MessageBoxButtons.YesNo);
            string name;
            if (dr == DialogResult.Yes)
            {
                name = Microsoft.VisualBasic.Interaction.InputBox("Enter stock name of company: ");

                List<float> data = LocalLoad.GetLocalData(this.local_search + "\\" +name + ".json");
                if(data.Count == 0)
                {
                    MessageBox.Show("Error: Cannot find json file");
                    return;
                }
                data.Reverse();
                listBox1.Items.Add(name);
                Company xyz = new Company(name, data);
                this.portfolio.Add(xyz);

            }
            else
            {
                name = Microsoft.VisualBasic.Interaction.InputBox("Enter stock name of company: ");
                string path = Microsoft.VisualBasic.Interaction.InputBox("Enter path to Json file: ");
                if (name.Length == 0 || path.Length == 0)
                {
                    return;
                }
                List<float> data = LocalLoad.GetLocalData(path);
                data.Reverse();
                //  Console.WriteLine(data);
                listBox1.Items.Add(name);
                Company xyz = new Company(name, data);
                this.portfolio.Add(xyz);
            }
            }

        //DO NOT TOUCH
        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void UpdateChart(string item)
        {
            chart1.Series.Clear();

            Series s = new Series(item);
            s.ChartType = SeriesChartType.Line;

  
            int i = 0;
            for(; i<this.portfolio.Count; i++)
            {
                if (this.portfolio[i].name == item) { break; }
            }

            float min = this.portfolio[i].course[0];
            float max = 0;

            float cur;
            for (int j = 0; j < this.portfolio[i].course.Count; j++)
            {
                cur = this.portfolio[i].course[j];
                s.Points.AddXY(j+1, cur);

                if (cur < min)
                {
                    min = cur;
                }
                if(cur > max) { max = cur; }

            }

            chart1.Series.Add(s);
            chart1.Titles.Clear();
            chart1.Titles.Add($"{item} course");


            var chartScale = chart1.ChartAreas[0];

            //back
            chartScale.BackColor = Color.Black;
            //grid
            chartScale.AxisY.MajorGrid.LineColor = Color.Red;


            chartScale.AxisY.Minimum = min - 15;
            chartScale.AxisY.Maximum = max + 15;
//            chartScale.AxisY.Interval = 10;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            string p = listBox1.SelectedItem as string;
            if (p != null)
            {
                UpdateChart(p);
            }
        }


        //load chart
        private void button6_Click(object sender, EventArgs e)
        {
            string p = listBox1.SelectedItem as string;
            Console.WriteLine("!!!!!!!!");
            if (p != null)
            {
                UpdateChart(p);
            }
        }
        //Change API key
        private void button4_Click(object sender, EventArgs e)
        {

        }

        //Show API key
        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to view API key?", "Privacy warning", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show(this.api_key, "API key");
            }
        }

        
        //local machine search path
        private void button10_Click(object sender, EventArgs e)
        {
            string path = Microsoft.VisualBasic.Interaction.InputBox("Enter path for local machine: ");
            this.local_search = path;
        }
    }
}
