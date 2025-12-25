// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(options =>
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApp API v1");
//         options.RoutePrefix = "swagger"; // This makes it available at /swagger/index.html
//         options.DisplayRequestDuration();
//         options.EnableTryItOutByDefault();
//     });
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// // Add health check endpoint for Kubernetes
// app.MapGet("/health", () => Results.Ok(new 
// { 
//     status = "healthy", 
//     environment = app.Environment.EnvironmentName,
//     timestamp = DateTime.UtcNow 
// }));

// // Add root endpoint
// app.MapGet("/", () => "MyApp API is running. Use /swagger for API documentation.");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }























var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApp API v1");
        options.RoutePrefix = "swagger"; // This makes it available at /swagger/index.html
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// Add health check endpoint for Kubernetes
app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy", 
    environment = app.Environment.EnvironmentName,
    timestamp = DateTime.UtcNow 
}));

//  BEAUTIFUL CI/CD DASHBOARD INTERFACE
app.MapGet("/", () => Results.Content(@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title> MyApp CI/CD Dashboard</title>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css'>
    <style>
        :root {
            --primary: #0078d4;
            --success: #4CAF50;
            --warning: #ff9800;
            --dark: #1a1a1a;
            --light: #f8f9fa;
        }
        
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Segoe UI', 'SF Pro Display', -apple-system, BlinkMacSystemFont, sans-serif;
        }
        
        body {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            color: white;
            padding: 20px;
        }
        
        .dashboard-container {
            max-width: 1200px;
            margin: 0 auto;
        }
        
        .header {
            text-align: center;
            padding: 40px 20px;
        }
        
        .title {
            font-size: 3.5em;
            font-weight: 800;
            margin-bottom: 15px;
            background: linear-gradient(45deg, #ff9a9e, #fad0c4, #a1c4fd);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            text-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        
        .subtitle {
            font-size: 1.2em;
            opacity: 0.9;
            max-width: 600px;
            margin: 0 auto 30px;
        }
        
        .status-badge {
            display: inline-block;
            background: linear-gradient(45deg, var(--success), #2ecc71);
            color: white;
            padding: 10px 25px;
            border-radius: 50px;
            font-weight: bold;
            box-shadow: 0 5px 15px rgba(76, 175, 80, 0.3);
        }
        
        .metrics-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 25px;
            margin: 40px 0;
        }
        
        .metric-card {
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 15px;
            padding: 30px;
            transition: all 0.3s ease;
            border: 1px solid rgba(255, 255, 255, 0.1);
        }
        
        .metric-card:hover {
            transform: translateY(-5px);
            background: rgba(255, 255, 255, 0.15);
            box-shadow: 0 15px 30px rgba(0,0,0,0.2);
        }
        
        .metric-icon {
            font-size: 2.5em;
            margin-bottom: 15px;
        }
        
        .metric-title {
            font-size: 1.3em;
            font-weight: 600;
            margin-bottom: 10px;
        }
        
        .metric-value {
            font-size: 2em;
            font-weight: 700;
            margin: 10px 0;
        }
        
        .metric-label {
            opacity: 0.8;
            font-size: 0.9em;
        }
        
        .links-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
            margin: 40px 0;
        }
        
        .link-card {
            background: rgba(255, 255, 255, 0.08);
            border-radius: 12px;
            padding: 25px;
            text-decoration: none;
            color: white;
            display: flex;
            align-items: center;
            transition: all 0.3s ease;
        }
        
        .link-card:hover {
            background: rgba(255, 255, 255, 0.15);
            transform: translateY(-3px);
        }
        
        .link-icon {
            font-size: 2em;
            margin-right: 20px;
            opacity: 0.9;
        }
        
        .link-text h3 {
            font-size: 1.2em;
            margin-bottom: 5px;
        }
        
        .link-text p {
            opacity: 0.7;
            font-size: 0.9em;
        }
        
        .tech-stack {
            display: flex;
            justify-content: center;
            flex-wrap: wrap;
            gap: 15px;
            margin: 40px 0;
        }
        
        .tech-badge {
            background: rgba(255, 255, 255, 0.1);
            padding: 10px 20px;
            border-radius: 8px;
            font-size: 0.9em;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        
        .pipeline-flow {
            background: rgba(255, 255, 255, 0.05);
            border-radius: 15px;
            padding: 30px;
            margin: 40px 0;
        }
        
        .flow-steps {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            margin-top: 20px;
        }
        
        .flow-step {
            text-align: center;
            flex: 1;
            min-width: 150px;
            padding: 15px;
        }
        
        .step-icon {
            width: 50px;
            height: 50px;
            background: var(--primary);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 15px;
            font-weight: bold;
            font-size: 1.2em;
        }
        
        .step-text {
            font-weight: 600;
            margin-bottom: 5px;
        }
        
        .step-desc {
            font-size: 0.85em;
            opacity: 0.8;
        }
        
        .footer {
            text-align: center;
            padding: 30px;
            margin-top: 40px;
            border-top: 1px solid rgba(255, 255, 255, 0.1);
            font-size: 0.9em;
            opacity: 0.7;
        }
        
        @media (max-width: 768px) {
            .title { font-size: 2.5em; }
            .flow-steps { flex-direction: column; }
            .flow-step { margin-bottom: 20px; }
        }
    </style>
</head>
<body>
    <div class='dashboard-container'>
        <div class='header'>
            <h1 class='title'>🚀 Azure DevOps CI/CD Dashboard</h1>
            <p class='subtitle'>A fully automated deployment pipeline for .NET 8 applications</p>
            <div class='status-badge'>
                <i class='fas fa-check-circle'></i> Deployment Successful
            </div>
        </div>
        
        <div class='metrics-grid'>
            <div class='metric-card'>
                <div class='metric-icon'>🔧</div>
                <div class='metric-title'>Build Pipeline</div>
                <div class='metric-value'>Azure DevOps</div>
                <div class='metric-label'>Multi-stage YAML configuration</div>
            </div>
            
            <div class='metric-card'>
                <div class='metric-icon'></div>
                <div class='metric-title'>Container Registry</div>
                <div class='metric-value'>Azure ACR</div>
                <div class='metric-label'>myacrdemo2025.azurecr.io</div>
            </div>
            
            <div class='metric-card'>
                <div class='metric-icon'>⚓</div>
                <div class='metric-title'>Orchestration</div>
                <div class='metric-value'>Kubernetes</div>
                <div class='metric-label'>Minikube local cluster</div>
            </div>
            
            <div class='metric-card'>
                <div class='metric-icon'></div>
                <div class='metric-title'>Agent</div>
                <div class='metric-value'>Self-Hosted</div>
                <div class='metric-label'>melvin-xps-13-9350</div>
            </div>
        </div>
        
        <div class='links-grid'>
            <a href='/swagger' class='link-card'>
                <div class='link-icon'></div>
                <div class='link-text'>
                    <h3>API Documentation</h3>
                    <p>Interactive Swagger UI with full API specs</p>
                </div>
            </a>
            
            <a href='/weatherforecast' class='link-card'>
                <div class='link-icon'></div>
                <div class='link-text'>
                    <h3>Weather Forecast API</h3>
                    <p>Sample API endpoint with mock data</p>
                </div>
            </a>
            
            <a href='/health' class='link-card'>
                <div class='link-icon'></div>
                <div class='link-text'>
                    <h3>Health Check</h3>
                    <p>System status & Kubernetes health</p>
                </div>
            </a>
            
            <a href='https://dev.azure.com' target='_blank' class='link-card'>
                <div class='link-icon'></div>
                <div class='link-text'>
                    <h3>Azure DevOps</h3>
                    <p>Pipeline execution & monitoring</p>
                </div>
            </a>
        </div>
        
        <div class='pipeline-flow'>
            <h2 style='text-align: center; margin-bottom: 30px;'>CI/CD Pipeline Flow</h2>
            <div class='flow-steps'>
                <div class='flow-step'>
                    <div class='step-icon'>1</div>
                    <div class='step-text'>Git Commit</div>
                    <div class='step-desc'>Code push triggers pipeline</div>
                </div>
                
                <div class='flow-step'>
                    <div class='step-icon'>2</div>
                    <div class='step-text'>Build & Test</div>
                    <div class='step-desc'>.NET build, Docker container</div>
                </div>
                
                <div class='flow-step'>
                    <div class='step-icon'>3</div>
                    <div class='step-text'>Push to ACR</div>
                    <div class='step-desc'>Versioned image storage</div>
                </div>
                
                <div class='flow-step'>
                    <div class='step-icon'>4</div>
                    <div class='step-text'>Deploy to K8s</div>
                    <div class='step-desc'>Kubernetes deployment</div>
                </div>
            </div>
        </div>
        
        <div class='tech-stack'>
            <div class='tech-badge'>
                <i class='fab fa-microsoft'></i> Azure DevOps
            </div>
            <div class='tech-badge'>
                <i class='fas fa-code'></i> .NET 8
            </div>
            <div class='tech-badge'>
                <i class='fab fa-docker'></i> Docker
            </div>
            <div class='tech-badge'>
                <i class='fas fa-ship'></i> Kubernetes
            </div>
            <div class='tech-badge'>
                <i class='fas fa-cloud'></i> Azure ACR
            </div>
            <div class='tech-badge'>
                <i class='fab fa-github'></i> GitHub
            </div>
        </div>
        
        <div class='footer'>
            <p>Deployed via self-hosted Azure DevOps agent | Kubernetes: Minikube | Container Registry: Azure ACR</p>
            <p style='margin-top: 10px;'>Environment: Production | Uptime: 100% | Last Deployment: Today</p>
        </div>
    </div>
    
    <script>
        // Add some interactivity
        document.addEventListener('DOMContentLoaded', function() {
            // Animate metric cards on scroll
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.style.opacity = '1';
                        entry.target.style.transform = 'translateY(0)';
                    }
                });
            });
            
            document.querySelectorAll('.metric-card').forEach(card => {
                card.style.opacity = '0';
                card.style.transform = 'translateY(20px)';
                card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
                observer.observe(card);
            });
            
            // Update timestamp
            const timeElements = document.querySelectorAll('.timestamp');
            const now = new Date();
            timeElements.forEach(el => {
                el.textContent = now.toLocaleString();
            });
        });
    </script>
</body>
</html>
", "text/html"));

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}