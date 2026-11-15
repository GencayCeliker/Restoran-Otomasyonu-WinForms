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
    public partial class UcKasiyerKasaIslemleri : UserControl
    {
        public UcKasiyerKasaIslemleri()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmKasiyerKasaGiris frm = new FrmKasiyerKasaGiris();
            frm.ShowDialog();
            kasahareketlistele();
            KasaBakiyesiHesapla();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmKasiyerCikis frm = new frmKasiyerCikis();
            frm.ShowDialog();
            kasahareketlistele();
            KasaBakiyesiHesapla();
        }
        DbRestaurant26AEntities1 db = new DbRestaurant26AEntities1();
        public void kasahareketlistele()
        {
            var kasaHareketleri = from k in db.TBLKASAHARAKET
                                  select new
                                  {
                                      k.ID,
                                      ISLEMTURU = k.ISLEMTURU == true ? "Giriş" : "Çıkış",
                                      k.TARIHSAAT,
                                      k.TUTAR,
                                      k.ACIKLAMA
                                  };
            dataGridView1.DataSource = kasaHareketleri.ToList();
        }
        private void UcKasiyerKasaIslemleri_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new Font("Microsoft Sans Serif", 12);
            kasahareketlistele();

            KasaBakiyesiHesapla();
        }
        private void KasaBakiyesiHesapla()
        {
            DateTime bugun = DateTime.Today;
            DateTime yarin = bugun.AddDays(1);
            decimal restoranGeliri = db.TBLHESAPODEME
                .Where(x => x.DURUM == true && x.TARIH >= bugun && x.TARIH < yarin)
                .Select(x => (decimal?)x.TOPLAMTUTAR)
                .Sum() ?? 0;
            decimal kasaGirisi = db.TBLKASAHARAKET
                .Where(x => x.ISLEMTURU == true && x.TARIHSAAT >= bugun && x.TARIHSAAT < yarin)
                .Select(x => (decimal?)x.TUTAR)
                .Sum() ?? 0;
            decimal kasaCikisi = db.TBLKASAHARAKET
                .Where(x => x.ISLEMTURU == false && x.TARIHSAAT >= bugun && x.TARIHSAAT < yarin)
                .Select(x => (decimal?)x.TUTAR)
                .Sum() ?? 0;
            decimal netBakiye = restoranGeliri + kasaGirisi - kasaCikisi;
            label2.Text =  netBakiye.ToString("C2");
        }
    }
}
