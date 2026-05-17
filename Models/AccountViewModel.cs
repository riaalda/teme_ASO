using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Numele de utilizator este obligatoriu.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Numele de utilizator este obligatoriu.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Intre 3 si 50 de caractere.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email-ul este obligatoriu.")]
    [EmailAddress(ErrorMessage = "Email invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Minim 3 caractere.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Parolele nu coincid.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}