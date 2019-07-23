using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.WebPages.Models
{
    public class TextBlocks
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public string Text { get; set;  }


        public enum MultiLanguageSelector
        {
            English,
            French,
            Spanish,
            Russian,
            German,
            Japanese
        }

    }
}
