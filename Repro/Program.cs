using Azure.Identity;
using System;
using System.Data.SqlClient;

namespace Repro
{
    /// <summary>
    /// Simple sample to demonstrate accessing Azure SQL DB passwordlessly even when the library accesses the DB
    /// is built against a much older version of the Framework (where Microsoft.Data.SqlClient and 
    /// System.Data.SqlClient.SqlConnection.AccessToken are not available).
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            // Should only need a server name and database name since auth is supplied through the token
            // For example: Server=tcp:eshopdb-mikerou.database.windows.net;Initial Catalog=eShop
            Console.Write("Connection string: ");
            var connectionString = Console.ReadLine();

            Console.Write("Table name: ");
            var tableName = Console.ReadLine();

            // This uses InteractiveBrowserCredential to get a token for the user
            // but it could just as easily be ManagedIdentityCredential or DefaultAzureCredential to use managed identity.
            var credential = new InteractiveBrowserCredential();
            var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net//.default" }));
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.AccessToken = token.Token;
                sqlConnection.Open();
                Console.WriteLine($"Table row count: {ClassLibrary1.Class1.GetTableRowCount(sqlConnection, tableName)}");
            }
        }
    }
}
