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
    public class Album
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int AlbumId { get; set; }

        [Column]
        public string AlbumGuid { get; set; }

        [Column]
        public int AlbumCategoryId { get; set; }

        [Column]
        public string AlbumName { get; set; }

        [Column]
        public int AlbumReleaseYear { get; set; }

        [Column]
        public int AlbumSongCount { get; set; }

        [Column]
        public string AlbumLabelName { get; set; }

        [Column]
        public string AlbumBestSong { get; set; }

        [Column]
        public int AlbumRating { get; set; }

        [Column]
        public string AlbumComment { get; set; }

        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }

        [Column]
        public string AlbumInformation { get; set; }

        [Column]
        public string AlbumNameRating { get; set; }
    }
}