using System.Data.SqlClient;

namespace ClassLibrary1
{
    public class Class1
    {
        public static int GetTableRowCount(SqlConnection connection, string tableName)
        {
            var sqlCommand = new SqlCommand($"SELECT COUNT(1) FROM {tableName}", connection);
            return (int)sqlCommand.ExecuteScalar();
        }
    }
}
