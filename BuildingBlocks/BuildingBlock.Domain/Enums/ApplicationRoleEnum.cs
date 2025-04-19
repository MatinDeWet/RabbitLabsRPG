using System.ComponentModel.DataAnnotations;

namespace BuildingBlock.Domain.Enums;

public enum ApplicationRoleEnum
{
    [Display(Name = "None")]
    None = 0,

    [Display(Name = "Admin")]
    Admin = 1,

    [Display(Name = "Super Admin")]
    SuperAdmin = Admin | 2
}
