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

namespace RestaurantOtomasyon26A
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void Form3_Load(object sender, EventArgs e)
        {
            ButonYuvarla(button1, 20);
            PanelYuvarla(panel1, 20);
            PanelYuvarla(panel2, 20);
            PanelYuvarla(panel3, 20);
            PanelYuvarla(panel4, 20);
            panel1.BackColor = Color.FromArgb(120, Color.Black);
            panel2.BackColor = Color.FromArgb(120, Color.Black);
            panel3.BackColor = Color.FromArgb(120, Color.Black);
            panel4.BackColor = Color.FromArgb(120, Color.Black);
        }
        public void PanelYuvarla(Control panel, int radius)
        {
            int w = panel.Width;
            int h = panel.Height;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            path.AddArc(0, 0, radius, radius, 180, 90);

            path.AddArc(w - radius, 0, radius, radius, 270, 90);

            path.AddArc(w - radius, h - radius, radius, radius, 0, 90);

            path.AddArc(0, h - radius, radius, radius, 90, 90);

            path.CloseAllFigures();

            panel.Region = new Region(path);

        }

        public void ButonYuvarla(Button btn, int radius)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(0, 0, radius, radius, 180, 90);

            gp.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);

            gp.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);

            gp.AddArc(0, btn.Height - radius, radius, radius, 90, 90);

            gp.CloseAllFigures();

            btn.Region = new Region(gp);

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //burdan personelin ıd sini public tanımlayıp diğer formlarda kullanarak personel işlemlerini yapacağız
                public static int PersonelID;

        private void button1_Click(object sender, EventArgs e)
        {
            var kullanici = db.TBLPERSONEL.Where(x => x.KULLANICIADI == textBox1.Text && x.SIFRE == textBox2.Text).FirstOrDefault();
            PersonelID = kullanici.ID;
            if (kullanici != null && kullanici.GOREV == "Garson")
            { 
                Garson g = new Garson();
                g.Show();
            }
            if (kullanici != null && kullanici.GOREV == "Kasiyer")
            {
                Kasiyer k = new Kasiyer();
                k.Show();
            }
            if (kullanici != null && kullanici.GOREV == "Admin")
            {
                EskiAdminPaneli a = new EskiAdminPaneli();
                a.Show();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
