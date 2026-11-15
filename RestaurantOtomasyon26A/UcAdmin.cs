using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;       
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantOtomasyon26A
{
    public partial class UcAdmin : UserControl
    {DbRestaurant26AEntities1 db= new DbRestaurant26AEntities1();
        public UcAdmin()
        {
            InitializeComponent();
            yuvarla();
        }


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
            PanelSaydamVeYuvarla(panel4, 45, 210);
            PanelSaydamVeYuvarla(panel5, 45, 210);
            PanelSaydamVeYuvarla(panel6, 45, 210);
            PanelSaydamVeYuvarla(panel7, 45, 210);

            // 2. Butonları Döngü ile Stilize Et ve Efektleri Tanımla
            // Form üzerindeki button1'den button8'e kadar olan butonları bir diziye alıyoruz
            Button[] menubutonlari = { button9, button10, button11, button12};

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





        private void button9_Click(object sender, EventArgs e)
        {
            frmPerEkle frm = new frmPerEkle();
            frm.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            frmUrunEkle frm = new frmUrunEkle();
            frm.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            frmKatEkle frm = new frmKatEkle();
            frm.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            frmSatışAnalizi frm = new frmSatışAnalizi();
            frm.ShowDialog();
        }

        private void UcAdmin_Load(object sender, EventArgs e)
        {
            DateTime bugun = DateTime.Today;
            DateTime yarin = bugun.AddDays(1);

            decimal gunlukCiro = db.TBLHESAPODEME
                                   .Where(x => x.TARIH >= bugun && x.TARIH < yarin)
                                   .Sum(x => (decimal?)x.TOPLAMTUTAR) ?? 0;


            label7.Text = db.TBLMASA.Count().ToString();
            label10.Text = gunlukCiro.ToString("C2");
            label12.Text = db.TBLPERSONEL.Count().ToString();
            label14.Text = db.TBLURUN.Count().ToString();
            SonSatislariGetir();
        }
        public void SonSatislariGetir()
        {
           
            var sonSatislar = (from x in db.TBLHESAPODEME
                               orderby x.ID descending
                               select new
                               {
                                   Tarih = x.TARIH,
                                   AdisyonNo = x.ADISYONID,

                                   OdemeTipi = x.ODEMETURU == 1 ? "Nakit" : (x.ODEMETURU == 2 ? "Kredi Kartı" : "Diğer"),

                                   Musteri = x.TBLMUSTERI.ADSOYAD,

                                   Tutar = x.TOPLAMTUTAR
                               }).Take(15).ToList(); 

            dataGridView1.DataSource = sonSatislar;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns["Tutar"].DefaultCellStyle.Format = "C2";

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
