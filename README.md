# WebApiNet5CodeFirst
My first web api

# Create Database using EF core
**Create database using Entity Framework core**

import 2 namespace DataAnnotations and DataAnnotations.Schema to use properties such as `[Key] [Required]...`  to create database for project code first.
```
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
```

```
   [Table("HangHoa")]
    public class HangHoa
    {
        [Key]
        public Guid MaHangHoa { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenHh { get; set; }

        public string MoTa { get; set; }

        [Range(0,double.MaxValue)]
        public string GiamGia { get; set; }


        public double DonGia { get; set; }

    }
```
# Config appsetting.json
**I need config appsetting.json to add ConnectionString**
```
"ConnectionStrings": {
    "MyDB": "Data Source=DESKTOP-AKO8HKM\\SQLEXPRESS;Initial Catalog=Net5CodeFirst;Integrated Security=True"
  }
```

