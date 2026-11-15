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
    public partial class ucMasaYonetimi : UserControl
    {
        public ucMasaYonetimi()
        {
            InitializeComponent();
        }
        int seciliMasaID = 0;
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        private void button9_Click(object sender, EventArgs e)
        {
            frmMasaEkle frm = new frmMasaEkle();
            frm.ShowDialog();
            masagetir();
            masarenklendir();
        }
        #region Masa 
        Button[] ms = new Button[101];

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
            if (checkBox1.Checked==true)
            {
                masalar = (from x in db.TBLMASA
                           where x.MASAID != 999
                           select new
                           {
                               x.MASAID,
                               x.DURUM
                           }).ToList();
            }
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
            seciliMasaID = (int)b.Tag;

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

        private void ucMasaYonetimi_Load(object sender, EventArgs e)
        {
            masagetir();
            masarenklendir();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // 1. KONTROL: Kullanıcı bir masaya tıklamış mı?
            if (seciliMasaID == 0)
            {
                MessageBox.Show("Lütfen önce yukarıdaki panelden güncellemek istediğiniz masaya tıklayarak seçin!", "Masa Seçilmedi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // İşlemi durdur
            }

            // 2. Formu seçili ID ile aç
            frmMasaGuncelle frm = new frmMasaGuncelle(seciliMasaID);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();

            // 3. Form kapandıktan sonra sayfayı yenile
            masagetir();
            masarenklendir();


            seciliMasaID = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (seciliMasaID == 0)
            {
                MessageBox.Show("Lütfen önce yukarıdaki panelden silmek istediğiniz masaya tıklayarak seçin!", "Masa Seçilmedi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var silinecekMasa = db.TBLMASA.Find(seciliMasaID);

            if (silinecekMasa != null)
            {
                if (silinecekMasa.DURUM == "D")
                {
                    MessageBox.Show("İçerisinde müşteri oturan (Dolu) bir masayı silemezsiniz! Lütfen önce hesabı kapatın.", "Silme Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (silinecekMasa.DURUM == "R")
                {
                    MessageBox.Show("Rezerve edilmiş bir masayı silemezsiniz! Lütfen önce rezervasyonu iptal edin.", "Silme Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                DialogResult cevap = MessageBox.Show($"Masa {seciliMasaID} isimli masayı silmek istediğinize emin misiniz?", "Masa Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (cevap == DialogResult.Yes)
                {
                    silinecekMasa.AKTIFMI = false;
                    db.SaveChanges(); 
                    MessageBox.Show("Masa başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    masagetir();
                    masarenklendir();
                    seciliMasaID = 0;
                }
                else
                {
                    MessageBox.Show("Masa silme işlemi iptal edildi.", "İptal Edildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            masagetir();
            masarenklendir();
        }
    }
}
