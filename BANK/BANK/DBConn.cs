using System.Data.SqlClient;

public static class DBConn
{
    public static SqlConnection GetConnection()
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\GIGABYTE\\Documents\\BANK.mdf;Integrated Security=True;Connect Timeout=30"; 
        return new SqlConnection(connectionString);
    }
}
