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
    public partial class Garson : Form
    {
        public Garson()
        {
            InitializeComponent();
        }
        DbRestaurant26AEntities1 db= new DbRestaurant26AEntities1();
        private void Garson_Load(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ButonYuvarla(button1, 20);
            form2.ButonYuvarla(button2, 20);
            form2.ButonYuvarla(button3, 20);
            form2.ButonYuvarla(button4, 20);
            form2.ButonYuvarla(button5, 20);
            form2.ButonYuvarla(button6, 20);
            form2.ButonYuvarla(button11, 20);
            form2.ButonYuvarla(button12, 20);
            form2.ButonYuvarla(button9, 20);
            form2.ButonYuvarla(button10, 20);
            form2.PanelYuvarla(panel1, 20);
            masagetir();
            masarenklendir();
            var personel = db.TBLPERSONEL.SingleOrDefault(x => x.ID == Yonetim.PersonelID);
            if (personel != null) label1.Text = "Hoşgeldiniz, " + personel.ADSOYAD;
        }
        #region Masa 
        Button[] ms = new Button[101];

        public void masagetir()
        {
            // Arayüzü temizle
            listView2.Items.Clear();
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

            // ID'yi Tag'den güvenli bir şekilde alıyoruz
            int secilenmasaID = (int)b.Tag;

            //lblMasa.Text = secilenmasaID.ToString();

            // KRİTİK DÜZELTME: Her tıklamada önce listeyi ve toplamları sıfırla
            //listView1.Items.Clear();
            //lblAraToplam.Text = "₺0,00";
            //lblfiyat.Text = "₺0,00";

            var masa = db.TBLMASA.Find(secilenmasaID);

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

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
