using System.Text.Json.Serialization;

namespace my_virtual_pets_class_library.DTO;

public class ControllerResponseDTO<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }

    [JsonConstructor]
    private ControllerResponseDTO(bool isSuccess, T? data, string? errorMessage = null)
    {
        if (isSuccess && data == null && errorMessage == null) Data = default;

        else if (isSuccess && data == null)
        {
            throw new InvalidOperationException("Error message must be null when isSuccess is true");
        }

        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    // Static factory methods
    public static ControllerResponseDTO<T> Success(T data) => new ControllerResponseDTO<T>(true, data);
    public static ControllerResponseDTO<T> SuccessNoData() => new ControllerResponseDTO<T>(true, default);
    public static ControllerResponseDTO<T> Error(string errorMessage) => new ControllerResponseDTO<T>(false, default, errorMessage);
}
