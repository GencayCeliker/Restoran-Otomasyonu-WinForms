using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantOtomasyon26A
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1  ();
        private void Form1_Load(object sender, EventArgs e)
        {
            kategorigoster();
            masagetir();
            masarenklendir();
            musterigetir();
        }

        Button[] ktg = new Button[100];
        public void kategorigoster()
        {
            flowLayoutPanel1.Controls.Clear();

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
                flowLayoutPanel1.Controls.Add(ktg[i]);
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
            flowLayoutPanel2.Controls.Clear();

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
                ms[i].Size = new Size(48, 30);
                ms[i].Location = new Point(i * 30 + 20, i * 35 + 30);
                ms[i].Text = x.MASAID.ToString();
                ms[i].Name = "ms" + x.MASAID;
                ms[i].BackColor = Color.DarkCyan;
                ms[i].ForeColor = Color.LightPink;
                ms[i].Font = new Font("verdana", 12, FontStyle.Bold);
                flowLayoutPanel2.Controls.Add(ms[i]);
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
                MasaSiparisGetir(secilenmasaID);
            }
            else
            {
                dataGridView3.DataSource = null;
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
                    ms[no].BackColor = Color.Red;
                    ms[no].ForeColor = Color.White;
                }
            }

            if (bosmasalar.Count > 0)
            {
                foreach (var a in bosmasalar)
                {
                    int no = int.Parse(a);
                    ms[no].BackColor = Color.Green;
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
        private void button2_Click(object sender, EventArgs e)
        {
            if (lblmasano.Text != "999")  //sipariş masaya gidecek
            {
                //masaya ilk sipariş mi gidecek yoksa eklememi yapılacak

                var masabul = db.TBLMASA.Find(int.Parse(lblmasano.Text));

                if (masabul.DURUM == "B") //masa boş ise ilk sipariş gidecek!!!
                {
                    // 1. ADİSYON tablosuna kaydet

                    TBLADISYON yeni = new TBLADISYON();
                    yeni.SERVISTURU = 1;
                    yeni.PERSONELID = int.Parse(lblpersno.Text);
                    yeni.MASAID = masabul.MASAID;
                    yeni.TARIH = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    yeni.DURUM = false;
                    db.TBLADISYON.Add(yeni);
                    db.SaveChanges();

                    // 2. MASANIN DURUMUN "D" OLARAK GÜNCELLENECEK

                    masabul.DURUM = "D";
                    db.SaveChanges();

                    // 3. BU MASAYA AÇILAN "ADISYINID" SİNİ BULALIM
                    var SonAdisyonBul = db.TBLADISYON.OrderByDescending(x => x.ID).FirstOrDefault();

                    int sonadisyonID = SonAdisyonBul.ID;
                    lblAdisyonID.Text = sonadisyonID.ToString();

                    //4. bu adisyondaki ürünleri SATIŞ tablosuna kaydet

                    if (listView1.Items.Count > 0)   //sipariş verildiyse
                    {
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            TBLSATISLAR yenisatis = new TBLSATISLAR();
                            yenisatis.ADISYONID = sonadisyonID;
                            yenisatis.URUNID = int.Parse(listView1.Items[i].SubItems[2].Text);
                            yenisatis.MASAID = int.Parse(lblmasano.Text);
                            yenisatis.ADET = int.Parse(listView1.Items[i].SubItems[3].Text);
                            yenisatis.DURUM = true;
                            db.TBLSATISLAR.Add(yenisatis);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Siparişiniz Kaydedildi");
                        masarenklendir();
                        listView1.Items.Clear();
                        MasaSiparisGetir(int.Parse(lblmasano.Text));
                    }
                }
                else   //masa doluysa mevcut adisyona (SATIŞLAR TABLOSUNA) ekleme yapılacak
                {
                    //o zaman bu masaya ait olan ADISYONID sini bulalım
                    var bumasaninsonadisyonu = db.TBLADISYON.Where(x => x.MASAID == masabul.MASAID && x.DURUM == false).FirstOrDefault();

                    if (listView1.Items.Count > 0)   //sipariş verildiyse
                    {
                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            TBLSATISLAR yenisatis = new TBLSATISLAR();
                            yenisatis.ADISYONID = bumasaninsonadisyonu.ID;
                            yenisatis.URUNID = int.Parse(listView1.Items[i].SubItems[2].Text);
                            yenisatis.MASAID = int.Parse(lblmasano.Text);
                            yenisatis.ADET = int.Parse(listView1.Items[i].SubItems[3].Text);
                            yenisatis.DURUM = true;
                            db.TBLSATISLAR.Add(yenisatis);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Siparişiniz Adisyona Eklendi");
                        listView1.Items.Clear();
                    }


                }
            }
            else   //paket sipariş ise
            {
                //1. ADİSYON EKLE

                TBLADISYON yeniadisyon = new TBLADISYON();
                yeniadisyon.SERVISTURU = 2;
                yeniadisyon.PERSONELID = int.Parse(lblpersno.Text);
                yeniadisyon.MASAID = 999;
                yeniadisyon.TARIH = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                yeniadisyon.DURUM = false;
                db.TBLADISYON.Add(yeniadisyon);
                db.SaveChanges();


                // 2. BU MASAYA AÇILAN "ADISYINID" SİNİ BULALIM
                var SonAdisyonBul = db.TBLADISYON.OrderByDescending(x => x.ID).FirstOrDefault();

                int sonadisyonID = SonAdisyonBul.ID;
                lblAdisyonID.Text = sonadisyonID.ToString();


                //3. bu adisyondaki ürünleri SATIŞ tablosuna kaydet

                if (listView1.Items.Count > 0)   //sipariş verildiyse
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        TBLSATISLAR yenisatis = new TBLSATISLAR();
                        yenisatis.ADISYONID = sonadisyonID;
                        yenisatis.URUNID = int.Parse(listView1.Items[i].SubItems[2].Text);
                        yenisatis.MASAID = int.Parse(lblmasano.Text);
                        yenisatis.ADET = int.Parse(listView1.Items[i].SubItems[3].Text);
                        yenisatis.DURUM = true;
                        db.TBLSATISLAR.Add(yenisatis);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Paket Siparişiniz Kaydedildi");
                    listView1.Items.Clear();

                    // 4. Bu sipariş datagridde listelensin

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
                    dataGridView3.DataSource = paketsiparistekisatislar.ToList();

                    lblaratoplam.Text = paketsiparistekisatislar.Sum(x => x.aratoplam).ToString();

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                                             aratoplam = x.TBLURUN.FIYAT * x.ADET
                                         });

            dataGridView3.DataSource = buadisyondakisatislar.ToList();

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
        private void button1_Click(object sender, EventArgs e)
        {
            int odemeturu = 0;
            if (radioButton1.Checked) odemeturu = 1;
            if (radioButton2.Checked) odemeturu = 2;
            if (radioButton3.Checked) odemeturu = 3;
            if (radioButton4.Checked) odemeturu = 5;

            // 1- Ödenen Adisyonun Durumu True olmalı
            var adisyonbul = db.TBLADISYON.Find(Convert.ToInt32(lblAdisyonID.Text));
            if (adisyonbul != null)
            {
                adisyonbul.DURUM = true;
                db.SaveChanges();
            }

            // 2-HESAPÖDEME tablosuna kaydedelim
            TBLHESAPODEME yh = new TBLHESAPODEME();
            yh.ADISYONID = adisyonbul.ID;
            yh.ODEMETURU = odemeturu;
            yh.ARATOPLAM = Convert.ToDecimal(lblaratoplam.Text);
            yh.KDV = 0;
            yh.INDIRIM = Convert.ToDecimal(tbindirim.Text);
            yh.TOPLAMTUTAR = Convert.ToDecimal(lblfiyat.Text);
            yh.TARIH = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            yh.MUSTERIID = int.Parse(lblmusteriID.Text);
            yh.DURUM = true;
            db.TBLHESAPODEME.Add(yh);
            db.SaveChanges();

            // 3-müşterinin sipariş sayısı artmalı
            if (lblmusteriID.Text != "1")
            {
                var bulmusteri = db.TBLMUSTERI.Find(int.Parse(lblmusteriID.Text));
                bulmusteri.SIPARISSAYISI++;
                db.SaveChanges();
            }

            // 4- Masanın durumunu "B" değiştir
            var bulmasa = db.TBLMASA.Find(int.Parse(lblmasano.Text));
            bulmasa.DURUM = "B";
            db.SaveChanges();

            MessageBox.Show(bulmasa.MASAID + " nolu masanın hesabı kapanmıştır");

            masarenklendir();

            dataGridView3.DataSource = null;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;

            int secilenmusteriID = Convert.ToInt32(dataGridView2.Rows[secilen].Cells[0].Value.ToString());

            lblmusteriID.Text= secilenmusteriID.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmSatışAnalizi f=new frmSatışAnalizi();
            f.Show();
        }
    }
}
