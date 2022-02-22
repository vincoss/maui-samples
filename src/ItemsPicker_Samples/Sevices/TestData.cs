using ShortMvvm.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ItemsPicker_Samples.Sevices
{
    public class TestData
    {
        public static KeyDataIntString[] Platforms = new[]
    {
            new KeyDataIntString { Key = 1, Value =  "Android" },
            new KeyDataIntString { Key = 2, Value =  "iOS" },
            new KeyDataIntString { Key = 3, Value =  "MacCatalyst" },
            new KeyDataIntString { Key = 4, Value =  "Windows" },
            new KeyDataIntString { Key = 5, Value =  "Linux" },
            new KeyDataIntString { Key = 21, Value =  "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." },
        };
    }
}