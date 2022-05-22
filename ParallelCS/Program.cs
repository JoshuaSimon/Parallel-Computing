using Microsoft.Data.Sqlite;


string database = "testdata.db";
CreateTestTable(database);
InsertTableData(database, 10);
Console.WriteLine("Done!");


void CreateTestTable(string database)
{
    string query = @"CREATE TABLE t_test (id INTEGER PRIMARY KEY, strValue TEXT, intValue INTEGER);";
    Connection.ExecuteQuery(database, query);
}

void InsertTableData(string database, int num_rows)
{
    string query = @"INSERT INTO t_test (strValue, intValue) VALUES ($str, $int)";
    for (int i = 0; i < num_rows; i++)
    {
        var args = new List<object> { "Some test text.", i };
        Connection.ExecuteQuery(database, query, args);
    }
}


/// <summary>
/// Wrapper class for a SQLite database connection.
/// </summary>
class Connection
{
    private SqliteConnection _connection;

    public Connection(string database)
    {
        _connection = new SqliteConnection("Data Source = " + database);
    }

    public void ExecuteCommand(string query, List<object>? args = null)
    {
        using (_connection)
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = query;

            if (args != null)
            {
                foreach (var arg in args)
                {
                    command.Parameters.Add(arg);
                }
            }

            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Executes an INSERT, UPDATE DELETE or CREATE query on the provided database.
    /// </summary>
    /// <param name="database"></param>
    /// <param name="query"></param>
    /// <param name="args"></param>
    public static void ExecuteQuery(string database, string query, List<object>? args = null)
    {
        var connection = new Connection(database);
        connection.ExecuteCommand(query, args);
    }
}

