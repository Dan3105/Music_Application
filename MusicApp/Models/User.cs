using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace RazorPagesMusic.Users;

[Table("user_account")]
public class User
{
    public int Id {get; set;}
    public int Public_id {get; set; }
    public int Email {get; set;}

    public string? Password {get; set;}

    [DataType(DataType.DateTime)]
    public DateTime created {get; set;}

    public bool is_staff {get; set;}
    public bool is_superuser {get; set;}
    public bool is_activate {get; set;}
}