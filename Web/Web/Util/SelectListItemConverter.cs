using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Util
{
    public static class SelectListItemConverter
    {
        public static SelectList CreateSelectList<TEnum>() where TEnum : struct, IConvertible
        {
            // Lógica que garante que o Value seja o número e o Text seja o nome do Enum.
            var items = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = Convert.ToInt32(e).ToString()
                }).ToList();

            // Usa o SelectList(items, dataValueField, dataTextField)
            return new SelectList(items, "Value", "Text");
        }
    }
}