# Menu
* [Create Database using EF core](#Create Database using EF core)
* [Config appsetting.json](#Config appsetting.json)
* [Create DbSet](#Create DbSet)
* [Migration](#Migration)
* [Add relationship](#Add relationship)
* [Create first Api](#Create first Api)


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

**More details**
I consider `DbSet<HangHoa> HangHoas` is convert structure HangHoa -> DbSet. And
`get` is `select sql`
`set` is `insert sql`

Below the constructor, I created a DbSet HangHoas
> DbSet represents a set of HangHoa entities
> Every where that want to interact with HangHoa EF => inject MyDbContext and use


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

> In `Up` method I can see all properties of HangHoa table was generate, And if I use `Update-database` this method will be run and create table database same properties described on it
> And `Down` method will run when `Remove-migration` and it drop this table database same name.

**So I run `update-database` **


# Add relationship
**Add relationship Table Loai and Table HangHoa**

**at Table Loai I have general property, and a special property named HangHoas, it is represent for 1-n relationship**
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

> `?` is exist or not `[ForeignKey("MaLoai")]` set the MaLoai is foreign key

After that I add new `DbSet`
```c#
public DbSet<Loai> Loais { get; set; }
```


# Create first Api
**Overview**
Create new file `LoaisController.cs` at Controllers folder

In the first lines I can see 
```c#
[Route("api/[controller]")]
[ApiController]
public class LoaisController : ControllerBase
{}
```
> The `controller` in `[Route("api/[controller]")]` will map with the class name bellow
In this situation it become `[Route("api/Loais")]`

**Inside the class LoaisController**
```c#
private readonly MyDbContext _context;

public LoaisController(MyDbContext context)
{
   _context = context;
}
```
> `readonly` keyword is mean Run-Time constant 
-> it will be assigned value from `LoaisController()` at the first run of program, and **Can not be changed after**
So `_context` is represent for `MyDbContext` and I will working on it.

**Structure of Get and Get by Id api method**


**GET**
```c#
[HttpGet]
public IActionResult GetAll()
{
   var dsLoai = _context.Loais.ToList();
   return Ok(dsLoai);
}
```
> `ToList()` is a LINQ syntax, help parse `Loais` to `List` structure
It convert all data in `Loais` into a `List`
<hr></hr>

**GET by id**
```c#
[HttpGet("{id}")]
public IActionResult GetAll(int id)
{
   var dsLoai = _context.Loais.SingleOrDefault(loai => loai.MaLoai == id);
   if (dsLoai == null)
       return Ok(dsLoai);
   else
       return NotFound();
}
```
`SingleOrDefault` Returns a single, specific element of a sequence, or a default value if that element is not found (return `[]`).
<hr></hr>

**POST**
In post i need define (create) model 

Create `LoaiModel.cs` file in `Models` folder
> Everything was defined in this file, it **will show** in swagger `POST` method

```c#
public class LoaiModel
{
  [Required]
  [MaxLength(50)]
  public string Tenloai { get; set; }
}
```
At here I only define the `TenLoai` because `MaLoai` will be **automatic increase**

After create models

```c#
[HttpPost]
public IActionResult CreateNew(LoaiModel model)
{
   var loai = new Loai
   {
       Tenloai = model.Tenloai,
   };
   _context.Add(loai);
   _context.SaveChanges();
   return Ok(new
   {
       loai.MaLoai,
       loai.Tenloai
   });
}
```

1. Create a instance `Loai` named `loai`
2. assign `model.TenLoai` (which required I input on swagger) to `Tenloai` (An property of `loai` - instance of `Loai`)
**Note:** `model.TenLoai` is data of me input from keyboard...
3. Add value to `_context`
4. SaveChanges() will trigger the insert SQL
5. return 2 field `MaLoai` and `Tenloai` that just input
<hr></hr>

**PUT**
```c#
[HttpPut("{id}")]
public IActionResult UpdateLoaiById(int id, LoaiModel model)
{
   var dsLoai = _context.Loais.SingleOrDefault(loai => loai.MaLoai == id);
   if (dsLoai != null)
   {
       dsLoai.Tenloai = model.Tenloai;
       _context.SaveChanges();
       return NoContent();
   }
   else
       return NotFound();
}
```
<hr></hr>
