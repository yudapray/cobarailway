# Buku Tamu

Aplikasi buku tamu sederhana menggunakan ASP.NET Core MVC dan C#.

## Menjalankan aplikasi

Pasang .NET SDK `10.0.401`, lalu jalankan perintah berikut dari folder proyek:

```powershell
dotnet restore
dotnet run
```

Buka URL yang ditampilkan oleh `dotnet run` di browser. Isian nama dan alamat divalidasi di server, lalu ditampilkan pada daftar catatan pengunjung.

## Penyimpanan

Data saat ini disimpan di memori proses dan akan kosong kembali ketika aplikasi dimulai ulang. Untuk penggunaan permanen, hubungkan ke database seperti SQLite atau SQL Server.