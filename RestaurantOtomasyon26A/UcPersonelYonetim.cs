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
    public partial class UcPersonelYonetim : UserControl
    {
        DbRestaurant26AEntities1 db=new DbRestaurant26AEntities1 ();
        public UcPersonelYonetim()
        {
            InitializeComponent();
        }

        private void UcPersonelYonetim_Load(object sender, EventArgs e)
        {
            PersonelleriListele();
        }
        public void PersonelleriListele()
        {
            var personeller = from x in db.TBLPERSONEL
                              select new
                              {
                                  ID = x.ID,
                                  ADSOYAD = x.ADSOYAD,
                                  GOREV = x.GOREV,
                                  KULLANICIADI = x.KULLANICIADI,
                                  SIFRE = x.SIFRE,
                                  DURUM = x.DURUM
                              };
            dataGridView1.DataSource = personeller.ToList();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            string aranan = textBox1.Text;
            var personeller = db.TBLPERSONEL.Where(x => x.ADSOYAD.Contains(aranan)).ToList();
            dataGridView1.DataSource = personeller;
        }

        private void button10_Click(object sender, EventArgs e)
        {       
            if (dataGridView1.CurrentRow != null)
            {
                int seciliId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                DialogResult cevap = MessageBox.Show("Bu personeli silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (cevap == DialogResult.Yes)
                {
                    var silinecekPersonel = db.TBLPERSONEL.Find(seciliId);
                    if (silinecekPersonel != null)
                    {
                        db.TBLPERSONEL.Remove(silinecekPersonel);
                        db.SaveChanges();

                        MessageBox.Show("Personel başarıyla silindi.");

                         PersonelleriListele(); 
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için listeden bir personel seçin.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmPerEkle frm = new frmPerEkle();
            frm.ShowDialog();
            PersonelleriListele();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                frmPerGuncelle frmGuncelle = new frmPerGuncelle(secilenId);
               
                frmGuncelle.ShowDialog();

                PersonelleriListele(); 
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz personeli listeden seçin.", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                PersonelleriListele();
            }
          
        }

     
    }
    
}
