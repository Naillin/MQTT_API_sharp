using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MQTT_API_sharp.Core.Exceptions;

namespace MQTT_API_sharp.Core.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(IWebHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Логирование ошибки
            LogException(exception);

            // Маппим тип исключения на HTTP-статус
            var (statusCode, title) = MapException(exception);

            // Формирование RFC 7807 совместимый ProblemDetails ответ
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            // Вывод StackTrace ТОЛЬКО в режиме разработки
            if (_env.IsDevelopment())
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            // Запись JSON-ответа в поток
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Возврат true - ошибка успешно обработана
        }

        private (HttpStatusCode StatusCode, string Title) MapException(Exception exception) =>
            exception switch
            {
                AuthException => (HttpStatusCode.Unauthorized, "Authorization Error"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resource Not Found"),
                ArgumentException or ArgumentOutOfRangeException => (HttpStatusCode.BadRequest, "Invalid Arguments"),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

        private void LogException(Exception exception)
        {
            if (exception is AuthException or KeyNotFoundException or ArgumentException) // todo: по хорошему нужно сделать список эксепшенов
                _logger.LogWarning("Business rule violation: {Message}", exception.Message);
            else
                _logger.LogError(exception, "Unhandled system exception occurred.");
        }
    }