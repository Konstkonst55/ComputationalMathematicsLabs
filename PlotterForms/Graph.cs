using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ScottPlot;
using Color = System.Drawing.Color;

namespace PlotterForms
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();
            InitializePlot();
        }

        private readonly double _axisLimit = 100;
        private readonly FormsPlot formsPlot = new FormsPlot();

        private void InitializePlot()
        {
            formsPlot.Dock = DockStyle.Fill;
            Controls.Add(formsPlot);
            CenterAxes();
        }

        private void CenterAxes()
        {
            formsPlot.Plot.SetAxisLimits(-10, 10, -10, 10);

            formsPlot.Plot.AddLine(-_axisLimit, 0, _axisLimit, 0, Color.Black, 1); 
            formsPlot.Plot.AddLine(0, -_axisLimit, 0, _axisLimit, Color.Black, 1);

            formsPlot.Plot.XAxis.Ticks(true);
            formsPlot.Plot.YAxis.Ticks(true);

            formsPlot.Render();
        }

        public void PlotPoints(GraphParameters param)
        {
            CenterAxes();

            double[] xs = param.Points.Select(p => p.x).ToArray();
            double[] ys = param.Points.Select(p => p.y).ToArray();

            formsPlot.Plot.AddScatter(xs, ys, markerSize: param.MarkerSize, lineWidth: param.LineWidth, color: param.Color);
            formsPlot.Render();
        }

        public void PlotSinglePoint(double x, double y, float markerSize = 5)
        {
            formsPlot.Plot.AddPoint(x, y, Color.Red, markerSize);
            formsPlot.Render();
        }
    }
}
