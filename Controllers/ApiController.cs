using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT150Site.Models;
using NuGet.Protocol.Core.Types;
using System.Linq;

namespace MSIT150Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context; //新的注入方法
        private readonly IWebHostEnvironment _host; //檔案上傳
        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(5000); //睡五秒鐘，再往下執行
            return Content("Hello Ajax!!");
        }

        public IActionResult GetDemo(UserViewModel user) //運用類別的方式 //運用變數的方式 => GetDemo(string name, int age = 30)
        {
            if (string.IsNullOrEmpty(user.name)) {
                user.name = "guest";
            }
            return Content($"Hello {user.name}, You Are { user.age } years old");
        }

        //=============檢查姓名是否重複
        public IActionResult CheckDuplicateName(string name)
        {
            bool result = _context.Members.Where(c => c.Name == name).Count() >= 1 ? true : false;

            return Json(new { IsDuplicate = result });
        }

        //=============新增資料到DB（Demo => Members）
        public IActionResult Register(Members member, IFormFile file)
        {
            string filePath = Path.Combine(_host.WebRootPath, "uploads", file.FileName);

            //========上傳檔案到wwwroot底下的資料夾
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            //return Content($"新增檔案到 {filePath}"); //顯示在下方的字串

            //========將圖轉成二進位
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }

            //=======其他欄位的值
            member.FileName = file.FileName;
            member.FileData = imgByte;
            //=========新增至DB
            _context.Members.Add(member);
            _context.SaveChanges();

            return Content("新增成功");
        }
        //=============讀DB（DEMO=>address Table）城市資料API
        public IActionResult Cities() //https://localhost:7267/api/Cities
        {
            var cities = _context.Address.Select(c => c.City).Distinct();
            return Json(cities);
        }
        //=============讀DB（DEMO=>address Table）鄉鎮資料API
        //根據城市名稱，回傳城市的鄉鎮區JSON資料
        public IActionResult Districts(string city) //https://localhost:7267/api/Districts?city=金門縣
        {
            var districts = _context.Address.Where(a => a.City == city).Select(c => c.SiteId).Distinct();
            return Json(districts);
        }
        //=============讀DB（DEMO=>address Table）路名API
        //根據鄉鎮區名稱，回傳鄉鎮區的路名JSON資料
        public IActionResult Roads(string siteId) //https://localhost:7267/api/Roads?siteId=金門縣金沙鎮
        {
            var roads = _context.Address.Where(a => a.SiteId == siteId).Select(c => c.Road).Distinct();
            return Json(roads);
        }


        [HttpGet]
        //在使用數據庫操作時，建議使用異步操作async await，以避免阻塞線程和提高應用的性能。如果你不使用異步和等待操作，可能會導致應用在執行數據庫查詢時變得不夠響應。
        public async Task<IActionResult> GetAutoCompleteData(string searchTerm)
        {
            var results = await _context.Address
                .Where(a => a.Road.Contains(searchTerm))
                .Select(a => a.Road)
                .Distinct()
                .ToListAsync();

            return Json(results);
        }
    }
}
