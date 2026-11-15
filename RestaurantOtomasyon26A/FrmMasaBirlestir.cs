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
    public partial class FrmMasaBirlestir : Form
    {
        public FrmMasaBirlestir()
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
            Button[] menubutonlari = { button1 };

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







        private void button1_Click(object sender, EventArgs e)
        {
            int tasinacakMasaID = Convert.ToInt32(comboBox1.SelectedValue);
            int hedefMasaID = Convert.ToInt32(comboBox2.SelectedValue);

            if (tasinacakMasaID == hedefMasaID)
            {
                MessageBox.Show("Aynı masayı kendiyle birleştiremezsiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tasinacakAdisyon = db.TBLADISYON.FirstOrDefault(x => x.MASAID == tasinacakMasaID && x.DURUM == false);
            var hedefAdisyon = db.TBLADISYON.FirstOrDefault(x => x.MASAID == hedefMasaID && x.DURUM == false);

            if (tasinacakAdisyon != null && hedefAdisyon != null)
            {
                var tasinacakSiparisler = db.TBLSATISLAR.Where(x => x.ADISYONID == tasinacakAdisyon.ID).ToList();

                foreach (var siparis in tasinacakSiparisler)
                {
                    siparis.ADISYONID = hedefAdisyon.ID; // Siparişler yeni masaya geçti
                }

                // HATA VEREN KISMIN ÇÖZÜMÜ: Remove komutunu kaldırdık!
                // Eski adisyonu silmek yerine, "Kapatıldı" (true) konumuna getiriyoruz.
                tasinacakAdisyon.DURUM = true;

                // Eski masayı Boş (B) durumuna getiriyoruz
                var eskiMasa = db.TBLMASA.FirstOrDefault(x => x.MASAID == tasinacakMasaID);
                if (eskiMasa != null)
                {
                    eskiMasa.DURUM = "B";
                }

                db.SaveChanges(); // Artık program çökmeyecek!

                MessageBox.Show("Masalar başarıyla birleştirildi! Tüm hesap hedef masaya aktarıldı.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Seçilen masaların açık adisyonları bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMasaBirlestir_Load(object sender, EventArgs e)
        {
            var doluMasalarKaynak = db.TBLMASA.Where(x => x.DURUM == "D").ToList();
            var doluMasalarHedef = db.TBLMASA.Where(x => x.DURUM == "D").ToList();
            comboBox1.DataSource = doluMasalarKaynak;
            comboBox1.DisplayMember = "MASAID";
            comboBox1.ValueMember = "MASAID";
            comboBox2.DataSource = doluMasalarHedef;
            comboBox2.DisplayMember = "MASAID";
            comboBox2.ValueMember = "MASAID";
        }
    
    }
}
