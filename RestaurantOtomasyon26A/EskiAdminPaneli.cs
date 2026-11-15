using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantOtomasyon26A
{
    public partial class EskiAdminPaneli : Form
    {
        public EskiAdminPaneli()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void AdminPaneli_Load(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcAdmin dashboardSayfasi = new UcAdmin();
            dashboardSayfasi.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(dashboardSayfasi);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pnlEkran_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
