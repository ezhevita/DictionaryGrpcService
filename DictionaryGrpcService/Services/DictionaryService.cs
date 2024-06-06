using System.Collections.Concurrent;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryGrpcService.Services;

public class DictionaryService : DictionaryStorageService.DictionaryStorageServiceBase
{
	public const string StorageKey = "DictionaryStorage";
	private readonly ConcurrentDictionary<string, string> _dictionary;

	public DictionaryService([FromKeyedServices(StorageKey)] ConcurrentDictionary<string, string> dictionary) =>
		_dictionary = dictionary;

	public override Task<ResultResponse> Add(AddRequest request, ServerCallContext context) =>
		Task.FromResult(new ResultResponse {Success = _dictionary.TryAdd(request.Key, request.Value)});

	public override Task<GetResponse> Get(GetRequest request, ServerCallContext context)
	{
		var found = _dictionary.TryGetValue(request.Key, out var value);
		var response = new GetResponse {Found = found};
		if (value != null)
		{
			response.Value = value;
		}

		return Task.FromResult(response);
	}

	public override Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
	{
		var data = new GetAllResponse();
		data.Data.Add(_dictionary);

		return Task.FromResult(data);
	}

	public override Task<RemoveResponse> Remove(RemoveRequest request, ServerCallContext context)
	{
		var success = _dictionary.TryRemove(request.Key, out var value);
		var response = new RemoveResponse {Success = success};
		if (value != null)
		{
			response.Value = value;
		}

		return Task.FromResult(response);
	}
}
