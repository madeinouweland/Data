using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Database
    {
        private Streamer _streamer = new Streamer();

        private string GetFileNameFromType(Type type)
        {
            return $"{type.Name}.json";
        }

        public async Task<List<T>> GetAll<T>() where T : Document
        {
            var stream = await _streamer.StreamForReadAsync(GetFileNameFromType(typeof(T)));
            if (stream != null)
            {
                var serializer = new JSONSerializer<List<T>>();
                return serializer.DeserializeFromStream(stream);
            }
            return new List<T>();
        }

        public async Task<T> Save<T>(T document) where T : Document
        {
            var list = await GetAll<T>();
            var exists = list.FirstOrDefault(x => x.Id == document.Id);
            if (exists != null)
            {
                exists = document;
            }
            else
            {
                if (list.Count == 0)
                {
                    document.Id = 1;
                }
                else
                {
                    document.Id = list.Max(x => x.Id) + 1;
                }
                list.Add(document);
            }

            var serializer = new JSONSerializer<List<T>>();
            using (var json = serializer.SerializeAsStream(list))
            {
                using (var stream = await _streamer.StreamForWriteAsync(GetFileNameFromType(typeof(T))))
                {
                    await json.CopyToAsync(stream);
                    await stream.FlushAsync();
                }
            }
            return document;
        }
    }
}
