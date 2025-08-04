using System.ComponentModel.DataAnnotations;

namespace RstApiExample.DTO.Requests;

public class UserLogInDTO
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    public string Password { get; set; }
}