using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL.Enum
{
    public enum CategoryType
    {
        [Display(Name = "Food")]
        Food = 1,
        [Display(Name = "Garments")]
        Garments = 2,
        [Display(Name = "Automobile")]
        Automobile = 3,
        [Display(Name = "Leather Accessories")]
        LeatherAccessories = 4,
        [Display(Name = "Electronics")]
        Electronics = 5,
        [Display(Name = "Health")]
        Health = 6,
        [Display(Name = "Medicine")]
        Medicine = 7,
        [Display(Name = "Cosmetics")]
        Cosmetics = 8,
        [Display(Name = "Decoration")]
        Decoration = 9,
        [Display(Name = "Industrial Parts")]
        IndustrialParts = 10,
        [Display(Name = "Machinery")]
        Machinery = 11,
        [Display(Name = "Metallurgy")]
        Metallurgy = 12,
        [Display(Name = "Chemicals")]
        Chemicals = 13,
        [Display(Name = "Printing & Advertising")]
        PrintingAndAdvertising = 14,
        [Display(Name = "Others")]
        Others = 15

    }
}
