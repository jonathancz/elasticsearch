using System;
using Nest;

public class BulkAllObserver : IObserver<BulkAllResponse>
{
    private readonly Action<BulkAllResponse> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;

    public BulkAllObserver(Action<BulkAllResponse> onNext, Action<Exception> onError, Action onCompleted)
    {
        _onNext = onNext ?? throw new ArgumentNullException(nameof(onNext));
        _onError = onError ?? throw new ArgumentNullException(nameof(onError));
        _onCompleted = onCompleted ?? throw new ArgumentNullException(nameof(onCompleted));
    }

    public void OnNext(BulkAllResponse value) => _onNext(value);

    public void OnError(Exception error) => _onError(error);

    public void OnCompleted() => _onCompleted();
}