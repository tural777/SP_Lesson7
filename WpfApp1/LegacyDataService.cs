using System;
using System.Collections.Generic;
using System.IO;

namespace WpfApp1
{
    class LegacyDataService
    {
        public IEnumerable<string> GetNames()
        {
            var fs = new FileStream("file.txt", FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs);
            var text = sr.ReadToEnd();

            var names = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            fs.Dispose();
            sr.Dispose();

            return names;
        }
    }
}
