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
        /// <summary>
        /// add object to DB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddObject(object obj)
        {
            try
            {
                BsonDocument doc = BsonDocument.Parse(obj.ToString());
                await _context.Create(doc);
                _logger.LogInformation("Object added into DB");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }

        }

        /// <summary>
        /// search objects by string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<object>>> SearchObjects(string str)
        {
            try
            {
                List<BsonDocument> docs = (await _context.GetItems(str)).ToList();
                List<object> result = docs.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                _logger.LogInformation("Found {} objects", docs.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }

        }
    }
}
