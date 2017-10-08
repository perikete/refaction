using System.Configuration;

namespace refactor_me.Infrastructure
{
    /// <summary>
    /// Configuration Class for the Refactor Me Application
    /// </summary>
    public static class RefactorMeConfiguration
    {
        /// <summary>
        /// Connection String
        /// </summary>
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    }
}