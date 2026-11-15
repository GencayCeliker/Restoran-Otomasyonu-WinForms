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
    public partial class frmSatışAnalizi : Form
    {
        public frmSatışAnalizi()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void frmSatışAnalizi_Load(object sender, EventArgs e)
        {
            var masalar = from x in db.TBLMASA
                          where x.AKTIFMI == true
                          select x;
            comboBox2.DataSource = masalar.ToList();
            comboBox2.DisplayMember = "MASAID";
            comboBox2.ValueMember = "MASAID";

            var personeller = from x in db.TBLPERSONEL
                              where x.DURUM == true
                              select x;
            comboBox1.DataSource = personeller.ToList();
            comboBox1.DisplayMember = "ADSOYAD";
            comboBox1.ValueMember = "ID";

            //form açıldığında bugünkü satışlar listelensin
            var satislar = from x in db.TBLSATISLAR
                           where x.TBLADISYON.TARIH == DateTime.Today
                           select new
                           {
                               x.ID,
                               x.TBLURUN.URUNAD,
                               x.ADET,
                               x.TBLURUN.FIYAT,
                               Toplam = x.ADET * x.TBLURUN.FIYAT
                           };
            dataGridView1.DataSource = satislar.ToList();

            label3.Text = Convert.ToDecimal(satislar.Sum(x => x.Toplam)).ToString("C2");

            label8.Text = Convert.ToDecimal(satislar.Sum(x => x.Toplam)).ToString("C2");


            //form açıldığında en çok satılan 5 ürün gelsin
            var encoksatilanlar = db.TBLSATISLAR.Where(x => x.TBLADISYON.TARIH == DateTime.Today).GroupBy(x => x.TBLURUN.URUNAD).Select(g => new
            {
                Urun = g.Key,
                ToplamAdet = g.Sum(x => x.ADET)
            }).OrderByDescending(x => x.ToplamAdet).Take(5);

            dataGridView4.DataSource = encoksatilanlar.ToList();

            //form açıldığında bugünün adisyon adetini yaz

            var adisyonlar = db.TBLADISYON.Where(x => x.TARIH == DateTime.Today).ToList();

            label10.Text = adisyonlar.Count.ToString();

            //bugüne ait en aktif masa

            var aktifmasa = db.TBLADISYON.Where(x => x.TARIH == DateTime.Today).GroupBy(x => x.MASAID).Select(g => new
            {
                Masa = g.Key,
                ToplamAdet = g.Count()
            }).OrderByDescending(x => x.ToplamAdet).FirstOrDefault();

            //label12.Text= "(Masa" + aktifmasa.Masa  + ")_" + aktifmasa.ToplamAdet.ToString() + "\n kez kullanıldı" ;

            //en aktif müşteri
            var aktifmust = db.TBLMUSTERI.OrderByDescending(x => x.SIPARISSAYISI).ToList();

            label14.Text = aktifmust.FirstOrDefault().ADSOYAD;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime ilktarih = Convert.ToDateTime(dateTimePicker1.Value.ToShortDateString());

            DateTime sontarih = Convert.ToDateTime(dateTimePicker2.Value.ToShortDateString());
            var satislar = from x in db.TBLSATISLAR
                           where x.TBLADISYON.TARIH >= ilktarih && x.TBLADISYON.TARIH <= sontarih
                           select new
                           {
                               x.ID,
                               x.TBLURUN.URUNAD,
                               x.ADET,
                               x.TBLURUN.FIYAT,
                               Toplam = x.ADET * x.TBLURUN.FIYAT
                           };
            dataGridView1.DataSource = satislar.ToList();

            label3.Text = satislar.Sum(x => x.Toplam).ToString() + "₺";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int perno = Convert.ToInt32(comboBox1.SelectedValue);

            var personelsatislar = from x in db.TBLSATISLAR
                                   where x.TBLADISYON.PERSONELID == perno && x.TBLADISYON.TARIH == DateTime.Today
                                   select new
                                   {
                                       x.ID,
                                       x.TBLURUN.URUNAD,
                                       x.ADET
                                   };
            dataGridView2.DataSource = personelsatislar.ToList();

            label16.Text = personelsatislar.Count().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int masaID = Convert.ToInt32(comboBox2.SelectedValue);

            DateTime bugun = DateTime.Today;
            DateTime yarin = DateTime.Today.AddDays(1);

            var masasatislar = from x in db.TBLSATISLAR
                               where x.MASAID == masaID && x.TBLADISYON.TARIH >= bugun && x.TBLADISYON.TARIH < yarin
                               select new
                               {
                                   x.ID,
                                   x.TBLURUN.URUNAD,
                                   x.ADET,
                                   x.TBLURUN.FIYAT,
                                   Toplam = x.ADET * x.TBLURUN.FIYAT
                               };
            dataGridView3.DataSource = masasatislar.ToList();

            label5.Text = Convert.ToDecimal(masasatislar.Sum(x => x.Toplam)).ToString("C2");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
