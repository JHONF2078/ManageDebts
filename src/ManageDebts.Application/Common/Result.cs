using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Application.Common
{

    public enum ErrorType
    {
        None,
        Unauthorized,
        NotFound,
        Validation,
        Conflict,
        Internal
    }

    /// <summary>
    ///  clase genérica para representar el resultado de una operación
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// como es private solo puede ser llamado desde dentro de la clase (por los métodos estáticos)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="error"></param>
        /// <param name="isSuccess"></param>
        /// <param name="errorType"></param>
        private Result(T? value, string? error, bool isSuccess, ErrorType errorType = ErrorType.None)
            => (Value, Error, IsSuccess, ErrorType) = (value, error, isSuccess, errorType);

        public T? Value { get; }
        public string? Error { get; }
        public bool IsSuccess { get; }
        public ErrorType ErrorType { get; }

        //Metodos fabrica para crear instancias de la clase result de forma controlada
        //crear objetos Result<T> desde fuera de la clase, ya que el constructor es privado.
        public static Result<T> Success(T value) => new(value, null, true);
        public static Result<T> Failure(string error, ErrorType errorType = ErrorType.Internal) => new(default, error, false, errorType);
    }
}
