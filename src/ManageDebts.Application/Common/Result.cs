using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Application.Common
{
    public class Result<T>
    {
        private Result(T? value, string? error, bool isSuccess)
            => (Value, Error, IsSuccess) = (value, error, isSuccess);

        public T? Value { get; }
        public string? Error { get; }
        public bool IsSuccess { get; }

        //Metodos fabrica para crear instancias de la clase result
        //crear objetos Result<T> desde fuera de la clase, ya que el constructor es privado.
        public static Result<T> Success(T value) => new(value, null, true);
        public static Result<T> Failure(string error) => new(default, error, false);
    }
}
