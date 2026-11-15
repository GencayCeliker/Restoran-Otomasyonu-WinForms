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
    public partial class Kasiyer : Form
    {
        public Kasiyer()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void Kasiyer_Load(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ButonYuvarla(button1, 20);
            form2.ButonYuvarla(button2, 20);
            form2.ButonYuvarla(button3, 20);
            form2.ButonYuvarla(button4, 20);
            form2.ButonYuvarla(button5, 20);
            form2.ButonYuvarla(button6, 20);
            form2.ButonYuvarla(button11, 20);
            form2.ButonYuvarla(button12, 20);
            form2.ButonYuvarla(button9, 20);
            form2.ButonYuvarla(button10, 20);
            form2.PanelYuvarla(panel1, 20);
            //burda label1 e personelin hoşgeldiniz yazısı gelecek ve personel adı gelecek
            var personel = db.TBLPERSONEL.SingleOrDefault(x => x.ID == Form3.PersonelID);
            if (personel != null) label1.Text = "Hoşgeldiniz, " + personel.ADSOYAD;

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
