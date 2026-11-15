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
    public partial class FrmKasiyer : Form
    {
        public FrmKasiyer()
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
            // 1. Paneli şeffaflaştır ve hocanınki gibi 45 derece köşelerini yumuşat
            PanelSaydamVeYuvarla(panel1, 45, 210);

            // 2. Butonları Döngü ile Stilize Et ve Efektleri Tanımla
            // Form üzerindeki button1'den button8'e kadar olan butonları bir diziye alıyoruz
            Button[] menubutonlari = { button1, button2, button3, button4, button5, button6, button7};

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















        private void button5_Click(object sender, EventArgs e)
        {
            bool acikMasaVarMi = db.TBLMASA.Any(x => x.DURUM == "D");

            if (acikMasaVarMi)
            {
                MessageBox.Show("DİKKAT: İçeride hala açık (hesabı ödenmemiş) masalar var! Tüm hesapları kapatmadan gün sonu alamazsınız.", "Gün Sonu Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime bugun = DateTime.Today;
            DateTime yarin = bugun.AddDays(1);

            decimal nakitToplam = db.TBLHESAPODEME.Where(x => x.DURUM == true && x.ODEMETURU == 1 && x.TARIH >= bugun && x.TARIH < yarin).Select(x => (decimal?)x.TOPLAMTUTAR).Sum() ?? 0;
            decimal kartToplam = db.TBLHESAPODEME.Where(x => x.DURUM == true && x.ODEMETURU == 2 && x.TARIH >= bugun && x.TARIH < yarin).Select(x => (decimal?)x.TOPLAMTUTAR).Sum() ?? 0;
            decimal ticketToplam = db.TBLHESAPODEME.Where(x => x.DURUM == true && x.ODEMETURU == 3 && x.TARIH >= bugun && x.TARIH < yarin).Select(x => (decimal?)x.TOPLAMTUTAR).Sum() ?? 0;
            decimal digerToplam = db.TBLHESAPODEME.Where(x => x.DURUM == true && x.ODEMETURU == 4 && x.TARIH >= bugun && x.TARIH < yarin).Select(x => (decimal?)x.TOPLAMTUTAR).Sum() ?? 0;
            decimal qrToplam = db.TBLHESAPODEME.Where(x => x.DURUM == true && x.ODEMETURU == 5 && x.TARIH >= bugun && x.TARIH < yarin).Select(x => (decimal?)x.TOPLAMTUTAR).Sum() ?? 0;

            decimal kasaGiris = db.TBLKASAHARAKET.Where(x => x.ISLEMTURU == true && x.TARIHSAAT >= bugun && x.TARIHSAAT < yarin).Select(x => (decimal?)x.TUTAR).Sum() ?? 0;
            decimal kasaCikis = db.TBLKASAHARAKET.Where(x => x.ISLEMTURU == false && x.TARIHSAAT >= bugun && x.TARIHSAAT < yarin).Select(x => (decimal?)x.TUTAR).Sum() ?? 0;

            decimal netKasa = (nakitToplam + kartToplam + ticketToplam + digerToplam + qrToplam + kasaGiris) - kasaCikis;


            string mesaj = $"GÜN SONU ÖZETİ:\n\n" +
                           $"Nakit (1): {nakitToplam:C2}\n" +
                           $"Kredi Kartı (2): {kartToplam:C2}\n" +
                           $"Ticket (3): {ticketToplam:C2}\n" +
                           $"Diğer (4): {digerToplam:C2}\n" +
                           $"QR (5): {qrToplam:C2}\n" +
                           $"----------------------\n" +
                           $"Kasa Girişleri: {kasaGiris:C2}\n" +
                           $"Kasa Çıkışları: {kasaCikis:C2}\n\n" +
                           $"NET KASA BAKİYESİ: {netKasa:C2}\n\n" +
                           $"Gün sonunu onaylıyor musunuz?";

            DialogResult onay = MessageBox.Show(mesaj, "Z Raporu Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay == DialogResult.Yes)
            {
                TBLGUNSONU yeniGunSonu = new TBLGUNSONU();
                yeniGunSonu.TARIH = DateTime.Now;
                yeniGunSonu.TOPLAMNAKIT = nakitToplam;
                yeniGunSonu.TOPLAMKART = kartToplam;
                yeniGunSonu.TOPLAMTICKET = ticketToplam;
                yeniGunSonu.TOPLAMDIGER = digerToplam;
                yeniGunSonu.TOPLAMQR = qrToplam;
                yeniGunSonu.KASAGIRISI = kasaGiris;
                yeniGunSonu.KASACIKISI = kasaCikis;
                yeniGunSonu.NETBAKIYE = netKasa;

                db.TBLGUNSONU.Add(yeniGunSonu);
                db.SaveChanges();

                MessageBox.Show("Gün Sonu (Z Raporu) başarıyla arşivlendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();
            Form2 siparis = new Form2();
            siparis.TopLevel = false;
            siparis.Dock = DockStyle.Fill;
            pnlEkran.Controls.Add(siparis);
            siparis.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();
            UcKasiyerOdemeAl adisyonlar = new UcKasiyerOdemeAl();
            adisyonlar.Dock = DockStyle.Fill;
            pnlEkran.Controls.Add(adisyonlar);

        }

        private void FrmKasiyer_Load(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();
            UcKasiyerAna ana = new UcKasiyerAna();
            ana.Dock = DockStyle.Fill;
            pnlEkran.Controls.Add(ana);
            var personel = db.TBLPERSONEL.SingleOrDefault(x => x.ID == Yonetim.PersonelID);
            if (personel != null) label1.Text = "Hoşgeldiniz, " + personel.ADSOYAD;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();
            UcKasiyerAna ana = new UcKasiyerAna();
            ana.Dock = DockStyle.Fill;
            pnlEkran.Controls.Add(ana);
            var personel = db.TBLPERSONEL.SingleOrDefault(x => x.ID == Yonetim.PersonelID);
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();
            UcKasiyerKasaIslemleri kasa = new UcKasiyerKasaIslemleri();
            kasa.Dock = DockStyle.Fill;
            pnlEkran.Controls.Add(kasa);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            Yonetim y=new Yonetim();
            y.Dock = DockStyle.Fill;
            y.Show();

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            pnlEkran.Controls.Clear();

            frmSatışAnalizi frmSatis = new frmSatışAnalizi();

            frmSatis.TopLevel = false;

            frmSatis.Dock = DockStyle.Fill;

            pnlEkran.Controls.Add(frmSatis);

            frmSatis.Show();
        }

        private void pnlEkran_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
