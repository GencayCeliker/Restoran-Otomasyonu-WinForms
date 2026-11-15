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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void Dashboard_Load(object sender, EventArgs e)
        {
            //1-bugün en çok satılan ürün
            var sonuc = db.TBLSATISLAR.Where(x => x.TBLADISYON.TARIH == DateTime.Today).GroupBy(g => g.TBLURUN.URUNAD).Select(z => new
            {
                Urun = z.Key,
                Toplam = z.Sum(x => x.ADET)
            }).OrderByDescending(x => x.Toplam).FirstOrDefault();
            if (sonuc != null)
            {
                label2.Text = sonuc.Urun.ToString();
            }


            //2- en çok sipariş veren müşteri
            label3.Text = db.TBLMUSTERI.Where(x => x.DURUM == true).OrderByDescending(x => x.SIPARISSAYISI).FirstOrDefault().ADSOYAD;

            //3-en çok kazandıran ürün
            var sonuc1 = db.TBLSATISLAR.GroupBy(x => x.TBLURUN.URUNAD).Select(g => new
            {
                Urun = g.Key,
                Kazanc = g.Sum(x => x.ADET * x.TBLURUN.FIYAT)
            }).OrderByDescending(x => x.Kazanc).FirstOrDefault();

            label5.Text = sonuc1.Urun.ToString();

            //4- en çok sipariş alan kategori
            var sonuc2 = db.TBLSATISLAR.GroupBy(x => x.TBLURUN.TBLKATEGORI.KATEGORI).Select(g => new
            {
                Kategori = g.Key,
                Adet = g.Sum(x => x.ADET)
            }).OrderByDescending(x => x.Adet).FirstOrDefault();

            label7.Text = sonuc2.Kategori.ToString();

            //5- En çok seçilen ödeme türü

            var sonuc3 = db.TBLHESAPODEME.GroupBy(x => x.TBLODEMETURU.ODEMETURU).Select(g => new
            {
                ÖdemeTürü = g.Key,
                Toplam = g.Count()
            });
            label9.Text = sonuc3.OrderByDescending(x => x.Toplam).FirstOrDefault().ÖdemeTürü.ToString();


            //6-en pahalı ürün

            label11.Text = db.TBLURUN.Where(x => x.DURUM == true).OrderByDescending(x => x.FIYAT).FirstOrDefault().URUNAD;

            //7- ürün satış grafiği göster

            var sonuc5 = db.TBLSATISLAR.Where(x => x.TBLADISYON.TARIH == DateTime.Today).GroupBy(g => g.TBLURUN.URUNAD).Select(z => new
            {
                Urun = z.Key,
                Toplam = z.Sum(x => x.ADET)
            }).OrderByDescending(x => x.Toplam).Take(5).ToList();

            chart1.Series.Clear();
            chart1.Series.Add("Satis");
            chart1.Series["Satis"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            foreach (var x in sonuc5)
            {
                chart1.Series["Satis"].Points.AddXY(x.Urun, x.Toplam);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sonuc = db.TBLSATISLAR.Where(x => x.TBLADISYON.TARIH == DateTime.Today).GroupBy(g => g.TBLURUN.URUNAD).Select(z => new
            {
                Urun = z.Key,
                Toplam = z.Sum(x => x.ADET)
            }).OrderByDescending(x => x.Toplam).Take(5);

            dataGridView1.DataSource = sonuc.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var sonuc = db.TBLSATISLAR.Where(x => x.TBLADISYON.TARIH == DateTime.Today).GroupBy(g => g.TBLURUN.URUNAD).Select(z => new
            {
                Urun = z.Key,
                Toplam = z.Sum(x => x.ADET)
            }).OrderBy(x => x.Toplam).Take(5);

            dataGridView1.DataSource = sonuc.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var sonuc = db.TBLSATISLAR.GroupBy(x => x.TBLURUN.URUNAD).Select(g => new
            {
                Urun = g.Key,
                Kazanc = g.Sum(x => x.ADET * x.TBLURUN.FIYAT)
            }).OrderByDescending(x => x.Kazanc).Take(5);

            dataGridView1.DataSource = sonuc.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var sonuc2 = db.TBLSATISLAR.GroupBy(x => x.TBLURUN.TBLKATEGORI.KATEGORI).Select(g => new
            {
                Kategori = g.Key,
                Adet = g.Sum(x => x.ADET)
            }).OrderByDescending(x => x.Adet).Take(5);

            dataGridView1.DataSource = sonuc2.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sonuc = db.TBLMASA.Where(x => !db.TBLSATISLAR.Any(s => s.MASAID == x.MASAID && s.TBLADISYON.TARIH == DateTime.Today) && x.MASAID != 999).Select(a => new
            {
                a.MASAID
            });

            dataGridView1.DataSource = sonuc.ToList();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime ilktarih = Convert.ToDateTime(DateTime.Today.AddDays(-7));

            DateTime sontarih = Convert.ToDateTime(DateTime.Today);

            var satilmayan = db.TBLURUN.Where(x => !db.TBLSATISLAR.Any(s => s.URUNID == x.ID && s.TBLADISYON.TARIH >= ilktarih && s.TBLADISYON.TARIH <= sontarih)).Select(x => new
            {
                x.URUNAD,
                x.FIYAT
            });
            dataGridView1.DataSource = satilmayan.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var sonuc2 = db.TBLSATISLAR.GroupBy(x => x.TBLURUN.TBLKATEGORI.KATEGORI).Select(g => new
            {
                Kategori = g.Key,
                Adet = g.Sum(x => x.ADET)
            }).OrderByDescending(x => x.Adet).Take(5).ToList();

            chart1.Series.Clear();
            chart1.Series.Add("Kategori");
            chart1.Series["Kategori"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            foreach (var x in sonuc2)
            {
                chart1.Series["Kategori"].Points.AddXY(x.Kategori, x.Adet);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var sonuc2 = db.TBLADISYON.GroupBy(x => x.TBLPERSONEL.ADSOYAD).Select(g => new
            {
                Per = g.Key,
                Adet = g.Count()
            }).OrderByDescending(x => x.Adet).ToList();

            chart1.Series.Clear();
            chart1.Series.Add("Personel");
            chart1.Series["Personel"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            foreach (var x in sonuc2)
            {
                chart1.Series["Personel"].Points.AddXY(x.Per, x.Adet);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DateTime tar = Convert.ToDateTime(DateTime.Today.AddDays(-7));

            var sonuc = db.TBLSATISLAR.Where(x => x.TBLADISYON.TARIH >= tar).GroupBy(x => x.TBLADISYON.TARIH).Select(s => new
            {
                tarih = s.Key,
                ciro = s.Sum(c => c.ADET * c.TBLURUN.FIYAT)

            }).OrderBy(x => x.tarih).ToList();

            chart1.Series.Clear();
            chart1.Series.Add("ciro");
            chart1.Series["ciro"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            foreach (var x in sonuc)
            {
                chart1.Series["ciro"].Points.AddXY(x.tarih, x.ciro);
            }

        }
    }
}
