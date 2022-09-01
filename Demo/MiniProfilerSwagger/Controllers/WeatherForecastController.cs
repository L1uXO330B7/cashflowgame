using Microsoft.AspNetCore.Mvc;
using MiniProfilerSwagger.EF;
using MiniProfilerSwagger.Filter;
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
        /// �G�N����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetError")]
        public int GetError()
        {
            int i = 0;

            Error();

            i = GetOne();

            return i;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void Error()
        {
            int i = 0;
            i = 1 / i;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public int GetOne()
        {
            return 1;
        }

        /// <summary>
        /// Sql �d��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLinq")]
        public dynamic GetLinq()
        {
            //var _MiniProfiler = MiniProfiler.Current;
            //using (_MiniProfiler.Step("GetLinq")) // �g�� AOP ���b�d�I���οz�ﾹ
            //{
            var test = new List<string>() { "aa", "bb", "cc", "dd" };
            return test.Where(x => x == "aa").ToList();
            //}
        }

        /// <summary>
        /// Sql �d��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEFCore")]
        public dynamic GetEFCore()
        {
            //var _MiniProfiler = MiniProfiler.Current;
            //using (_MiniProfiler.Step("GetEFCore")) // �g�� AOP ���b�d�I���οz�ﾹ
            //{

            _db.Users.ToList();

            return _db.Users
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .ToList();

            //}
        }

        /// <summary>
        /// ��� MiniProfiler HTML ���q
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