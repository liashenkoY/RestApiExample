using System.ComponentModel.DataAnnotations;

namespace RstApiExample.DTO.Requests;

public class UserCreateOrUpdateDTO
{
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }
}