namespace App;

public static class Settings
{
    public static string Secret = "thisisasecretkey";

    public static string GetConnectionString()
    {
        var host = "localhost";
        var port = "5432";
        var dbName = "Sistema_Controle_OP_Cerveja";
        var user = "postgres";
        var password = "postgresweb";

        return $"Host={host};Port={port};Database={dbName};Username={user};Password={password}";
    }
}
