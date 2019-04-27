using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VOD.Common.Extensions
{
    public static class ListExtensions
    {
        public static SelectList ToSelectList<TEntity>(this List<TEntity> items, string valueField, string textField) where TEntity : class
        {
            return new SelectList(items, valueField, textField);
        }

    }
}
