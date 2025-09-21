using A2ASMS.Utility.Logger;
using API.Template.Domain.Exceptions;
using API.Template.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace API.Template.WebAPI.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            A2ALogger.Warning($"NotFoundException occurred: {ex.Message}");
            await HandleExceptionAsync(context, "NOT_FOUND", ex.Message, StatusCodes.Status404NotFound);
        }
        catch (EnvironmentVariableNotSetException ex)
        {
            A2ALogger.Warning($"EnvironmentVariableNotSetException occurred: {ex.Message}");
            await HandleExceptionAsync(context, "ENV_VAR_MISSING", ex.Message, StatusCodes.Status500InternalServerError);
        }
        catch (ValidationException ex)
        {
            A2ALogger.Warning($"ValidationException occurred: {ex.Message}");
            await HandleExceptionAsync(context, "VALIDATION_ERROR", ex.Message, StatusCodes.Status400BadRequest);
        }
        catch (UnauthenticatedException ex)
        {
            A2ALogger.Warning($"UnauthenticatedException occurred: {ex.Message}");
            await HandleExceptionAsync(context, "UNAUTHENTICATED", ex.Message, StatusCodes.Status401Unauthorized);
        }
        catch (UnauthorizedException ex)
        {
            A2ALogger.Warning($"UnauthorizedException occurred: {ex.Message}");
            await HandleExceptionAsync(context, "UNAUTHORIZED", ex.Message, StatusCodes.Status403Forbidden);
        }
        catch (ConflictException ex)
        {
            A2ALogger.Warning($"ConflictException occurred: {ex.Message}");
            await HandleExceptionAsync(context, "CONFLICT", ex.Message, StatusCodes.Status409Conflict);
        }
        catch (CustomValidationException ex)
        {
            A2ALogger.Warning($"CustomValidationException occurred: {ex.Message}");
            await HandleExceptionAsync(
                context,
                "VALIDATION_ERROR",
                JoinErrors(ex.Errors),
                StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            A2ALogger.Error($"An unexpected error occurred: {ex.Message}");
            await HandleExceptionAsync(context, "UNEXPECTED_ERROR", "An unexpected error occurred.", StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string errorCode, string message, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        // Json Serialization Options to make all camelCase
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true // Optional: for pretty printing
        };

        ApiResponse<object> response = ApiResponse<object>.ErrorResponse(errorMessage: message, errorCode: errorCode);
        string result = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(result);
    }

    private string JoinErrors(IEnumerable<string> errors) => string.Join(", ", errors);
}