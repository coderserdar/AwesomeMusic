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
    public class AlbumArtist
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int AlbumArtistId { get; set; }

        [Column]
        public int AlbumId { get; set; }

        [Column]
        public int ArtistId { get; set; }
    }
}