using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WpfApp1
{
    class DataService
    {
        public async Task<IEnumerable<string>> GetNamesAsync()
        {
            var fs = new FileStream("file.txt", FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs);
            var text = await sr.ReadToEndAsync();

            var names = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            fs.Dispose();
            sr.Dispose();

            return names;
        }
    }
}
