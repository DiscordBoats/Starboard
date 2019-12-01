using MongoDB.Bson;

namespace Starboat.Models
{
    public class StarModel
    {
        private ObjectId _Id { get; set; }
        public string Content { get; set; }
        public string MessageId { get; set; }
        public string AuthorId { get; set; }
        public int Stars { get; set; }
    }
}