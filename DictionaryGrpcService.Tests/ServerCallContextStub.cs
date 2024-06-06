using Grpc.Core;

namespace DictionaryGrpcService.Tests;

public class ServerCallContextStub : ServerCallContext
{
	protected override string MethodCore { get; }
	protected override string HostCore { get; }
	protected override string PeerCore { get; }
	protected override DateTime DeadlineCore { get; }
	protected override Metadata RequestHeadersCore { get; }
	protected override CancellationToken CancellationTokenCore { get; }
	protected override Metadata ResponseTrailersCore { get; }
	protected override Status StatusCore { get; set; }
	protected override WriteOptions? WriteOptionsCore { get; set; }
	protected override AuthContext AuthContextCore { get; }

	protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options) =>
		throw new NotImplementedException();

	protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders) => throw new NotImplementedException();
}
