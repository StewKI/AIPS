namespace AipsE2ETests.Infrastructure;

public static class EnvBuilder
{
    public static Dictionary<string, string> CreateCommon(
        string db,
        string rabbit)
    {
        return new Dictionary<string, string>
        {
            ["DB_CONN_STRING"] = db,
            ["ConnectionStrings__Default"] = db,
            ["POSTGRES_DB"] = "testDb",
            ["POSTGRES_USER"] = "postgres",
            ["POSTGRES_PASSWORD"] = "postgres",

            ["RABBITMQ_AMQP_URI"] = rabbit,
            ["RABBITMQ_DEFAULT_USER"] = "guest",
            ["RABBITMQ_DEFAULT_PASS"] = "guest",
            ["RABBITMQ_DEFAULT_VHOST"] = "/",
            ["RABBITMQ_EXCHANGE"] = "worker",

            ["JWT_ISSUER"] = "AIPS",
            ["JWT_AUDIENCE"] = "AIPSWebApi",
            ["JWT_KEY"] = "7fY2sK9xLpQmN4vRtXzW8aBcDeFgHiJkLmNoPqRsTuVwXyZ1234567890ABCDEF",
            ["JWT_EXPIRATION_MINUTES"] = "15",
            ["JWT_REFRESH_TOKEN_EXPIRATION_DAYS"] = "7"
        };
    }
}