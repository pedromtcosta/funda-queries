using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Net;

namespace FundaQueries.Spec
{
    public static class ActionResultExtensions
    {
        public static T As<T>(this IActionResult actionResult) where T : IActionResult
        {
            return (T)actionResult;
        }

        public static T GetValue<T>(this IActionResult actionResult)
        {
            if (actionResult is ObjectResult objectResult)
            {
                if (objectResult.Value is T value)
                {
                    return value;
                }
                else
                {
                    throw new ArgumentException($"The underlying value from ObjectResult can't be casted to {typeof(T).FullName}");
                }
            }
            else
            {
                throw new ArgumentException("actionResult does not inherit from ObjectResult");
            }
        }

        public static void ShouldHaveStatusCode(this IActionResult actionResult, HttpStatusCode expectedStatusCode)
        {
            actionResult
                .As<IStatusCodeActionResult>()
                .StatusCode.Should().Be((int)expectedStatusCode);
        }
    }
}
