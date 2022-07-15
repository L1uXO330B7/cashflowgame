using Microsoft.AspNetCore.Mvc;
using MiniProfilerSwagger.EF;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly MiniProfilerDbContext _db;
        private static readonly string[] Summaries = new[]
        {
           "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MiniProfilerDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 故意報錯
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetError")]
        public int GetError()
        {
            int i = 0;
            i = 1 / i;
            return i;
        }

        /// <summary>
        /// Sql 查詢
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLinq")]
        public dynamic GetLinq()
        {
            var _MiniProfiler = MiniProfiler.Current;
            using (_MiniProfiler.Step("GetLinq")) // 寫成 AOP 掛在攔截器或篩選器
            {
                var test = new List<string>() { "aa", "bb", "cc", "dd" };
                return test.Where(x => x == "aa").ToList();
            }
        }

        /// <summary>
        /// Sql 查詢
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEFCore")]
        public dynamic GetEFCore()
        {
            var _MiniProfiler = MiniProfiler.Current;
            using (_MiniProfiler.Step("GetEFCore")) // 寫成 AOP 掛在攔截器或篩選器
            {
 
                    return _db.Users.ToList();
            }
        }

        /// <summary>
        /// 獲取 MiniProfiler HTML 片段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMiniProfilerScript")]
        public IActionResult GetMiniProfilerScript()
        {
            var html = MiniProfiler.Current.RenderIncludes(HttpContext);
            return Ok(html.Value);
        }
    }
}