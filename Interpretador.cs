using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class Interpretador : LinguagemBaseListener
{
    private readonly Stack<Dictionary<string, object>> escopos = new();
    private readonly Dictionary<string, LinguagemParser.FuncaoContext> funcoes = new();
    private readonly Dictionary<string, LinguagemParser.ClasseContext> classes = new();
    private object? valorRetorno = null;

    public Interpretador()
    {
        // Escopo global inicial
        escopos.Push(new Dictionary<string, object>());

        // Funções nativas
        funcoes["escreva"] = null;
        funcoes["leia"] = null;
    }

    // Gerenciamento de escopos
    private void EntrarEscopo() => escopos.Push(new Dictionary<string, object>());
    private void SairEscopo() => escopos.Pop();

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
        // Atualiza variável existente
        foreach (var escopo in escopos)
        {
            if (escopo.ContainsKey(nome))
            {
                escopo[nome] = valor;
                return;
            }
        }
        // Nova variável no escopo atual
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
                DefinirValor(nomeParametro, valorArgumento);  // Vincula parâmetro ao argumento
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

    // Sistema de tipos aprimorado
    public override void EnterDeclaracao(LinguagemParser.DeclaracaoContext context)
    {
        string nome = context.ID().GetText();
        object? valor = null;

        // Inicialização
        if (context.expressao() != null)
        {
            valor = InterpretarExpressao(context.expressao());
        }
        // Vetores
        else if (context.ABRE_COLCH() != null)
        {
            int tamanho = int.Parse(context.INT().GetText());
            valor = new object[tamanho];
        }

        DefinirValor(nome, valor);
    }

    // Atribuição com verificação de tipos
    public override void EnterAtribuicao(LinguagemParser.AtribuicaoContext context)
    {
        string nome = context.ID().GetText();
        object valor = InterpretarExpressao(context.expressao());

        // Verificação de tipo para vetores
        if (context.ABRE_COLCH() != null)
        {
            int indice = int.Parse(context.INT().GetText());
            var vetor = ObterValor(nome) as object[]
                ?? throw new Exception($"{nome} não é um vetor");

            // Verificação de limites
            if (indice < 0 || indice >= vetor.Length)
                throw new Exception($"Índice {indice} fora dos limites");

            vetor[indice] = valor;
        }
        else
        {
            DefinirValor(nome, valor);
        }
    }

    // Interpretação de expressões completa
    private object InterpretarExpressao(LinguagemParser.ExpressaoContext ctx)
    {
        // Dentro de InterpretarExpressao(LinguagemParser.ExpressaoContext ctx)
        if (ctx.chamadaFuncaoExpr() != null)
        {
            return InterpretarChamadaFuncaoExpr(ctx.chamadaFuncaoExpr());
        }

        // Literais e identificadores
        if (ctx.INT() != null) return int.Parse(ctx.INT().GetText());
        if (ctx.FLOAT_LITERAL() != null) return double.Parse(ctx.FLOAT_LITERAL().GetText());
        if (ctx.STRING_LITERAL() != null) return ctx.STRING_LITERAL().GetText()[1..^1];
        if (ctx.CHAR_LITERAL() != null) return ctx.CHAR_LITERAL().GetText()[1];
        if (ctx.ID() != null)
        {
            var valor = ObterValor(ctx.ID().GetText());
            return valor ?? throw new Exception($"Variável não inicializada: {ctx.ID().GetText()}");
        }

        // Acesso a vetores
        if (ctx.ID() != null && ctx.INT() != null)
        {
            string nome = ctx.ID().GetText();
            int indice = int.Parse(ctx.INT().GetText());
            var vetor = ObterValor(nome) as object[]
                ?? throw new Exception($"{nome} não é um vetor");

            return vetor[indice];
        }

        // Operações binárias
        if (ctx.expressao().Length == 2)
        {
            dynamic esq = InterpretarExpressao(ctx.expressao(0));
            dynamic dir = InterpretarExpressao(ctx.expressao(1));

            return ctx.op.Type switch
            {
                LinguagemParser.MAIS => esq + dir,
                LinguagemParser.MENOS => esq - dir,
                LinguagemParser.MULT => esq * dir,
                LinguagemParser.DIV => esq / dir,
                LinguagemParser.MAIOR => esq > dir,
                LinguagemParser.MENOR => esq < dir,
                LinguagemParser.MAIOR_IGUAL => esq >= dir,
                LinguagemParser.MENOR_IGUAL => esq <= dir,
                LinguagemParser.IGUAL_IGUAL => esq == dir,
                LinguagemParser.DIFERENTE => esq != dir,
                _ => throw new Exception("Operador inválido")
            };
        }

        // Parênteses
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

        valorRetorno = null; // Resetar antes da execução da função

        var walker = new ParseTreeWalker();
        walker.Walk(this, funcaoContext.bloco());

        SairEscopo();

        if (valorRetorno == null)
            throw new Exception($"Função '{nomeFuncao}' não retornou valor.");

        return valorRetorno;
    }

}
