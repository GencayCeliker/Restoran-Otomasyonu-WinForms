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
    public partial class UcGarsonAdisyonlar : UserControl
    {
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        public UcGarsonAdisyonlar()
        {
            InitializeComponent();
        }

        private void UcGarsonAdisyonlar_Load(object sender, EventArgs e)
        {
            
            adisyonlar();
        }
        private void adisyonlar()
        {//tersten sıralama yaparak en son açılan adisyonun en üstte görünmesini sağlıyoruz

            var adisyon = from x in db.TBLADISYON
                          orderby x.ID descending
                          select new
                          {
                              x.ID,
                              x.MASAID,
                              x.TARIH,
                              x.PERSONELID,
                              x.SERVISTURU,
                              x.DURUM
                          };
            dataGridView1.DataSource = adisyon.ToList();
        }
        public void EskiSiparisleriListele(int adisyonID)
        {
            listView1.Items.Clear();
            decimal genelToplam = 0;

            // DURUM == true şartını sildik. Bakalım gizlenen ürünler ortaya çıkacak mı?
            var siparisler = (from s in db.TBLSATISLAR
                              where s.ADISYONID == adisyonID
                              select new
                              {
                                  SatisID = s.ID,
                                  UrunID = s.URUNID,
                                  UrunAdi = s.TBLURUN.URUNAD,
                                  Fiyat = s.TBLURUN.FIYAT,
                                  Adet = s.ADET,
                                  AraToplam = s.ADET * s.TBLURUN.FIYAT
                              }).ToList();

            foreach (var siparis in siparisler)
            {
                ListViewItem item = new ListViewItem(siparis.UrunAdi);
                item.SubItems.Add(siparis.Fiyat.ToString());
                item.SubItems.Add(siparis.UrunID.ToString());
                item.SubItems.Add(siparis.Adet.ToString());
                item.SubItems.Add(siparis.SatisID.ToString());
                listView1.Items.Add(item);

                genelToplam += Convert.ToDecimal(siparis.AraToplam);
            }
            lblaratoplam.Text = genelToplam.ToString("C2");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int secilenAdisyonID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                string secilenMasaID = dataGridView1.Rows[e.RowIndex].Cells["MASAID"].Value.ToString();
                int secilenPersonelID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["PERSONELID"].Value);
                int servisTuru = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SERVISTURU"].Value);
                DateTime tarih = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["TARIH"].Value);
                label6.Text = secilenAdisyonID.ToString();
                label9.Text = secilenMasaID;
                lblgarson.Text = db.TBLPERSONEL.Where(x => x.ID == secilenPersonelID).Select(x => x.ADSOYAD).FirstOrDefault();
                label2.Text = servisTuru == 2 ? "Paket Servis" : "Masa Servisi";
                label4.Text = tarih.ToString("dd/MM/yyyy HH:mm");

                EskiSiparisleriListele(secilenAdisyonID);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            adisyonlar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime baslangicTarihi = dateTimePicker1.Value.Date;
            DateTime bitisTarihi = dateTimePicker2.Value.Date.AddDays(1);

            // DÜZELTME BURADA: Sütun isimlerini CellClick'in aradığı isimlerle (ID, MASAID vb.) BİREBİR AYNI YAPTIK
            var filtrelenmisListe = (from x in db.TBLADISYON
                                     orderby x.ID descending
                                     where x.TARIH >= baslangicTarihi && x.TARIH < bitisTarihi
                                     select new
                                     {
                                         ID = x.ID,
                                         MASAID = x.MASAID,
                                         TARIH = x.TARIH,
                                         PERSONELID = x.PERSONELID,
                                         SERVISTURU = x.SERVISTURU,
                                         DURUM = x.DURUM
                                     }).ToList();

            dataGridView1.DataSource = filtrelenmisListe;

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["ID"].HeaderText = "Adisyon No";
                dataGridView1.Columns["MASAID"].HeaderText = "Masa No";
                dataGridView1.Columns["TARIH"].HeaderText = "Açılış Tarihi";
                dataGridView1.Columns["PERSONELID"].Visible = false;
                dataGridView1.Columns["SERVISTURU"].Visible = false;
            }
        }
    }
}
