using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using MongoDB.Bson.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace PlumsailTest_API
{
    public class MDBService
    {
        IMongoCollection<BsonDocument> ObjectItems;
        public MDBService()
        {
            string connectionString = "mongodb+srv://Plumsail2020:Plumsail2020@cluster0.pbjca.azure.mongodb.net/plumsail?retryWrites=true&w=majority";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);

            ObjectItems = database.GetCollection<BsonDocument>("Objects");
        }

        /// <summary>
        /// get documents by search string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BsonDocument>> GetItems(string str)
        {
            var filter = Builders<BsonDocument>.Filter.Regex("props.v", new BsonRegularExpression(str));

            List<BsonDocument> found = ObjectItems.Find(filter).Project(new BsonDocument { { "props", 1 }, { "_id", 0 } }).ToList();
     
            return found;
        }


        /// <summary>
        /// adding document into DB
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public async Task Create(BsonDocument doc)
        {
            //BsonDocument data = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(str);
            //convert fields of object to list of props for better indexing in db

            BsonArray arr = new BsonArray();
            foreach (var d in doc)
            {
                arr.Add(new BsonDocument() { { "k", d.Name }, { "v", d.Value.ToString() } });
            }

            var resultDoc = new BsonDocument { { "props", arr } };

            await ObjectItems.InsertOneAsync(resultDoc);
        }


    }
}

