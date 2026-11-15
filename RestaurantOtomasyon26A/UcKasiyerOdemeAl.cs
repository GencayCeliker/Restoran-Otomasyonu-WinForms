using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RestaurantOtomasyon26A
{
    public partial class UcKasiyerOdemeAl : UserControl
    {
        public UcKasiyerOdemeAl()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void UcKasiyerAdisyonlar_Load(object sender, EventArgs e)
        {
            masagetir();
            masarenklendir();
            int gelenID = UcKasiyerAna.kasiyerSecilenAdisyonID;
            AdisyonBilgileriniDoldur(gelenID);
         
         
        }
        #region Masa 
        Button[] ms = new Button[101];

        public void masagetir()
        {
            // Arayüzü temizle
            listView1.Items.Clear();
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
            int secilenmasaID = (int)b.Tag;

            var masa = db.TBLMASA.Find(secilenmasaID);

            if (masa != null && masa.DURUM == "D")
            {
                var aktifAdisyon = db.TBLADISYON.FirstOrDefault(x => x.MASAID == secilenmasaID && x.DURUM == false);

                if (aktifAdisyon != null)
                {
                    AdisyonBilgileriniDoldur(aktifAdisyon.ID);
                }
            }
            else
            {
             Temizle();
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
        // 1. HER YERDEN ÇAĞRILABİLECEK, EKRANI DOLDURAN ANA METODUMUZ

        private void AdisyonBilgileriniDoldur(int adisyonID)
        {
            if (adisyonID <= 0) return; // Yanlış bir ID geldiyse hiç işlem yapma

            // A) MAVİ PANELİ DOLDURMA (Adisyon, Masa, Garson, Tarih vb.)
            var adisyon = db.TBLADISYON.Find(adisyonID);
            if (adisyon != null)
            {
                lblAdisyonNo.Text = adisyon.ID.ToString();
                lblMasaNo.Text = adisyon.MASAID.ToString();
                lblTarih.Text = Convert.ToDateTime(adisyon.TARIH).ToString("dd MMMM yyyy");

                var garson = db.TBLPERSONEL.Find(adisyon.PERSONELID);
                lblGarson.Text = garson != null ? garson.ADSOYAD : "Bilinmiyor";
                lblServisTuru.Text = adisyon.SERVISTURU == 1 ? "Masa Servisi" : "Paket / Özel";
            }

            // B) SAĞDAKİ BEYAZ FİŞİ (LISTVIEW) DOLDURMA
            listView1.Items.Clear();

            // Her iki tabloda ihtiyacımız olan UrunID, SatisID gibi tüm verileri çekiyoruz
            var satilanUrunler = db.TBLSATISLAR
                                   .Where(x => x.ADISYONID == adisyonID && x.DURUM == false)
                                   .Select(s => new
                                   {
                                       SatisID = s.ID,
                                       UrunID = s.URUNID, // Sütundaki 'ID' başlığı için ekledik
                                       UrunAdi = s.TBLURUN.URUNAD,
                                       Fiyat = s.TBLURUN.FIYAT,
                                       Adet = s.ADET
                                   }).ToList();

            decimal araToplam = 0;

            foreach (var item in satilanUrunler)
            {
                // Tasarımındaki sütun sırasına göre tam eşleştirme yapıyoruz:
                ListViewItem ic = new ListViewItem(item.UrunAdi); // 1. Sütun: ÜRÜN
                ic.SubItems.Add(item.Fiyat.ToString());       // 2. Sütun: FİYAT
                ic.SubItems.Add(item.UrunID.ToString());          // 3. Sütun: ID (Ürünün kendi ID'si)
                ic.SubItems.Add(item.Adet.ToString());            // 4. Sütun: ADET
                ic.SubItems.Add(item.SatisID.ToString());         // 5. Sütun: SID (Satış benzersiz ID'si)

                listView1.Items.Add(ic);

                // Fiyat * Adet yaparak toplamı buluyoruz
                araToplam += (decimal)(item.Fiyat * item.Adet);
            }

            // C) MATEMATİKSEL TUTARLARI GÜNCELLEME
            lblAraToplam.Text = araToplam.ToString("");
            lblToplamTutar.Text = araToplam.ToString("");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int odemeturu = 0;
            if (radioButton1.Checked) odemeturu = 1;
            if (radioButton2.Checked) odemeturu = 2;
            if (radioButton3.Checked) odemeturu = 3;
            if (radioButton4.Checked) odemeturu = 5;

            int adisyonID = Convert.ToInt32(lblAdisyonNo.Text);

            // 1- Adisyonu Kapat
            var adisyonbul = db.TBLADISYON.Find(adisyonID);
            if (adisyonbul != null) adisyonbul.DURUM = true;

            // 2- ZATEN VAR OLAN Hesap Ödeme kaydını bul ve GÜNCELLE (Ekleme yapmıyoruz!)
            var hesap = db.TBLHESAPODEME.FirstOrDefault(x => x.ADISYONID == adisyonID);

            if (hesap != null)
            {
                // Garsonun açtığı boş satırın içini ödeme bilgileriyle dolduruyoruz
                hesap.ODEMETURU = odemeturu;
                hesap.ARATOPLAM = Convert.ToDecimal(lblAraToplam.Text.Replace("₺", "").Trim());
                hesap.INDIRIM = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToDecimal(textBox1.Text);
                hesap.TOPLAMTUTAR = Convert.ToDecimal(lblToplamTutar.Text.Replace("₺", "").Trim());
                hesap.TARIH = DateTime.Now;
                hesap.DURUM = true; // Hesabı kapandı olarak işaretle

                // 3- Müşterinin sipariş sayısını artır (Müşteri ID zaten bu tabloda kayıtlıydı!)
                if (hesap.MUSTERIID != null && hesap.MUSTERIID != 1)
                {
                    var bulmusteri = db.TBLMUSTERI.Find(hesap.MUSTERIID);
                    if (bulmusteri != null) bulmusteri.SIPARISSAYISI++;
                }
            }
            var bulmasa = db.TBLMASA.Find(Convert.ToInt32(lblMasaNo.Text));
            if (bulmasa != null) bulmasa.DURUM = "B";
            db.SaveChanges();
            MessageBox.Show(lblMasaNo.Text + " nolu masanın hesabı başarıyla kapanmıştır.");
            masarenklendir();
            Temizle();


        }
        public void Temizle()
        {
          
            lblAdisyonNo.Text = "0";
            lblMasaNo.Text = "0";
            lblTarih.Text = "";
            lblGarson.Text = "";
            lblServisTuru.Text = "";
            listView1.Items.Clear();
            lblAraToplam.Text = "₺0,00";
            lblToplamTutar.Text = "₺0,00";
            textBox1.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {

                // 2. Fiş içeriğini hazırla
                StringBuilder fis = new StringBuilder();
                decimal genelToplam = 0; // Toplamı tutmak için değişken

                fis.AppendLine("******** ZMYO RESTORANT  ********");
                fis.AppendLine("Masa No: " + lblMasaNo.Text);
                fis.AppendLine("Tarih: " + DateTime.Now.ToString());
                fis.AppendLine("------------------------------");

                foreach (ListViewItem item in listView1.Items)
                {
                    // Verileri alalım
                    string urunAd = item.Text;
                    decimal fiyat = Convert.ToDecimal(item.SubItems[1].Text);
                    int adet = Convert.ToInt32(item.SubItems[3].Text);

                    decimal araToplam = fiyat * adet;
                    genelToplam += araToplam; // Her ürünü genel toplama ekle

                    fis.AppendLine($"{urunAd} x {adet} = {araToplam:N2} TL");
                }

                fis.AppendLine("------------------------------");
                // İşte toplamın yazıldığı kısım:
                fis.AppendLine("TOPLAM TUTAR: " + genelToplam.ToString("N2") + " TL");
                fis.AppendLine("******************************");

                MessageBox.Show(fis.ToString(), "Hesap Özeti");
            }
            else
            {
                MessageBox.Show("Adisyon boş, yazdırılacak bir şey yok!");
            }
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();

            pd.PrintPage += new PrintPageEventHandler(AdisyonTasarimi);

            PrintPreviewDialog pdd = new PrintPreviewDialog();

            pdd.Document = pd;

            pdd.ShowDialog();
        }
        private void AdisyonTasarimi(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font baslikFont = new Font("Arial", 14, FontStyle.Bold);
            Font altBaslikFont = new Font("Arial", 10, FontStyle.Bold);
            Font icerikFont = new Font("Arial", 10, FontStyle.Regular);
            Font toplamFont = new Font("Arial", 12, FontStyle.Bold); // Toplam için biraz daha büyük font

            float y = 20;
            decimal genelToplam = 0; // Toplamı tutacağımız değişken

            // Başlık ve Tarih Bilgileri
            g.DrawString("RESTORAN 26A", baslikFont, Brushes.Black, 50, y);
            y += 40;
            g.DrawString("Masa No: " + lblMasaNo.Text, altBaslikFont, Brushes.Black, 20, y);
            y += 20;
            g.DrawString("Tarih: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"), icerikFont, Brushes.Black, 20, y);
            y += 30;
            g.DrawString("------------------------------------------", icerikFont, Brushes.Black, 20, y);
            y += 20;

            // Ürünleri ListView'den çekiyoruz
            foreach (ListViewItem item in listView1.Items)
            {
                // ListView sütunların: [0]ÜrünAd, [1]Fiyat, [2]ID, [3]Adet
                string urunAd = item.Text;
                decimal fiyat = Convert.ToDecimal(item.SubItems[1].Text);
                int adet = Convert.ToInt32(item.SubItems[3].Text);
                decimal araToplam = fiyat * adet;

                genelToplam += araToplam; // Her ürünü genel toplama ekle

                string satir = $"{urunAd} ({adet} adet) = {araToplam}";
                g.DrawString(satir, icerikFont, Brushes.Black, 20, y);
                y += 25;
            }

            // Alt Çizgi ve GENEL TOPLAM
            y += 10;
            g.DrawString("------------------------------------------", icerikFont, Brushes.Black, 20, y);
            y += 20;

            // Toplamı daha belirgin yazdırıyoruz
            g.DrawString("GENEL TOPLAM: " + genelToplam.ToString(""), toplamFont, Brushes.Black, 20, y);

            y += 40;
            g.DrawString("Afiyet Olsun!", altBaslikFont, Brushes.Black, 70, y);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblToplamTutar.Text = (Convert.ToDecimal(lblAraToplam.Text) - Convert.ToDecimal(textBox1.Text)).ToString();
            }
        }
    }
}
