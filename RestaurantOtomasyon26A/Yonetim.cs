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
    public partial class Yonetim : Form
    {
        DbRestaurant26AEntities1 db=new DbRestaurant26AEntities1 ();
        public Yonetim()
        {
            InitializeComponent();
        }
        // --- Kurumsal Renk Paleti ---
        public static Color PremiumAltin = Color.FromArgb(234, 150, 68);    // Butonlar ve logolar için asil turuncu/altın
        public static Color CamZemini = Color.FromArgb(24, 24, 32);       // Saydam panellerin iç rengi (Mürdüm/Siyah)
        public static Color YaziAcik = Color.FromArgb(240, 240, 245);     // Ana başlıklar için kırık beyaz
        public static Color YaziMuted = Color.FromArgb(155, 155, 165);    // Slogan ve açıklamalar için sönük gri
        public static Color InputArkaPlan = Color.FromArgb(16, 16, 22);   // TextBox içleri için koyu zemin
        private void Yonetim_Load(object sender, EventArgs e)
        {
            // 1. Panelleri Yeni Kurumsal Renk ve Saydamlıkla Yuvarla
            // (Panel, Oval Kenar Çapı, Saydamlık Derecesi)
            PanelSaydamVeYuvarla(panel1, 40,190 );
            PanelSaydamVeYuvarla(panel2, 40, 190);
            PanelSaydamVeYuvarla(panel3, 40, 190);
            PanelSaydamVeYuvarla(panel4, 40, 190);

            // panel5'i 300 yerine 45 yaparak hocanınki gibi köşeleri yumuşak bir kart yapıyoruz
            PanelSaydamVeYuvarla(panel5, 45, 210);

            PanelSaydamVeYuvarla(panel6, 40, 190);
            //PanelSaydamVeYuvarla(panel7, 40, 190);
            PanelSaydamVeYuvarla(panel8, 40, 190);
            PanelSaydamVeYuvarla(panel9, 40, 190);
            PanelSaydamVeYuvarla(panel10, 40, 190);

            // 2. Giriş Butonlarını Stilize Et
            button1.BackColor = PremiumAltin;
            button1.ForeColor = Color.Black; // Yazı siyah veya çok koyu gri olmalı ki altın renkte okunsun
            ButonYuvarla(button1, 15);

            button2.BackColor = Color.FromArgb(35, 35, 45); // Misafir modu butonu daha sönük koyu gri
            button2.ForeColor = YaziAcik;
            ButonYuvarla(button2, 15);

            // 3. Formdaki Yazı Renklerini Güncelle
            // Tasarım ekranından veya buradan başlıkları "YaziAcik", açıklamaları "YaziMuted" yapabilirsin
            label1.ForeColor = YaziAcik; // Örn: Hoş Geldiniz yazısı
            panel2.ForeColor = CamZemini;
            panel3.ForeColor = CamZemini;
            panel4.ForeColor = CamZemini;
            panel5.ForeColor = CamZemini;
            panel6.ForeColor = CamZemini;
            panel7.ForeColor = CamZemini;
            panel8.ForeColor = CamZemini;
            panel9.ForeColor = CamZemini;
            panel10.ForeColor = CamZemini;
           

        }
        #region Modern panel 

        public class ModernPanel : Panel
        {
            public int BorderRadius { get; set; } = 20;
            public Color BorderColor { get; set; } = Color.FromArgb(50, 50, 60);
            public int BorderSize { get; set; } = 0;

            // YENİ ÖZELLİKLER: Saydamlık derecesi (0-255 arası) ve Panel Rengi
            // 0: Tamamen görünmez, 255: Tamamen mat. Hocanın tasarımı için 180-200 arası idealdir.
            public int Opacity { get; set; } = 185;
            public Color PanelColor { get; set; } = Color.FromArgb(20, 20, 25); // Koyu arka plan rengi

            public ModernPanel()
            {
                // WinForms'a bu panelin arkasındaki resmi/formu süzerek göstermesini söylüyoruz
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.Transparent;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                // Kenarların pürüzsüz (anti-alias) olması için
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
                {
                    // Region kullanmıyoruz! Çünkü region saydamlığı engeller. 
                    // Onun yerine alanı doğrudan yarı saydam bir fırçayla dolduruyoruz:
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(Opacity, PanelColor)))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    // Kenarlık (border) çizimi
                    if (BorderSize > 0)
                    {
                        using (Pen pen = new Pen(BorderColor, BorderSize))
                        {
                            pen.Alignment = PenAlignment.Inset;
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
            }

            private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                float size = radius * 2F;

                // Taşmaları önlemek için güvenlik kontrolü
                if (size > rect.Width) size = rect.Width;
                if (size > rect.Height) size = rect.Height;

                path.StartFigure();
                path.AddArc(rect.X, rect.Y, size, size, 180, 90);
                path.AddArc(rect.Right - size, rect.Y, size, size, 270, 90);
                path.AddArc(rect.Right - size, rect.Bottom - size, size, size, 0, 90);
                path.AddArc(rect.X, rect.Bottom - size, size, size, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
        #endregion
       
        #region modernTextbox
        public class ModernTextBox : UserControl
        {
            private TextBox txtInput;
            public int BorderRadius { get; set; } = 10;
            public Color BoxBackColor { get; set; } = Color.FromArgb(26, 26, 32);

            // Standart TextBox özelliklerini dışarı açıyoruz
            public string TextValue
            {
                get { return txtInput.Text; }
                set { txtInput.Text = value; }
            }

            public bool UseSystemPasswordChar
            {
                get { return txtInput.UseSystemPasswordChar; }
                set { txtInput.UseSystemPasswordChar = value; }
            }

            public ModernTextBox()
            {
                txtInput = new TextBox();
                txtInput.BorderStyle = BorderStyle.None;
                txtInput.BackColor = BoxBackColor;
                txtInput.ForeColor = Color.White;
                txtInput.Font = new Font("Segoe UI", 11F);
                txtInput.Dock = DockStyle.Fill;

                this.Controls.Add(txtInput);
                this.Padding = new Padding(10, 8, 10, 8); // Yazının kenarlara yapışmaması için padding
                this.BackColor = BoxBackColor;
                this.Size = new Size(250, 35);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
                {
                    this.Region = new Region(path);
                }
            }
          
            private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                float size = radius * 2F;
                path.StartFigure();
                path.AddArc(rect.X, rect.Y, size, size, 180, 90);
                path.AddArc(rect.Right - size, rect.Y, size, size, 270, 90);
                path.AddArc(rect.Right - size, rect.Bottom - size, size, size, 0, 90);
                path.AddArc(rect.X, rect.Bottom - size, size, size, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
        #endregion
        public void PanelSaydamVeYuvarla(Panel pnl, int radius, int opacity)
        {
            // 1. Paneli şeffaf yapıyoruz ki arkadaki formun resmini geçirebilsin
            pnl.BackColor = Color.Transparent;

            // 2. Panelin kendi çizim olayına (Paint) müdahale ediyoruz
            pnl.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, pnl.Width, pnl.Height);

                using (GraphicsPath path = new GraphicsPath())
                {
                    float size = radius * 2F;

                    // Panel boyutundan büyük yarıçap girilmesini engelleme kontrolü
                    if (size > rect.Width) size = rect.Width;
                    if (size > rect.Height) size = rect.Height;

                    path.StartFigure();
                    path.AddArc(rect.X, rect.Y, size, size, 180, 90);
                    path.AddArc(rect.Right - size, rect.Y, size, size, 270, 90);
                    path.AddArc(rect.Right - size, rect.Bottom - size, size, size, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - size, size, size, 90, 90);
                    path.CloseFigure();

                    
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(opacity, 20, 20, 25)))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };

            // Paneli yeniden çizmeye zorla
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
        public static int PersonelID;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null && textBox2.Text == null)
            {
                MessageBox.Show("Lütfen Kullanıcı Adı Şifre Giriniz");
            }
            else
            {
                
           
            var kullanici = db.TBLPERSONEL.Where(x => x.KULLANICIADI == textBox1.Text && x.SIFRE == textBox2.Text).FirstOrDefault();
            PersonelID = kullanici.ID;
            if (kullanici != null && kullanici.GOREV == "Garson")
            {
                FrmGarson g = new FrmGarson();
                g.Show();
            }
            if (kullanici != null && kullanici.GOREV == "Kasiyer")
            {
                FrmKasiyer k = new FrmKasiyer();
                k.Show();
            }
            if (kullanici != null && kullanici.GOREV == "Admin")
            {
                FrmAdminPaneli a = new FrmAdminPaneli();
                a.Show();
            }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
