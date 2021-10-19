using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeMusic
{
    public class AwesomeMusicDataContext : DataContext
    {
        public const string ConnectionString = @"Data Source=isostore:/MyMusicLibrary.sdf";
        public AwesomeMusicDataContext(string connectionString)
            : base(connectionString) { }
        public Table<Category> Categories;
        public Table<Artist> Artists;
        public Table<Album> Albums;
        public Table<AppSettings> AppSettings;
        public Table<AlbumArtist> AlbumArtists;
        public Table<CategoryArtist> CategoryArtists;
    }
}