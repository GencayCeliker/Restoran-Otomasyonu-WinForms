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
    public partial class FrmAdminPaneli : Form
    {
        public FrmAdminPaneli()
        {
            InitializeComponent();
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
        private void FrmAdminPaneli_Load(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcAdmin admin = new UcAdmin();
            admin.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(admin);
            var personel = db.TBLPERSONEL.SingleOrDefault(x => x.ID == Yonetim.PersonelID);
            if (personel != null) label1.Text = "Hoşgeldiniz, " + personel.ADSOYAD;
            
            yuvarla();
        }


        private void yuvarla()
        {
            // 1. Paneli şeffaflaştır ve hocanınki gibi 45 derece köşelerini yumuşat
            PanelSaydamVeYuvarla(panel1, 45, 210);

            // 2. Butonları Döngü ile Stilize Et ve Efektleri Tanımla
            // Form üzerindeki button1'den button8'e kadar olan butonları bir diziye alıyoruz
            Button[] menubutonlari = { button1, button2, button3, button4, button5, button6, button7, button8 };

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

            // 3. Formdaki Yazı Renklerini Güncelle
            label1.ForeColor = RestoranTemasi.YaziAcik; 
            label3.ForeColor = RestoranTemasi.YaziAcik; 
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




        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
            Yonetim y = new Yonetim();
            y.Dock = DockStyle.Fill;
            y.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcDashboard dashboardSayfasi = new UcDashboard();
            dashboardSayfasi.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(dashboardSayfasi);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcPersonelYonetim per = new UcPersonelYonetim();
            per.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(per);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcUrunYonetimi urun = new UcUrunYonetimi();
            urun.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(urun);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcKategoriYontemi kategori = new UcKategoriYontemi();
            kategori.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(kategori);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            ucMasaYonetimi masa = new ucMasaYonetimi();
            masa.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(masa);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            frmSatışAnalizi frmSatis = new frmSatışAnalizi();

            frmSatis.TopLevel = false;

            frmSatis.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(frmSatis);

            frmSatis.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            UcAdmin admin = new UcAdmin();
            admin.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(admin);
        }
    }
}
