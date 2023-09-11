#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogReg.Models;

public class LoginUser

{
[Required]
[EmailAddress]
[Display(Name = "Email")]
public string LogEmail {get; set;}

[Required]
[DataType(DataType.Password)]
[Display(Name = "Password")]
[MinLength(8, ErrorMessage ="Password Must Be At Least 8 Characters")]
public string LogPassword {get; set;}
}



