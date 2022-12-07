# WebApiNet5CodeFirst
My first web api

# Create Database using EF core
**Create database using Entity Framework core**

import 2 namespace DataAnnotations and DataAnnotations.Schema to use properties such as `[Key] [Required]...`  to create database for project code first.
```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
```

```c#
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
```c#
"ConnectionStrings": {
    "MyDB": "Data Source=DESKTOP-AKO8HKM\\SQLEXPRESS;Initial Catalog=Net5CodeFirst;Integrated Security=True"
  }
```

# Create DbSet
**At MyDbContext.cs**
Create a `MyDbContext` class inherit `DbContext` to use properties of `DbContext`
Create a constructor that has `options` property that inherit 

```c#
public class MyDbContext : DbContext
{
   public MyDbContext(DbContextOptions options) : base(options){}
   public DbSet<HangHoa> HangHoas { get; set; }
}
```

Below the constructor, I created a DbSet HangHoas
*DbSet represents a set of HangHoa entities*
*Every where that want to interact with HangHoa EF => inject MyDbContext and use*

# Migration
**Migration to create database mapped DbSet**
```
Add-migration Initial
```

After migration, I will see a folder Migration. In folder Migration I only pay attention to `_Inital.cs`
In this file I have 2 method `Up` and `Down`:
```c#
protected override void Up(MigrationBuilder migrationBuilder)
{
   migrationBuilder.CreateTable(
       name: "HangHoa",
       columns: table => new
       {
           MaHangHoa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
           TenHh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
           MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
           GiamGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
           DonGia = table.Column<double>(type: "float", nullable: false)
       },
       constraints: table =>
       {
           table.PrimaryKey("PK_HangHoa", x => x.MaHangHoa);
       });
}

protected override void Down(MigrationBuilder migrationBuilder)
{
   migrationBuilder.DropTable(
       name: "HangHoa");
}
```

In `Up` method I can see all properties of HangHoa table was generate, And if I use `Update-database` this method will be run and create table database same properties described on it
And `Down` method will run when `Remove-migration` and it drop this table database same name.

**So I run `update-database` **

# Add relationship
**Add relationship Table Loai and Table HangHoa**

**at Table Loai I have general property, and a special property named HangHoas, it is represent for 1-n relationship

*1 Loai has many HangHoas*
```c#
[Table("Loai")]
public class Loai
{
  [Key]
  public int MaLoai { get; set; }
  [Required]
  [MaxLength(50)]
  public string Tenloai { get; set; }

  public ICollection<HangHoa> HangHoas { get; set; }

}
```

**HangHoa table**
*1 HangHoa only have 1 MaLoai*
```c#
public class HangHoa
{
   ...
   public int? MaLoai { get; set; }
   [ForeignKey("MaLoai")]
   public Loai Loai { get; set; }
}
```

`?` is exist or not `[ForeignKey("MaLoai")]` set the MaLoai is foreign key

After that I add new `DbSet`
```c#
public DbSet<Loai> Loais { get; set; }
```

# Create first Api

Create new file `LoaisController.cs` at Controllers folder


