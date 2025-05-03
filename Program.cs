using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Digite um comando:");
        string input = Console.ReadLine();

        // Inicializando o lexer e parser com o input
        AntlrInputStream inputStream = new AntlrInputStream(input);
        LinguagemLexer lexer = new LinguagemLexer(inputStream);
        CommonTokenStream tokenStream = new CommonTokenStream(lexer);
        LinguagemParser parser = new LinguagemParser(tokenStream);

        // Gerando a árvore de análise
        var tree = parser.programa(); // Inicia pela regra "programa"

        // Criando o interpretador e fazendo o walk pela árvore
        var interpretador = new Interpretador();
        ParseTreeWalker.Default.Walk(interpretador, tree); // Caminha pela árvore com o interpretador
    }
}
