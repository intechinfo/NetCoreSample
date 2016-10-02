using System;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class Result<TSuccess>
    {
        public Result( Status status, TSuccess success, string errorMessage )
        {
            Status = status;
            if( errorMessage == null ) errorMessage = string.Empty;
            ErrorMessage = errorMessage;
            if( errorMessage == string.Empty )
            {
                Content = success;
            }
        }

        public Status Status { get; }

        public TSuccess Content { get; }

        public bool HasError => ErrorMessage != string.Empty;

        public string ErrorMessage { get; }
    }

    public class Result
    {
        public Result( Status status )
            : this( status, string.Empty )
        {
        }

        public Result( Status status, string errorMessage )
        {
            Status = status;
            if( errorMessage == null ) errorMessage = string.Empty;
            ErrorMessage = errorMessage;
        }

        public Status Status { get; }

        public bool HasError => ErrorMessage != string.Empty;

        public string ErrorMessage { get; }

        public static Result<T> Success<T>( Status status, T content )
        {
            return new Result<T>( status, content, null );
        }

        public static Result<T> Failure<T>( Status status, string errorMessage )
        {
            if( string.IsNullOrEmpty( errorMessage ) ) throw new ArgumentException( "The error message must be not null nor whitespace.", nameof( errorMessage ) );
            return new Result<T>( status, default( T ), errorMessage );
        }

        public static Result Success( Status status )
        {
            return new Result( status );
        }

        public static Result Failure( Status status, string errorMessage )
        {
            return new Result( status, errorMessage );
        }
    }

    public enum Status
    {
        Ok,
        BadRequest,
        NotFound,
        Created
    }
}
