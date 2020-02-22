using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.DictionaryHelpers
{

    public sealed class Dictionary
    {
        private static readonly Dictionary<string, string> MyDictionary = null;

        public Dictionary() {

            MyDictionary.Add("Excel.Cols.idx", "idx");
            MyDictionary.Add("Excel.Cols.MCStep", "MCStep");
            MyDictionary.Add("Excel.Cols.RG", "RG");
            MyDictionary.Add("Excel.Cols.E", "E");
            MyDictionary.Add("Excel.Cols.energy", "energy");
            MyDictionary.Add("Excel.Cols.delta", "delta");
            MyDictionary.Add("Excel.Cols.value", "value");

            MyDictionary.Add("Excel.Sheet.value", "value");
            
        }


        public static bool TryGetDictionary(string key, out string value)
        {
            return MyDictionary.TryGetValue(key, out value);
        }
    }
}
