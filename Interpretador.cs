using System;
using System.Collections.Generic;

public class Interpretador : LinguagemBaseListener
{
    private readonly Dictionary<string, object> memoria = new();

    public override void EnterEscrita(LinguagemParser.EscritaContext context)
    {
        var valor = InterpretarExpressao(context.expressao());
        Console.WriteLine(valor);
    }

    public override void EnterDeclaracao(LinguagemParser.DeclaracaoContext context)
    {
        string nome = context.ID().GetText();

        if (context.ABRE_COLCH() != null)
        {
            int tamanho = int.Parse(context.INT().GetText());
            memoria[nome] = new object[tamanho];
        }
        else
        {
            memoria[nome] = null;
        }

        if (context.expressao() != null)
        {
            memoria[nome] = InterpretarExpressao(context.expressao());
        }
    }

    public override void EnterAtribuicao(LinguagemParser.AtribuicaoContext context)
    {
        string nome = context.ID().GetText();
        object valor = InterpretarExpressao(context.expressao());

        if (context.ABRE_COLCH() != null)
        {
            int indice = int.Parse(context.INT().GetText());

            if (!memoria.ContainsKey(nome) || memoria[nome] is not object[] vetor)
                throw new Exception($"Vetor não declarado: {nome}");

            vetor[indice] = valor;
        }
        else
        {
            memoria[nome] = valor;
        }
    }

    private object InterpretarExpressao(LinguagemParser.ExpressaoContext ctx)
    {
        if (ctx.children.Count == 1)
        {
            if (ctx.ID() != null)
            {
                string nome = ctx.ID().GetText();
                if (!memoria.ContainsKey(nome))
                    throw new Exception($"Variável não declarada: {nome}");
                return memoria[nome];
            }

            if (ctx.INT() != null)
                return int.Parse(ctx.INT().GetText());

            if (ctx.FLOAT_LITERAL() != null)
                return double.Parse(ctx.FLOAT_LITERAL().GetText());

            if (ctx.STRING_LITERAL() != null)
                return ctx.STRING_LITERAL().GetText().Trim('"');

            if (ctx.CHAR_LITERAL() != null)
                return ctx.CHAR_LITERAL().GetText().Trim('\'');
        }

        if (ctx.op != null)
        {
            var esquerda = InterpretarExpressao(ctx.expressao(0));
            var direita = InterpretarExpressao(ctx.expressao(1));

            return ctx.op.Type switch
            {
                LinguagemParser.MAIS => Convert.ToDouble(esquerda) + Convert.ToDouble(direita),
                LinguagemParser.MENOS => Convert.ToDouble(esquerda) - Convert.ToDouble(direita),
                LinguagemParser.MULT => Convert.ToDouble(esquerda) * Convert.ToDouble(direita),
                LinguagemParser.DIV => Convert.ToDouble(esquerda) / Convert.ToDouble(direita),
                LinguagemParser.MAIOR => Convert.ToDouble(esquerda) > Convert.ToDouble(direita),
                LinguagemParser.MENOR => Convert.ToDouble(esquerda) < Convert.ToDouble(direita),
                LinguagemParser.MAIOR_IGUAL => Convert.ToDouble(esquerda) >= Convert.ToDouble(direita),
                LinguagemParser.MENOR_IGUAL => Convert.ToDouble(esquerda) <= Convert.ToDouble(direita),
                LinguagemParser.IGUAL_IGUAL => Equals(esquerda, direita),
                LinguagemParser.DIFERENTE => !Equals(esquerda, direita),
                _ => throw new Exception("Operador desconhecido")
            };
        }

        if (ctx.expressao().Length == 1)
        {
            return InterpretarExpressao(ctx.expressao(0)); // Parênteses
        }

        if (ctx.ID() != null && ctx.INT() != null)
        {
            string nome = ctx.ID().GetText();
            int indice = int.Parse(ctx.INT().GetText());

            if (!memoria.ContainsKey(nome) || memoria[nome] is not object[] vetor)
                throw new Exception($"Vetor não declarado: {nome}");

            return vetor[indice];
        }

        throw new Exception("Expressão não reconhecida");
    }
}
