using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGICInfoSystem.Models
{
    
    public class DisplayFilterModel
    {
        [Display(Name = "Отображение")]
        public string Display { get; set; }
        [Display(Name = "Номер")]
        public bool nomer { get; set; }
        [Display(Name = "Рубрика осн.")]
        public bool tema1 { get; set; }
        [Display(Name = "Рубрики вспом.")]
        public bool tema2 { get; set; }
        [Display(Name = "Автор(ы)")]
        public bool Aftor { get; set; }
        [Display(Name = "Название")]
        public bool Naz { get; set; }
        [Display(Name = "Организация")]
        public bool Org { get; set; }
        [Display(Name = "Город")]
        public bool Gorod { get; set; }
        [Display(Name = "Год")]
        public bool God { get; set; }
        [Display(Name = "Страница")]
        public bool Str { get; set; }
        [Display(Name = "Иллюстрации")]
        public bool Ilustr { get; set; }
        [Display(Name = "Ключевые слова")]
        public bool Slova { get; set; }
        [Display(Name = "Реферат")]
        public bool Referat { get; set; }
        [Display(Name = "Место")]
        public bool Mesto { get; set; }

        public DisplayFilterModel(string display)
        {
            Display = display;
        }
        public DisplayFilterModel DefaultDisplayFilter()
        {
            nomer = tema1 = tema2 = Aftor = Naz = Org = Gorod = God = Str = Ilustr
                   = Slova = Referat = Mesto = true;
            return this;
        }
    }
}