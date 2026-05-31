namespace Infrastructure.DataSeeding
{
    /// <summary>
    /// Interface for database seeding operations
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Seeds the database with initial data
        /// </summary>
        Task SeedAsync();
    }
}
