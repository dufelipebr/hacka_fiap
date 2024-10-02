using apibronco.bronco.com.br.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace apibronco.bronco.com.br.Entity
{
    public abstract class Entidade 
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdateOn { get; set; }


        #region methods
        public abstract bool IsValid();
        #endregion
    }
}
