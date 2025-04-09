using HydraScript.Domain.BackEnd.Impl.Values;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Parser.Impl;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Infrastructure;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class TopDownParserTests
{
    private readonly TopDownParser _parser = new(new RegexLexer(
        new Structure<GeneratedRegexContainer>(new TokenTypesProvider()),
        new TextCoordinateSystemComputer()));

    [Theory]
    [ClassData(typeof(ParserSuccessTestData))]
    public void ParserDoesNotThrowTest(string text)
    {
        var ex = Record.Exception(() =>
        {
            // ReSharper disable once UnusedVariable
            var ast = _parser.Parse(text);
        });
        Assert.Null(ex);
    }
    
    
    [Theory]
    [ClassData(typeof(FunctionDeclarationsTestData))]
    public void FunctionDeclarationsTest(string text, List<(string, string[])> expected)
    {
        var ast = _parser.Parse(text);
        var actual = new List<(string, string[])>();
        
        foreach (var abstractSyntaxTreeNode in ast.Root)
        {
            if (abstractSyntaxTreeNode is FunctionDeclaration function)
            {
                string functionName = function.Name;
                var functionArguments = function.Arguments
                    .Select(arg => arg.ToString())
                    .ToArray();
                
                actual.Add((functionName, functionArguments));
            }
        }
        
        Assert.Equal(expected.Count, actual.Count);
    
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Item1, actual[i].Item1);
            Assert.Equal(expected[i].Item2, actual[i].Item2);
        }
    }
    
}