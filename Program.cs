using Antlr4.Runtime;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Digite um comando:");
        var input = Console.ReadLine();

        var inputStream = new AntlrInputStream(input);
        var lexer = new LinguagemLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);

        tokens.Fill(); // Força a leitura de todos os tokens

        Console.WriteLine("\nTokens reconhecidos:");
        foreach (var token in tokens.GetTokens())
        {
            var nome = lexer.Vocabulary.GetSymbolicName(token.Type);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"<{nome}> : '{token.Text}'");
        }

        // (Opcional) continuar com parsing
        var parser = new LinguagemParser(tokens);
        var tree = parser.programa();
    }
}
