using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNet5CodeFirst.Models;

namespace WebApiNet5CodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hanghoas = new List<HangHoa>();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(hanghoas);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var hanghoa = hanghoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(hanghoa);
                }
            }
            catch
            {
                return BadRequest();
            }
            
        }

        [HttpPost]
        public IActionResult Create(HangHoaVM hangHoaVM)
        {
            var hanghoa = new HangHoa
            {
                MaHangHoa = Guid.NewGuid(),
                TenHangHoa = hangHoaVM.TenHangHoa,
                DonGia = hangHoaVM.DonGia
            };
            hanghoas.Add(hanghoa);
            return Ok(new
            {
                Success = true,
                Data = hanghoa,
            });
        }

        [HttpPut("{id}")]
        public IActionResult Edit (string id, HangHoa hanghoaEdit)
        {
            var hanghoa = hanghoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
            if(hanghoa == null)
            {
                return NotFound();
            }
            else
            {
                hanghoa.TenHangHoa = hanghoaEdit.TenHangHoa;
                hanghoa.DonGia = hanghoaEdit.DonGia;
                return Ok(new
                {
                    Success = true,
                    Data = hanghoa,
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete (string id)
        {
            var hanghoa = hanghoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
            hanghoas.Remove(hanghoa);
            return Ok();
        }

    }
}
