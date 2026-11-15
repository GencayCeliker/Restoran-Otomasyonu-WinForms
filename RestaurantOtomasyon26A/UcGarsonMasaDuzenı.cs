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
    public partial class UcGarsonMasaDuzenı : UserControl
    {
        public UcGarsonMasaDuzenı()
        {
            InitializeComponent();
            yuvarla();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        #region Masa 
        Button[] ms = new Button[101];





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
            Button[] menubutonlari = { button9, button10, button11, button12 };

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















        public void masagetir()
        {
            flowLayoutPanel1.Controls.Clear();


            var masalar = (from x in db.TBLMASA
                           where x.AKTIFMI == true && x.MASAID != 999
                           select new
                           {
                               x.MASAID,
                               x.DURUM
                           }).ToList();
            
            foreach (var x in masalar)
            {
                int id = x.MASAID; // Butonları diziye eklerken indeks karmaşasını önlemek için direkt ID kullanıyoruz
                ms[id] = new Button();

                // Buton boyutunu büyüttük ki hem masa no hem de durum yazısı rahatça sığsın
                ms[id].Size = new Size(100, 80);
                ms[id].Name = "ms" + id;

                // ID bilgisini Tag içinde saklamak Substring ile uğraşmanızı engeller
                ms[id].Tag = id;

                ms[id].Font = new Font("Verdana", 10, FontStyle.Bold);

                // Başlangıç metni ve renk ataması
                string durumMetni = "";
                if (x.DURUM == "B")
                {
                    durumMetni = "Boş";
                    ms[id].BackColor = Color.Gainsboro;
                    ms[id].ForeColor = Color.Black;
                }
                else if (x.DURUM == "D")
                {
                    durumMetni = "Dolu";
                    ms[id].BackColor = Color.FromArgb(0, 192, 0);
                    ms[id].ForeColor = Color.White;
                }
                else if (x.DURUM == "R")
                {
                    durumMetni = "Rezerve";
                    ms[id].BackColor = Color.FromArgb(192, 0, 192);
                    ms[id].ForeColor = Color.White;
                }

                // Text özelliğine hem masa nosunu hem de durumu alt satıra geçerek (\n) yazdırıyoruz
                ms[id].Text = $"Masa {id}\n{durumMetni}";

                ms[id].Click += ms_Click;

                flowLayoutPanel1.Controls.Add(ms[id]);
            }
        }

        private void ms_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;


            // Masanın Tag özelliğine sakladığımız ID'sini hafızaya alıyoruz

            // ID'yi Tag'den güvenli bir şekilde alıyoruz
            int seciliMasaID = (int)b.Tag;

            //lblMasa.Text = secilenmasaID.ToString();

            // KRİTİK DÜZELTME: Her tıklamada önce listeyi ve toplamları sıfırla
            //listView1.Items.Clear();
            //lblAraToplam.Text = "₺0,00";
            //lblfiyat.Text = "₺0,00";

            var masa = db.TBLMASA.Find(seciliMasaID);

            if (masa != null && masa.DURUM == "D")
            {
                // Masa doluysa verileri getir
                //MasaSiparisGetir(secilenmasaID);
            }
        }

        public void masarenklendir()
        {
            List<string> dolumasalar = (from x in db.TBLMASA
                                        where x.AKTIFMI == true && x.MASAID != 999 && x.DURUM == "D"
                                        select x.MASAID.ToString()).ToList();

            List<string> bosmasalar = (from x in db.TBLMASA
                                       where x.AKTIFMI == true && x.MASAID != 999 && x.DURUM == "B"
                                       select x.MASAID.ToString()).ToList();

            List<string> rezervemasalar = (from x in db.TBLMASA
                                           where x.AKTIFMI == true && x.MASAID != 999 && x.DURUM == "R"
                                           select x.MASAID.ToString()).ToList();

            if (dolumasalar.Count > 0)
            {
                foreach (var a in dolumasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.FromArgb(0, 192, 0);
                    ms[no].ForeColor = Color.White;
                    ms[no].Text = $"Masa {no}\nDolu"; // Dolu yazısını güncelliyoruz
                }
            }

            if (bosmasalar.Count > 0)
            {
                foreach (var a in bosmasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.Gainsboro;
                    ms[no].ForeColor = Color.Black;
                    ms[no].Text = $"Masa {no}\nBoş"; // Boş yazısını güncelliyoruz
                }
            }

            if (rezervemasalar.Count > 0)
            {
                foreach (var a in rezervemasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.FromArgb(192, 0, 192);
                    ms[no].ForeColor = Color.White;
                    ms[no].Text = $"Masa {no}\nRezerve"; // Sadece "R" yerine "Rezerve" yazdırıyoruz
                }
            }
        }
        #endregion
        private void button9_Click(object sender, EventArgs e)
        {
            Form2 siparis = new Form2();
            siparis.FormBorderStyle = FormBorderStyle.Sizable;
            siparis.Text = "Masa Sipariş Ekranı"; 
            siparis.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UcGarsonAdisyonlar adisyonlar = new UcGarsonAdisyonlar();
            adisyonlar.Dock = DockStyle.Fill; 
            Form tasiyiciForm = new Form();
            tasiyiciForm.Text = "Adisyon Yönetimi"; 
            tasiyiciForm.WindowState = FormWindowState.Maximized; 
            tasiyiciForm.Controls.Add(adisyonlar);
            tasiyiciForm.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var acikAdisyonlar = db.TBLADISYON
                           .Where(x => x.DURUM == false)
                           .Select(x => new { x.ID, x.MASAID })
                           .ToList();
            if (acikAdisyonlar.Count == 0)
            {
                MessageBox.Show("Şu anda sistemde açık hiçbir adisyon bulunmuyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. KODLA KÜÇÜK ŞIK BİR PENCERE YARATIYORUZ (Sürükle-bırak tasarıma gerek yok)
            Form miniPencere = new Form();
            miniPencere.Text = "Açık Adisyonlar ";
            miniPencere.Size = new Size(300, 350);
            miniPencere.StartPosition = FormStartPosition.CenterScreen; 
            miniPencere.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Sadece çarpı ikonu olan, boyutu değiştirilemeyen çerçeve
            miniPencere.BackColor = Color.White;

            // 3. PENCERENİN İÇİNE LİSTE (ListBox) EKLİYORUZ
            ListBox liste = new ListBox();
            liste.Dock = DockStyle.Top;
            liste.Height = 220;
            liste.Font = new Font("Segoe UI", 11);

            // Açık adisyonları listeye dolduruyoruz
            foreach (var adisyon in acikAdisyonlar)
            {
                liste.Items.Add($"Adisyon No: {adisyon.ID}  -  Masa No: {adisyon.MASAID}");
            }

            // 4. PENCERENİN ALTINA "KAPAT" BUTONU EKLİYORUZ
            Button btnKapat = new Button();
            btnKapat.Text = "Seçili Adisyonu Kapat";
            btnKapat.Dock = DockStyle.Bottom;
            btnKapat.Height = 50;
            btnKapat.BackColor = Color.Crimson; // Dikkat çekici kırmızı renk
            btnKapat.ForeColor = Color.White;
            btnKapat.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnKapat.FlatStyle = FlatStyle.Flat;

            // 5. İŞTE CAN ALICI KISIM: BUTONA TIKLANINCA VERİLECEK YETKİ HATASI
            btnKapat.Click += (s, args) =>
            {
                if (liste.SelectedIndex != -1) // Eğer listeden bir adisyon seçildiyse
                {
                    MessageBox.Show("Yetkiniz olmadığı için adisyon kapatamazsınız!\n\nLütfen hesap kesimi ve kapatma işlemleri için Kasaya yönlendiriniz.",
                                    "Erişim Reddedildi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lütfen listeden kapatmak istediğiniz adisyonu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            miniPencere.Controls.Add(liste);
            miniPencere.Controls.Add(btnKapat);
            miniPencere.ShowDialog();

        }

        private void UcGarsonMasaDuzenı_Load(object sender, EventArgs e)
        {
            masagetir();
            masarenklendir();
            AktifSiparisleriListele();
        }
        private void AktifSiparisleriListele()
        {
            listView2.Items.Clear();

            var aktifAdisyonlar = db.TBLADISYON
                                    .Where(x => x.DURUM == false)
                                    .ToList();

            foreach (var adisyon in aktifAdisyonlar)
            {
                ListViewItem iç =
                    new ListViewItem("Masa " + adisyon.MASAID.ToString());

                string musteri = "Kayıtsız";

                var hesap = db.TBLHESAPODEME
                              .AsEnumerable()
                              .FirstOrDefault(h => h.ADISYONID == adisyon.ID);

                if (hesap != null)
                {
                    var musteriKayit = db.TBLMUSTERI
                                         .AsEnumerable()
                                         .FirstOrDefault(m => m.ID == hesap.MUSTERIID);

                    if (musteriKayit != null)
                    {
                        musteri = musteriKayit.ADSOYAD;
                    }
                }

                iç.SubItems.Add(musteri);

                iç.SubItems.Add(
                    Convert.ToDateTime(adisyon.TARIH)
                    .ToString("HH:mm"));

                decimal toplamTutar = db.TBLSATISLAR
                    .Where(s => s.ADISYONID == adisyon.ID
                             && s.DURUM == false)
                    .Sum(s => (decimal?)(s.ADET * s.TBLURUN.FIYAT)) ?? 0;

                iç.SubItems.Add(toplamTutar.ToString("C2"));

                iç.SubItems.Add("Açık");

                listView2.Items.Add(iç);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FrmMasaBirlestir frm = new FrmMasaBirlestir();
            frm.ShowDialog();
            masagetir();
            masarenklendir();
            AktifSiparisleriListele();
        }
    }
}
