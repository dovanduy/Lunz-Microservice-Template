using FluentValidation.Validators;

namespace Lunz.Microservice.Common
{
    public static class FluentValidationExtensions
    {
        public static void AddHttpResponseError(this CustomContext context,
            string httpStatusCode, string propertyName, string errorMessage)
        {
            context.AddFailure(propertyName, $"[{httpStatusCode}]{errorMessage}");
        }

        public static void AddHttpResponseError(this CustomContext context,
            string httpStatusCode, string errorMessage)
        {
            AddHttpResponseError(context, httpStatusCode, null, errorMessage);
        }

        public static void AddBadRequestError(this CustomContext context, string propertyName, string errorMessage)
        {
            AddHttpResponseError(context, "400", propertyName, errorMessage);
        }

        public static void AddBadRequestError(this CustomContext context, string errorMessage)
        {
            AddBadRequestError(context, null, errorMessage);
        }

        public static void AddNotFoundError(this CustomContext context, string propertyName, string errorMessage)
        {
            AddHttpResponseError(context, "404", propertyName, errorMessage);
        }

        public static void AddNotFoundError(this CustomContext context, string errorMessage)
        {
            AddNotFoundError(context, null, errorMessage);
        }

        public static void AddNoContentError(this CustomContext context, string propertyName, string errorMessage)
        {
            AddHttpResponseError(context, "204", propertyName, errorMessage);
        }

        public static void AddNoContentError(this CustomContext context, string errorMessage)
        {
            AddNoContentError(context, null, errorMessage);
        }
    }
}