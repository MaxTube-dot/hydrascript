using System.Collections;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Domain.FrontEnd.Parser.Impl;

public class TokensStream : IEnumerator<Token>
{
    private readonly IEnumerator<Token> _inner;
    private int _position = 0;
    public int Position
    {
        get
        {
            return _position;
        }
    }
    private TokensStream(IEnumerator<Token> enumerator)
    {
        _inner = enumerator;
        _inner.MoveNext();
    }
    public void SetPosition(int position)
    {
        _inner.Reset();
        _inner.MoveNext();
        _position = 0;
        
        for (int i = 0; i < position; i++)
        {
            MoveNext();
        }
    }

    public bool MoveNext()
    {
        var result = _inner.MoveNext();
        if (result)
            _position++;
        return result;
    }

    public void Reset()
    {
        _inner.Reset();
        _position = -1;
    }

    public Token Current => _inner.Current;

    object IEnumerator.Current => Current;

    public void Dispose() => _inner.Dispose();

    public static implicit operator TokensStream(List<Token> tokens) => 
        new (tokens.GetEnumerator());
}