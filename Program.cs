using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;
using Compilador; // Namespace onde estão Interpretador, TabelaSimbolos, Simbolo

class Program
{
    static void Main(string[] args)
    {
        string caminhoArquivo = @"C:\Users\Luana\Documents\ProjetoCompiladores\Compilador1\entrada.txt";

        string input;
        try
        {
            input = File.ReadAllText(caminhoArquivo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler o arquivo: {ex.Message}");
            return;
        }

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Conteúdo do arquivo:");
        Console.WriteLine(input);
        Console.WriteLine("\nTokens reconhecidos:");

        try
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new LinguagemLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);

            tokens.Fill();

            foreach (var token in tokens.GetTokens())
            {
                var nome = lexer.Vocabulary.GetSymbolicName(token.Type);
                Console.WriteLine($"<{nome}> : '{token.Text}'");
            }

            var parser = new LinguagemParser(tokens);
            var tree = parser.programa();

            Console.WriteLine("\nAnálise sintática concluída.");

            Console.WriteLine("\nÁrvore Sintática (formato texto):");
            Console.WriteLine(tree.ToStringTree(parser));

            Console.WriteLine("\nÁrvore Sintática (formatada):");
            PrintTree(tree, parser);

            // Executar interpretador
            var interpretador = new Interpretador();
            var walker = new ParseTreeWalker();
            walker.Walk(interpretador, tree);

            Console.WriteLine("\nExecução concluída.");

            Console.WriteLine("\nTabela de Símbolos após execução:");
            interpretador.TabelaSimbolos.ExibirTabela();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante a análise ou execução: {ex.Message}");
        }

        Console.WriteLine("\nPressione ENTER para sair...");
        Console.ReadLine();
    }

    static void PrintTree(IParseTree tree, Parser parser, string indent = "")
    {
        string nodeText = Trees.GetNodeText(tree, parser);
        Console.WriteLine(indent + nodeText);

        for (int i = 0; i < tree.ChildCount; i++)
        {
            PrintTree(tree.GetChild(i), parser, indent + "  ");
        }
    }
}
