# 🍽️ Restoran Otomasyonu (Windows Forms)

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=dotnet&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D7?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6-6C1D5F?style=for-the-badge)

Restoranda **garson, kasiyer ve yönetici** rollerine göre ayrı paneller sunan masaüstü otomasyon. Masa ve adisyon takibi, sipariş, ödeme, gün sonu ve grafikli raporlar içerir.

> Ekip arkadaşlarımla birlikte geliştirdiğim WPF tabanlı [Restoran-Otomasyonu](https://github.com/foxseSReal/Restoran-Otomasyonu) projesinden ayrı, ders kapsamında Windows Forms ile yazdığım bir projedir.

---

## 🚀 Özellikler

### 🔐 Giriş
Kullanıcı adı ve şifre ile giriş; personelin görevine göre (**Admin / Kasiyer / Garson**) ilgili panel açılır.

### 🧑‍🍳 Garson paneli
- Masa düzeni: boş / dolu / seçili masalar
- Adisyon açma, ürün ekleme, adet artırma/azaltma, sipariş kaydetme
- **Paket sipariş** (masa 999)
- Masa birleştirme
- Müşteri ekleme ve güncelleme, aktif siparişler

### 💳 Kasiyer paneli
- Açık adisyonlar, ödeme alma (**nakit, kart, ticket, QR**), indirim ve KDV hesabı
- Kasa girişi ve çıkışı
- **Gün sonu** kaydı ve raporlar

### 📊 Yönetici paneli
- **Dashboard (grafikler):** Son 1 haftanın ciro grafiği, personel satış grafiği, en çok satılan kategoriler, en çok kazandıran ürünler, bugün en çok ve en az satılan ürünler
- **Satış analizi:** Toplam ciro ve sipariş sayısı, en aktif müşteri, en çok satılan 5 ürün, masaya ve personele göre satışlar
- Masa, kategori, ürün ve personel yönetimi (ekle / güncelle / aktif-pasif)

## 🗄️ Veritabanı (`DbRestaurant26A`)

13 tablo: `TBLMASA`, `TBLKATEGORI`, `TBLURUN`, `TBLADISYON`, `TBLSATISLAR`, `TBLMUSTERI`, `TBLPERSONEL`, `TBLHESAPODEME`, `TBLODEMETURU`, `TBLSERVISTURU`, `TBLKASAHARAKET`, `TBLGUNSONU`, `TBLISLEMLOG`

---

## ⚙️ Kurulum

**Gereksinimler:** Visual Studio 2022/2026 (*.NET masaüstü geliştirme*) · SQL Server (Express yeterli) · SQL Server Management Studio (SSMS) 22

1. **İndirin**
   ```bash
   git clone https://github.com/GencayCeliker/Restoran-Otomasyonu-WinForms.git
   ```
2. **Veritabanını kurun (SSMS 22):** `Veri Tabanı/DbRestaurant26A.sql` dosyasını açıp **Execute (F5)**'e basın. Örnek masalar, ürünler ve yaklaşık 450 satır satış/adisyon verisi yüklenir; grafikler bu veriyle dolu görünür.
3. **Bağlantıyı kontrol edin:** `RestaurantOtomasyon26A/App.config` içinde `data source=.\SQLEXPRESS` kullanılır. Varsayılan SQL Server örneği kullanıyorsanız `data source=.` yapın.
4. **Çalıştırın:** `RestaurantOtomasyon26A.sln` dosyasını açın; NuGet paketleri otomatik yüklenir; **F5** ile başlatın.

**Demo girişler** (şifre hepsinde `1`):

| Rol | Kullanıcı adı |
| --- | --- |
| Admin | `a` |
| Kasiyer | `k` |
| Garson | `g` |

## 📄 Lisans

Bu proje [MIT](LICENSE) lisansı altında sunulmuştur.
