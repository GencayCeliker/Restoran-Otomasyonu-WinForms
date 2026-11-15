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
    public partial class UcGarsonMusteriler : UserControl
    {
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        public UcGarsonMusteriler()
        {
            InitializeComponent();
            yuvarla();
        }






        public static class RestoranTemasi
        {
            // --- ZEMİN VE METİN RENKLERİ ---
            public static Color CamZemini = Color.FromArgb(24, 24, 32);       // Saydam panellerin iç rengi (Mürdüm/Siyah)
          
            public static Color YaziAcik = Color.FromArgb(240, 240, 245);     // Ana başlıklar için kırık beyaz
            public static Color YaziMuted = Color.FromArgb(155, 155, 165);    // Slogan ve açıklamalar için sönük gri

            // --- BUTON VE AKTİF DURUM RENKLERİ ---
            public static Color ButonNormalArkaPlan = Color.FromArgb(32, 32, 42); // Menüdeki normal butonların koyu rengi

            // İstediğiniz Admin Panelindeki Özel Altın/Turuncu Tonu (Tıklanma/Aktif efekti için)
            public static Color AdminVurguAltin = Color.FromArgb(219, 131, 48);   // Admin panelindeki o özel renk (#DB8330)

          
        }




        private void yuvarla()
        {
            // 1. Paneli şeffaflaştır ve hocanınki gibi 45 derece köşelerini yumuşat
            //   PanelSaydamVeYuvarla(panel1, 45, 210);
            PanelSaydamVeYuvarla(panel2, 45, 210);
           

            // 2. Butonları Döngü ile Stilize Et ve Efektleri Tanımla
            // Form üzerindeki button1'den button8'e kadar olan butonları bir diziye alıyoruz
            Button[] menubutonlari = { button1, button2, button3, button4 };

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

        public void PanelSaydamVeYuvarla(Panel pnl, int radius, int opacity)
        {
            pnl.BackColor = Color.Transparent;
            pnl.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, pnl.Width, pnl.Height);

                using (GraphicsPath path = new GraphicsPath())
                {
                    float size = radius * 2F;


                    if (size > rect.Width) size = rect.Width;
                    if (size > rect.Height) size = rect.Height;

                    path.StartFigure();
                    path.AddArc(rect.X, rect.Y, size, size, 180, 90);
                    path.AddArc(rect.Right - size, rect.Y, size, size, 270, 90);
                    path.AddArc(rect.Right - size, rect.Bottom - size, size, size, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - size, size, size, 90, 90);
                    path.CloseFigure();


                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(opacity, RestoranTemasi.CamZemini.R, RestoranTemasi.CamZemini.G, RestoranTemasi.CamZemini.B)))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };


            pnl.Invalidate();
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








        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string aranan = textBox1.Text.ToLower();
            dataGridView1.DataSource = db.TBLMUSTERI
                .Where(x => x.ID != 1 && x.DURUM == true)
                .Where(x => x.ADSOYAD.ToLower().Contains(aranan) || x.TEL.Contains(aranan))
                .Select(x => new { x.ID, x.ADSOYAD, x.TEL })
                .ToList();
        }
        public void MusteriListele()
        {
            dataGridView1.DataSource = db.TBLMUSTERI
                .Where(x => x.ID != 1 && x.DURUM == true)
                .Select(x => new { x.ID, x.ADSOYAD, x.TEL })
                .ToList();
        }

        private void UcMusteriler_Load(object sender, EventArgs e)
        {
            MusteriListele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int seciliId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                var musteri = db.TBLMUSTERI.Find(seciliId);
                if (musteri != null)
                {
                    label1.Text = musteri.ADSOYAD;
                    label2.Text = musteri.TEL;
                    label4.Text = musteri.SIPARISSAYISI.ToString();
                    int siparis = musteri.SIPARISSAYISI ?? 0;
                    if (siparis >= 10) { panel1.BackColor = Color.FromArgb(255, 215, 0); } // Gold Müşteri
                    else if (siparis >= 5) { panel1.BackColor = Color.FromArgb(192, 192, 192); } // Silver Müşteri
                    else { panel1.BackColor = Color.Gainsboro; } // Normal Müşteri
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmGarsonMEkle frm = new frmGarsonMEkle();
            frm.ShowDialog();
            MusteriListele();
        }

        private void button3_Click(object sender, EventArgs e)
        {//müşteri silme işlemi
            if (dataGridView1.CurrentRow != null)
            {
                int seciliId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                var musteri = db.TBLMUSTERI.Find(seciliId);
                if (musteri != null)
                {
                    DialogResult result = MessageBox.Show("Bu müşteriyi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        musteri.DURUM = false; 
                        db.SaveChanges();
                        MusteriListele();
                    }
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int seciliId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                frmGarsonMGuncelle frm = new frmGarsonMGuncelle(seciliId);
                frm.ShowDialog();
                MusteriListele();
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//Silinmiş müşterileri gösterme işlemi
            if (checkBox1.Checked)
            {
                dataGridView1.DataSource = db.TBLMUSTERI
           
             .Where(x => x.ID != 1 && x.DURUM != true)
             .Select(x => new
             {
                 x.ID,
                 x.ADSOYAD,
                 x.TEL,
                 DURUM = "Silinmiş" 
             })
             .ToList();
            }
            else
            {
                MusteriListele();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
               
                int seciliMusteriID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                string seciliMusteriAd = dataGridView1.CurrentRow.Cells["ADSOYAD"].Value.ToString();
                Form2.secilenmusteriID = seciliMusteriID;
                Form2 siparisFormu = new Form2();
                siparisFormu.FormBorderStyle = FormBorderStyle.Sizable;
                siparisFormu.Text = "Masa Sipariş Ekranı";
                Control[] bulunanButonlar = siparisFormu.Controls.Find("button4", true);

                if (bulunanButonlar.Length > 0)
                {
                    bulunanButonlar[0].Enabled = false;
                }
             

               
                siparisFormu.Show();
            }
            else
            {
                MessageBox.Show("Lütfen önce listeden bir müşteri seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
