using MongoDB.Bson;

namespace Starboat.Models
{
    public class StarModel
    {
        private ObjectId _Id { get; }
        public string MessageId { get; }
        public string ChannelId { get; }
        public string AuthorId { get; }
        public int Stars { get; }
    }
}