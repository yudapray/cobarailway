using System.ComponentModel.DataAnnotations;

namespace GuestbookApp.Models;

public class GuestEntry
{
    [Required(ErrorMessage = "Nama wajib diisi.")]
    [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
    [Display(Name = "Nama")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Alamat wajib diisi.")]
    [StringLength(300, ErrorMessage = "Alamat maksimal 300 karakter.")]
    [Display(Name = "Alamat")]
    public string Address { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}