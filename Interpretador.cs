using Antlr4.Runtime.Tree;
using Compilador;
using LLVMSharp.Interop;

public class Interpretador : LinguagemBaseListener
{
    private readonly Stack<Dictionary<string, object>> escopos = new();
    private readonly TabelaSimbolos tabelaSimbolos = new();
    private readonly Dictionary<string, LinguagemParser.FuncaoContext> funcoes = new();
    private readonly Dictionary<string, LinguagemParser.ClasseContext> classes = new();
    private object? valorRetorno = null;

    public TabelaSimbolos TabelaSimbolos { get; } = new TabelaSimbolos();

    private GeradorLLVM gerador;

    public Interpretador()
    {


        gerador = new GeradorLLVM();

        escopos.Push(new Dictionary<string, object>());

        // Funções nativas
        funcoes["escreva"] = null;
        funcoes["leia"] = null;
    }

    

    private LLVMValueRef GerarCodigoExpressao(LinguagemParser.ExpressaoContext ctx)
    {
        if (ctx.INT() != null)
        {
            int valor = int.Parse(ctx.INT().GetText());
            return gerador.ConstInt32(valor);
        }

        if (ctx.expressao().Length == 2)
        {
            var esquerda = GerarCodigoExpressao(ctx.expressao(0));
            var direita = GerarCodigoExpressao(ctx.expressao(1));

            switch (ctx.op.Type)
            {
                case LinguagemParser.MAIS:
                    return gerador.GerarSoma(esquerda, direita);
                case LinguagemParser.MENOS:
                    return gerador.Builder.BuildSub(esquerda, direita, "sub_tmp");
                    // Adicione outros operadores conforme necessário
            }
        }

        throw new Exception("Expressão não suportada para geração LLVM");
    }



    private void EntrarEscopo()
    {
        escopos.Push(new Dictionary<string, object>());
        tabelaSimbolos.EntrarEscopo();
    }

    private void SairEscopo()
    {
        escopos.Pop();
        tabelaSimbolos.SairEscopo();
    }

    private void InserirSimbolo(string nome, string tipo)
    {
        tabelaSimbolos.InserirSimbolo(nome, tipo, CategoriaSimbolo.Variavel);
    }

    private Simbolo? BuscarSimbolo(string nome)
    {
        return tabelaSimbolos.BuscarSimbolo(nome);
    }

    private object? ObterValor(string nome)
    {
        foreach (var escopo in escopos.Reverse())
        {
            if (escopo.TryGetValue(nome, out var valor))
                return valor;
        }
        return null;
    }

    private void DefinirValor(string nome, object valor)
    {
        foreach (var escopo in escopos)
        {
            if (escopo.ContainsKey(nome))
            {
                escopo[nome] = valor;
                return;
            }
        }
        escopos.Peek()[nome] = valor;
    }

    // Manipulação de funções
    public override void EnterFuncao(LinguagemParser.FuncaoContext context)
    {
        string nome = context.ID().GetText();
        funcoes[nome] = context;
    }

    public override void EnterChamadaFuncao(LinguagemParser.ChamadaFuncaoContext context)
    {
        var chamadaExpr = context.chamadaFuncaoExpr();
        string nomeFuncao = chamadaExpr.ID().GetText();

        // Tratamento das funções nativas
        if (nomeFuncao == "escreva")
        {
            var argumentos = chamadaExpr.expressao();
            foreach (var arg in argumentos)
            {
                var valor = InterpretarExpressao(arg);
                Console.Write(valor);
            }
            Console.WriteLine();
            return; // Não precisa continuar o processamento normal
        }
        else if (nomeFuncao == "leia")
        {
            var argumentos = chamadaExpr.expressao();
            if (argumentos.Length != 1)
                throw new Exception("Função 'leia' deve receber exatamente um argumento (variável).");

            string nomeVariavel = argumentos[0].GetText();

            string entrada = Console.ReadLine() ?? "";

            var simbolo = BuscarSimbolo(nomeVariavel);
            if (simbolo == null)
                throw new Exception($"Variável '{nomeVariavel}' não declarada.");

            object valorConvertido;

            // Converte a entrada para o tipo da variável
            switch (simbolo.Tipo)
            {
                case "inteiro":
                    if (!int.TryParse(entrada, out int intVal))
                        throw new Exception($"Entrada inválida para inteiro: '{entrada}'");
                    valorConvertido = intVal;
                    break;
                case "float":
                    if (!double.TryParse(entrada, out double doubleVal))
                        throw new Exception($"Entrada inválida para float: '{entrada}'");
                    valorConvertido = doubleVal;
                    break;
                case "char":
                    if (string.IsNullOrEmpty(entrada))
                        throw new Exception("Entrada vazia para char");
                    valorConvertido = entrada[0];
                    break;
                case "texto":
                    valorConvertido = entrada;
                    break;
                default:
                    throw new Exception($"Tipo não suportado para leitura: {simbolo.Tipo}");
            }

            DefinirValor(nomeVariavel, valorConvertido);
            simbolo.Inicializado = true;
            return;
        }

        if (!funcoes.TryGetValue(nomeFuncao, out var funcaoContext))
            throw new Exception($"Função não definida: {nomeFuncao}");

        var parametros = funcaoContext.parametros();
        var argumentosFuncao = chamadaExpr.expressao();

        EntrarEscopo();

        if (parametros != null)
        {
            var listaParametros = parametros.parametro();
            for (int i = 0; i < listaParametros.Length; i++)
            {
                string nomeParametro = listaParametros[i].ID().GetText();
                object valorArgumento = InterpretarExpressao(argumentosFuncao[i]);
                DefinirValor(nomeParametro, valorArgumento);
            }
        }

        valorRetorno = null;

        var walker = new ParseTreeWalker();
        walker.Walk(this, funcaoContext.bloco());

        SairEscopo();

        if (valorRetorno == null)
            throw new Exception($"Função '{nomeFuncao}' não retornou valor.");
    }


    public override void EnterDecisao(LinguagemParser.DecisaoContext context)
    {
        bool condicao = Convert.ToBoolean(InterpretarExpressao(context.expressao()));
        var blocos = context.bloco();

        EntrarEscopo();

        if (condicao)
        {
            WalkBloco(blocos[0]);
        }
        else if (blocos.Length > 1)
        {
            WalkBloco(blocos[1]);
        }

        SairEscopo();
    }

    public override void EnterRepeticao(LinguagemParser.RepeticaoContext context)
    {
        while (Convert.ToBoolean(InterpretarExpressao(context.expressao())))
        {
            EntrarEscopo();
            WalkBloco(context.bloco());
            SairEscopo();
        }
    }

    private void WalkBloco(LinguagemParser.BlocoContext bloco)
    {
        var walker = new ParseTreeWalker();
        walker.Walk(this, bloco);
    }

    public override void EnterRetorno(LinguagemParser.RetornoContext context)
    {
        valorRetorno = InterpretarExpressao(context.expressao());
    }

    public override void EnterDeclaracao(LinguagemParser.DeclaracaoContext context)
    {
        string nome = context.ID().GetText();
        string tipo = context.tipo().GetText();

        InserirSimbolo(nome, tipo);

        object? valor = null;

        if (context.expressao() != null)
        {
            valor = InterpretarExpressao(context.expressao());
            var simbolo = BuscarSimbolo(nome);
            if (simbolo != null)
                simbolo.Inicializado = true;
        }
        else if (context.ABRE_COLCH() != null)
        {
            int tamanho = int.Parse(context.INT().GetText());
            valor = new object[tamanho];
        }

        DefinirValor(nome, valor);
    }

    public override void EnterAtribuicao(LinguagemParser.AtribuicaoContext context)
    {
        string nome = context.ID().GetText();
        var simbolo = BuscarSimbolo(nome);
        if (simbolo == null)
            throw new Exception($"Variável '{nome}' não declarada.");

        object valor = InterpretarExpressao(context.expressao());
        var valorLLVM = GerarCodigoExpressao(context.expressao());

        if (!TipoCompativel(simbolo.Tipo, valor))
            throw new Exception($"Tipo incompatível na atribuição à variável '{nome}'. Esperado '{simbolo.Tipo}', mas recebeu '{valor.GetType().Name}'.");

        simbolo.Inicializado = true;

        if (context.ABRE_COLCH() != null)
        {
            int indice = int.Parse(context.INT().GetText());
            var vetor = ObterValor(nome) as object[]
                ?? throw new Exception($"{nome} não é um vetor");

            if (indice < 0 || indice >= vetor.Length)
                throw new Exception($"Índice {indice} fora dos limites");

            vetor[indice] = valor;
        }
        else
        {
            DefinirValor(nome, valor);
        }
    }

    private object InterpretarExpressao(LinguagemParser.ExpressaoContext ctx)
    {
        if (ctx.chamadaFuncaoExpr() != null)
        {
            return InterpretarChamadaFuncaoExpr(ctx.chamadaFuncaoExpr());
        }

        if (ctx.INT() != null) return int.Parse(ctx.INT().GetText());
        if (ctx.FLOAT_LITERAL() != null) return double.Parse(ctx.FLOAT_LITERAL().GetText());
        if (ctx.STRING_LITERAL() != null) return ctx.STRING_LITERAL().GetText()[1..^1];
        if (ctx.CHAR_LITERAL() != null) return ctx.CHAR_LITERAL().GetText()[1];

        if (ctx.ID() != null)
        {
            var simbolo = BuscarSimbolo(ctx.ID().GetText());
            if (simbolo == null)
                throw new Exception($"Variável '{ctx.ID().GetText()}' não declarada.");

            if (!simbolo.Inicializado)
                throw new Exception($"Variável '{ctx.ID().GetText()}' não inicializada.");

            var valor = ObterValor(ctx.ID().GetText());
            return valor ?? throw new Exception($"Variável '{ctx.ID().GetText()}' sem valor definido.");
        }

        if (ctx.ID() != null && ctx.INT() != null)
        {
            string nome = ctx.ID().GetText();
            int indice = int.Parse(ctx.INT().GetText());
            var vetor = ObterValor(nome) as object[]
                ?? throw new Exception($"{nome} não é um vetor");

            return vetor[indice];
        }

        if (ctx.expressao().Length == 2)
        {
            object esq = InterpretarExpressao(ctx.expressao(0));
            object dir = InterpretarExpressao(ctx.expressao(1));

            // Verificar se os operandos são numéricos para operações aritméticas
            bool esqNumerico = esq is int || esq is double;
            bool dirNumerico = dir is int || dir is double;

            switch (ctx.op.Type)
            {
                case LinguagemParser.MAIS:
                case LinguagemParser.MENOS:
                case LinguagemParser.MULT:
                case LinguagemParser.DIV:
                    if (!esqNumerico || !dirNumerico)
                        throw new Exception("Operações aritméticas requerem operandos numéricos.");
                    break;
                case LinguagemParser.MAIOR:
                case LinguagemParser.MENOR:
                case LinguagemParser.MAIOR_IGUAL:
                case LinguagemParser.MENOR_IGUAL:
                    if (!esqNumerico || !dirNumerico)
                        throw new Exception("Operações relacionais requerem operandos numéricos.");
                    break;
                case LinguagemParser.IGUAL_IGUAL:
                case LinguagemParser.DIFERENTE:
                    // Permitir comparação entre tipos iguais
                    if (esq.GetType() != dir.GetType())
                        throw new Exception("Comparação entre tipos incompatíveis.");
                    break;
            }

            dynamic dEsq = esq;
            dynamic dDir = dir;

            return ctx.op.Type switch
            {
                LinguagemParser.MAIS => dEsq + dDir,
                LinguagemParser.MENOS => dEsq - dDir,
                LinguagemParser.MULT => dEsq * dDir,
                LinguagemParser.DIV => dEsq / dDir,
                LinguagemParser.MAIOR => dEsq > dDir,
                LinguagemParser.MENOR => dEsq < dDir,
                LinguagemParser.MAIOR_IGUAL => dEsq >= dDir,
                LinguagemParser.MENOR_IGUAL => dEsq <= dDir,
                LinguagemParser.IGUAL_IGUAL => dEsq == dDir,
                LinguagemParser.DIFERENTE => dEsq != dDir,
                _ => throw new Exception("Operador inválido")
            };
        }

        if (ctx.ABRE_PAREN() != null)
            return InterpretarExpressao(ctx.expressao(0));

        throw new Exception("Expressão não reconhecida");
    }

    private object? InterpretarChamadaFuncaoExpr(LinguagemParser.ChamadaFuncaoExprContext ctx)
    {
        string nomeFuncao = ctx.ID().GetText();

        if (!funcoes.TryGetValue(nomeFuncao, out var funcaoContext))
            throw new Exception($"Função não definida: {nomeFuncao}");

        var parametros = funcaoContext.parametros();
        var argumentos = ctx.expressao();

        EntrarEscopo();

        if (parametros != null)
        {
            var listaParametros = parametros.parametro();
            for (int i = 0; i < listaParametros.Length; i++)
            {
                string nomeParametro = listaParametros[i].ID().GetText();
                object valorArgumento = InterpretarExpressao(argumentos[i]);
                DefinirValor(nomeParametro, valorArgumento);
            }
        }

        valorRetorno = null;

        var walker = new ParseTreeWalker();
        walker.Walk(this, funcaoContext.bloco());

        SairEscopo();

        if (valorRetorno == null)
            throw new Exception($"Função '{nomeFuncao}' não retornou valor.");

        return valorRetorno;
    }

    private bool TipoCompativel(string tipoVariavel, object valor)
    {
        if (valor == null) return false;

        return tipoVariavel switch
        {
            "inteiro" => valor is int,
            "float" => valor is double || valor is int,
            "char" => valor is char,
            "texto" => valor is string,
            _ => false,
        };
    }

}
