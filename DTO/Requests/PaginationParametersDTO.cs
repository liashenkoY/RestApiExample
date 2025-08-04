namespace RstApiExample.DTO.Requests;

public class PaginationParametersDTO
{
    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
}