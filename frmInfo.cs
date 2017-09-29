using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Prj_Simulador_B01
{
    public partial class FrmInfo : Form
    {
        public FrmInfo()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
            //int hra = trackBar1.Value / 60;
            //int mn = trackBar1.Value - (hra * 60);
            //string min;

            //if (mn < 10)
            //    min = "0" + mn.ToString();
            //else
            //    min = mn.ToString();

            //if(hra < 10)
            //    lblHra.Text = "0"+hra.ToString() + ":" + min;
            //else
            //    lblHra.Text =  hra.ToString() + ":" + min;

        }
        public void recebeDados()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //CultureInfo provider = CultureInfo.InvariantCulture;
            //DateTime md = DateTime.Parse("00:00");
            //gtr.Series[0].Points.AddXY(md.ToString("HH:mm:ss"), 34);
            
            //CultureInfo p = CultureInfo.InstalledUICulture;
            //DateTime mydt;
            //for(int n = 0; n <= 23; n++)
            //{
            //    mydt = DateTime.Parse(n.ToString() + ":00");
            //    gtr.Series[0].Points.AddXY(mydt.ToString("HH:mm:ss"), 20 + n);
            //    gtr.Series[1].Points.AddXY(mydt.ToString("HH:mm:ss"), 23 + n);
            //}

            
        }
        ToolTip tt = new ToolTip();
        Point tl = Point.Empty;

        private void gtr_MouseMove(object sender, MouseEventArgs e)
        {
            //if (tt == null) tt = new ToolTip();

            //ChartArea ca = gtr.ChartAreas[0];

            //if (InnerPlotPositionClientRectangle(gtr, ca).Contains(e.Location))
            //{

            //    Axis ax = ca.AxisX;
            //    Axis ay = ca.AxisY;
            //    double x = ax.PixelPositionToValue(e.X);
            //    double y = ay.PixelPositionToValue(e.Y);

            //    DateTime dt;
            //    string s = DateTime.FromOADate(x).ToShortDateString();
            //    string z = DateTime.FromOADate(x).ToShortTimeString();
            //    if (e.Location != tl)
            //        tt.SetToolTip(gtr, string.Format("X={0} ; \n Y={1:0.00}", z, y));
            //    tl = e.Location;
            //}
            //else tt.Hide(gtr);
        }
        RectangleF ChartAreaClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }

        RectangleF InnerPlotPositionClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF IPP = CA.InnerPlotPosition.ToRectangleF();
            RectangleF CArp = ChartAreaClientRectangle(chart, CA);

            float pw = CArp.Width / 100f;
            float ph = CArp.Height / 100f;

            return new RectangleF(CArp.X + pw * IPP.X, CArp.Y + ph * IPP.Y,
                                    pw * IPP.Width, ph * IPP.Height);
        }

        private void btnTerminais_Click(object sender, EventArgs e)
        {

        }
    }
}
