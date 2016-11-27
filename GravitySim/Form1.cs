using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GravitySim
{
    public partial class Form1 : Form
    {
        private Simulation _sim = new Simulation();
        private Timer _timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            _timer.Tick += _timer_Tick;
            _timer.Interval = 200;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _panel.Invalidate();
            _txtParticleCount.Text = _sim.Items.Count().ToString();
            _txtAM.Text = _sim.Items.AngularMomentum().Magnitude.ToString();
            _txtEnergy.Text = _sim.Items.Select(p => p.Energy).Sum().Value.ToString();
            _txtMomentum.Text = _sim.Items.Momentum().Magnitude.ToString();
        }

        private void drawParticle(Graphics g, Particle p)
        {
            if (p.Position.Z > Simulation.Size.Value || p.Position.Z < -Simulation.Size.Value)
            {
                return;
            }

            var color = Color.FromArgb((int)(255 * (1 - getSpacePortion(p.Position.QZ + Simulation.Size))), getColor(p.Mass));

            var path = p.History.Select(h => getPixel(h)).ToArray();
            if (path.Any())
            {
                var types = Enumerable.Range(0, path.Length).Select(x => (byte)PathPointType.Line).ToArray();
                g.DrawPath(new Pen(color), new GraphicsPath(path, types));
            }

            var pt = getPixel(p.Position);
            float radius = getPixelLength(p.Radius);
            g.FillEllipse(new SolidBrush(color), pt.X - radius, pt.Y - radius, 2 * radius, 2 * radius);
        }

        private PointF getPixel(Vector3<M> pos)
        {
            return new PointF()
            {
                X = getPixelLength(pos.QX + Simulation.Size),
                Y = getPixelLength(pos.QY + Simulation.Size)
            };
        }

        private float getPixelLength(Q<M> len)
        {
            return (float)(_panel.Width * getSpacePortion(len));
        }

        private double getSpacePortion(Q<M> len)
        {
            return len.X(Simulation.Size.Inv()).X(0.5).Value;
        }

        private Color getColor(Q<KG> mass)
        {
            double x = Math.Log(mass.Value, _sim.Mass.Value);
            return Color.FromArgb(255, (int)(255 * x), 0);
        }

        private void _panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, _panel.Width, _panel.Height);
            var center = getPixel(_sim.Items.CenterOfMass());
            e.Graphics.FillRectangle(Brushes.Blue, center.X - 0.5f, center.Y - 0.5f, 1f, 1f);
            foreach (var p in _sim.Items.OrderBy(pp => pp.Position.Z))
            {
                drawParticle(e.Graphics, p);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 24; i++)
            {
                _sim.Step(new Q<S>(3600));
            }
            _timer.Stop();
            _timer.Start();
        }
    }
}
