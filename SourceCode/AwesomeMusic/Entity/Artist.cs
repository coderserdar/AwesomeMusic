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
    public class Artist
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int ArtistId { get; set; }

        [Column]
        public string ArtistName { get; set; }

        [Column]
        public int ArtistAlbumCount { get; set; }

        [Column]
        public string AlbumOrderBy { get; set; }

        [Column]
        public string AlbumOrderStyle { get; set; }

        [Column]
        public string ArtistNameCount { get; set; }

        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }
    }
}