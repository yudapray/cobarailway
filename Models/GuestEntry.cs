using System.ComponentModel.DataAnnotations;

namespace GuestbookApp.Models;

public class GuestEntry
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama wajib diisi.")]
    [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
    [Display(Name = "Nama Lengkap")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Alamat wajib diisi.")]
    [StringLength(300, ErrorMessage = "Alamat maksimal 300 karakter.")]
    [Display(Name = "Alamat / Asal Kota")]
    public string Address { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Pesan maksimal 500 karakter.")]
    [Display(Name = "Pesan atau Kesan (Opsional)")]
    public string? Message { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}