using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_EF_Samples_Library.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? JobId { get; set; }

        public string Status { get; set; }
        public string Priority { get; set; }
        public string Assigne { get; set; }
        public string Tags { get; set; }
        
        public DateTimeOffset? DueDateUtc { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
        public Guid RowUuid { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return base.ToString();
            }
            return Name;
        }

        public static Item Create(string name, DateTime createdDateUtc)
        {
            var item = new Item
            {
                Name = name,
                CreatedDateTimeUtc = createdDateUtc,
                ModifiedDateTimeUtc = createdDateUtc,
                RowUuid = Guid.NewGuid()
            };

            return item;
        }
    }
}
