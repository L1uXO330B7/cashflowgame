using Microsoft.AspNetCore.Mvc;
using MiniProfilerSwagger.EF;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
        [Route("GetUsers")]
        public List<User> GetUsers()
        {
            //var _MiniProfiler = MiniProfiler.Current;
            //using (_MiniProfiler.Step("GetUsers")) // 寫成 AOP 掛在攔截器或篩選器
            //{

            using (var Db = new MiniProfilerDbContext())
            {
                try
                {
                    return Db.Users.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //}
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