namespace DDDWithEventSourcingBasedOnMarten.Infrastructure.Config
{
    public class ApplicationConfig
    {
        public bool RunProjections { get; set; } = true;
        public int Port { get; set; }
    }
}