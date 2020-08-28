using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace SGICInfoSystem.Models
{
    public class Essay
    {
        public int Id { get; set; }

        [Display(Name ="Номер")]
        public int? nomer { get; set; }

        [Display(Name = "Рубрика осн.")]
        public string tema1 { get; set; }
        [Display(Name = "Рубрики вспом.")]
        public string tema2 { get; set; }
        [Display(Name = "Автор(ы)")]
        public string Aftor { get; set; }
        [Display(Name = "Название")]
        public string Naz { get; set; }
        [Display(Name = "Организация")]
        public string Org { get; set; }
        [Display(Name = "Город")]
        public string Gorod { get; set; }
        [Display(Name = "Год")]
        public int? God { get; set; }
        [Display(Name = "Страница")]
        public string Str { get; set; }
        [Display(Name = "Иллюстрации")]
        public string Ilustr { get; set; }
        [Display(Name = "Ключевые слова")]
        public string Slova { get; set; }
        [Display(Name = "Реферат")]
        public string Referat { get; set; }
        [Display(Name = "Место")]
        public string Mesto { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Номер: ").Append(nomer)
                .Append("\nРубрика осн.: ").Append(tema1)
                .Append("\nРубрики вспом.: ").Append(tema2)
                .Append("\nАвтор(ы): ").Append(Aftor)
                .Append("\nНазвание: ").Append(Naz)
                .Append("\nОрганизация: ").Append(Org)
                .Append("\nГород: ").Append(Gorod)
                .Append("\nГод: ").Append(God)
                .Append("\nСтраница: ").Append(Str)
                .Append("\nИллюстрации: ").Append(Ilustr)
                .Append("\nРеферат: ").Append(Referat)
                .Append("\nМесто: ").Append(Mesto);
            return sb.ToString();
        }
    }
}