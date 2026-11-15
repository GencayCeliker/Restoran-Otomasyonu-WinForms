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
    public partial class UcUrunYonetimi : UserControl
    {
        public UcUrunYonetimi()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void UcUrunYonetimi_Load(object sender, EventArgs e)
        {
            var kategoriler = db.TBLKATEGORI.ToList();
            comboBox1.DisplayMember = "KATEGORI";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = kategoriler;
            UrunListele();
        }
        public void UrunListele()
        {
            var urunler = from x in db.TBLURUN
                              select new
                              {
                                  ID = x.ID,
                                  ÜRÜNADI = x.URUNAD,
                                  KATEGORI = db.TBLKATEGORI.Where(k => k.ID == x.KATEGORI).Select(k => k.KATEGORI).FirstOrDefault(),
                                  FİYAT = x.FIYAT,
                                  AÇIKLAMA = x.ACIKLAMA,
                                  DURUM = x.DURUM
                              };
            dataGridView1.DataSource = urunler.ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string aranan = textBox1.Text;
            var personeller = from x in db.TBLURUN.Where(x => x.URUNAD.Contains(aranan))
                               select new
                               {
                                   ID = x.ID,
                                   ÜRÜNADI = x.URUNAD,
                                   KATEGORI = db.TBLKATEGORI.Where(k => k.ID == x.KATEGORI).Select(k => k.KATEGORI).FirstOrDefault(),
                                   FİYAT = x.FIYAT,
                                   AÇIKLAMA = x.ACIKLAMA,
                                   DURUM = x.DURUM
                               };
            dataGridView1.DataSource = personeller.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmUrunEkle frm = new frmUrunEkle();
            frm.ShowDialog();
            UrunListele();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int secilenKategoriId = Convert.ToInt32(comboBox1.SelectedValue);

           
            var filtrelenmisUrunler = from x in db.TBLURUN
                                      where x.KATEGORI == secilenKategoriId 
                                      select new
                                      {
                                          ID = x.ID,
                                          ÜRÜNADI = x.URUNAD,
                                          KATEGORI = db.TBLKATEGORI.Where(k => k.ID == x.KATEGORI).Select(k => k.KATEGORI).FirstOrDefault(),
                                          FİYAT = x.FIYAT,
                                          AÇIKLAMA = x.ACIKLAMA,
                                          DURUM = x.DURUM
                                      };

            dataGridView1.DataSource = filtrelenmisUrunler.ToList();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Text="";
            UrunListele();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int seciliId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                DialogResult cevap = MessageBox.Show("Bu ürünü silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cevap == DialogResult.Yes)
                {
                    var silinecekUrun = db.TBLURUN.Find(seciliId);
                    if (silinecekUrun != null)
                    {
                        db.TBLURUN.Remove(silinecekUrun);
                        db.SaveChanges();
                        MessageBox.Show("Ürün başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UrunListele();
                    }
                    else
                    {
                        MessageBox.Show("Seçilen ürün bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                frmUrunGuncelle frmGuncelle = new frmUrunGuncelle(secilenId);

                frmGuncelle.ShowDialog();

                UrunListele();
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz ürünü listeden seçin.", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                UrunListele();
            }
        }
    }
}
