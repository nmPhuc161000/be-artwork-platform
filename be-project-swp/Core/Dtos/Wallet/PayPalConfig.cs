using PayPalCheckoutSdk.Core;

namespace be_project_swp.Core.Dtos.Wallet
{
    public class PayPalConfig
    {
        public static string ClientId = "AZQ4oO2IGuXP8l4YKp9gGqXLvsLfnJ1d_g5h7H49vh8qvijQYhETmiSZSeTnXQiHTYh5m0U12HSzwSoT";
        public static string Secret = "EIqA07EIiqF-rZEUNifUp-ccpzVjvB2_ePstFKZhlVBZYPJQPuw3TktQeyBcQYi2mB90t5dOpoZZMSuo";
/*
        public static HttpClient Client()
        {
            return new PayPalHttpClient(new SandboxEnvironment(ClientId, Secret));
        }*/
    }
}
