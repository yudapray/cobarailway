# Buku Tamu Digital

Aplikasi buku tamu modern dengan desain *soft pink aesthetic* yang elegan dan nyaman dipandang, menggunakan ASP.NET Core MVC, C#, dan penyimpanan permanen berbasis SQLite.

## Fitur Utama

- **Desain Bernuansa Merah Muda (Soft Pink & Rose Aesthetic):**
  - Kombinasi palet pastel blush, rose, dan warm ivory yang lembut dan ramah di mata (*eye-friendly*).
  - Aksen tipografi Playfair Display & Plus Jakarta Sans yang rapi dan modern.
  - Kartu formulir dan daftar pengunjung interaktif dengan inisial avatar dinamis.
  - Notifikasi sukses dan badge indikator koneksi SQLite langsung di antarmuka.
- **Penyimpanan Database SQLite:**
  - Data buku tamu disimpan secara permanen di file SQLite (`App_Data/guestbook.db`).
  - Dilengkapi migrasi tabel otomatis saat aplikasi pertama kali dijalankan.
  - Mendukung input Nama Lengkap, Asal Kota / Alamat, serta Pesan/Kesan opsional.
  - Parameterized queries untuk mencegah SQL Injection.

## Menjalankan Aplikasi

Jalankan perintah berikut dari folder proyek:

```powershell
dotnet run
```

Buka URL yang ditampilkan oleh terminal (misal: `http://localhost:5000` atau `https://localhost:5001`) di browser.

## Konfigurasi Database

Lokasi database SQLite dapat dikonfigurasi melalui `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "GuestbookDb": "Data Source=App_Data/guestbook.db"
  }
}
```