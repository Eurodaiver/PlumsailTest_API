using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;

namespace PlumsailTest_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ObjectsController : ControllerBase
    {
        private readonly ILogger<ObjectsController> _logger;
        private readonly MDBService _context;

        public ObjectsController(ILogger<ObjectsController> logger, MDBService context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddObject(object obj)
        {
            var t = obj.ToBsonDocument();
            var str = JsonSerializer.Serialize(obj);

            BsonDocument doc = BsonDocument.Parse(str);
            await _context.Create(doc);
            _logger.LogInformation("Object added into DB");
            return Ok();
        }

        /// <summary>
        /// поиск объектов по строке
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<object>> SearchObjects(string str)
        {
            List<BsonDocument> docs = (await _context.GetItems(str)).ToList();
            _logger.LogInformation("Found {} objects", docs.Count);
            List<object> result = docs.ConvertAll(BsonTypeMapper.MapToDotNetValue);

            return result;
        }
    }
}
