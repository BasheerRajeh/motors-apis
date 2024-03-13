namespace WebApi.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            //Business Services
            services.AddScoped<CarService>();
            services.AddScoped<ArticleService>();
            services.AddScoped<ServiceService>();
            services.AddScoped<TestimonialService>();
            services.AddScoped<BookingService>();
            services.AddScoped<ContactSubmissionService>();
            services.AddScoped<MessageService>();
            return services;
        }
    }
}
