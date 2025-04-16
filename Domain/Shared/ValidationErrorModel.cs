using FluentValidation.Results;

namespace Domain.Shared
{
    public class ValidationErrorModel
    {
        public string? PropertyName { get; set; }

        public string? ErrorMessage { get; set; }

        public object? AttemptedValue { get; set; }

        public string? ErrorCode { get; set; }

        public ValidationErrorModel(ValidationFailure? error)
        {
            if (error is null) return;

            PropertyName = error.PropertyName;
            ErrorMessage = error.ErrorMessage;
            AttemptedValue = error.AttemptedValue;
            ErrorCode = error.ErrorCode;
        }
    }
}