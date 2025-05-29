using Antlr4.Runtime;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Caminho absoluto do arquivo de entrada no meu projeto
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

        Console.WriteLine("Conteúdo do arquivo:");
        Console.WriteLine(input);
        Console.WriteLine("\nTokens reconhecidos:");

        try
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new LinguagemLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);

            tokens.Fill(); // Força a leitura de todos os tokens

            foreach (var token in tokens.GetTokens())
            {
                var nome = lexer.Vocabulary.GetSymbolicName(token.Type);
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine($"<{nome}> : '{token.Text}'");
            }

            // (Opcional) continuar com parsing
            var parser = new LinguagemParser(tokens);
            var tree = parser.programa();

            Console.WriteLine("\nAnálise concluída.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante a análise: {ex.Message}");
        }
    }
}
