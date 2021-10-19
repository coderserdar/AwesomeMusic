using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeMusic
{
    [Table]
    public class CategoryArtist
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int CategoryArtistId { get; set; }

        [Column]
        public int CategoryId { get; set; }

        [Column]
        public int ArtistId { get; set; }
    }
}