using System.Collections.Concurrent;
using DictionaryGrpcService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddKeyedSingleton<ConcurrentDictionary<string, string>>(DictionaryService.StorageKey);

var app = builder.Build();

app.MapGrpcService<DictionaryService>();
app.Run();
