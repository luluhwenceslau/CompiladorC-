namespace Compilador
{
    public enum CategoriaSimbolo
    {
        Variavel,
        Funcao,
        Classe,
        PalavraReservada,
        SimboloEspecial
    }

    public class Simbolo
    {
        public string Nome { get; }
        public string Tipo { get; }
        public CategoriaSimbolo Categoria { get; }
        public string Escopo { get; }
        public object? Valor { get; set; }
        public bool Inicializado { get; set; }

        public Simbolo(string nome, string tipo, CategoriaSimbolo categoria, string escopo, object? valor = null)
        {
            Nome = nome;
            Tipo = tipo;
            Categoria = categoria;
            Escopo = escopo;
            Valor = valor;
            Inicializado = false;
        }

        public override string ToString()
        {
            return $"Nome: {Nome}, Tipo: {Tipo}, Categoria: {Categoria}, Escopo: {Escopo}, Valor: {Valor ?? "null"}, Inicializado: {Inicializado}";
        }
    }

    public class TabelaSimbolos
    {
        private readonly Stack<Dictionary<string, Simbolo>> escopos = new();

        public TabelaSimbolos()
        {
            escopos.Push(new Dictionary<string, Simbolo>());
            InserirPalavrasReservadas();
            InserirSimbolosEspeciais();
        }

        public void EntrarEscopo()
        {
            escopos.Push(new Dictionary<string, Simbolo>());
        }

        public void SairEscopo()
        {
            if (escopos.Count > 1)
                escopos.Pop();
            else
                throw new InvalidOperationException("Não é possível sair do escopo global.");
        }

        public void InserirSimbolo(string nome, string tipo, CategoriaSimbolo categoria, object? valor = null)
        {
            var escopoAtual = escopos.Peek();

            if (escopoAtual.ContainsKey(nome))
                throw new Exception($"Símbolo '{nome}' já declarado no escopo atual.");

            string nomeEscopo = ObterNomeEscopoAtual();

            var simbolo = new Simbolo(nome, tipo, categoria, nomeEscopo, valor);
            escopoAtual[nome] = simbolo;
        }

        public Simbolo? BuscarSimbolo(string nome)
        {
            foreach (var escopo in escopos)
            {
                if (escopo.TryGetValue(nome, out var simbolo))
                    return simbolo;
            }
            return null;
        }

        public bool ExisteNoEscopoAtual(string nome)
        {
            return escopos.Peek().ContainsKey(nome);
        }

        public void AtualizarValor(string nome, object valor)
        {
            var simbolo = BuscarSimbolo(nome);
            if (simbolo == null)
                throw new Exception($"Símbolo '{nome}' não declarado.");

            simbolo.Valor = valor;
            simbolo.Inicializado = true;
        }

        public object ObterValor(string nome)
        {
            var simbolo = BuscarSimbolo(nome);
            if (simbolo == null)
                throw new Exception($"Símbolo '{nome}' não declarado.");

            if (!simbolo.Inicializado)
                throw new Exception($"Símbolo '{nome}' não inicializado.");

            return simbolo.Valor!;
        }

        public void ExibirTabela()
        {
            Console.WriteLine("Tabela de Símbolos:");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("| Nome           | Tipo           | Categoria       | Escopo   | Inicializado | Valor");
            Console.WriteLine("--------------------------------------------------------------------------------");

            foreach (var escopo in escopos.Reverse())
            {
                foreach (var simbolo in escopo.Values)
                {
                    Console.WriteLine($"| {simbolo.Nome,-14} | {simbolo.Tipo,-14} | {simbolo.Categoria,-14} | {simbolo.Escopo,-8} | {simbolo.Inicializado,-12} | {simbolo.Valor ?? "-"}");
                }
            }

            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        private string ObterNomeEscopoAtual()
        {
            return escopos.Count == 1 ? "global" : "local";
        }

        private void InserirPalavrasReservadas()
        {
            string[] palavras = new string[]
            {
                "inteiro", "float", "char", "texto",
                "return", "se", "senao", "enquanto",
                "leia", "escreva", "classe"
            };

            foreach (var palavra in palavras)
            {
                escopos.Peek()[palavra] = new Simbolo(palavra, "palavraReservada", CategoriaSimbolo.PalavraReservada, "global", null) { Inicializado = true };
            }
        }

        private void InserirSimbolosEspeciais()
        {
            string[] simbolos = new string[]
            {
                "+", "-", "*", "/", "=", ">", "<", ">=", "<=", "==", "!=",
                "(", ")", "{", "}", "[", "]", ";", ","
            };

            foreach (var simbolo in simbolos)
            {
                escopos.Peek()[simbolo] = new Simbolo(simbolo, "simboloEspecial", CategoriaSimbolo.SimboloEspecial, "global", null) { Inicializado = true };
            }
        }
    }
}
