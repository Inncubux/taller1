using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Helpers
{
    /// <summary>
    /// Generic API response wrapper for standardizing API responses.
    /// Contains information about the operation's success, a message, optional data, and a list of errors.
    /// </summary>
    /// <typeparam name="T">The type of the data returned in the response.</typeparam>
    public class ApiResponse<T>(bool success, string message, T? data = default, List<string>? errors = null)
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Success { get; set; } = success;

        /// <summary>
        /// Message describing the result of the operation.
        /// </summary>
        public string Message { get; set; } = message;

        /// <summary>
        /// The data returned by the operation, if any.
        /// </summary>
        public T? Data { get; set; } = data;

        /// <summary>
        /// List of error messages, if any occurred during the operation.
        /// </summary>
        public List<string>? Errors { get; set; } = errors;
    }
}