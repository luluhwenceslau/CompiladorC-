using Antlr4.Runtime.Tree;

public class Simbolo
{
    public string Nome { get; }
    public string Tipo { get; }
    public bool Inicializado { get; set; }

    public Simbolo(string nome, string tipo)
    {
        Nome = nome;
        Tipo = tipo;
        Inicializado = false;
    }
}

public class Interpretador : LinguagemBaseListener
{
    private readonly Stack<Dictionary<string, object>> escopos = new();
    private readonly Stack<Dictionary<string, Simbolo>> tabelaSimbolos = new();
    private readonly Dictionary<string, LinguagemParser.FuncaoContext> funcoes = new();
    private readonly Dictionary<string, LinguagemParser.ClasseContext> classes = new();
    private object? valorRetorno = null;

    public Interpretador()
    {
        // Escopo global inicial
        escopos.Push(new Dictionary<string, object>());
        tabelaSimbolos.Push(new Dictionary<string, Simbolo>());

        // Funções nativas
        funcoes["escreva"] = null;
        funcoes["leia"] = null;
    }

    // Gerenciamento de escopos
    private void EntrarEscopo()
    {
        escopos.Push(new Dictionary<string, object>());
        tabelaSimbolos.Push(new Dictionary<string, Simbolo>());
    }

    private void SairEscopo()
    {
        escopos.Pop();
        tabelaSimbolos.Pop();
    }

    // Tabela de símbolos: inserir e buscar
    private void InserirSimbolo(string nome, string tipo)
    {
        var escopoAtual = tabelaSimbolos.Peek();
        if (escopoAtual.ContainsKey(nome))
            throw new Exception($"Símbolo '{nome}' já declarado neste escopo.");

        escopoAtual[nome] = new Simbolo(nome, tipo);
    }

    private Simbolo? BuscarSimbolo(string nome)
    {
        foreach (var escopo in tabelaSimbolos.Reverse())
        {
            if (escopo.TryGetValue(nome, out var simbolo))
                return simbolo;
        }
        return null;
    }

    // Gerenciamento de valores
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

        if (!funcoes.TryGetValue(nomeFuncao, out var funcaoContext))
            throw new Exception($"Função não definida: {nomeFuncao}");

        var parametros = funcaoContext.parametros();
        var argumentos = chamadaExpr.expressao();

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

        var walker = new ParseTreeWalker();
        walker.Walk(this, funcaoContext.bloco());

        SairEscopo();
    }

    // Controle de fluxo aprimorado
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

    // Manipulação de retorno
    public override void EnterRetorno(LinguagemParser.RetornoContext context)
    {
        valorRetorno = InterpretarExpressao(context.expressao());
    }

    // Declaração com análise semântica
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

    // Atribuição com verificação semântica
    public override void EnterAtribuicao(LinguagemParser.AtribuicaoContext context)
    {
        string nome = context.ID().GetText();
        var simbolo = BuscarSimbolo(nome);
        if (simbolo == null)
            throw new Exception($"Variável '{nome}' não declarada.");

        object valor = InterpretarExpressao(context.expressao());

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

    // Interpretação de expressões com análise semântica
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
            "float" => valor is double || valor is int, // aceita int para float (conversão implícita)
            "char" => valor is char,
            "texto" => valor is string,
            _ => false,
        };
    }

}
