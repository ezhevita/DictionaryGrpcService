using System.Collections.Concurrent;
using DictionaryGrpcService.Services;

namespace DictionaryGrpcService.Tests;

[TestClass]
public class DictionaryServiceTests
{
	private const string keyName = "TestKey";
	private const string valueName = "TestValue";
	private const string nonExistingKey = "AnotherKey";
	private const string defaultValue = "";
	private ConcurrentDictionary<string, string> _dictionary = null!;
	private DictionaryService _service = null!;

	[TestMethod]
	public async Task Add_AddsSuccessfully()
	{
		var request = new AddRequest {Key = keyName, Value = valueName};

		var result = await _service.Add(request, new ServerCallContextStub());

		Assert.IsTrue(result.Success);
		Assert.IsTrue(_dictionary.ContainsKey(keyName));
		Assert.AreEqual(valueName, _dictionary[keyName]);
	}

	[TestMethod]
	public async Task Get_ReturnsNotFoundOnNonExistingKey()
	{
		var request = new GetRequest {Key = nonExistingKey};

		var result = await _service.Get(request, new ServerCallContextStub());

		Assert.IsFalse(result.Found);
		Assert.AreEqual(defaultValue, result.Value);
	}

	[TestMethod]
	public async Task Get_ReturnsValue()
	{
		_dictionary.TryAdd(keyName, valueName);
		var request = new GetRequest {Key = keyName};

		var result = await _service.Get(request, new ServerCallContextStub());

		Assert.IsTrue(result.Found);
		Assert.AreEqual(valueName, result.Value);
	}

	[TestMethod]
	public async Task GetAll_ReturnsAllData()
	{
		_dictionary.TryAdd("testKey1", "testValue1");
		_dictionary.TryAdd("testKey2", "testValue2");
		var request = new GetAllRequest();

		var result = await _service.GetAll(request, new ServerCallContextStub());

		Assert.AreEqual(2, result.Data.Count);
		Assert.AreEqual("testValue1", result.Data["testKey1"]);
		Assert.AreEqual("testValue2", result.Data["testKey2"]);
	}

	[TestMethod]
	public async Task Remove_RemovesSucessfully()
	{
		_dictionary.TryAdd(keyName, valueName);
		var request = new RemoveRequest {Key = keyName};

		var result = await _service.Remove(request, new ServerCallContextStub());

		Assert.IsTrue(result.Success);
		Assert.IsFalse(_dictionary.ContainsKey(keyName));
		Assert.AreEqual(valueName, result.Value);
	}

	[TestMethod]
	public async Task Remove_ReturnsFalseOnNonExistingKey()
	{
		var request = new RemoveRequest {Key = nonExistingKey};

		var result = await _service.Remove(request, new ServerCallContextStub());

		Assert.IsFalse(result.Success);
		Assert.AreEqual(defaultValue, result.Value);
	}

	[TestInitialize]
	public void Setup()
	{
		_dictionary = new ConcurrentDictionary<string, string>();
		_service = new DictionaryService(_dictionary);
	}
}
