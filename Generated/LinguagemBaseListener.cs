//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Gramatica/Linguagem.g4 by ANTLR 4.13.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ILinguagemListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class LinguagemBaseListener : ILinguagemListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.programa"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrograma([NotNull] LinguagemParser.ProgramaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.programa"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrograma([NotNull] LinguagemParser.ProgramaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.comando"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComando([NotNull] LinguagemParser.ComandoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.comando"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComando([NotNull] LinguagemParser.ComandoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.escreva"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEscreva([NotNull] LinguagemParser.EscrevaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.escreva"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEscreva([NotNull] LinguagemParser.EscrevaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.leia"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLeia([NotNull] LinguagemParser.LeiaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.leia"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLeia([NotNull] LinguagemParser.LeiaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.retorne"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRetorne([NotNull] LinguagemParser.RetorneContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.retorne"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRetorne([NotNull] LinguagemParser.RetorneContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.declaracao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeclaracao([NotNull] LinguagemParser.DeclaracaoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.declaracao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeclaracao([NotNull] LinguagemParser.DeclaracaoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.atribuicao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAtribuicao([NotNull] LinguagemParser.AtribuicaoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.atribuicao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAtribuicao([NotNull] LinguagemParser.AtribuicaoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.se"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSe([NotNull] LinguagemParser.SeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.se"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSe([NotNull] LinguagemParser.SeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.enquanto"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEnquanto([NotNull] LinguagemParser.EnquantoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.enquanto"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEnquanto([NotNull] LinguagemParser.EnquantoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.bloco"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBloco([NotNull] LinguagemParser.BlocoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.bloco"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBloco([NotNull] LinguagemParser.BlocoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>texto</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTexto([NotNull] LinguagemParser.TextoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>texto</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTexto([NotNull] LinguagemParser.TextoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>agrupamento</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAgrupamento([NotNull] LinguagemParser.AgrupamentoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>agrupamento</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAgrupamento([NotNull] LinguagemParser.AgrupamentoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>inteiro</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInteiro([NotNull] LinguagemParser.InteiroContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>inteiro</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInteiro([NotNull] LinguagemParser.InteiroContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>booleano</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBooleano([NotNull] LinguagemParser.BooleanoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>booleano</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBooleano([NotNull] LinguagemParser.BooleanoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>operacao</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperacao([NotNull] LinguagemParser.OperacaoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>operacao</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperacao([NotNull] LinguagemParser.OperacaoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>comparacao</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComparacao([NotNull] LinguagemParser.ComparacaoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>comparacao</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComparacao([NotNull] LinguagemParser.ComparacaoContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>variavel</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariavel([NotNull] LinguagemParser.VariavelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>variavel</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariavel([NotNull] LinguagemParser.VariavelContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>decimal</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDecimal([NotNull] LinguagemParser.DecimalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>decimal</c>
	/// labeled alternative in <see cref="LinguagemParser.expressao"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDecimal([NotNull] LinguagemParser.DecimalContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinguagemParser.tipo"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTipo([NotNull] LinguagemParser.TipoContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinguagemParser.tipo"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTipo([NotNull] LinguagemParser.TipoContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
