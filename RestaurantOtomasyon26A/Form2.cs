using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RestaurantOtomasyon26A
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void Form2_Load(object sender, EventArgs e)
        {
            PanelYuvarla(panel3, 20);
            PanelYuvarla(panel4, 20);
            PanelYuvarla(panel5, 20);
            PanelYuvarla(panel6, 20);
            PanelYuvarla(panel7, 20);
            PanelYuvarla(panel8, 20);
            PanelYuvarla(panel9, 20);
            PanelYuvarla(panel12, 20);
            PanelYuvarla(panel13, 5);
            PanelYuvarla(panel14, 5);
            PanelYuvarla(panel16, 5);
            PanelYuvarla(panel17, 5);
            PanelYuvarla(panel21, 20);
            PanelYuvarla(panel22, 20);
            PanelYuvarla(panel23, 20);
            PanelYuvarla(panel25, 20);
            PanelYuvarla(panel29, 20);

            ButonYuvarla(button1, 20);
            ButonYuvarla(button2, 20);
            ButonYuvarla(button3, 20);

            kategorigoster();
            masagetir();
            masarenklendir();
            musterigetir();
            if (secilenmusteriID > 1)
            {
               
                label34.Text = db.TBLMUSTERI.Where(x => x.ID == secilenmusteriID).Select(x => x.ADSOYAD).FirstOrDefault();
                lblmusteriID.Text = secilenmusteriID.ToString();
            }
            else
            {
                label34.Text = "Kayıtsız Müşteri";
                lblmusteriID.Text = "1";
            }
            label4.Text = DateTime.Now.ToLongDateString();
            var personel = db.TBLPERSONEL.SingleOrDefault(x => x.ID == Yonetim.PersonelID);
            if (personel != null) label6.Text = "Hoşgeldiniz, " + personel.ADSOYAD;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void PanelYuvarla(Control panel, int radius)
        {
            int w = panel.Width;
            int h = panel.Height;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            path.AddArc(0, 0, radius, radius, 180, 90);

            path.AddArc(w - radius, 0, radius, radius, 270, 90);

            path.AddArc(w - radius, h - radius, radius, radius, 0, 90);

            path.AddArc(0, h - radius, radius, radius, 90, 90);

            path.CloseAllFigures();

            panel.Region = new Region(path);

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


        Button[] ktg = new Button[100];
        public void kategorigoster()
        {
            flowLayoutPanel2.Controls.Clear();

            var kategoriler = (from x in db.TBLKATEGORI
                               where x.DURUM == true
                               select new
                               {
                                   x.ID,
                                   x.KATEGORI
                               }).ToList();

            int i = 1;
            foreach (var x in kategoriler)
            {
                ktg[i] = new Button();
                ktg[i].Size = new Size(150, 30);
                ktg[i].Location = new Point(i * 30 + 20, i * 35 + 30);
                ktg[i].Text = x.KATEGORI;
                ktg[i].Name = "ktg" + x.ID;
                ktg[i].BackColor = Color.Beige;
                ktg[i].ForeColor = Color.DarkGreen;
                ktg[i].Font = new Font("verdana", 10, FontStyle.Bold);
                flowLayoutPanel2.Controls.Add(ktg[i]);
                ktg[i].Click += ktg_Click;
            }
        }

        private void ktg_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            int secilenkategoriID = Convert.ToInt32(b.Name.ToString().Substring(3, b.Name.Length - 3));

            var secilenkategoridekiurunler = (from x in db.TBLURUN
                                              where x.DURUM == true && x.KATEGORI == secilenkategoriID
                                              select new
                                              {
                                                  x.ID,
                                                  x.URUNAD,
                                                  x.FIYAT
                                              });
            dataGridView1.DataSource = secilenkategoridekiurunler.ToList();

            dataGridView1.Font = new Font("verdana", 12, FontStyle.Bold);
            dataGridView1.ForeColor = Color.DarkGreen;
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns["FIYAT"].DefaultCellStyle.Format = "C2";
        }

        Button[] ms = new Button[100];

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
            int i = 1;
            foreach (var x in masalar)
            {
                ms[i] = new Button();
                ms[i].Size = new Size(55, 35);
                ms[i].Location = new Point(i * 30 + 20, i * 35 + 30);
                ms[i].Text = x.MASAID.ToString();
                ms[i].Name = "ms" + x.MASAID;
                // ms[i].BackColor = Color.DarkCyan;
                // ms[i].ForeColor = Color.LightPink;
                ms[i].Font = new Font("verdana", 12, FontStyle.Bold);
                flowLayoutPanel1.Controls.Add(ms[i]);
                ms[i].Click += ms_Click;
                i++;
            }
        }

        private void ms_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            int secilenmasaID = Convert.ToInt32(b.Name.ToString().Substring(2, b.Name.Length - 2));

            lblmasano.Text = secilenmasaID.ToString();

            var masa = db.TBLMASA.Find(secilenmasaID);

            if (masa.DURUM == "D")
            {
                listView1.Items.Clear();
                MasaSiparisGetir(secilenmasaID);
            }
            else
            {
                listView1.Items.Clear();
            }

        }

        public void masarenklendir()
        {
            List<string> dolumasalar = (from x in db.TBLMASA
                                        where x.AKTIFMI == true && x.MASAID != 999 && x.DURUM == "D"
                                        select x.MASAID.ToString()).ToList();
            //1  5  8  v.s liste oluşur

            List<string> bosmasalar = (from x in db.TBLMASA
                                       where x.AKTIFMI == true && x.MASAID != 999 && x.DURUM == "B"
                                       select x.MASAID.ToString()).ToList();
            // 2  7    v.s liste oluşur

            List<string> rezervemasalar = (from x in db.TBLMASA
                                           where x.AKTIFMI == true && x.MASAID != 999 && x.DURUM == "R"
                                           select x.MASAID.ToString()).ToList();
            // 3  9  v.s liste oluşur

            if (dolumasalar.Count > 0)
            {
                foreach (var a in dolumasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.FromArgb(0, 192, 0);
                    ms[no].ForeColor = Color.White;
                }
            }

            if (bosmasalar.Count > 0)
            {
                foreach (var a in bosmasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.Gainsboro;
                    ms[no].ForeColor = Color.White;
                }
            }

            if (rezervemasalar.Count > 0)
            {
                foreach (var a in rezervemasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.Orange;
                    ms[no].ForeColor = Color.White;
                    ms[no].Text = "R";
                }
            }


        }

        public void musterigetir()
        {
            var musteriler = (from x in db.TBLMUSTERI
                              where x.DURUM == true
                              select new
                              {
                                  x.ID,
                                  x.ADSOYAD,
                                  x.TEL,
                                  x.SIPARISSAYISI
                              });

            dataGridView2.DataSource = musteriler.ToList();
            dataGridView2.Font = new Font("verdana", 10, FontStyle.Regular);
            dataGridView2.ForeColor = Color.Red;
            dataGridView2.Columns[0].Width = 40;
            dataGridView2.Columns[3].Width = 40;
            dataGridView2.Columns[3].HeaderText = "SS";
        }


        int sayac = 0;
        bool varmi = false;
        int indis;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (lblmasano.Text == "0") //masa seçilmemiş
            {
                MessageBox.Show("Önce Masa Seçmelisiniz!!!");
            }
            else
            {
                if (listView1.Items.Count == 0) //atılan ilk ürün
                {
                    int secilen = dataGridView1.SelectedCells[0].RowIndex;

                    sayac = listView1.Items.Count;

                    listView1.Items.Add(dataGridView1.Rows[secilen].Cells[1].Value.ToString());

                    listView1.Items[sayac].SubItems.Add(dataGridView1.Rows[secilen].Cells[2].Value.ToString());

                    listView1.Items[sayac].SubItems.Add(dataGridView1.Rows[secilen].Cells[0].Value.ToString());

                    listView1.Items[sayac].SubItems.Add("1");

                    sayac++;

                }
                else //atılan ilk ürün değilse bu ürün daha önce atılmışmı kontrol edilmeli
                {
                    int secilen = dataGridView1.SelectedCells[0].RowIndex;

                    int secilenurunID = Convert.ToInt32(dataGridView1.Rows[secilen].Cells[0].Value.ToString());

                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (Convert.ToInt32(listView1.Items[i].SubItems[2].Text) == secilenurunID)
                        {
                            varmi = true;
                            indis = i;
                            break;
                        }
                        else
                        {
                            varmi = false;
                        }
                    }

                    if (varmi == true) //olan üründen tekrar seçilmiş
                    {
                        int adet = int.Parse(listView1.Items[indis].SubItems[3].Text);

                        listView1.Items[indis].SubItems[3].Text = "" + (adet + 1);
                    }
                    else  //listede olmayan bir ürün seçilmiş
                    {
                        sayac = listView1.Items.Count;

                        listView1.Items.Add(dataGridView1.Rows[secilen].Cells[1].Value.ToString());

                        listView1.Items[sayac].SubItems.Add(dataGridView1.Rows[secilen].Cells[2].Value.ToString());

                        listView1.Items[sayac].SubItems.Add(dataGridView1.Rows[secilen].Cells[0].Value.ToString());

                        listView1.Items[sayac].SubItems.Add("1");

                        sayac++;
                    }


                }
            }
        }
        public static int adisyonID;
        private void button3_Click(object sender, EventArgs e)
        {
            int musteriID = 1;
            if (!string.IsNullOrEmpty(lblmusteriID.Text))
            {
                int.TryParse(lblmusteriID.Text, out musteriID);
            }

            if (lblmasano.Text != "999")  
            {
                var masabul = db.TBLMASA.Find(int.Parse(lblmasano.Text));

                if (masabul.DURUM == "B") // Masa boş ise ilk sipariş gidecek!!!
                {
                    
                    TBLADISYON yeni = new TBLADISYON();
                    yeni.SERVISTURU = 1;
                    yeni.PERSONELID = int.Parse(lblpersno.Text);
                    yeni.MASAID = masabul.MASAID;
                    yeni.TARIH = DateTime.Now;
                    yeni.DURUM = false;
                    db.TBLADISYON.Add(yeni);
                    db.SaveChanges();
                    masabul.DURUM = "D";
                    db.SaveChanges();
                    int sonadisyonID = yeni.ID;
                    lblAdisyonID.Text = sonadisyonID.ToString();

                    TBLHESAPODEME onKayit = new TBLHESAPODEME();
                    onKayit.ADISYONID = sonadisyonID;
                    onKayit.MUSTERIID = musteriID;
                    onKayit.TARIH = DateTime.Now;
                    onKayit.ODEMETURU = 1;
                    onKayit.ARATOPLAM = 0;
                    onKayit.KDV = 0;
                    onKayit.INDIRIM = 0;
                    onKayit.TOPLAMTUTAR = 0;
                    onKayit.DURUM = false; // Hesap açık
                    db.TBLHESAPODEME.Add(onKayit);
                    db.SaveChanges();
                }

                foreach (ListViewItem item in listView1.Items)
                {
                    int urunID = int.Parse(item.SubItems[2].Text);
                    int adet = int.Parse(item.SubItems[3].Text);
                    adisyonID = int.Parse(lblAdisyonID.Text);

                    var satisVarmi = db.TBLSATISLAR.FirstOrDefault(x => x.ADISYONID == adisyonID && x.URUNID == urunID);

                    if (satisVarmi != null)  
                    {
                        satisVarmi.ADET = adet;
                    }
                    else 
                    {
                        TBLSATISLAR yenisatis = new TBLSATISLAR();
                        yenisatis.ADISYONID = adisyonID;
                        yenisatis.URUNID = urunID;
                        yenisatis.MASAID = int.Parse(lblmasano.Text);
                        yenisatis.ADET = adet;

                        yenisatis.DURUM = false;

                        db.TBLSATISLAR.Add(yenisatis);
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Siparişiniz Kaydedildi");
                masarenklendir();
                listView1.Items.Clear();
            }
            else   
            {
                
                TBLADISYON yeniadisyon = new TBLADISYON();
                yeniadisyon.SERVISTURU = 2;
                yeniadisyon.PERSONELID = int.Parse(lblpersno.Text);
                yeniadisyon.MASAID = 999;
                yeniadisyon.TARIH = DateTime.Now; 
                yeniadisyon.DURUM = false;
                db.TBLADISYON.Add(yeniadisyon);
                db.SaveChanges();

                int sonadisyonID = yeniadisyon.ID;
                lblAdisyonID.Text = sonadisyonID.ToString();

                TBLHESAPODEME paketOnKayit = new TBLHESAPODEME();
                paketOnKayit.ADISYONID = sonadisyonID;
                paketOnKayit.MUSTERIID = musteriID;
                paketOnKayit.TARIH = DateTime.Now;
                paketOnKayit.ODEMETURU = 1;
                paketOnKayit.ARATOPLAM = 0;
                paketOnKayit.KDV = 0;
                paketOnKayit.INDIRIM = 0;
                paketOnKayit.TOPLAMTUTAR = 0;
                paketOnKayit.DURUM = false;
                db.TBLHESAPODEME.Add(paketOnKayit);
                db.SaveChanges();

                if (listView1.Items.Count > 0)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        TBLSATISLAR yenisatis = new TBLSATISLAR();
                        yenisatis.ADISYONID = sonadisyonID;
                        yenisatis.URUNID = int.Parse(listView1.Items[i].SubItems[2].Text);
                        yenisatis.MASAID = int.Parse(lblmasano.Text);
                        yenisatis.ADET = int.Parse(listView1.Items[i].SubItems[3].Text);
                        yenisatis.DURUM = false; // Aktif sipariş false
                        db.TBLSATISLAR.Add(yenisatis);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Paket Siparişiniz Kaydedildi");
                    listView1.Items.Clear();

                    var paketsiparistekisatislar = (from x in db.TBLSATISLAR
                                                    where x.ADISYONID == sonadisyonID
                                                    select new
                                                    {
                                                        x.ID,
                                                        x.TBLURUN.URUNAD,
                                                        x.TBLURUN.FIYAT,
                                                        x.ADET,
                                                        x.ADISYONID,
                                                        aratoplam = x.ADET * x.TBLURUN.FIYAT
                                                    });

                    lblaratoplam.Text = paketsiparistekisatislar.Sum(x => x.aratoplam).ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblmasano.Text = "999";
        }

        int adsno = 0;
        public void MasaSiparisGetir(int masano)
        {
            var adisyonbul = db.TBLADISYON.Where(x => x.MASAID == masano && x.DURUM == false).FirstOrDefault();

            if (adisyonbul != null)
            {
                adsno = adisyonbul.ID;
                lblAdisyonID.Text = adsno.ToString();
            }
            else
            {
                adsno = 0;
            }
            var buadisyondakisatislar = (from x in db.TBLSATISLAR
                                         where x.ADISYONID == adsno
                                         select new
                                         {
                                             x.ID,
                                             x.TBLURUN.URUNAD,
                                             fiyat = x.TBLURUN.FIYAT,
                                             x.ADET,
                                             x.ADISYONID,
                                             aratoplam = x.TBLURUN.FIYAT * x.ADET,
                                             urunID = x.TBLURUN.ID

                                         });
            //daha önceki siparişleri adisyona gelsin(listview'e)

            foreach (var satis in buadisyondakisatislar)
            {
                ListViewItem item = new ListViewItem(satis.URUNAD);
                item.SubItems.Add(satis.fiyat.ToString());
                item.SubItems.Add(satis.urunID.ToString());
                item.SubItems.Add(satis.ADET.ToString());
                item.SubItems.Add(satis.ID.ToString());
                item.SubItems.Add("➕");
                item.SubItems.Add("➖");
                item.SubItems.Add("🗑️");

                listView1.Items.Add(item);
            }

            decimal aratoplam = buadisyondakisatislar.Sum(x => (decimal?)x.aratoplam) ?? 0;

            lblaratoplam.Text = aratoplam.ToString();

            lblfiyat.Text = (aratoplam - Convert.ToDecimal(tbindirim.Text)).ToString();

        }

        private void tbindirim_TextChanged(object sender, EventArgs e)
        {
            if (tbindirim.Text != "")
            {
                lblfiyat.Text = (Convert.ToDecimal(lblaratoplam.Text) - Convert.ToDecimal(tbindirim.Text)).ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int odemeturu = 0;
            if (radioButton1.Checked) odemeturu = 1;
            if (radioButton2.Checked) odemeturu = 2;
            if (radioButton3.Checked) odemeturu = 3;
            if (radioButton4.Checked) odemeturu = 5;

            int adisyonID = Convert.ToInt32(lblAdisyonID.Text);

            // 1- Adisyonu Kapat
            var adisyonbul = db.TBLADISYON.Find(adisyonID);
            if (adisyonbul != null) adisyonbul.DURUM = true;

            // 2- ZATEN VAR OLAN Hesap Ödeme kaydını bul ve GÜNCELLE (Ekleme yapmıyoruz!)
            var hesap = db.TBLHESAPODEME.FirstOrDefault(x => x.ADISYONID == adisyonID);

            if (hesap != null)
            {
                // Garsonun açtığı boş satırın içini ödeme bilgileriyle dolduruyoruz
                hesap.ODEMETURU = odemeturu;
                hesap.ARATOPLAM = Convert.ToDecimal(lblaratoplam.Text.Replace("₺", "").Trim());
                hesap.INDIRIM = string.IsNullOrEmpty(tbindirim.Text) ? 0 : Convert.ToDecimal(tbindirim.Text);
                hesap.TOPLAMTUTAR = Convert.ToDecimal(lblfiyat.Text.Replace("₺", "").Trim());
                hesap.TARIH = DateTime.Now;
                hesap.DURUM = true; // Hesabı kapandı olarak işaretle

                // 3- Müşterinin sipariş sayısını artır (Müşteri ID zaten bu tabloda kayıtlıydı!)
                if (hesap.MUSTERIID != null && hesap.MUSTERIID != 1)
                {
                    var bulmusteri = db.TBLMUSTERI.Find(hesap.MUSTERIID);
                    if (bulmusteri != null) bulmusteri.SIPARISSAYISI++;
                }
            }
            var bulmasa = db.TBLMASA.Find(Convert.ToInt32(lblmasano.Text));
            if (bulmasa != null) bulmasa.DURUM = "B";
            db.SaveChanges();
            MessageBox.Show(lblmasano.Text + " nolu masanın hesabı başarıyla kapanmıştır.");
            masarenklendir();
           

            listView1.Items.Clear();
        }
        public static int secilenmusteriID;
        int secilen;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          secilen = dataGridView2.SelectedCells[0].RowIndex;

          secilenmusteriID = Convert.ToInt32(dataGridView2.Rows[secilen].Cells[0].Value.ToString());

            lblmusteriID.Text = secilenmusteriID.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {

                // 2. Fiş içeriğini hazırla
                StringBuilder fis = new StringBuilder();
                decimal genelToplam = 0; // Toplamı tutmak için değişken

                fis.AppendLine("******** ZMYO RESTORANT  ********");
                fis.AppendLine("Masa No: " + lblmasano.Text);
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
            g.DrawString("Masa No: " + lblmasano.Text, altBaslikFont, Brushes.Black, 20, y);
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

                string satir = $"{urunAd} ({adet} adet) = {araToplam:C2}";
                g.DrawString(satir, icerikFont, Brushes.Black, 20, y);
                y += 25;
            }

            // Alt Çizgi ve GENEL TOPLAM
            y += 10;
            g.DrawString("------------------------------------------", icerikFont, Brushes.Black, 20, y);
            y += 20;

            // Toplamı daha belirgin yazdırıyoruz
            g.DrawString("GENEL TOPLAM: " + genelToplam.ToString("C2"), toplamFont, Brushes.Black, 20, y);

            y += 40;
            g.DrawString("Afiyet Olsun!", altBaslikFont, Brushes.Black, 70, y);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();

            pd.PrintPage += new PrintPageEventHandler(AdisyonTasarimi);

            PrintPreviewDialog pdd = new PrintPreviewDialog();

            pdd.Document = pd;

            pdd.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("HH:mm:ss");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        void Artır(ListViewItem item)
        {
            int adet = int.Parse(item.SubItems[3].Text);
            adet++;

            item.SubItems[3].Text = adet.ToString();
            int id = int.Parse(item.SubItems[4].Text);

            var satis = db.TBLSATISLAR.Find(id);
            if (satis != null)
            {
                satis.ADET = adet;
                db.SaveChanges();
            }
        }
        void Azalt(ListViewItem item)
        {
            int adet = int.Parse(item.SubItems[3].Text);
            if (adet > 1)
            {
                adet--;
                item.SubItems[3].Text = adet.ToString();
                int id = int.Parse(item.SubItems[4].Text);

                var satis = db.TBLSATISLAR.Find(id);
                if (satis != null)
                {
                    satis.ADET = adet;
                    db.SaveChanges();
                }
            }
        }
        void Sil(ListViewItem item, int satisID)
        {
            var satis = db.TBLSATISLAR.Find(satisID);

            if (satis != null)
            {
                db.TBLSATISLAR.Remove(satis);
                db.SaveChanges();
            }

            listView1.Items.Remove(item);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
        
            var item = listView1.GetItemAt(e.X, e.Y);

            if (item == null) return;

            int satisID = int.Parse(item.SubItems[4].Text);

            //hangi kolona tıkladık

            var a = listView1.HitTest(e.Location);
            int columIndex = a.Item.SubItems.IndexOf(a.SubItem);

            //5 ise artıracak
            if (columIndex == 5)
            {
                string islem = a.SubItem.Text;
                Artır(item);
            }
            if (columIndex == 6)
            {
                string islem = a.SubItem.Text;
                Azalt(item);
            }
             if (columIndex == 7)
            {
                string islem = a.SubItem.Text;
                Sil(item,satisID);
            }

        }
    }
}
