using System.Collections.Concurrent;


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

           
            
            var storage = new ConcurrentDictionary<string, string>();
            app.MapGet("/storage/{key}", (string key) =>
            {
                if (storage.TryGetValue(key, out var value))
                {
                    return Results.Ok(value);
                }
                return Results.NotFound();
            });
            app.MapPost("/storage", (StorageItem item) =>
            {
                if (string.IsNullOrWhiteSpace(item.Key) || item.Value == null)
                {
                    return Results.BadRequest(new { message = "Key value not matched" });
                }
                storage[item.Key] = item.Value;
                return Results.Created($"/storage/{item.Key}", item.Value);
            });


            app.Run();

