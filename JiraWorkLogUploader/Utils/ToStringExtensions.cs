using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraWorkLogUploader.Utils
{
    public static class ToStringExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToMonospacedColumns<TModel>(this IEnumerable<TModel> e, System.Linq.Expressions.Expression<Func<TModel, string>> valueExpression, string delimiter, params System.Linq.Expressions.Expression<Func<TModel, string>>[] textExpressions)
        {
            var valueMethod = valueExpression.Compile();
            var textMethods = textExpressions.Select(i => i.Compile()).ToList();

            var itemList = e.ToList();
            var textColumnSizes = textMethods.Select(m => itemList.Select(i => m(i).Length).OrderByDescending(i => i).First()).ToList();

            var result = itemList
                .Select(i => new KeyValuePair<string, string>(
                    valueMethod(i),
                    textColumnSizes
                        .Select((cs, index) =>
                        {
                            var str = textMethods[index](i);
                            var appendSize = textColumnSizes[index] - str.Length;
                            return str + new string(' ', appendSize); // it's alt+255 and not a space !
                        })
                        .Aggregate((current, next) => current + delimiter + next)
                    )
                )
                .ToList();

            return result;
        }
    }
}
