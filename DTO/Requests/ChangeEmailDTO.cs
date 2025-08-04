using System.ComponentModel.DataAnnotations;

namespace RstApiExample.DTO.Requests;

public class ChangeEmailDTO
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}