using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantOtomasyon26A
{
    public partial class UcKategoriYontemi : UserControl
    {
        public UcKategoriYontemi()
        {
            InitializeComponent();
            yuvarla();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();




        public static class RestoranTemasi
        {
            // --- ZEMİN VE METİN RENKLERİ ---
            public static Color CamZemini = Color.FromArgb(24, 24, 32);       // Saydam panellerin iç rengi (Mürdüm/Siyah)
            public static Color InputArkaPlan = Color.FromArgb(16, 16, 22);   // TextBox içleri için koyu zemin
            public static Color YaziAcik = Color.FromArgb(240, 240, 245);     // Ana başlıklar için kırık beyaz
            public static Color YaziMuted = Color.FromArgb(155, 155, 165);    // Slogan ve açıklamalar için sönük gri

            // --- BUTON VE AKTİF DURUM RENKLERİ ---
            public static Color ButonNormalArkaPlan = Color.FromArgb(32, 32, 42); // Menüdeki normal butonların koyu rengi

            // İstediğiniz Admin Panelindeki Özel Altın/Turuncu Tonu (Tıklanma/Aktif efekti için)
            public static Color AdminVurguAltin = Color.FromArgb(219, 131, 48);   // Admin panelindeki o özel renk (#DB8330)

            // Giriş ekranındaki parlak ana buton rengi
            public static Color PremiumAltin = Color.FromArgb(234, 150, 68);      // Giriş yap butonu tonu (#EA9644)
        }
        private void yuvarla()
        {
        

            // 2. Butonları Döngü ile Stilize Et ve Efektleri Tanımla
            // Form üzerindeki button1'den button8'e kadar olan butonları bir diziye alıyoruz
            Button[] menubutonlari = { button9, button10, button11};

            foreach (Button btn in menubutonlari)
            {
                if (btn != null)
                {
                    // İlk açılışta butonların rengi (Koyu zemin ve açık renk yazı)
                    btn.BackColor = RestoranTemasi.ButonNormalArkaPlan;
                    btn.ForeColor = RestoranTemasi.YaziAcik;
                    ButonYuvarla(btn, 15);

                    // MOUSE ÜZERİNE GELDİĞİNDE (HOVER) VEYA TIKLANDIĞINDA ADMİN PANELİNDEKİ ALTIN RENGİ OLSUN
                    btn.MouseEnter += (s, e) => {
                        btn.BackColor = RestoranTemasi.AdminVurguAltin;
                        btn.ForeColor = Color.Black;
                    };
                    btn.MouseDown += (s, e) => {
                        btn.BackColor = RestoranTemasi.AdminVurguAltin;
                        btn.ForeColor = Color.Black;
                    };

                    // MOUSE AYRILDIĞINDA VEYA TIKLAMA BİTTİĞİNDE ESKİ KOYU HALİNE DÖNSÜN
                    btn.MouseLeave += (s, e) => {
                        btn.BackColor = RestoranTemasi.ButonNormalArkaPlan;
                        btn.ForeColor = RestoranTemasi.YaziAcik;
                    };
                    btn.MouseUp += (s, e) => {
                        btn.BackColor = RestoranTemasi.ButonNormalArkaPlan;
                        btn.ForeColor = RestoranTemasi.YaziAcik;
                    };
                }
            }

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







        private void UcKategoriYontemi_Load(object sender, EventArgs e)
        {
            kategoriListele();
        }
        private void kategoriListele()
        {
            var kategoriler =from x in db.TBLKATEGORI select new 
            {
                x.ID,
                x.KATEGORI,
                x.DURUM
            };
            dataGridView1.DataSource = kategoriler.ToList() ;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmKatEkle frm = new frmKatEkle();
            frm.ShowDialog();
            kategoriListele();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                frmKatGuncelle frmGuncelle = new frmKatGuncelle(secilenId);

                frmGuncelle.ShowDialog();

                kategoriListele();
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz kategoriyi listeden seçin.", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                kategoriListele();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
          
            if (dataGridView1.CurrentRow != null)
            {
                int seciliId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                bool urunVarMi = db.TBLURUN.Any(u => u.KATEGORI == seciliId);

                if (urunVarMi)
                {
                    MessageBox.Show("Bu kategoriye ait kayıtlı ürünler bulunmaktadır! İçerisinde ürün olan bir kategoriyi silemezsiniz. Lütfen önce kategorideki ürünleri silin veya başka kategoriye taşıyın.", "Silme Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }

                DialogResult cevap = MessageBox.Show("Bu kategoriyi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (cevap == DialogResult.Yes)
                {
                    var silinecekKategori = db.TBLKATEGORI.Find(seciliId);
                    if (silinecekKategori != null)
                    {
                        db.TBLKATEGORI.Remove(silinecekKategori);
                        db.SaveChanges();

                        MessageBox.Show("Kategori başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        kategoriListele();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz kategoriyi listeden seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
