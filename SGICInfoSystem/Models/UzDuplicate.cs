using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace SGICInfoSystem.Models
{
    public class UzDuplicate
    {
        public int Id { get; set; }

        [Display(Name = "Рақам")]
        public int? nomer { get; set; }

        [Display(Name = "Асосий рукни")]
        public string tema1 { get; set; }
        [Display(Name = "Қўшимча рукни")]
        public string tema2 { get; set; }
        [Display(Name = "Муаллиф")]
        public string Aftor { get; set; }
        [Display(Name = "Номи")]
        public string Naz { get; set; }
        [Display(Name = "Ташкилот")]
        public string Org { get; set; }
        [Display(Name = "Шаҳар")]
        public string Gorod { get; set; }
        [Display(Name = "Йил")]
        public int? God { get; set; }
        [Display(Name = "Бетлар")]
        public string Str { get; set; }
        [Display(Name = "Иллюстрациялар")]
        public string Ilustr { get; set; }
        [Display(Name = "Асосий сўзлар")]
        public string Slova { get; set; }
        [Display(Name = "Реферат")]
        public string Referat { get; set; }
        [Display(Name = "Сақлаш жойи")]
        public string Mesto { get; set; }
    }
}