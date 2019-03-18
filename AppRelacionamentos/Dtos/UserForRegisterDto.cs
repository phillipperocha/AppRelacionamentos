using System.ComponentModel.DataAnnotations;

namespace AppRelacionamentos.Dtos
{
    public class UserForRegisterDto
    {

    // Aqui usaremos Anotations. 
    // Temos como validar o tipo de Email, Tamanho de Strings, Telefone, etc...
    [Required]
     public string Username { get; set; }   

     [Required]
    [StringLength(16,MinimumLength = 6, ErrorMessage = "You must specify passwords between 6 and 16 characters.")]
     public string Password { get; set; }
    }
}